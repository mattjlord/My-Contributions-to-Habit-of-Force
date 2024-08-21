using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Abilities.PassiveAbilities
{
    /// <summary>
    /// Represents an IAbility that uses ICombatEffectTriggers to trigger ICombatEffects.
    /// </summary>
    public interface IPassiveAbility : IAbility
    {
        /// <summary>
        /// Sets the ICombatUnit of this to the given one
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit to set</param>
        public void SetCombatUnit(ICombatUnit combatUnit);

        /// <summary>
        /// Adds the given effect trigger to this ability's list of effectTriggers
        /// </summary>
        /// <param name="effect trigger">the IEffectTrigger to add</param>
        public void AddEffectTrigger(IEffectTrigger effectTrigger);

        /// <summary>
        /// Modifies the effectTriggers of this with the given visitor
        /// </summary>
        /// <param name="visitor">the IEffectPropertyVisitor to use</param>
        public void ModifyEffectTriggers<T, U>(IEffectPropertyVisitor<T, U> visitor);
    }
}
