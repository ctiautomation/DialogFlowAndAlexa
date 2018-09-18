using Microsoft.Bot.Builder.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Scorpio.Bot.Services
{
    public class LuisEntityToPropertyMapper
    {
        /// <summary>
        /// Method to save the user's inputs to the conversation state
        /// which were found as entities in LUIS.
        /// </summary>
        /// <param name="propertyName">The property name of the conversation prompt</param>
        /// <param name="entityName">The name of the entity in the conversation prompt</param>
        /// <param name="state">The ConversationState to save the inputs to.</param>
        /// <param name="luisResult">The LUIS result to get the inputs from.</param>
        public void Map(string propertyName, string entityName, Dictionary<string, object> state, RecognizerResult luisResult)
        {
            foreach (var entity in luisResult.Entities)
            {
                // If the entity found in LUIS equals the one in the conversation model
                if (entityName.Equals(entity.Key))
                {
                    // Add the user's input of that entity to the conversation state
                    state.TryAdd(propertyName, entity.Value.FirstOrDefault().FirstOrDefault().ToString());
                }
            }
        }
    }
}
