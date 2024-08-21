using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.Custom_Visitors
{
    /// <summary>
    /// VISITS: a TargetUnitProperty
    /// <para>Sets the ICombatUnit value</para>
    /// </summary>
    public class SetTargetUnitVisitor : EffectPropertyVisitor<TargetUnitProperty, ICombatUnit>
    {
        private ICombatUnit _targetUnit;

        public SetTargetUnitVisitor(ICombatUnit targetUnit)
        {
            _targetUnit = targetUnit;
        }

        public override TargetUnitProperty Visit(TargetUnitProperty property)
        {
            property.SetValue(_targetUnit);
            return property;
        }
    }
}