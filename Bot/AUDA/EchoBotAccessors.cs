// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.BotBuilderSamples
{

    public class EchoBotAccessors
    {
        public EchoBotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
        }

        public static string CounterStateName { get; } = $"{nameof(EchoBotAccessors)}.CounterState";

        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }

        public ConversationState ConversationState { get; }
    }
}
