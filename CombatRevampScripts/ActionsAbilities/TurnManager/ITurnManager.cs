using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.CustomEventArgs;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;

namespace CombatRevampScripts.ActionsAbilities.TurnManager
{
    /// <summary>
    /// Contains a list of ICombatUnits in the current combat encounter and manages the rules for
    /// turn order.
    /// </summary>
    public interface ITurnManager
    {
        /// <summary>
        /// Sorts the ICombatUnits of this by speed,
        /// establishing turn order
        /// </summary>
        public void SortTurnOrder();

        /// <summary>
        /// Starts the next turn in the turn order
        /// </summary>
        /// <returns>the ICombatUnit that corresponds to the current turn</returns>
        public ICombatUnit NextTurn();

        /// <summary>
        /// If it isn't already part of the turn order,
        /// adds the given combat unit to the list of units in this,
        /// otherwise does nothing.
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit to add</param>
        public void AddCombatUnit(ICombatUnit combatUnit);

        /// <summary>
        /// Gets a list of the combat units in this TurnManager.
        /// </summary>
        /// <returns>A list of combat units in this TurnManager</returns>
        public List<ICombatUnit> GetCombatUnits();

        public List<MechTilePiece> GetMechTilePieces();

        /// <summary>
        /// Starts combat for this TurnManager.
        /// </summary>
        public void StartCombat();

        /// <summary>
        /// Gets the unit whose turn it is currently.
        /// </summary>
        /// <returns>the ICombatUnit whose turn it is currently</returns>
        public ICombatUnit GetCurrentCombatUnit();

        public MechTilePiece GetCurrentMechTilePiece();

        public void OnUnitStartOfTurn(ICombatUnit unit);

        public void OnUnitEndOfTurn(ICombatUnit unit);

        public void OnUnitMove(ICombatUnit unit, int distance);

        public void OnUnitDoMechDamage(ICombatUnit unit, ICombatUnit target, float value);

        public void OnUnitDoPilotDamage(ICombatUnit unit, ICombatUnit target, float value);

        public void OnUnitDoMechHealing(ICombatUnit unit, ICombatUnit target, float value);

        public void OnUnitDoPilotHealing(ICombatUnit unit, ICombatUnit target, float value);

        public void OnUnitIncomingMechDamageFromUnit(ICombatUnit to, ICombatUnit from, float value);

        public void OnUnitIncomingPilotDamageFromUnit(ICombatUnit to, ICombatUnit from, float value);

        public void OnUnitTakeMechDamageFromUnit(ICombatUnit to, ICombatUnit from, float value);

        public void OnUnitTakePilotDamageFromUnit(ICombatUnit to, ICombatUnit from, float value);

        public void OnUnitTakeMechHealingFromUnit(ICombatUnit to, ICombatUnit from, float value);

        public void OnUnitTakePilotHealingFromUnit(ICombatUnit to, ICombatUnit from, float value);

        public void OnUnitKillUnitMech(ICombatUnit unit, ICombatUnit target);

        public void OnUnitKillUnitPilot(ICombatUnit unit, ICombatUnit target);

        public void OnUnitMechDie(ICombatUnit unit);

        public void OnUnitPilotDie(ICombatUnit unit);

        public void OnUnitMechKilledByUnit(ICombatUnit to, ICombatUnit from);

        public void OnUnitPilotKilledByUnit(ICombatUnit to, ICombatUnit from);

        public void OnTileEnteredByUnit(ICombatUnit unit, ITile tile);

        public void OnTileExitedByUnit(ICombatUnit unit, ITile tile);

        public void OnUnitTurnStartOnTile(ICombatUnit unit, ITile tile);

        public void OnUnitTurnEndOnTile(ICombatUnit unit, ITile tile);

        /// <summary>
        /// Gets the current turn represented as an int
        /// </summary>
        /// <returns>the turn index</returns>
        public int GetTurnIndex();
    }
}