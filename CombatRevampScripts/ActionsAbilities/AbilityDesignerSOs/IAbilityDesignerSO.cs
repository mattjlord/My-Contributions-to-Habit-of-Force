using CombatRevampScripts.ActionsAbilities.TurnManager;

namespace CombatRevampScripts.ActionsAbilities.AbilitySOs
{
    /// <summary>
    /// Represents a specific non-serializable type and can be converted into that type with a Build function.
    /// </summary>
    /// <typeparam name="T">the non-serializable type this represents</typeparam>
    public interface IAbilityDesignerSO<T>
    {
        /// <summary>
        /// Builds this scriptable object as the class it represents (T)
        /// </summary>
        /// <param name="turnManager">the ITurnManager instance to be used as context for building this</param>
        /// <returns>the class this object represents</returns>
        public T Build(ITurnManager turnManager);
    }
}
