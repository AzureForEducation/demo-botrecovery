using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace EchoBotWithCounter.Dialogs
{
    public class SlotFillingDialog : Dialog
    {
        private const string SlotName = "slot";
        private const string PersistedValues = "values";
        private readonly List<SlotDetails> _slots;

        public SlotFillingDialog(string dialogId, List<SlotDetails> slots)
            : base(dialogId)
        {
            _slots = slots ?? throw new ArgumentNullException(nameof(slots));
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dialogContext, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            if (dialogContext.Context.Activity.Type != ActivityTypes.Message)
            {
                return await dialogContext.EndDialogAsync(new Dictionary<string, object>());
            }

            return await RunPromptAsync(dialogContext, cancellationToken);
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            if (dialogContext.Context.Activity.Type != ActivityTypes.Message)
            {
                return EndOfTurn;
            }

            return await RunPromptAsync(dialogContext, cancellationToken);
        }

        public override async Task<DialogTurnResult> ResumeDialogAsync(DialogContext dialogContext, DialogReason reason, object result, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            var slotName = (string)dialogContext.ActiveDialog.State[SlotName];
            var values = GetPersistedValues(dialogContext.ActiveDialog);
            values[slotName] = result;

            return await RunPromptAsync(dialogContext, cancellationToken);
        }

        private static IDictionary<string, object> GetPersistedValues(DialogInstance dialogInstance)
        {
            object obj;
            if (!dialogInstance.State.TryGetValue(PersistedValues, out obj))
            {
                obj = new Dictionary<string, object>();
                dialogInstance.State.Add(PersistedValues, obj);
            }

            return (IDictionary<string, object>)obj;
        }

        private Task<DialogTurnResult> RunPromptAsync(DialogContext dialogContext, CancellationToken cancellationToken)
        {
            var state = GetPersistedValues(dialogContext.ActiveDialog);

            var unfilledSlot = _slots.FirstOrDefault((item) => !state.ContainsKey(item.Name));

            if (unfilledSlot != null)
            {
                dialogContext.ActiveDialog.State[SlotName] = unfilledSlot.Name;

                return dialogContext.BeginDialogAsync(unfilledSlot.DialogId, unfilledSlot.Options, cancellationToken);
            }
            else
            {
                return dialogContext.EndDialogAsync(state);
            }
        }
    }
}
