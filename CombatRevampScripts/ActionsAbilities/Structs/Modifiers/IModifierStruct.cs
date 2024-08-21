using CombatRevampScripts.ActionsAbilities.ActionOrPassive;

namespace CombatRevampScripts.ActionsAbilities.Structs.Modifiers
{
    /// <summary>
    /// A storage object that can be built into a modifier of the given type
    /// </summary>
    /// <typeparam name="T">the type of modifier represented by this</typeparam>
     public interface IModifierStruct<T>
    {
        /// <summary>
        /// Builds this into the modifier it represents
        /// </summary>
        /// <param name="source">the action or passive that this modifier originates from</param>
        /// <returns>the modifier</returns>
        public T Build(IActionOrPassive source);
    }
}