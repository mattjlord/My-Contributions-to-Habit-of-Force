using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using UnityEngine;

namespace CombatRevampScripts.Board.Board
{
	/// <summary>
	/// A serializable board that can be observed, and the tiles
	/// it contains can also be modified. Invariants are to be maintained
	/// in all of the methods.
	/// </summary>
	public interface ISerializableBoard : ISerializableObservableBoard
	{
		/// <summary>
		/// Adds a tile at the given position, which uses the logical
		/// notion. Adding a tile to a position that already has a tile
		/// is a no-op.
		/// </summary>
		/// <param name="pos">the logical position</param>
		/// <param name="tileGO">the tile game object to add</param>
		public void AddTileAt(Vector2Int pos, GameObject tileGO);

		/// <summary>
		/// Removes a tile at the given position, which uses the logical
		/// notion. Removing a tile to a position that does not have a tile
		/// is a no-op.
		/// </summary>
		/// <param name="pos">the logical position</param>
		public void RemoveTileAt(Vector2Int pos);

		/// <summary>
		/// Initializes all necessary CombatUnits for combat on this board with the given TurnManager.
		/// </summary>
		/// <param name="turnManager">the ITurnManager to initialize with</param>
		public void InitializeAllUnits(ITurnManager turnManager);

		public void DeleteUnit();

		public bool AllAllies();

		public bool AreTheyDead(List<string> names);
	}
}