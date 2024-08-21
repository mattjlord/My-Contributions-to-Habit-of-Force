using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatUnit;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.Custom_Visitors
{
    /// <summary>
    /// VISITS: a TargetLoUnitProperty
    /// <para>Sets the List<ICombatUnit> value</para>
    /// </summary>
    public class SetTargetLoUnitVisitor : EffectPropertyVisitor<Properties.Custom_Effect_Properties.TargetLoUnitProperty, List<ICombatUnit>>
    {
        private List<ICombatUnit> _targetUnits;

        public SetTargetLoUnitVisitor(List<ICombatUnit> targetUnits)
        {
            _targetUnits = targetUnits;
        }

        public override Properties.Custom_Effect_Properties.TargetLoUnitProperty Visit(Properties.Custom_Effect_Properties.TargetLoUnitProperty property)
        {
            property.SetValue(_targetUnits);
            return property;
        }
    }
}