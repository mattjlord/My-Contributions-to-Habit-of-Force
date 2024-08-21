using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Effects.Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors
{
    public enum PropertyModifierVisitorMode
    {
        AddModifier,
        RemoveModifier
    }

    public class PropertyModifierVisitor<T, U> : EffectPropertyVisitor<T, U>
    {
        private PropertyModifier _propertyModifier;
        private PropertyModifierVisitorMode _mode;

        public PropertyModifierVisitor(PropertyModifier propertyModifier, PropertyModifierVisitorMode mode)
        {
            _propertyModifier = propertyModifier;
            _mode = mode;
        }

        internal void ApplyToProperty(INumericProperty<U> property)
        {
            switch (_mode)
            {
                case PropertyModifierVisitorMode.AddModifier:
                    property.AddPropertyModifier(_propertyModifier);
                    break;
                case PropertyModifierVisitorMode.RemoveModifier:
                    property.RemovePropertyModifier(_propertyModifier);
                    break;
                default:
                    break;
            }
        }
    }
}