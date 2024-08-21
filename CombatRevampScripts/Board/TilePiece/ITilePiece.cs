using CombatRevampScripts.ActionsAbilities.AOEBoardUtils;
using CombatRevampScripts.Board.TilePiece.GeneralVisitors;
using CombatRevampScripts.CombatVisuals.Handler;
using UnityEngine;

namespace CombatRevampScripts.Board.TilePiece
{
	/// <summary>
	/// Represents that which occupies a tile on a board.
	/// </summary>
	public interface ITilePiece : IAOEBoardUtils
	{
		/// <summary>
		/// Initializes this tile piece.
		/// </summary>
		void Initialize();
		
		/// <summary>
		/// Shuts down this tile piece.
		/// </summary>
		void ShutDown();

		/// <summary>
		/// Sets the position of this tile piece.
		/// </summary>
		/// <param name="pos">the position</param>
		void SetPosition(Vector2Int pos);

		/// <summary>
		/// Gets the position of this tile piece.
		/// </summary>
		/// <returns>the position of this tile piece</returns>
		Vector2Int GetPosition();

		/// <summary>
		/// Accepts the given visitor
		/// </summary>
		/// <param name="visitor">the tile piece visitor</param>
		/// <typeparam name="T">the return type</typeparam>
		/// <returns>whatever the visitor return</returns>
		T Accept<T>(ITilePieceVisitor<T> visitor);

		/// <summary>
		/// Returns the combat visual handler of this tile piece
		/// </summary>
		/// <returns>the ICombatVisualHandler</returns>
		ICombatVisualHandler GetVisualHandler();
	}
}