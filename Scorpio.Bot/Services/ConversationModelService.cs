using Newtonsoft.Json;
using Scorpio.Bot.Models;
using System.IO;
using System.Linq;

namespace Scorpio.Bot.Services
{
    public class ConversationModelService
    {
        /// <summary>
        /// Store for the deserialized <c>ConversationModel</c>s.
        /// </summary>
        private static ConversationModel _conversationModel;

        /// <summary>
        /// Deserializes and stores the <c>ConversationModel</c>s from ConversationModel.json
        /// </summary>
        /// <returns>Returns the <c>ConversationModel</c>s deserialized from ConversationModel.json.</returns>
        public ConversationModel ConversationModel()
        {
            if (_conversationModel == null)
            {
                Load();
            }

            return _conversationModel;
        }

        public void Load()
        {
            var conversationModelData = File.ReadAllText(@"Assets/ConversationModel.json");
            _conversationModel = JsonConvert.DeserializeObject<ConversationModel>(conversationModelData);
        }
        
        public Mapper GetMapper(string entity)
        {
            return _conversationModel.Mappers.Where(m => m.EntityName == entity).FirstOrDefault();
        }

        public Prompt GetPrompt(string entity)
        {
            return _conversationModel.Prompts.Where(p => p.EntityName == entity).FirstOrDefault();
        }
    }
}
