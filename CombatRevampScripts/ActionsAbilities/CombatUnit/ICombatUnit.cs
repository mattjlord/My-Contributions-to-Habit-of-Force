using CombatRevampScripts.ActionsAbilities.Abilities.ActivatedAbilities;
using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.AOEBoardUtils;
using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using System.Collections.Generic;
using CombatRevampScripts.CombatVisuals.Handler;
using System;

namespace CombatRevampScripts.ActionsAbilities.CombatUnit
{
    /// <summary>
    /// Represents a participant in a combat encounter, controlled by either a player or AI.
    /// </summary>
    public interface ICombatUnit : IAOEBoardUtils
    {
        /// <summary>
        /// Gets the MechTilePiece of this unit
        /// </summary>
        /// <returns>the MechTilePiece of this unit</returns>
        public MechTilePiece GetTilePiece();

        public MechSO GetMechSO();

        public PilotSO GetPilotSO();

        /// <summary>
        /// Sets the turn manager of this
        /// </summary>
        /// <param name="turnManager">the ITurnManager of the current combat encounter</param>
        public void SetTurnManager(ITurnManager turnManager);

        /// <summary>
        /// Gets the turn manager of this
        /// </summary>
        /// <param name="turnManager">the ITurnManager of this</param>
        public ITurnManager GetTurnManager();

        /// <summary>
        /// Gets the IActionEconomy of this unit
        /// </summary>
        /// <returns>the IActionEconomy of this unit</returns>
        public IActionEconomy GetActionEconomy();

        /// <summary>
        /// Gets the combat visual handler for this unit.
        /// </summary>
        /// <returns>the handler</returns>
        public ICombatVisualHandler GetVisualHandler();

        /// <summary>
        /// Sets the busy status of this unit to the given value.
        /// </summary>
        /// <param name="value">the value to set for busy</param>
        public void SetBusyStatus(bool value);

        /// <summary>
        /// Returns the busy status of this unit
        /// </summary>.
        /// <returns>the busy status of this unit</returns>
        public bool IsBusy();

        /// <summary>
        /// Gets a list of actions belonging to this unit
        /// </summary>
        /// <returns>a list of ICombatActions belonging to this unit</returns>
        public List<ICombatAction> GetActions();

        /// <summary>
        /// Gets a list of all actions that can be legally performed by this unit currently.
        /// </summary>
        /// <returns>a list of ICombatActions that are legal</returns>
        public List<ICombatAction> GetLegalActions();

        public ICombatAction GetMoveAction();

        public ICombatAction GetAttackAction();

        public ICombatAction GetDefendAction();

        /// <summary>
        /// Is the action economy for this unit empty?
        /// </summary>
        /// <returns>whether the action economy is empty</returns>
        public bool IsActionEconomyEmpty();

        /// <summary>
        /// Adds the given action to this unit and sets its ICombatUnit variable to this, if it isn't already present,
        /// otherwise does nothing
        /// </summary>
        /// <param name="action">the ICombatAction to add</param>
        public void AddAction(ICombatAction action);

        /// <summary>
        /// Adds the given activated ability to this unit, if it isn't already present in this unit's IActivatedAbility dictionary,
        /// otherwise does nothing.
        /// </summary>
        /// <param name="activatedAbility">the ActivatedAbility to add</param>
        public void AddActivatedAbility(ActivatedAbility activatedAbility);

        /// <summary>
        /// Adds the given combat passive to this unit's list of assigned passives, as well as the owner unit's list of owned passives,
        /// if it isn't already present, otherwise replaces the existing one.
        /// </summary>
        /// <param name="passive">the ICombatPassive to add</param>
        /// <param name="owner">the ICombatUnit that owns this passive</param>
        /// <param name="allowStacking">can this Passive stack with other assigned Passives that have the same name (true), or should it replace
        /// assigned Passives that share its name (false)</param>
        public void AddAssignedPassive(ICombatPassive passive, ICombatUnit owner, bool allowStacking);

        /// <summary>
        /// Adds the given combat passive to this unit's list of owned passives, if it isn't already present, otherwise replaces the
        /// existing one.
        /// </summary>
        /// <param name="passive"></param>
        public void AddOwnedPassive(ICombatPassive passive);

        /// <summary>
        /// Removes the given combat passive from this unit's list of assigned passives if it can find it, and removes it from
        /// the owner as well,
        /// otherwise does nothing.
        /// </summary>
        /// <param name="passive">the ICombatPassive to remove</param>
        public void RemoveAssignedPassive(ICombatPassive passive);

        /// <summary>
        /// Removes the given combat passive from this unit's list of owned passives if it can find it,
        /// otherwise does nothing.
        /// </summary>
        /// <param name="passive">the ICombatPassive to remove</param>
        public void RemoveOwnedPassive(ICombatPassive passive);

