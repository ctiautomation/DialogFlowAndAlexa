using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectLineAPI.Models
{
    public interface IScorpioDirectLineClient
    {
        Task<IEnumerable<string>> ReadBotMessages();
        Task<IEnumerable<string>> ReadBotMessages(string clientConversation);
        Task SendBotMessage(string message);
        Task SendBotMessage(string message, string clientConversation);
    }
}