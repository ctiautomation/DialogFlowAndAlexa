using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Ai.QnA;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Scorpio.Bot.Dialogs;
using Scorpio.Bot.Models;
using Scorpio.Bot.Services;
using TextPrompt = Microsoft.Bot.Builder.Dialogs.TextPrompt;

namespace Scorpio.Bot
{
    /// <summary>
    /// Defines the dialogs this bot will handle by reading the ConversationModel.json
    /// </summary>
    public class Bot : IBot
    {
        private const double LUIS_INTENT_THRESHOLD = 0.7d;

        private readonly DialogSet dialogs;

        public List<Prompt> collectSteps = new List<Prompt>();
        public bool stepsCollected = false;

        public Bot(IConfiguration configuration)
        {
            // Get QnA Maker and add None and Greeting Dialogs
            var (knowledgeBaseId, endpointKey, host) = Startup.GetQnAMakerConfiguration(configuration);
            dialogs = new DialogSet();
            dialogs.Add(None.Id, None.Instance(new QnAMakerEndpoint
            {
                EndpointKey = endpointKey,
                KnowledgeBaseId = knowledgeBaseId,
                Host = host
            }));
            dialogs.Add(Greeting.Id, Greeting.Instance);

            // Load ConversationModel.json
            var service = new ConversationModelService();
            var conversationModel = service.ConversationModel();

            var collectStepCount = 0;
            foreach (var entity in conversationModel.Entities)
            {
                // Count the entities we need to prompt for
                if (service.GetMapper(entity).Type.Equals(MapperTypes.LuisEntityToPropertyMapper))
                {
                    collectStepCount++;
                }

                // Get the prompts
                if (!stepsCollected)
                {
                    var prompt = service.GetPrompt(entity);

                    if (!collectSteps.Contains(prompt))
                    {
                        collectSteps.Add(prompt);
                    }
                }

                // Add a validator for each entity prompt
                dialogs.Add($"{entity}Prompt", new TextPrompt(CollectStepValidator));
            }
            stepsCollected = true;

            // Create the conversation flow to start with check for authentication
            var waterfall = new WaterfallStep[collectStepCount + 2];
            waterfall[0] = AuthenticateStep;

            // Add a step to the flow to collect the user's input for each entity prompt
            for (var i = 1; i < collectStepCount + 1; i++)
            {
                waterfall[i] = CollectStep;
            }

            // Add the final step
            waterfall[collectStepCount + 1] = CompleteOrder;

            // Add the conversation dialog
            dialogs.Add(conversationModel.Purpose, waterfall);
        }

        /// <summary>
        /// Method to authenticate the user.
        /// </summary>
        private async Task AuthenticateStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            // Load ConversationModel.json
            var service = new ConversationModelService();
            var conversationModel = service.ConversationModel();
            if (!conversationModel.AuthenticationType.Equals(AuthenticationTypes.None))
            {
                // Auth will come through ChannelData
                switch (conversationModel.AuthenticationType)
                {
                    case AuthenticationTypes.ChannelData:
                        if (dialogContext.Context.Activity.ChannelData != null)
                        {
                            var channelData = JsonConvert.DeserializeObject<Dictionary<string, string>>(dialogContext.Context.Activity.ChannelData.ToString());
                            channelData.TryGetValue("UserName", out string userName);
                            channelData.TryGetValue("Token", out string token);

                            // User is signed in
                            if (!token.Equals(string.Empty) && !userName.Equals(string.Empty))
                            {
                                // TODO: Act on the token
                                await dialogContext.Context.SendActivity($"Ok, {userName}.", $"Ok, {userName}.");
                                await dialogContext.Continue();
                            }
                            // User is not signed in
                            else
                            {
                                await dialogContext.Context.SendActivity("Please sign into your account.", "Please sign into your account.", InputHints.AcceptingInput);
                                await dialogContext.End();
                            }
                        }
                        break;
                    // TODO: other types of auth
                    default:
                        break;
                }
            }
            else
            {
                await dialogContext.Continue();
            }
        }

