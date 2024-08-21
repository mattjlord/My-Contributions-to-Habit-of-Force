using CombatRevampScripts.ActionsAbilities.ActionOrPassive;

namespace CombatRevampScripts.ActionsAbilities.Abilities
{
    /// <summary>
    /// Represents an Activated or Passive Ability with an AbilityType
    /// </summary>
    public interface IAbility : IActionOrPassive
    {
        /// <summary>
        /// Gets the AbilityType of this, either MechAbility or PilotAbility
        /// </summary>
        /// <returns>the AbilityType of this</returns>
        public AbilityType GetAbilityType();

        /// <summary>
        /// Sets this AbilityType to the given
        /// </summary>
        /// <param name="abilityType">the AbilityType to set</param>
        public void SetAbilityType(AbilityType abilityType);
    }
}
