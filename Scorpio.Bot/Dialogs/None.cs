using Microsoft.Bot.Builder.Ai.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Linq;

namespace Scorpio.Bot.Dialogs
{
    /// <summary>
    /// Searches QnA Maker when no other intent was found
    /// </summary>
    public class None : DialogContainer
    {
        public const string Id = "None";
        
        private QnAMakerEndpoint qnaMakerEndpoint = new QnAMakerEndpoint();
        public static None Instance(QnAMakerEndpoint qnaEndpoint)
        {
            return new None(qnaEndpoint);
        }

        public None(QnAMakerEndpoint qnaEndpoint) : base(Id)
        {
            this.Dialogs.Add(Id, new WaterfallStep[]
            {
                async (dc, args, next) =>
                {
                    QnAMaker qnaMaker = new QnAMaker(qnaEndpoint);
                    if (!string.IsNullOrEmpty(dc.Context.Activity.Text))
                    {
                        var results = await qnaMaker.GetAnswers(dc.Context.Activity.Text.Trim()).ConfigureAwait(false);
                        if (results.Any())
                        {
                            await dc.Context.SendActivity(results.First().Answer, results.First().Answer, InputHints.AcceptingInput);
                        }
                        else
                        {
                            await dc.Context.SendActivity("Sorry, I don't understand.", "Sorry, I don't understand.", InputHints.AcceptingInput);
                        }
                    }
                }
            });
        }        
    }
}
