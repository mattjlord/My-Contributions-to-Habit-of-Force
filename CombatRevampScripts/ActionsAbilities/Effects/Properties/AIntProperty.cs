using CombatRevampScripts.ActionsAbilities.CombatModifiers;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties
{
    /// <summary>
    /// Represents a numeric property containing an int value
    /// </summary>
    public abstract class AIntProperty : ANumericProperty<int>
    {
        public AIntProperty() : base() { }

        public AIntProperty(int value) : base(value) { }

        public override int GetValue()
        {
            float valueAsFloat = value;

            foreach (PropertyModifier modifier in propertyModifiers)
            {
                valueAsFloat = modifier.ApplyToValue(valueAsFloat);
            }

            if (valueAsFloat < 0f) { valueAsFloat = 0f; }

            return (int)valueAsFloat;
        }
    }
}