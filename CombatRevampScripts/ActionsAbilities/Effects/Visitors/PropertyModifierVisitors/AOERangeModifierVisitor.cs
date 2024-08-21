using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors
{
    public class AOERangeModifierVisitor : PropertyModifierVisitor<AOERangeProperty, int>
    {
        public AOERangeModifierVisitor(PropertyModifier propertyModifier, PropertyModifierVisitorMode mode) : base(propertyModifier, mode) { }

        public override AOERangeProperty Visit(AOERangeProperty property)
        {
            ApplyToProperty(property);
            return property;
        }
    }
}
