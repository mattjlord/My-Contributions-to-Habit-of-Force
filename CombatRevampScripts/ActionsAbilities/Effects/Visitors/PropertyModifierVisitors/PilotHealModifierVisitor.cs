using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors
{
    public class PilotHealModifierVisitor : PropertyModifierVisitor<PilotHealProperty, float>
    {
        public PilotHealModifierVisitor(PropertyModifier propertyModifier, PropertyModifierVisitorMode mode) : base(propertyModifier, mode) { }

        public override PilotHealProperty Visit(PilotHealProperty property)
        {
            ApplyToProperty(property);
            return property;
        }
    }
}
