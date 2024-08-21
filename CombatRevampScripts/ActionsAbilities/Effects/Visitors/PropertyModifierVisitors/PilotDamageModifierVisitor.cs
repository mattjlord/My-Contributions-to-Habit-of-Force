using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors
{
    public class PilotDamageModifierVisitor : PropertyModifierVisitor<PilotDamageProperty, float>
    {
        public PilotDamageModifierVisitor(PropertyModifier propertyModifier, PropertyModifierVisitorMode mode) : base(propertyModifier, mode) { }

        public override PilotDamageProperty Visit(PilotDamageProperty property)
        {
            ApplyToProperty(property);
            return property;
        }
    }
}
