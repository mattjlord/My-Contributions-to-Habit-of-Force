using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for containing an ICombatUnit value and an int value, as well as an option
    /// to override the sender property with another ICombatUnit instead of the default.
    /// </summary>
    public class UnitIntArgs : EventArgs
    {
        public ICombatUnit Value1 { get; private set; }
        public int Value2 { get; private set; }
        public ICombatUnit OverrideSender { get; private set; }

        public UnitIntArgs(ICombatUnit value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public UnitIntArgs(ICombatUnit value1, int value2, ICombatUnit overrideSender) : this(value1, value2)
        {
            OverrideSender = overrideSender;
        }
    }
}
