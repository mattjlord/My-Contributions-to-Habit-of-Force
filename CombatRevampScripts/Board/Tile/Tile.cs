using System;
using CombatRevampScripts.Board.TilePiece;
using CombatRevampScripts.Extensions;
using UnityEngine;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using CombatRevampScripts.Board.Board;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.ActionsAbilities.CustomEventArgs;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CombatRevampScripts.Board.Tile
{
	/// <summary>
	/// An MonoBehaviour implementation of a tile. 
	/// </summary>
	public class Tile : MonoBehaviour, ITile
	{
		// BAD CODING, NEED TO FIX
		[SerializeField] public bool _isHill;
		[HideInInspector] public int _hillCounter = 0;
		[HideInInspector] public int _howLongHasPieceBeen = 0;
		[SerializeField] public bool _isTerminal;
		[HideInInspector] public bool _terminalFixed = true;
		[SerializeField] private GameObject workingTerminal;
		[SerializeField] private GameObject notWorkingTerminal;
		
		
		private ITilePiece _tilePiece;
		private ISerializableObservableBoard _board;
		[HideInInspector] [SerializeField] private Vector2Int _boardPosition;

		public static event EventHandler OnUnitStartTurnOnThis;
		public static event EventHandler OnUnitEndTurnOnThis;
		public static event EventHandler OnUnitEnterThis;
		public static event EventHandler OnUnitExitThis;
		
		public void SetTilePiece(ITilePiece tilePiece)
		{
			// If it is during edit-time, then we can assume that
			// the given tile piece is a MonoBehaviour as that is how
			// tile to tile piece relationships are encoded
			#if UNITY_EDITOR
			if (tilePiece != null)
			{
				MonoBehaviour tilePieceMB = (MonoBehaviour)tilePiece;
				Transform tilePieceTransform = tilePieceMB.transform;
				// tilePieceTransform.SetParent(transform, false);
				Undo.SetTransformParent(tilePieceTransform, transform, "Move Tile Piece");
				tilePieceTransform.localPosition = Vector3.zero;
			}

			#else
			if (tilePiece != null) {
				((MechTilePiece)tilePiece).transform.SetParent(transform, true);
			}

#endif
			_tilePiece = tilePiece;
		}

		public ITilePiece GetTilePiece()
		{
			// It is an invariant that when in the editor, the tile piece is encoded as a child
			// of the game object, and we know that is going to be a MonoBehaviour. So, we will
			// just access the tile piece that way.
			#if UNITY_EDITOR
				GameObject tilePieceGO = gameObject.FindChildWithTag<ITilePiece>("TilePiece");
				
				if (tilePieceGO == null)
				{
					return null;
				}
				
				if (tilePieceGO.TryGetComponent(out ITilePiece tilePiece))
				{
					return tilePiece;
				}
				else
				{
					throw new Exception("Game objects tagged as tile piece must have tile piece component.");
				}

			#else
			if (_tilePiece == null) { return null; }
				return _tilePiece;
			#endif
		}

		public void SetBoardPosition(Vector2Int pos)
		{
			_boardPosition = pos;
		}

		public Vector2Int GetBoardPosition()
		{
			return _boardPosition;
		}

		public Vector3 GetWorldPosition()
		{
			return transform.position;
		}

		public void SetBoard(ISerializableObservableBoard board)
		{
			_board = board;
		}

		public ISerializableObservableBoard GetBoard()
		{
			return _board;
		}

		public void RemoveTilePiece()
		{
			#if UNITY_EDITOR
				GameObject tilePieceGO = gameObject.FindChildWithTag<ITilePiece>("TilePiece");
				if (tilePieceGO == null)
				{
					return;
				}

				_tilePiece = tilePieceGO.GetComponent<ITilePiece>();
			#else
			if (_tilePiece == null) return;
			#endif
			
			// Destroys the tile piece game object as well.
			_tilePiece.
			ShutDown();
			SetTilePiece(null);
		}

		public void MoveTilePieceTo(ITile to)
		{
			to.SetTilePiece(_tilePiece);
			if (_tilePiece != null)
			{
				_tilePiece.SetPosition(to.GetBoardPosition());	
			}
			SetTilePiece(null);
		}

		public void InitializeCombatUnit(ITurnManager turnManager)
		{
			if (_tilePiece == null && GetComponentInChildren<MechTilePiece>() != null)
			{
				SetTilePiece(GetComponentInChildren<MechTilePiece>());
			}

			if (_isTerminal)
			{
				_terminalFixed = false;
			}
			if (_tilePiece == null) { return; }
			if (_tilePiece.GetType() != typeof(MechTilePiece)) { return; }
			MechTilePiece mechPiece = (MechTilePiece)_tilePiece;
			mechPiece.InitializeCombatUnit(turnManager);
		}

		public List<ITile> GetNeighbors(ICombatUnit source, bool selfInclusive, RelativeAffiliation relativeAffiliation, TileStatus tileStatus)
		{
			List<ITile> neighbors = new List<ITile>();
			int manhattanModifier = 1;
			int minX = _boardPosition.x - 1;
			int maxX = _boardPosition.x + 1;
			int minY = _boardPosition.y - 1;
			int maxY = _boardPosition.y + 1;

			for (int x = minX; x <= maxX; x++)
			{
                int modifiedMinY = minY + manhattanModifier;
                int modifiedMaxY = maxY - manhattanModifier;

                for (int y = modifiedMinY; y <= modifiedMaxY; y++)
				{
					GameObject tileGO = _board.GetTileAt(new Vector2Int(x, y));
					if (tileGO != null)
					{
                        ITile tile = tileGO.GetComponent<ITile>();
						if (tile != this && tile.MatchesCriteria(source, selfInclusive, relativeAffiliation, tileStatus))
						{
							neighbors.Add(tile);
						}
                    }
				}

                if (x < _boardPosition.x) { manhattanModifier -= 1; }
                else { manhattanModifier += 1; }
            }

			return neighbors;
		}

		public bool MatchesCriteria(ICombatUnit source, bool selfInclusive, RelativeAffiliation relativeAffiliation, TileStatus tileStatus)
		{
			bool isSelf = IsOccupiedByThisUnit(source);
			bool isOccupied = IsOccupied();
			bool isOccupiedByUnit = IsOccupiedByUnit();
			bool isFriendly = IsFriendlyToThisUnit(source);

			if (isSelf && !selfInclusive) { return false; }
			if (isOccupied && tileStatus == TileStatus.Empty) { return false; }
			if (!isOccupied && (tileStatus == TileStatus.Occupied || tileStatus == TileStatus.OccupiedByUnit)) { return false; }
			if (!isOccupiedByUnit && tileStatus == TileStatus.OccupiedByUnit) { return false; }
			if (!isFriendly && relativeAffiliation == RelativeAffiliation.Friendly) { return false; }
			if (isFriendly && relativeAffiliation == RelativeAffiliation.Hostile) { return false; }
			return true;
        }

		private bool IsOccupiedByThisUnit(ICombatUnit thisUnit)
		{
            ITilePiece occupyingPiece = GetTilePiece();
            ICombatUnit occupyingUnit = null;

            if (occupyingPiece != null && occupyingPiece.GetType() == typeof(MechTilePiece))
            {
                MechTilePiece occupyingMechTilePiece = (MechTilePiece)occupyingPiece;
                occupyingUnit = occupyingMechTilePiece.GetCombatUnit();
            }

			if (occupyingUnit == null) { return false; }
			return occupyingUnit == thisUnit;
        }

		private bool IsOccupied()
		{
			return GetTilePiece() != null;
		}

		private bool IsOccupiedByUnit()
		{
            ITilePiece occupyingPiece = GetTilePiece();
            ICombatUnit occupyingUnit = null;

            if (occupyingPiece != null && occupyingPiece.GetType() == typeof(MechTilePiece))
            {
                MechTilePiece occupyingMechTilePiece = (MechTilePiece)occupyingPiece;
                occupyingUnit = occupyingMechTilePiece.GetCombatUnit();
            }

			return occupyingUnit != null;
        }

		private bool IsFriendlyToThisUnit(ICombatUnit thisUnit)
		{
			ITilePiece occupyingPiece = GetTilePiece();
			ICombatUnit occupyingUnit = null;

			if (occupyingPiece != null && occupyingPiece.GetType() == typeof(MechTilePiece))
			{
				MechTilePiece occupyingMechTilePiece = (MechTilePiece)occupyingPiece;
				occupyingUnit = occupyingMechTilePiece.GetCombatUnit();
			}

			if (occupyingUnit == null) { return true; }

			Affiliation thisAffiliation = thisUnit.GetPilotSO().affiliation;
			Affiliation occupantAffiliation = occupyingUnit.GetPilotSO().affiliation;

			return thisAffiliation == occupantAffiliation;
        }

        public List<ITile> GetTilesInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation, 
			TileStatus tileStatus, PathfindingMode pathfindingMode)
        {
            List<ITile> allTiles = new List<ITile>();

			int minX = _boardPosition.x - radius;
			int maxX = _boardPosition.x + radius;
			int minY = _boardPosition.y - radius;
			int maxY = _boardPosition.y + radius;
			int manhattanModifier = radius;

			for (int x = minX; x <= maxX; x++)
			{
				int modifiedMinY = minY + manhattanModifier;
				int modifiedMaxY = maxY - manhattanModifier;

				for (int y = modifiedMinY; y <= modifiedMaxY; y++)
				{
					bool isSelf = x == _boardPosition.x && y == _boardPosition.y;
					if (!selfInclusive && isSelf) { continue; }
					GameObject addedTileGO = _board.GetTileAt(new Vector2Int(x, y));
					int distance = MathExtension.ManhattanDistance(_boardPosition, new Vector2Int(x, y));
					if (addedTileGO != null && distance <= radius)
					{
						ITile addedTile = addedTileGO.GetComponent<ITile>();
						if (!addedTile.MatchesCriteria(source, selfInclusive, relativeAffiliation, tileStatus)) { continue; }
						if (!isSelf && !PathfindingExtension.DoesPathExist(source, this, addedTile, radius, pathfindingMode)) { continue; }
                        ITilePiece tilePiece = addedTile.GetTilePiece();
                        if (tilePiece != null)
						{
                            MechTilePiece mechTilePiece = (MechTilePiece)tilePiece;
                            ICombatUnit unit = mechTilePiece.GetCombatUnit();
                            if (unit.IsDead()) { continue; }
                        }
                        
                        
                        
                        allTiles.Add(addedTile);
					}
				}

				if (x < _boardPosition.x) { manhattanModifier -= 1; }
				else { manhattanModifier += 1; }
			}

			return allTiles;
        }

        public List<ICombatUnit> GetUnitsInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation)
        {
			List<ITile> allTiles = GetTilesInAOE(source, radius, selfInclusive, relativeAffiliation, TileStatus.OccupiedByUnit, PathfindingMode.Standard);
			List<ICombatUnit> allUnits = new List<ICombatUnit>();

			foreach (ITile tile in allTiles)
			{
				ITilePiece tilePiece = tile.GetTilePiece();
				MechTilePiece mechTilePiece = (MechTilePiece)tilePiece;
				ICombatUnit unit = mechTilePiece.GetCombatUnit();
				if (unit.IsDead()) { continue; }
				allUnits.Add(unit);
            }

			return allUnits;
        }

        public void OnUnitEnter(ICombatUnit combatUnit)
        {
			OnUnitEnterThis?.Invoke(this, new OverrideSenderArgs(combatUnit));
        }

        public void OnUnitExit(ICombatUnit combatUnit)
        {
            OnUnitExitThis?.Invoke(this, new OverrideSenderArgs(combatUnit));
            _howLongHasPieceBeen = 0;
        }

        public void OnUnitStartTurnOn(ICombatUnit combatUnit)
        {
            OnUnitStartTurnOnThis?.Invoke(this, new OverrideSenderArgs(combatUnit));

            _howLongHasPieceBeen++;
            if (_isTerminal && combatUnit.GetPilotSO().affiliation == Affiliation.ally)
            {
	            _terminalFixed = true;
	            if (notWorkingTerminal != null)
	            {
		            GameObject o = Instantiate(workingTerminal, notWorkingTerminal.transform.position,
			            notWorkingTerminal.transform.rotation);
		            o.transform.localScale = notWorkingTerminal.transform.localScale;
		            Destroy(notWorkingTerminal);
	            }
            }
        }

        public void OnUnitEndTurnOn(ICombatUnit combatUnit)
        {
            OnUnitEndTurnOnThis?.Invoke(this, new OverrideSenderArgs(combatUnit));
        }

        public Material GetMaterial()
        {
	        return GetComponentsInChildren<Renderer>()[5].material;
        }
        
        public void DeleteUnit()
        {
	        if (_tilePiece == null) { return; }
	        if (_tilePiece.GetType() != typeof(MechTilePiece)) { return; }
	        MechTilePiece mechPiece = (MechTilePiece)_tilePiece;
			mechPiece.DeleteCombatUnit();
        }
	}
}