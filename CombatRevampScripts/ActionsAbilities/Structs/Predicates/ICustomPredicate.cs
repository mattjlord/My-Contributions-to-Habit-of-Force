using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;

namespace CombatRevampScripts.ActionsAbilities.Structs.Predicates
{
    /// <summary>
    /// Represents a serializable set of conditions that are tested on a specific kind of object. Belongs to an ICombatUnit.
    /// </summary>
    /// <typeparam name="T">the type of object to test</typeparam>
    public interface ICustomPredicate<T>
    {
        /// <summary>
        /// Tests this predicate on the given object
        /// </summary>
        /// <param name="obj">the object of type T to test</param>
        /// <returns>the result of the test</returns>
        public bool Test(T obj);

        /// <summary>
        /// Sets the unit that this Predicate belongs to
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit this belongs to</param>
        public void SetOwnerUnit(ICombatUnit combatUnit);

        /// <summary>
        /// Sets the EffectHolder that is using this Predicate
        /// </summary>
        /// <param name="effectHolder">the IEffectHolder this belongs to</param>
        public void SetEffectHolder(IEffectHolder effectHolder);
    }
}
