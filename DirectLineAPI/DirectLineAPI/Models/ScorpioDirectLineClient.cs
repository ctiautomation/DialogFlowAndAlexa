using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Configuration;

namespace DirectLineAPI.Models
{
    /// <summary>
    /// corpioDirectLineClient is a service that we use to connect to the botframework
    /// This service is instantiated on startup and is available to the controllers via constructor dependency injection
    /// </summary>
    public class ScorpioDirectLineClient : IScorpioDirectLineClient
    {
        private readonly IConfiguration _config;
        public DirectLineClient _client;
        private string fromUser;
        private Dictionary<string, string> conversationMappings;

        /// <summary>
        /// Constructor to initialize our service
        /// </summary>
        /// <param name="config">Configuration Interface</param>
        public ScorpioDirectLineClient(IConfiguration config)
        {
            _config = config;
            _client = new DirectLineClient(_config["Bot:Secret"]);
            fromUser = _config["Bot:FromUser"];
            conversationMappings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a conversation in the botframework using directline
        /// We add this to a dictionary where the Client sessionId is the key and the botframework conversation Id is the value
        /// </summary>
        /// <param name="clientConversationId">Client Session Id</param>
        /// <returns>BotFramework Conversation Id</returns>
        private string CreateConversation(string clientConversationId)
        {
            // Start the conversation.
            var conversation = _client.Conversations.StartConversation();
            // Set Conversation ID
            string conversationId = conversation.ConversationId;
            //Add to dictionary
            conversationMappings.Add(clientConversationId, conversationId);
            return conversationId;
        }

        /// <summary>
        /// Getter method for the Botframework Conversation Id
        /// First, we search the dictionary, then create the conversation if it does not exists
        /// </summary>
        /// <param name="clientConversationId"> Client Session Id </param>
        /// <returns> Bot Framework Conversation Id </returns>
        private string GetConversationId(string clientConversationId)
        {
            if(conversationMappings.ContainsKey(clientConversationId))
            {
                return conversationMappings.GetValueOrDefault(clientConversationId);
            }
            else
            {
                return CreateConversation(clientConversationId);
            }
        }

        /// <summary>
        /// Sends the BotFramework a Message without a clientConversationID
        /// Only used for testing
        /// </summary>
        /// <param name="message"> Message to send </param>
        /// <returns></returns>
        public async Task SendBotMessage(string message)
        {
            await SendBotMessage(message, null);
        }

        /// <summary>
        /// Sends the BotFramework a message
        /// We await the completion of this task so the code does not get ahead of the BotFramework
        /// </summary>
        /// <param name="message"> Message to send </param>
        /// <param name="clientConversationId"> Client Session Id </param>
        /// <returns></returns>
        public async Task SendBotMessage(string message, string clientConversationId)
        {
            string conversationId = GetConversationId(clientConversationId);
            if (message.Length > 0)
            {
                // Create a message activity with the text the user entered.
                Microsoft.Bot.Connector.DirectLine.Activity userMessage = new Microsoft.Bot.Connector.DirectLine.Activity
                {
                    From = new ChannelAccount(fromUser),
                    Text = message,
                    Type = ActivityTypes.Message
                };

                // Send the message activity to the bot.
                await _client.Conversations.PostActivityAsync(conversationId, userMessage);
            }
        }
        /// <summary>
        /// Reads the BotFramework directline for messages without a clientConversationID
        /// Only used for testing
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ReadBotMessages()
        {
            return await ReadBotMessages(null);
        }

        /// <summary>
        /// Reads the BotFramework directline for messages
        /// Return: A list of the activity.text
        /// TODO: We might need more than just the activity.text here.Should look into
        /// </summary>
        /// <param name="clientConversationId"> Client Session Id </param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ReadBotMessages(string clientConversationId)
        {
            string conversationId = GetConversationId(clientConversationId);
            string watermark = null;
            List<string> messages = new List<string>();

            // Retrieve the activity set from the bot.
            var activitySet = await _client.Conversations.GetActivitiesAsync(conversationId, watermark);
            watermark = activitySet?.Watermark;

            // Extract the activies sent from our bot.
            var activities = from x in activitySet.Activities
                             where x.From.Id == _config["Bot:Id"]
                             select x;
            foreach (Microsoft.Bot.Connector.DirectLine.Activity activity in activities)
            {
                messages.Add(activity.Text);
            }

            return messages;
        }
    }
}