        /// <summary>
        /// Adds the given modifier to this unit and initiates setup functions on that modifier
        /// </summary>
        /// <param name="modifier">the modifier to add</param>
        public void AddModifier(ICombatModifier modifier);

        /// <summary>
        /// Removes the given modifier from this (all other removal-related functionality is handled by the modifier).
        /// </summary>
        /// <param name="modifier">the modifier to remove</param>
        public void RemoveModifier(ICombatModifier modifier);

        /// <summary>
        /// Modifies the effects of all actions and owned passives of this unit with the given visitor
        /// </summary>
        /// <param name="visitor">the visitor to accept</param>
        public void ModifyOwnedEffects<T, U>(IEffectPropertyVisitor<T, U> visitor);

        /// <summary>
        /// Modifies the effects of all actions and passives of this unit with the given visitor
        /// </summary>
        /// <param name="visitor">the visitor to accept</param>
        public void ModifyAllEffects<T, U>(IEffectPropertyVisitor<T, U> visitor);

        /// <summary>
        /// Overrides the Effects of this unit's Move action.
        /// </summary>
        /// <param name="effects">the ICombatEffects to replace the default effects with</param>
        public void OverrideMoveEffects(List<ICombatEffect> effects);

        /// <summary>
        /// Overrides the Effects of this unit's Attack action.
        /// </summary>
        /// <param name="effects">the ICombatEffects to replace the default effects with</param>
        public void OverrideAttackEffects(List<ICombatEffect> effects);

        /// <summary>
        /// Overrides the Effects of this unit's Defend action.
        /// </summary>
        /// <param name="effects">the ICombatEffects to replace the default effects with</param>
        public void OverrideDefendEffects(List<ICombatEffect> effects);

        /// <summary>
        /// Returns whether or not an action with the given name can be done by this unit.
        /// </summary>
        /// <param name="actionName">the name of the action to check</param>
        /// <returns>whether an action with the given name can be done by this unit</returns>
        public bool CanDoAction(string actionName);

        /// <summary>
        /// Returns whether the given action can be done by this unit
        /// </summary>
        /// <param name="action">the ICombatAction to test</param>
        /// <returns>whether the action can be done</returns>
        public bool CanDoAction(ICombatAction action);

        /// <summary>
        /// Performs the effect of the action with the given name.
        /// NOTE: This does not check whether the action can actually be performed.
        /// </summary>
        /// <param name="actionName">the name of the action to do</param>
        public void DoAction(string actionName);

        /// <summary>
        /// Does the given action
        /// </summary>
        /// <param name="action">the ICombatAction to perform</param>
        public void DoAction(ICombatAction action);

        /// <summary>
        /// Does the given action, then executes the given callback on completion
        /// </summary>
        /// <param name="action">the ICombatAction to perform</param>
        /// <param name="callbackAction">the callback action to perform on completion</param>
        public void DoAction(ICombatAction action, Action callbackAction);

        /// <summary>
        /// Adds the given turn action to the IActionEconomy of this.
        /// </summary>
        /// <param name="turnAction"></param>
        public void AddTurnAction(IActionEconomyToken turnAction);

        /// <summary>
        /// Performs any necessary effects for the start of this unit's turn
        /// </summary>
        public void OnTurnStart();

        /// <summary>
        /// Performs any necessary effects for the end of this unit's turn
        /// </summary>
        public void OnTurnEnd();

        /// <summary>
        /// Moves this unit to the given tile.
        /// </summary>
        /// <param name="targetTile">the ITile to move to</param>
        /// <param name="range">the max range of the move</param>
        public void MoveTo(ITile targetTile, int range);

        /// <summary>
        /// Moves this unit to the given tile., and performs the given action
        /// once the destination is reached.
        /// </summary>
        /// <param name="targetTile">the ITile to move to</param>
        /// <param name="range">the max range of the move</param>
        /// <param name="actionOnComplete">the System.Action to perform on completion</param>
        public void MoveTo(ITile targetTile, int range, Action actionOnComplete);

        public void AddClarity(float value);

        /// <summary>
        /// Applies the given damage to this unit's MechSO, calls the corresponding
        /// "OnDo..." method on the source.
        /// </summary>
        /// <param name="damage">the float value of the damage</param>
        /// <param name="source">the ICombatUnit that dealt this damage</param>
        /// <returns>the value of the damage dealt.</returns>
        public float DamageMech(float damage, ICombatUnit source);

        /// <summary>
        /// Applies the given damage to this unit's PilotSO, calls the corresponding
        /// "OnDo..." method on the source.
        /// </summary>
        /// <param name="damage">the float value of the damage</param>
        /// <param name="source">the ICombatUnit that dealt this damage</param>
        /// <returns>the value of the damage dealt.</returns>
        public float DamagePilot(float damage, ICombatUnit source);

