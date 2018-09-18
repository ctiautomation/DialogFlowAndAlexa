using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Scorpio.Bot.Dialogs
{
    /// <summary>
    /// Send a welcome message with info about what the bot can do
    /// </summary>
    public class Greeting : DialogContainer
    {
        public const string Id = "Greeting";

        public static Greeting Instance { get; } = new Greeting();

        private Greeting() : base(Id)
        {
            this.Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) =>
                {
                    await dc.Context.SendActivity("Hi, I'm the E-ZPass Bot. I can answer your questions about E-ZPass or help you order a transponder. How may I help you today?",
                        "Hi, I'm the E-ZPass Bot. I can answer your questions about E-ZPass or help you order a transponder. How may I help you today?",
                        InputHints.ExpectingInput);
                    await dc.End();
                }
            });
        }
    }
}
