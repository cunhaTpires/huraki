using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LuisBot.Dialogs
{
    [Serializable]
    [QnAMakerService("9ac0df13b6354d4fafe1583a5bc59b95", "5bd00e82-a8de-4584-aca1-ed0c27e21ae0")]
    public class QnADialog : QnAMakerDialog<object>
    {
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync(string.Format("Sorry, I couldn't find an answer for **{0}**", originalQueryText));
            context.Wait(MessageReceived);
        }

        [QnAMakerResponseHandler(50)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            await context.PostAsync(string.Format("I found an answer that might help... {0}, {1}", result.Score, result.Answer));
            context.Wait(MessageReceived);
        }
    }
}