using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;

namespace CombatRevampScripts.ActionsAbilities.CombatActions
{
    /// <summary>
    /// Represents a type of IEffectHolder and IActionOrPassive that triggers its ICombatEffect from AI or player inputs.
    /// </summary>
    public interface ICombatAction : IEffectHolder, IActionOrPassive
    {
        /// <summary>
        /// Replaces the current ICombatEffects of this with the given list
        /// </summary>
        /// <param name="actionEffect">the list of ICombatEffects to replace the current one</param>
        public void OverrideEffects(List<ICombatEffect> effects);

        /// <summary>
        /// If this has a Clarity Cost, returns whether or not the cost is met,
        /// otherwise returns true.
        /// </summary>
        /// <returns>whether the Clarity Cost is met, or true if there is none</returns>
        public bool IsClarityCostMet();

        /// <summary>
        /// If this has a Clarity Cost, gets the cost, otherwise returns 0.
        /// </summary>
        /// <returns>the Clarity Cost of this action, or 0 if not applicable</returns>
        public float GetClarityCost();

        /// <summary>
        /// Gets the Clarity gain of this action (the amount to increase clarity by when this is triggered)
        /// </summary>
        /// <returns>the Clarity gain</returns>
        public float GetClarityGain();

        /// <summary>
        /// Sets the Clarity gain of this action to the given value
        /// </summary>
        /// <param name="value">the gain value to set</param>
        public void SetClarityGain(float value);

        /// <summary>
        /// Gets the action type of this
        /// </summary>
        /// <returns>the ActionType of this action</returns>
        public ActionType GetActionType();

        /// <summary>
        /// Returns the range of this
        /// </summary>
        /// <returns>the int range value of this</returns>
        public int GetRange();

        /// <summary>
        /// Returns whether or not the user should be prompted to select a damage type
        /// when they select this action.
        /// </summary>
        /// <returns>whether or not they should be prompted as a boolean value</returns>
        public bool ShouldAskForDamageType();

        /// <summary>
        /// If applicable, sets the damage type of this to the given using a visitor.
        /// If targetingRangeSource is UseAttackRangeFromDamageType, updates the range
        /// of this action accordingly.
        /// </summary>
        /// <param name="damageType">the given DamageType</param>
        public void SetDamageType(DamageType damageType);

        /// <summary>
        /// Gets a list of all tiles targetable by this action.
        /// </summary>
        /// <returns> list of ITiles targetable by this</returns>
        public List<ITile> GetTargetableTiles();

        /// <summary>
        /// Performs necessary results for this CombatAction when the given tile is selected.
        /// </summary>
        /// <param name="targetTile">the ITile to select</param>
        void SelectTarget(ITile targetTile);

        /// <summary>
        /// Returns whether all the necessary targets for this ability have been selected.
        /// </summary>
        /// <returns>whether all the necessary targets for this ability have been selected</returns>
        bool AreAllTargetsSelected();

        /// <summary>
        /// Updates important info for this that might have changed since initialization.
        /// </summary>
        public void UpdateInfo();

        public void SetDamageTypeVisualVariant(DamageType damageType, EffectVisualInfo variantInfo);
    }
}
