using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for overriding the sender with the given ICombatUnit value.
    /// </summary>
    public class OverrideSenderArgs : EventArgs
    {
        public ICombatUnit OverrideSender { get; private set; }

        public OverrideSenderArgs(ICombatUnit overrideSender)
        {
            OverrideSender = overrideSender;
        }
    }
}
