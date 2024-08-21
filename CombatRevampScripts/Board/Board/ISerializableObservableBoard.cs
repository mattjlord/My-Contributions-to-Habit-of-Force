using UnityEngine;

namespace CombatRevampScripts.Board.Board
{
	/// <summary>
	/// Represents a grid of tiles that can be serialized into Unity Engine.
	/// Positions with no tiles have a value of null.
	/// There are two invariants: the grid is always well-formed,
	/// the outermost rows and columns have at least one non-null value, and
	/// the tile game objects have the tile tag. 
	/// </summary>
	public interface ISerializableObservableBoard
	{
		/// <summary>
		/// Returns this board as a grid of game objects, meeting the well-formedness
		/// expectations of a board.
		/// </summary>
		/// <returns>the well-formed board</returns>
		public GameObject[,] GetBoardAsGOGrid();

		/// <summary>
		/// Gets a tile at the given position, which uses the logical
		/// notion. If there is no tile at the given position, return null.
		/// </summary>
		/// <param name="pos">the logical position</param>
		/// <returns>the tile at the given position if any, null if none</returns>
		GameObject GetTileAt(Vector2Int pos);

		/// <summary>
		/// Returns the logical height of this board.
		/// </summary>
		/// <returns>the logical height</returns>
		public int GetLogicalHeight();

		/// <summary>
		/// Returns the logical width of this board.
		/// </summary>
		/// <returns>the logical width</returns>
		public int GetLogicalWidth();

		/// <summary>
		/// Sets the board of all Tiles in this to this.
		/// </summary>
		public void UpdateTileBoardInfo();
	}
}