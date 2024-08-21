using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors
{
    public class MechHealModifierVisitor : PropertyModifierVisitor<MechHealProperty, float>
    {
        public MechHealModifierVisitor(PropertyModifier propertyModifier, PropertyModifierVisitorMode mode) : base(propertyModifier, mode) { }

        public override MechHealProperty Visit(MechHealProperty property)
        {
            ApplyToProperty(property);
            return property;
        }
    }
}
