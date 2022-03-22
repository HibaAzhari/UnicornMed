using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace UnicornMed.BotLibrary.Dialogs
{
    public class NewBookingDialog : Dialog
    {
        private const string SlotName = "slot";
        private const string PersistedValues = "values";
        private readonly List<BookingDetails> _details;

        public NewBookingDialog(string dialogId, List<BookingDetails> details)
            : base(dialogId)
        {
            _details = details ?? throw new ArgumentNullException(nameof(details));
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dialogContext, object options = null, CancellationToken cancellationToken = default)
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            // Don't do anything for non-message activities.
            if (dialogContext.Context.Activity.Type != ActivityTypes.Message)
            {
                return await dialogContext.EndDialogAsync(new Dictionary<string, object>(), cancellationToken);
            }

            // Run prompt
            return await RunPromptAsync(dialogContext, cancellationToken);
        }

        private Task<DialogTurnResult> RunPromptAsync(DialogContext dialogContext, CancellationToken cancellationToken)
        {
            var state = GetPersistedValues(dialogContext.ActiveDialog);

            // Run through the list of slots until we find one that hasn't been filled yet.
            var unfilledSlot = _details.FirstOrDefault((item) => !state.ContainsKey(item.Name));

            // If we have an unfilled slot we will try to fill it
            if (unfilledSlot != null)
            {
                // The name of the slot we will be prompting to fill.
                dialogContext.ActiveDialog.State[SlotName] = unfilledSlot.Name;

                // If the slot contains prompt text create the PromptOptions.

                // Run the child dialog
                return dialogContext.BeginDialogAsync(unfilledSlot.DialogId, unfilledSlot.Options, cancellationToken);
            }
            else
            {
                // No more slots to fill so end the dialog.
                return dialogContext.EndDialogAsync(state, cancellationToken);
            }
        }

        private static IDictionary<string, object> GetPersistedValues(DialogInstance activeDialog)
        {
            throw new NotImplementedException();
        }
    }
}
