using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.TurnTimer;

namespace CombatRevampScripts.ActionsAbilities.CombatModifiers
{
    /// <summary>
    /// Represents a modifier to a numerical value in combat that can also use a turn timer
    /// </summary>
    public interface ICombatModifier : ITurnTimer
    {
        /// <summary>
        /// Gets the value of this
        /// </summary>
        /// <returns>the float value of this</returns>
        public float GetModifierValue();

        /// <summary>
        /// Gets the source action or passive of this
        /// </summary>
        /// <returns>the source of this</returns>
        public IActionOrPassive GetModifierSource();

        /// <summary>
        /// Gets the type (either add or multiply) of this
        /// </summary>
        /// <returns>the type of this</returns>
        public ModifierType GetModifierType();

        public float ApplyToValue(float value);

        /// <summary>
        /// Sets up this modifier on the given unit
        /// </summary>
        /// <param name="combatUnit">the unit to set up for</param>
        public void SetupUnit(ICombatUnit combatUnit);

        /// <summary>
        /// Removes this modifier from combat
        /// </summary>
        public void RemoveModifier();
    }
}