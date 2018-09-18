using System.Collections.Generic;

namespace Scorpio.Bot.Models
{
    public class ConversationModel
    {
        public string Purpose { get; set; }
        public AuthenticationTypes AuthenticationType { get; set; }
        public List<string> Utterances { get; set; }
        public List<string> Entities { get; set; }
        public List<Mapper> Mappers { get; set; }
        public List<Prompt> Prompts { get; set; }
        public Outcome Outcome { get; set; }
    }
}
