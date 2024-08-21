using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors
{
    public class CombatUnitEventArgVisitor : EffectPropertyVisitor<CombatUnitEventArgProperty, ICombatUnit>
    {
        private ICombatUnit _value;

        public CombatUnitEventArgVisitor(ICombatUnit value)
        {
            _value = value;
        }

        public override CombatUnitEventArgProperty Visit(CombatUnitEventArgProperty property)
        {
            return SetValue(property, _value);
        }
    }
}
