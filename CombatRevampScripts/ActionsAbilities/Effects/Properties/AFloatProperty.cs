using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using System;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties
{
    /// <summary>
    /// Represents a numeric property containing a float value
    /// </summary>
    public abstract class AFloatProperty : ANumericProperty<float>
    {
        public AFloatProperty() : base() { }

        public AFloatProperty(float value) : base(value) { }

        public override float GetValue()
        {
            float copyValue = value;

            foreach (PropertyModifier modifier in propertyModifiers)
            {
                copyValue = modifier.ApplyToValue(copyValue);
            }

            if (copyValue < 0f) { copyValue = 0f; }

            return copyValue;
        }
    }
}