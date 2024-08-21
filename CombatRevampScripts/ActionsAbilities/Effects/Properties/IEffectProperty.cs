using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties
{
    /// <summary>
    /// Represents a public value attached to an ICombatEffect that can be modified by IEffectPropertyVisitors.
    /// </summary>
    /// <typeparam name="U">the type of value contained in this</typeparam>
    public interface IEffectProperty<U>
    {
        /// <summary>
        /// Accepts the given visitor
        /// </summary>
        /// <param name="visitor">the visitor to accept</param>
        /// <returns>a generic value determined by the visitor</returns>
        public T Accept<T>(IEffectPropertyVisitor<T, U> visitor);

        /// <summary>
        /// Gets the value of this property
        /// </summary>
        /// <returns>the value of this property</returns>
        public U GetValue();

        /// <summary>
        /// Sets the value of this property to the given one.
        /// If this property has numeric modifiers that can be applied to it, this method
        /// modifies only the base value, without removing the modifiers.
        /// </summary>
        /// <param name="value">the value to set</param>
        public void SetValue(U value);
    }
}
