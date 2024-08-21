using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors
{
    public class MechDamageModifierVisitor : PropertyModifierVisitor<MechDamageProperty, float>
    {
        public MechDamageModifierVisitor(PropertyModifier propertyModifier, PropertyModifierVisitorMode mode) : base(propertyModifier, mode) { }

        public override MechDamageProperty Visit(MechDamageProperty property)
        {
            ApplyToProperty(property);
            return property;
        }
    } 
}
