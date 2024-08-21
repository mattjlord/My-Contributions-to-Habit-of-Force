using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.CustomEventArgs;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.TurnManager
{
    /// <summary>
    /// Contains a list of ICombatUnits in the current combat encounter and manages the rules for
    /// turn order.
    /// </summary>
    public class TurnManager : ITurnManager
    {
        private List<ICombatUnit> combatUnits;

        private int _currentTurnIndex;

        public static event EventHandler OnCombatStart;

        public static event EventHandler OnUnitTurnStart;
        public static event EventHandler OnUnitTurnEnd;

        public static event EventHandler OnUnitMoveDistance;

        public static event EventHandler OnUnitIncomingMechDamage;
        public static event EventHandler OnUnitIncomingPilotDamage;
        public static event EventHandler OnUnitTakeMechDamage;
        public static event EventHandler OnUnitTakePilotDamage;
        public static event EventHandler OnUnitTakeMechHealing;
        public static event EventHandler OnUnitTakePilotHealing;

        public static event EventHandler OnUnitDamageMech;
        public static event EventHandler OnUnitDamagePilot;
        public static event EventHandler OnUnitHealMech;
        public static event EventHandler OnUnitHealPilot;

        public static event EventHandler OnUnitKillMech;
        public static event EventHandler OnUnitKillPilot;
        public static event EventHandler OnUnitMechDeath;
        public static event EventHandler OnUnitPilotDeath;
        public static event EventHandler OnUnitMechKilled;
        public static event EventHandler OnUnitPilotKilled;

        public static event EventHandler OnUnitEnterTile;
        public static event EventHandler OnUnitExitTile;
        public static event EventHandler OnUnitStartTurnOnTile;
        public static event EventHandler OnUnitEndTurnOnTile;

        public TurnManager()
        {
            combatUnits = new List<ICombatUnit>();
            _currentTurnIndex = 0;
        }

        public void SortTurnOrder()
        {
            List<ICombatUnit> newList = combatUnits;
            newList.Sort((u1, u2) => u2.InitializeInitiative().CompareTo(u1.InitializeInitiative()));
            combatUnits = newList;
        }

        public ICombatUnit NextTurn()
        {
            ICombatUnit prevCombatUnit = combatUnits[_currentTurnIndex];

            OnUnitTurnEnd?.Invoke(this, new UnitArgs(prevCombatUnit));
            prevCombatUnit.OnTurnEnd();

            int nextTurnIndex = GetNextTurnIndex();

            int maxAttempts = combatUnits.Count; // To prevent infinite loops
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                if (combatUnits[nextTurnIndex].IsDead())
                {
                    _currentTurnIndex = nextTurnIndex;
                    nextTurnIndex = GetNextTurnIndex();

                }
                attempts++;
            }

            _currentTurnIndex = nextTurnIndex;

            ICombatUnit currCombatUnit = combatUnits[_currentTurnIndex];

            OnUnitTurnStart?.Invoke(this, new UnitArgs(currCombatUnit));
            currCombatUnit.OnTurnStart();

            return currCombatUnit;

        }

        public void AddCombatUnit(ICombatUnit combatUnit)
        {
            if (combatUnits.Contains(combatUnit))
            {
                return;
            }
            combatUnit.SetTurnManager(this);
            combatUnits.Add(combatUnit);
        }

        public List<ICombatUnit> GetCombatUnits()
        {
            return combatUnits;
        }

        public void StartCombat()
        {
            SortTurnOrder();
            OnCombatStart?.Invoke(this, EventArgs.Empty);

            ICombatUnit currCombatUnit = combatUnits[_currentTurnIndex];

            OnUnitTurnStart?.Invoke(this, new UnitArgs(currCombatUnit));
            currCombatUnit.OnTurnStart();
        }

        public ICombatUnit GetCurrentCombatUnit()
        {
            return combatUnits[_currentTurnIndex];
        }

        public MechTilePiece GetCurrentMechTilePiece()
        {
            if (_currentTurnIndex >= combatUnits.Count)
            {
                return null;
            }
            return combatUnits[_currentTurnIndex].GetTilePiece();
        }

        /// <summary>
        /// Helper function for NextTurn that returns the index of the next turn
        /// </summary>
        /// <returns>the index of the next turn</returns>
        private int GetNextTurnIndex()
        {
            if (_currentTurnIndex + 1 == combatUnits.Count)
            {
                return 0;
            }
            return _currentTurnIndex + 1;
        }

        public void OnUnitStartOfTurn(ICombatUnit unit)
        {
            OnUnitTurnStart?.Invoke(this, new OverrideSenderArgs(unit));
        }

        public void OnUnitEndOfTurn(ICombatUnit unit)
        {
            OnUnitTurnEnd?.Invoke(this, new OverrideSenderArgs(unit));
        }

        public void OnUnitMove(ICombatUnit unit, int distance)
        {
            OnUnitMoveDistance?.Invoke(this, new IntArgs(distance, unit));
        }

        public void OnUnitDoMechDamage(ICombatUnit unit, ICombatUnit target, float value)
        {
            OnUnitDamageMech?.Invoke(this, new UnitFloatArgs(target, value, unit));
        }

        public void OnUnitDoPilotDamage(ICombatUnit unit, ICombatUnit target, float value)
        {
            OnUnitDamagePilot?.Invoke(this, new UnitFloatArgs(target, value, unit));
        }

        public void OnUnitDoMechHealing(ICombatUnit unit, ICombatUnit target, float value)
        {
            OnUnitHealMech?.Invoke(this, new UnitFloatArgs(target, value, unit));
        }

        public void OnUnitDoPilotHealing(ICombatUnit unit, ICombatUnit target, float value)
        {
            OnUnitHealPilot?.Invoke(this, new UnitFloatArgs(target, value, unit));
        }

        public void OnUnitIncomingMechDamageFromUnit(ICombatUnit to, ICombatUnit from, float value)
        {
            OnUnitIncomingMechDamage?.Invoke(this, new UnitFloatArgs(from, value, to));
        }

        public void OnUnitIncomingPilotDamageFromUnit(ICombatUnit to, ICombatUnit from, float value)
        {
            OnUnitIncomingPilotDamage?.Invoke(this, new UnitFloatArgs(from, value, to));
        }

        public void OnUnitTakeMechDamageFromUnit(ICombatUnit to, ICombatUnit from, float value)
        {
            OnUnitTakeMechDamage?.Invoke(this, new UnitFloatArgs(from, value, to));
        }

        public void OnUnitTakePilotDamageFromUnit(ICombatUnit to, ICombatUnit from, float value)
        {
            OnUnitTakePilotDamage?.Invoke(this, new UnitFloatArgs(from, value, to));
        }

        public void OnUnitTakeMechHealingFromUnit(ICombatUnit to, ICombatUnit from, float value)
        {
            OnUnitTakeMechHealing?.Invoke(this, new UnitFloatArgs(from, value, to));
        }

        public void OnUnitTakePilotHealingFromUnit(ICombatUnit to, ICombatUnit from, float value)
        {
            OnUnitTakePilotHealing?.Invoke(this, new UnitFloatArgs(from, value, to));
        }

        public void OnUnitKillUnitMech(ICombatUnit unit, ICombatUnit target)
        {
            OnUnitKillMech?.Invoke(this, new UnitArgs(target, unit));
        }

        public void OnUnitKillUnitPilot(ICombatUnit unit, ICombatUnit target)
        {
            OnUnitKillPilot?.Invoke(this, new UnitArgs(target, unit));
        }

        public void OnUnitMechDie(ICombatUnit unit)
        {
            OnUnitMechDeath?.Invoke(this, new OverrideSenderArgs(unit));
        }

        public void OnUnitPilotDie(ICombatUnit unit)
        {
            OnUnitPilotDeath?.Invoke(this, new OverrideSenderArgs(unit));
        }

        public void OnUnitMechKilledByUnit(ICombatUnit to, ICombatUnit from)
        {
            OnUnitMechKilled?.Invoke(this, new UnitArgs(from, to));
        }

        public void OnUnitPilotKilledByUnit(ICombatUnit to, ICombatUnit from)
        {
            OnUnitPilotKilled?.Invoke(this, new UnitArgs(from, to));
        }

        public void OnTileEnteredByUnit (ICombatUnit unit, ITile tile)
        {
            OnUnitEnterTile?.Invoke(this, new TileArgs(tile, unit));
        }

        public void OnTileExitedByUnit(ICombatUnit unit, ITile tile)
        {
            OnUnitExitTile?.Invoke(this, new TileArgs(tile, unit));
        }

        public void OnUnitTurnStartOnTile(ICombatUnit unit, ITile tile)
        {
            OnUnitStartTurnOnTile?.Invoke(this, new TileArgs(tile, unit));
        }

        public void OnUnitTurnEndOnTile(ICombatUnit unit, ITile tile)
        {
            OnUnitEndTurnOnTile?.Invoke(this, new TileArgs(tile, unit));
        }

        public List<MechTilePiece> GetMechTilePieces()
        {
            List<MechTilePiece> mechs = combatUnits.Select(mech => mech.GetTilePiece()).ToList();
            return mechs;
        }

        public int GetTurnIndex()
        {
            return _currentTurnIndex;
        }
    }
}
