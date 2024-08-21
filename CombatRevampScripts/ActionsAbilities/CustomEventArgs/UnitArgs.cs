using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for containing a single ICombatUnit Value, as well as an option to
    /// override the sender property with another ICombatUnit instead of the default.
    /// </summary>
    public class UnitArgs : EventArgs
    {
        public ICombatUnit Value { get; private set; }
        public ICombatUnit OverrideSender { get; private set; }

        public UnitArgs(ICombatUnit value)
        {
            Value = value;
        }

        public UnitArgs(ICombatUnit value, ICombatUnit overrideSender) : this(value)
        {
            OverrideSender = overrideSender;
        }
    }
}