        /// <summary>
        /// Method to collect info from the user.
        /// </summary>
        private async Task CollectStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            // Check to see if info was already collected 
            var state = dialogContext.Context.GetConversationState<Dictionary<string, object>>();
            var collected = state.TryGetValue(collectSteps[0].PropertyName, out object userInput);
            if (collected)
            {
                // If found, remove this step from the dialog and continue
                collectSteps.Remove(collectSteps[0]);
                await dialogContext.Continue();
            }
            else
            {
                // Prompt for info
                var options = new PromptOptions
                {
                    Speak = collectSteps[0].Value,
                };
                await dialogContext.Prompt($"{collectSteps[0].EntityName}Prompt", collectSteps[0].Value, options);
            }
        }

        /// <summary>
        /// Method to validate that the info was collected from the user.
        /// If the user's input wasn't valid, the bot will ask again.
        /// </summary>
        private async Task CollectStepValidator(ITurnContext context, TextResult result)
        {
            // Check to see if info was found in the LUIS entities
            var state = context.GetConversationState<Dictionary<string, object>>();
            var collected = state.TryGetValue(collectSteps[0].PropertyName, out object userInput);
            if (!collected)
            {
                // If not found in the LUIS entities, ask again
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity(collectSteps[0].RetryValue, collectSteps[0].RetryValue, InputHints.ExpectingInput);
            }
            else
            {
                // If found, remove this step from the dialog
                collectSteps.Remove(collectSteps[0]);
            }
        }

        /// <summary>
        /// Method to finish the order after all info is collected.
        /// Sends a response and clears the order details.
        /// </summary>
        private async Task CompleteOrder(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            // Get the current dialog
            var service = new ConversationModelService();
            var conversationModel = service.ConversationModel();

            // Send the flow's outcome response with the user's inputed values
            var state = dialogContext.Context.GetConversationState<Dictionary<string, object>>();
            Regex _regex = new Regex(@"\{(\w+)\}", RegexOptions.Compiled);
            var response = _regex.Replace(conversationModel.Outcome.Value,
                match => state[service.GetPrompt(match.Groups[1].Value).PropertyName].ToString());
            await dialogContext.Context.SendActivity(response, response, InputHints.AcceptingInput);

            // TODO: Business logic to process the user's inputed values would go here

            // Clear the user's inputed values for the current dialog
            foreach (var entity in conversationModel.Entities)
            {
                state.Remove(service.GetPrompt(entity).PropertyName);
            }

            // End dialog
            stepsCollected = false;
            await dialogContext.End();
        }

        /// <summary>
        /// Every Conversation turn for our bot will call this method. In here
        /// the bot checks the Activity type, and either sends a welcome message,
        /// a QnA answer, or a LUIS intent response.
        /// </summary>
        /// <param name="context">Turn scoped context containing all the data needed
        /// for processing this conversation turn. </param>        
        public async Task OnTurn(ITurnContext context)
        {
            var state = context.GetConversationState<Dictionary<string, object>>();
            var dialogContext = dialogs.CreateContext(context, state);

            switch (context.Activity.Type)
            {
                case ActivityTypes.ConversationUpdate:
                    // Send the welcome message when the user is added to the conversation
                    if (context.Activity.MembersAdded.FirstOrDefault()?.Id == context.Activity.Recipient.Id)
                    {
                        await dialogContext.Begin(Greeting.Id);
                    }
                    break;
                case ActivityTypes.Message:
                    var luisResult = context.Services.Get<RecognizerResult>(LuisRecognizerMiddleware.LuisRecognizerResultKey);

                    // Map each prompt property in the conversation model to LUIS entity results
                    var service = new ConversationModelService();
                    var conversationModel = service.ConversationModel();
                    foreach (var entity in conversationModel.Entities)
                    {
                        switch (service.GetMapper(entity).Type)
                        {
                            case MapperTypes.LuisEntityToPropertyMapper:
                                var propertyMapper = new LuisEntityToPropertyMapper();
                                propertyMapper.Map(service.GetPrompt(entity).PropertyName, entity, state, luisResult);
                                break;
                            default:
                                // TODO: default
                                break;
                        }
                    }

                    await dialogContext.Continue();
                    if (!context.Responded)
                    {
                        var (intent, score) = luisResult.GetTopScoringIntent();
                        var intentResult = score > LUIS_INTENT_THRESHOLD ? intent : None.Id;
                        await dialogContext.Begin(intentResult);
                    }
                    break;
            }
        }
    }
}
