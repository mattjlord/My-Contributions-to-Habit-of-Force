using CombatRevampScripts.ActionsAbilities.CombatModifiers;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties
{
    /// <summary>
    /// Represents a more complex property containing a numeric value that is modified by PropertyModifiers
    /// </summary>
    /// <typeparam name="U">the type of value contained in this</typeparam>
    public interface INumericProperty<U>
    {
        /// <summary>
        /// Adds the given modifier to this
        /// </summary>
        /// <param name="propertyModifier">the PropertyModifier to add</param>
        public void AddPropertyModifier(PropertyModifier propertyModifier);

        /// <summary>
        /// Removes the given modifier from this
        /// </summary>
        /// <param name="propertyModifier">the PropertyModifier to remove</param>
        public void RemovePropertyModifier(PropertyModifier propertyModifier);
    }
}