        /// <summary>
        /// Applies the given healing to this unit's MechSO, calls the corresponding
        /// "OnDo..." method on the source.
        /// </summary>
        /// <param name="healing">the float value of the healing</param>
        /// <param name="source">the ICombatUnit that dealt this damage</param>
        /// <returns>the value of the healing dealt.</returns>
        public float HealMech(float healing, ICombatUnit source);

        /// <summary>
        /// Applies the given healing to this unit's PilotSO, calls the corresponding
        /// "OnDo..." method on the source.
        /// </summary>
        /// <param name="healing">the float value of the healing</param>
        /// <param name="source">the ICombatUnit that dealt this damage</param>
        /// <returns>the value of the healing dealt.</returns>
        public float HealPilot(float healing, ICombatUnit source);

        /// <summary>
        /// Adds the given value to the health of this unit's MechSO, without invoking an event.
        /// </summary>
        /// <param name="value">the value to add</param>
        /// <param name="affectTempHealth">if this unit's MechSO has tempHealth, should that be modified instead of currHealth?</param>
        public void AddMechHealth(float value, bool affectTempHealth);

        /// <summary>
        /// Adds the given value to the health of this unit's PilotSO, without invoking an event.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="affectTempHealth">if this unit's PilotSO has tempHealth, should that be modified instead of currHealth?</param>
        public void AddPilotHealth(float value, bool affectTempHealth);

        /// <summary>
        /// Adds the given value to this unit's Mech's temp health
        /// </summary>
        /// <param name="value">the value to add</param>
        public void AddMechTempHealth(float value);

        /// <summary>
        /// Adds the given value to this unit's Pilot's temp health
        /// </summary>
        /// <param name="value">the value to add</param>
        public void AddPilotTempHealth(float value);

        /// <summary>
        /// Sets the unit's Mech's temp health to the given value
        /// </summary>
        /// <param name="value">the value to set</param>
        public void SetMechTempHealth(float value);

        /// <summary>
        /// Sets the unit's Pilot's temp health to the given value
        /// </summary>
        /// <param name="value">the value to set</param>
        public void SetPilotTempHealth(float value);

        /// <summary>
        /// Kills the Mech of this unit, invoking an "On Kill Mech" event on the given unit.
        /// </summary>
        /// <param name="source">the unit that killed this Mech</param>
        public void KillMech(ICombatUnit source);

        /// <summary>
        /// Kills the Pilot of this unit, invoking an "On Kill Pilot" event on the given unit.
        /// </summary>
        /// <param name="source">the unit that killed this Pilot</param>
        public void KillPilot(ICombatUnit source);

        /// <summary>
        /// Kills the Mech of this unit, without invoking any events on units other than this one.
        /// </summary>
        public void KillMech();

        /// <summary>
        /// Kills the Pilot of this unit, without invoking any events on units other than this one.
        /// </summary>
        public void KillPilot();

        /// <summary>
        /// Invokes an event on this unit for damaging a Mech, providing the given value
        /// as a custom FloatArgs.
        /// </summary>
        /// <param name="target">the target of the damage</param>
        /// <param name="value">the value to provide to the FloatArgs</param>
        public void OnDoMechDamage(ICombatUnit target, float value);

        /// <summary>
        /// Invokes an event on this unit for damaging a Pilot, providing the given value
        /// as a custom FloatArgs.
        /// </summary>
        /// <param name="target">the target of the damage</param>
        /// <param name="value">the value to provide to the FloatArgs</param>
        public void OnDoPilotDamage(ICombatUnit target, float value);

        /// <summary>
        /// Invokes an event on this unit for healing a Mech, providing the given value
        /// as a custom FloatArgs.
        /// </summary>
        /// <param name="target">the target of the healing</param>
        /// <param name="value">the value to provide to the FloatArgs</param>
        public void OnDoMechHealing(ICombatUnit target, float value);

        /// <summary>
        /// Invokes an event on this unit for healing a Pilot, providing the given value
        /// as a custom FloatArgs.
        /// </summary>
        /// <param name="target">the target of the healing</param>
        /// <param name="value">the value to provide to the FloatArgs</param>
        public void OnDoPilotHealing(ICombatUnit target, float value);

        /// <summary>
        /// Invokes an event on this unit for killing the Mech of the given unit, providing the given
        /// unit as a custom UnitArgs.
        /// </summary>
        /// <param name="unit">the unit whose Mech was killed</param>
        public void OnKillUnitMech(ICombatUnit unit);

        /// <summary>
        /// Invokes an event on this unit for killing the Pilot of the given unit, providing the given
        /// unit as a custom UnitArgs.
        /// </summary>
        /// <param name="unit">the unit whose Pilot was killed</param>
        public void OnKillUnitPilot(ICombatUnit unit);

        public void OnTileEntered(ITile tile);

        public void OnTileExited(ITile tile);

        /// <summary>
        /// checks if the combat unit is dead
        /// </summary>
        /// <returns>true if daed, false otherwise</returns>
        public bool IsDead();

        public float InitializeInitiative();

        public float GetInitiative();
    }
}
