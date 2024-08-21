using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.ActionsAbilities.Effects;

namespace CombatRevampScripts.ActionsAbilities.CombatPassives
{
    /// <summary>
    /// Represents an object on a CombatUnit that contains effectTriggers that are triggered by
    /// various events.
    /// </summary>
    public interface ICombatPassive : IActionOrPassive, ITurnTimer
    {
        /// <summary>
        /// Sets the assignee unit of this to the given one (this is the unit that is affected by this).
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit to set</param>
        public void SetAssignee(ICombatUnit combatUnit);

        /// <summary>
        /// Sets the owner unit of this to the given one.
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit to set</param>
        public void SetOwner(ICombatUnit combatUnit);

        /// <summary>
        /// Gets the assignee unit of this.
        /// </summary>
        /// <returns>the assignee unit</returns>
        public ICombatUnit GetAssignee();

        /// <summary>
        /// Gets the owner unit of this
        /// </summary>
        /// <returns>the owner unit</returns>
        public ICombatUnit GetOwner();

        /// <summary>
        /// Adds the given effect trigger to this list of effectTriggers.
        /// </summary>
        /// <param name="effect trigger">the IEffectTrigger to add</param>
        public void AddEffectTrigger(IEffectTrigger effectTrigger);

        /// <summary>
        /// Modifies the effectTriggers of this with the given visitor.
        /// </summary>
        /// <param name="visitor">the IEffectPropertyVisitor to use</param>
        public void ModifyEffectTriggers<T, U>(IEffectPropertyVisitor<T, U> visitor);

        /// <summary>
        /// Expends a charge of this passive
        /// </summary>
        public void ExpendCharge();

        /// <summary>
        /// Subscribes all relevant EffectTriggers to the given list of units.
        /// </summary>
        /// <param name="units">the list of units to subscribe to</param>
        public void SubscribeToUnits(List<ICombatUnit> units);

        /// <summary>
        /// Subscribes all relevant EffectTriggers to the given unit.
        /// </summary>
        /// <param name="unit">the unit to subscribe to</param>
        public void SubscribeToUnit(ICombatUnit unit);

        /// <summary>
        /// Subscribes all relevant EffectTriggers to the given list of tiles.
        /// </summary>
        /// <param name="tiles">the list of tiles to subscribe to</param>
        public void SubscribeToTiles(List<ITile> tiles);

        /// <summary>
        /// Subscribes all relevant EffectTriggers to the given tile.
        /// </summary>
        /// <param name="tile">the tile to subscribe to</param>
        public void SubscribeToTile(ITile tile);

        /// <summary>
        /// Sets the visual info of this to the given
        /// </summary>
        /// <param name="info">the passive visual info to set</param>
        public void SetVisualInfo(PassiveVisualInfo info);

        /// <summary>
        /// Adds the given release effect to this passive
        /// </summary>
        /// <param name="effect">the effect to add</param>
        public void AddReleaseEffect(ICombatEffect effect);
    }
}