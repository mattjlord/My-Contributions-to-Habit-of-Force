using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using System.Collections.Generic;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties
{
    /// <summary>
    /// Represents an INumericProperty that also contains the functionality of a normal AEffectProperty
    /// </summary>
    /// <typeparam name="U">the type of value contained in this</typeparam>
    public abstract class ANumericProperty<U> : AEffectProperty<U>, INumericProperty<U>
    {
        protected List<PropertyModifier> propertyModifiers;

        public ANumericProperty() : base() 
        {
            propertyModifiers = new List<PropertyModifier>();
        }

        public ANumericProperty(U value) : base(value) 
        {
            propertyModifiers = new List<PropertyModifier>();
        }

        public void AddPropertyModifier(PropertyModifier propertyModifier)
        {
            propertyModifiers.Add(propertyModifier);
        }

        public void RemovePropertyModifier(PropertyModifier propertyModifier)
        {
            propertyModifiers.Remove(propertyModifier);
        }
    }
}