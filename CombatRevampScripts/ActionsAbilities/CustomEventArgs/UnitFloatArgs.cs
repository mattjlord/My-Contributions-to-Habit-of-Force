using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for containing an ICombatUnit value and a float value, as well as an option
    /// to override the sender property with another ICombatUnit instead of the default.
    /// </summary>
    public class UnitFloatArgs : EventArgs
    {
        public ICombatUnit Value1 { get; private set; }
        public float Value2 { get; private set; }
        public ICombatUnit OverrideSender { get; private set; }

        public UnitFloatArgs(ICombatUnit value1, float value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public UnitFloatArgs(ICombatUnit value1, float value2, ICombatUnit overrideSender) : this(value1, value2)
        {
            OverrideSender = overrideSender;
        }
    }
}
