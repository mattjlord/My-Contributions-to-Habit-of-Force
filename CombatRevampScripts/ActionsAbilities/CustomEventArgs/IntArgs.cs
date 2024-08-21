using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for containing a single int Value, as well as an option
    /// to override the sender of this to an ICombatUnit.
    /// </summary>
    public class IntArgs : EventArgs
    {
        public int Value { get; private set; }
        public ICombatUnit OverrideSender { get; private set; }

        public IntArgs(int value)
        {
            Value = value;
        }

        public IntArgs(int value, ICombatUnit overrideSender) : this(value)
        {
            OverrideSender = overrideSender;
        }
    }
}
