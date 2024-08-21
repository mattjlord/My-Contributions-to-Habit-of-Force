using CombatRevampScripts.ActionsAbilities.AOEBoardUtils;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.Board.TilePiece;
using CombatRevampScripts.Board.Board;
using System.Collections.Generic;
using UnityEngine;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.ActionsAbilities.Enums;

namespace CombatRevampScripts.Board.Tile
{
	/// <summary>
	/// Represents the unit of space on a board that can hold a tile piece.
	/// </summary>
	public interface ITile : IAOEBoardUtils
	{
		/// <summary>
		/// Sets this tile piece to the given.
		/// </summary>
		/// <param name="tilePiece">The tile piece to set</param>
		void SetTilePiece(ITilePiece tilePiece);

		/// <summary>
		/// Gets this tile piece.
		/// </summary>
		/// <returns>the tile piece</returns>
		ITilePiece GetTilePiece();

		/// <summary>
		/// Sets this board position to the given.
		/// </summary>
		/// <param name="pos">The position</param>
		void SetBoardPosition(Vector2Int pos);
		
		/// <summary>
		/// Returns the board position of this tile.
		/// </summary>
		/// <returns>Vector2Int</returns>
		Vector2Int GetBoardPosition();

		/// <summary>
		/// Gets the position of this tile in world coordinates.
		/// </summary>
		/// <returns>the 3D position of this tile</returns>
		Vector3 GetWorldPosition();

		/// <summary>
		/// Sets the ISerializableObservableBoard of this tile.
		/// </summary>
		/// <param name="board">the board to set</param>
		void SetBoard(ISerializableObservableBoard board);

		/// <summary>
		/// Gets the ISerializableObservableBoard of this tile.
		/// </summary>
		/// <returns>the board of this</returns>
		ISerializableObservableBoard GetBoard();

        /// <summary>
        /// Gets all neighboring tiles of this that match the given criteria (diagonals not included).
        /// </summary>
        /// /// <param name="source">the source unit used to check affiliation and whether or not each tile includes this unit</param>
        /// <param name="selfInclusive">can a tile be occupied by the source unit?</param>
        /// <param name="relativeAffiliation">what should the affiliation of each tile be relative to the source?</param>
        /// <param name="tileStatus">what should the status of each tile be?</param>
        /// <returns>a list of neighboring tiles that match the criteria</returns>
        List<ITile> GetNeighbors(ICombatUnit source, bool selfInclusive, RelativeAffiliation relativeAffiliation, TileStatus tileStatus);

		/// <summary>
		/// Removes this tile piece. Does nothing if there is no tile piece.
		/// </summary>
		void RemoveTilePiece();

		/// <summary>
		/// Moves this tile piece to the given tile, and so this tile
		/// will no longer have this tile.
		/// </summary>
		/// <param name="to">the tile to move this tile piece to</param>
		void MoveTilePieceTo(ITile to);

		/// <summary>
		/// If there is a MechTilePiece in this tile, initializes the CombatUnit of that piece,
		/// otherwise does nothing
		/// </summary>
		/// <param name="turnManager">the ITurnManager to pass to the MechTilePiece for initialization</param>
		void InitializeCombatUnit(ITurnManager turnManager);

		/// <summary>
		/// Checks whether this matches the given criteria.
		/// </summary>
		/// <param name="source">the source unit used to check affiliation and whether or not this tile includes this unit</param>
		/// <param name="selfInclusive">can this tile be occupied by the source unit?</param>
		/// <param name="relativeAffiliation">what should the affiliation of this tile be relative to the source?</param>
		/// <param name="tileStatus">what should the status of this tile be?</param>
		/// <returns>whether this tile matches the criteria</returns>
		bool MatchesCriteria(ICombatUnit source, bool selfInclusive, RelativeAffiliation relativeAffiliation, TileStatus tileStatus);

		/// <summary>
		/// Performs necessary functionality for when the given unit enters this tile.
		/// </summary>
		/// <param name="combatUnit">the ICombatUnit that enters</param>
		void OnUnitEnter(ICombatUnit combatUnit);

        /// <summary>
        /// Performs necessary functionality for when the given unit exits this tile.
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit that exits</param>
        void OnUnitExit(ICombatUnit combatUnit);

        /// <summary>
        /// Performs necessary functionality for when the given unit starts its turn on this tile.
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit that starts its turn here</param>
        void OnUnitStartTurnOn(ICombatUnit combatUnit);

        /// <summary>
        /// Performs necessary functionality for when the given unit ends its turn on this tile.
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit that ends its turn here</param>
        void OnUnitEndTurnOn(ICombatUnit combatUnit);

        Material GetMaterial();

        void DeleteUnit();
	}
}