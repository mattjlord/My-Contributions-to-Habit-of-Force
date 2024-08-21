using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for containing a single float Value, as well as an option
    /// to override the sender of this to an ICombatUnit.
    /// </summary>
    public class FloatArgs : EventArgs
    {
        public float Value { get; private set; }
        public ICombatUnit OverrideSender { get; private set; }

        public FloatArgs(float value)
        {
            Value = value;
        }

        public FloatArgs(float value, ICombatUnit overrideSender) : this(value)
        {
            OverrideSender = overrideSender;
        }
    }
}
