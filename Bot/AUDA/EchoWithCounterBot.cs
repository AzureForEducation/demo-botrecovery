using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EchoBotWithCounter.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text;

namespace Microsoft.BotBuilderSamples
{
    public class EchoWithCounterBot : IBot
    {
        private const string WelcomeText = @"Hey, we're so glad you have accepted the challange to help us understand how to get better in our services and courses. Are you ready to start?";

        private readonly EchoBotAccessors _accessors;

        private DialogSet _dialogs;

        public EchoWithCounterBot(EchoBotAccessors accessors, ILoggerFactory loggerFactory)
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            _dialogs = new DialogSet(accessors.ConversationDialogState);

            var name_email_slots = new List<SlotDetails>
            {
                new SlotDetails("completename", "text", "Please, enter your complete name."),
                new SlotDetails("mail", "text", "Please, enter with mail you use in Americas University."),
            };

            var description_suggestion_slots = new List<SlotDetails>
            {
                new SlotDetails("problemdescription", "text", "We do know you're not happy with one of the courses you've made with us. Could you explain us why? Please, be detalistic."),
                new SlotDetails("suggestiontosolve", "text", "Get it. What do you think we should do to get that aspect(s) better?"),
            };

            var confirmation_list = new List<SlotDetails>
            {
                new SlotDetails("confirmationdata", "text", "May I send that info to our quality center?"),
            };

            // Dialogs can be nested and the slot filling dialog makes use of that. In this example some of the child
            // dialogs are slot filling dialogs themselves.
            var slots = new List<SlotDetails>
            {
                new SlotDetails("namemail", "namemail"),
                new SlotDetails("descriptionsuggestion", "descriptionsuggestion"),
            };

            // Add the various dialogs that will be used to the DialogSet.
            _dialogs.Add(new SlotFillingDialog("descriptionsuggestion", description_suggestion_slots));
            _dialogs.Add(new SlotFillingDialog("namemail", name_email_slots));
            _dialogs.Add(new TextPrompt("text"));
            _dialogs.Add(new SlotFillingDialog("confirmationdata", confirmation_list));
            _dialogs.Add(new ConfirmPrompt("conf"));
            _dialogs.Add(new SlotFillingDialog("slot-dialog", slots));

            // Defines a simple two step Waterfall to test the slot dialog.
            _dialogs.Add(new WaterfallDialog("root", new WaterfallStep[] { StartDialogAsync, ProcessResultsAsync }));
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                var results = await dialogContext.ContinueDialogAsync(cancellationToken);

                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dialogContext.BeginDialogAsync("root", null, cancellationToken);
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded.Any())
                {
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} activity detected", cancellationToken: cancellationToken);
            }

            await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var reply = turnContext.Activity.CreateReply();
                    reply.Text = WelcomeText;
                    await turnContext.SendActivityAsync(reply, cancellationToken);
                }
            }
        }

        private async Task<DialogTurnResult> StartDialogAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync("slot-dialog", null, cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessResultsAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is IDictionary<string, object> result && result.Count > 0)
            {
                var completename_email = (IDictionary<string, object>)result["namemail"];
                var description_suggestion = (IDictionary<string, object>)result["descriptionsuggestion"];

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(
                    $"Here are the data you just informed: \n" +
                    $"Complete name: {completename_email["completename"]} \n " +
                    $"Mail: {completename_email["mail"]} \n" +
                    $"Problem: {description_suggestion["problemdescription"]} \n" +
                    $"Solution suggestion: {description_suggestion["suggestiontosolve"]}"), cancellationToken);
            }

            var answer = await stepContext.PromptAsync("conf", new PromptOptions { Prompt = MessageFactory.Text("Do you confirm that?") }, cancellationToken);

            // Remember to call EndAsync to indicate to the runtime that this is the end of our waterfall.
            return await stepContext.EndDialogAsync();
        }
    }
}
