using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Structs.Events;
using CombatRevampScripts.ActionsAbilities.Structs.Predicates;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.EffectTriggers
{
    /// <summary>
    /// Represents a type of IEffectHolder that listens to Events and uses a PredicateSO<ICombatUnit> to filter them in order to trigger its ICombatEffect.
    /// </summary>
    public interface IEffectTrigger : IEffectHolder
    {
        /// <summary>
        /// Sets the owner unit of this effect trigger's effect to the given one.
        /// </summary>
        /// <param name="owner">the owner unit to set</param>
        public void SetEffectOwner(ICombatUnit owner);

        /// <summary>
        /// Sets the source of this effect trigger's effect to the given one.
        /// </summary>
        /// <param name="source">the action or passive to set</param>
        public void SetEffectSource(IActionOrPassive source);

        /// <summary>
        /// Sets the ongoing VFX of this, which plays as long as this EffectTrigger is active
        /// </summary>
        /// <param name="vfxObject">the VFX object to set</param>
        public void SetOngoingVFX(GameObject vfxObject);

        /// <summary>
        /// If possible, subscribes this effect trigger to the event that matches the given event source and event name.
        /// If there is not an event that matches, does nothing.
        /// </summary>
        /// <param name="eventSource">the object source of the event</param>
        /// <param name="eventName">the name of the event to subscribe to</param>
        public void SubscribeToEvent(object eventSource, string eventName);

        /// <summary>
        /// Subscribes to the named event with this EffectTrigger's CombatUnit as the source.
        /// </summary>
        /// <param name="eventName">the name of the event to subscribe to</param>
        public void SubscribeToThisUnitEvent(string eventName);

        /// <summary>
        /// Caches the given event type in a list that is later used to subscribe to this
        /// event from this unit, once this unit is set.
        /// </summary>
        /// <param name="type">the CombatUnitEventType to cache</param>
        public void CacheThisUnitEventType(CombatUnitEventType type);

        /// <summary>
        /// Caches the given event type in a list that is later used to subscribe to this
        /// event from specific sources.
        /// </summary>
        /// <param name="type">the CombatUnitEventType to cache</param>
        public void CacheUnitEventType(CombatUnitEventType type);

        /// <summary>
        /// Caches the given event type in a list that is later used to subscribe to this
        /// event from specific sources.
        /// </summary>
        /// <param name="type">the TileEventType to cache</param>
        public void CacheTileEventType(TileEventType type);

        /// <summary>
        /// For each cached CombatUnitEventType, subscribes to all instances of the
        /// represented event on the given list of units
        /// </summary>
        /// <param name="units">the list of units to subscribe to</param>
        public void SubscribeToUnits(List<ICombatUnit> units);

        /// <summary>
        /// For each cached CombatUnitEventType, subscribes to the represented event
        /// on the given unit
        /// </summary>
        /// <param name="unit">the unit to subscribe to</param>
        public void SubscribeToUnit(ICombatUnit unit);

        /// <summary>
        /// For each cached TileEventType, subscribes to all instances of the
        /// represented event on the given list of tiles
        /// </summary>
        /// <param name="tiles">the list of tiles to subscribe to</param>
        public void SubscribeToTiles(List<ITile> tiles);

        /// <summary>
        /// For each cached TileEventType, subscribes to the represented event
        /// on the given tile
        /// </summary>
        /// <param name="tile">the tile to subscribe to</param>
        public void SubscribeToTile(ITile tile);

        public List<ICombatUnit> GetSubscribedUnits();

        public List<ITile> GetSubscribedTiles();

        /// <summary>
        /// Adds the given predicate
        /// </summary>
        /// <param name="predicate">the unit predicate to add</param>
        public void AddUnitPredicate(ICustomPredicate<ICombatUnit> predicate);

        /// <summary>
        /// Deactivates this
        /// </summary>
        public void OnDeactivate();
    }
}
