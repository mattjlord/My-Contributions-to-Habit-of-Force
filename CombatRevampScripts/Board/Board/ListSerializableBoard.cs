using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.Board.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using Combat__Old_.Board.TilePiece;
using CombatRevampScripts.Board.TilePiece;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;
using ITilePiece = CombatRevampScripts.Board.TilePiece.ITilePiece;

namespace CombatRevampScripts.Board.Board
{
	/// <summary>
	/// A list implementation of the serializable tile grid interface.
	///
	/// The 2D list's inner list must all have the same length at any given
	/// time. Empty tiles are represented by a null. The outer-most rows and
	/// columns must have at least one non-null value.
	///
	/// Note that the internal origin may not actually be in the internal grid.
	/// </summary>
	/// <remarks>
	/// The invariant that the outer-most rows and columns must have
	/// at least one non-null value can be broken if the user deletes the tile
	/// not by the GUI feature, but by hitting delete on their keyboard. The
	/// best way to mitigate this right now is just to trim the board after adding
	/// a tile.
	/// </remarks>
	[Serializable]
	public class ListSerializableBoard : ISerializableBoard
	{
		[SerializeField] private List<TileGOs> _grid;

		// Unity cannot serialize a nested list, so the inner list is wrapped in a class.
		// It delegates all the list methods to the list contained within.
		// Stupid, I know.
		[Serializable]
		private class TileGOs
		{
			[SerializeField] private List<GameObject> _gameObjects;

			public TileGOs()
			{
				_gameObjects = new List<GameObject>();
			}

			public TileGOs(IEnumerable<GameObject> gameObjects)
			{
				_gameObjects = gameObjects.ToList();
			}

			public GameObject this[int index]
			{
				get
				{
					return _gameObjects[index];
				}
				set
				{
					_gameObjects[index] = value;
				}
			}

			public int Count => _gameObjects.Count;

			public void Insert(int i, GameObject o)
			{
				_gameObjects.Insert(i, o);
			}

			public void AddRange(IEnumerable<GameObject> range)
			{
				_gameObjects.AddRange(range);
			}

			public bool All(Func<GameObject, bool> pred)
			{
				return _gameObjects.All(pred);
			}

			public void RemoveAt(int i)
			{
				_gameObjects.RemoveAt(i);
			}

			public void SetTileBoardInfo(ISerializableObservableBoard board)
			{
				foreach (GameObject tileGO in _gameObjects)
				{
					if (tileGO != null)
					{
						ITile tile = tileGO.GetComponent<ITile>();
						if (tile != null)
						{
							tile.SetBoard(board);
						}
					}
				}
			}

			public void InitializeAllUnits(ITurnManager turnManager)
			{
				foreach (GameObject tileGO in _gameObjects)
				{
					if (tileGO != null)
					{
						ITile tile = tileGO.GetComponent<ITile>();
						if (tile != null)
						{
							tile.InitializeCombatUnit(turnManager);
						}
					}
				}
			}

			public void DeleteUnit()
			{
				foreach (GameObject tileGO in _gameObjects)
				{
					if (tileGO != null)
					{
						ITile tile = tileGO.GetComponent<ITile>();
						if (tile != null)
						{
							tile.DeleteUnit();
						}
					}
				}
			}
			
			public bool AllAllies()
			{
				foreach (GameObject tileGO in _gameObjects)
				{
					if (tileGO != null)
					{
						ITile tile = tileGO.GetComponent<ITile>();
						if (tile != null)
						{
							// ASSUMING THERE ARE JUST TILE PIECES
							MechTilePiece tilePiece = (MechTilePiece) tile.GetTilePiece();
							if (tilePiece != null)
							{
								if (tilePiece.GetCombatUnit().GetPilotSO().affiliation == Affiliation.enemy 
								    &&!tilePiece.GetCombatUnit().IsDead())
								{
									return false;
								}
							}
						}
					}
				}

				return true;
			}

			public bool AreTheyDead(List<string> names)
			{
				foreach (GameObject tileGO in _gameObjects)
				{
					if (tileGO != null)
					{
						ITile tile = tileGO.GetComponent<ITile>();
						if (tile != null)
						{
							// ASSUMING THERE ARE JUST TILE PIECES
							MechTilePiece tilePiece = (MechTilePiece) tile.GetTilePiece();
							if (tilePiece != null)
							{
								if (names.Contains(tilePiece.GetCombatUnit().GetPilotSO().pilotName) 
								    && tilePiece.GetCombatUnit().IsDead())
								{
									return true;
								}
							}
						}
					}
				}

				return false;
			}
		}

		// Initialized as the negative of the initial position added,
		// changes as the new left columns or top rows are added
		[SerializeField] private Vector2Int _internalOrigin;

		public ListSerializableBoard()
		{
			_grid = new List<TileGOs>();
		}
		
		public GameObject[,] GetBoardAsGOGrid()
		{
			if (_grid.Count == 0)
			{
				return new GameObject[0, 0];
			}

			int rows = _grid.Count;
			int cols = _grid[0].Count;
			
			var ret = new GameObject[rows, cols];

			for (int r = 0; r < rows; r++)
			{
				for (int c = 0; c < cols; c++)
				{
					ret[r, c] = _grid[r][c];
				}
			}	
			
			return ret;
		}

		public int GetLogicalHeight()
		{
			return _grid.Count;
		}

		public int GetLogicalWidth()
		{
			return _grid.Count == 0 ? 0 : _grid[0].Count;
		}

		/// <inheritdoc cref="ISerializableBoard.AddTileAt"/>
		/// <remarks>
		/// <para>
		/// The logical invariant that the edge-most row and column have at
		/// least one non-null value is preserved because if new space is
		/// made, it is made just enough for the new position to exist in
		/// the internal grid. Thus, the edge-most of the newly created row
		/// and/or column will contain that given tile game object.
		/// </para>
		/// <para>
		/// If the grid is empty, then the internal origin is to be initialized.
		/// </para>
		/// </remarks>
		public void AddTileAt(Vector2Int pos, GameObject tileGO)
		{
			if (_grid.Count == 0)
			{
				_internalOrigin = new Vector2Int(-1 * pos.x, -1 * pos.y);
			}
			
			Vector2Int internalPos = ExternalToInternalPos(pos);
			MakeSpace(internalPos);
			
			// This value could be the same as internalPos if no space was made
			Vector2Int updatedInternalPos = ExternalToInternalPos(pos);

			_grid[updatedInternalPos.y][updatedInternalPos.x] = tileGO;
			
			// Guarantees well-formedness just to be safe
			RemoveSpace();
		}

		public void UpdateTileBoardInfo()
		{
			foreach (TileGOs tileGOs in _grid)
			{
				tileGOs.SetTileBoardInfo(this);
			}
		}

		/// <summary>
		/// Returns the internal position that corresponds to the logical origin.
		/// </summary>
		/// <returns>
		/// The internal position that corresponds to the logical origin.
		/// </returns>
		private Vector2Int GetInternalOrigin()
		{
			return _internalOrigin;
		}

		/// <summary>
		/// Converts the given external position to the internal one. The return value
		/// does not get updated if the internal origin changes.
		/// </summary>
		/// <param name="pos">the external position</param>
		/// <returns>the corresponding internal position</returns>
		private Vector2Int ExternalToInternalPos(Vector2Int pos)
		{
			return GetInternalOrigin() + pos;
		}

		/// <summary>
		/// Makes horizontal and vertical space for the given position.
		/// </summary>
		/// <param name="pos">the internal position to make space for</param>
		/// <remarks>
		/// If there is a top row or a left column added, then the internal origin
		/// must be updated.
		/// </remarks>
		private void MakeSpace(Vector2Int pos)
		{
			// Order matters because if there are no rows, you cannot extend
			// horizontally!
			MakeVerticalSpace(pos.y);
			MakeHorizontalSpace(pos.x);	
		}

		/// <summary>
		/// Creates horizontal space if the given x-coordinate is below 0
		/// or at least the current logical width.
		/// </summary>
		/// <param name="xPos">represents the x-coordinate to make space for</param>
		private void MakeHorizontalSpace(int xPos)
		{
			if (xPos < 0)
			{
				ExtendLeftColumn(Math.Abs(xPos));
			}
			else if (xPos >= GetLogicalWidth())
			{
				// Since logical width is the max index + 1,
				// max index is logical width - 1.
				// Number to extend is xPos - max index.
				ExtendRightColumn(xPos - (GetLogicalWidth() - 1));	
			}
		}

		/// <summary>
		/// Creates vertical space if the given y-coordinate is below
		/// or at least the current logical height.
		/// </summary>
		/// <param name="yPos">represents the y-coordinate to make space for</param>
		private void MakeVerticalSpace(int yPos)
		{
			if (yPos < 0)
			{
				ExtendTopRow(Math.Abs(yPos));
			}
			else if (yPos >= GetLogicalHeight())
			{
				ExtendBottomRow(yPos - (GetLogicalHeight() - 1));
			}
		}

		/// <summary>
		/// Extends the top row of the internal grid by the
		/// given number
		/// </summary>
		/// <param name="num">the quantity to extend</param>
		private void ExtendTopRow(int num)
		{
			for (int count = 0; count < num; count++)
			{
				_grid.Insert(0, new TileGOs(Enumerable.Repeat<GameObject>(null, GetLogicalWidth()).ToList()));
			}
			
			_internalOrigin = new Vector2Int(_internalOrigin.x, _internalOrigin.y + num);
		}

		/// <summary>
		/// Extends the bottom row of the internal grid by the
		/// given number
		/// </summary>
		/// <param name="num">the quantity to extend</param>
		private void ExtendBottomRow(int num)
		{
			for (int count = 0; count < num; count++)
			{
				_grid.Add(new TileGOs(Enumerable.Repeat<GameObject>(null, GetLogicalWidth()).ToList()));
			}
		}

		/// <summary>
		/// Extends the left column of the internal grid by the
		/// given number
		/// </summary>
		/// <param name="num">the quantity to extend</param>
		private void ExtendLeftColumn(int num)
		{
			for (int r = 0; r < GetLogicalHeight(); r++)
			{
				for (int count = 0; count < num; count++)
				{
					_grid[r].Insert(0, null);
				}
			}

			_internalOrigin = new Vector2Int(_internalOrigin.x + num, _internalOrigin.y);
		}

		/// <summary>
		/// Extends the right column of the internal grid by the
		/// given number
		/// </summary>
		/// <param name="num">the quantity to extend</param>
		private void ExtendRightColumn(int num)
		{
			for (int r = 0; r < GetLogicalHeight(); r++)
			{
				_grid[r].AddRange(Enumerable.Repeat<GameObject>(null, num));
			}
		}
		
		/// <inheritdoc cref="ISerializableBoard.RemoveTileAt"/>
		/// <remarks>
		/// The logical invariant that the edge-most rows and columns
		/// have at least one non-null value is preserved because if there
		/// is a null-only edge-most rows and columns after removing the
		/// given position's tile from the internal grid, the extra space
		/// on the edges will be removed.
		/// </remarks>
		public void RemoveTileAt(Vector2Int pos)
		{
			Vector2Int internalPos = ExternalToInternalPos(pos);
			_grid[internalPos.y][internalPos.x] = null;
			RemoveSpace();
		}

		public GameObject GetTileAt(Vector2Int pos)
		{
			if (_grid.Count == 0)
			{
				return null;
			}
			
			Vector2Int internalPos = ExternalToInternalPos(pos);
			
			// If out of range, then it is null
			if (internalPos.x < 0 || internalPos.x >= GetLogicalWidth() ||
			    internalPos.y < 0 || internalPos.y >= GetLogicalHeight())
			{
				return null;
			}

			return _grid[internalPos.y][internalPos.x];
		}

		/// <summary>
		/// Removes the edge-most rows and columns in the internal grid with
		/// only null values.
		/// </summary>
		/// <remarks>
		/// When the left-most and top-most columns and rows are removed,
		/// the internal origin must change accordingly.
		/// </remarks>
		private void RemoveSpace()
		{
			// Vertical

			// From the top
			while (_grid.Count > 0 && _grid[0].All(x => x == null))
			{
				_grid.RemoveAt(0);
				_internalOrigin = new Vector2Int(_internalOrigin.x, _internalOrigin.y - 1);
			}
			
			// From the bottom
			while (_grid.Count > 0 && _grid[_grid.Count - 1].All(x => x == null))
			{
				_grid.RemoveAt(_grid.Count - 1);
			}
			
			// Horizontal
			
			// From the left
			while (_grid.Count > 0 && ColumnAll(0, x => x == null))
			{
				RemoveLeftMostColumn();
			}
			
			while (_grid.Count > 0 && ColumnAll(GetLogicalWidth() - 1, x => x == null))
			{
				RemoveRightMostColumn();
			}
		}

		/// <summary>
		/// Removes the left most column.
		/// </summary>
		private void RemoveLeftMostColumn()
		{
			for (int r = 0; r < GetLogicalHeight(); r++)
			{
				_grid[r].RemoveAt(0);
			}
			
			_internalOrigin = new Vector2Int(_internalOrigin.x - 1, _internalOrigin.y);
		}

		/// <summary>
		/// Removes the right most column.
		/// </summary>
		private void RemoveRightMostColumn()
		{
			int index = GetLogicalWidth() - 1;
			
			for (int r = 0; r < GetLogicalHeight(); r++)
			{
				_grid[r].RemoveAt(index);
			}
		}

		private bool ColumnAll(int colIndex, Func<GameObject, bool> pred)
		{
			// If you find an element that does not meet the predicate,
			// then the column does not meet the predicate
			for (int r = 0; r < GetLogicalHeight(); r++)
			{
				if (!pred(_grid[r][colIndex]))
				{
					return false;
				}
			}

			return true;
		}

		public void InitializeAllUnits(ITurnManager turnManager)
		{
			foreach (TileGOs tileGOs in _grid)
			{
				tileGOs.InitializeAllUnits(turnManager);
			}
		}
		
		public void DeleteUnit()
		{
			foreach (TileGOs tileGOs in _grid)
			{
				tileGOs.DeleteUnit();
			}
		}

		public bool AllAllies()
		{
			foreach (TileGOs tileGOs in _grid)
			{
				if (!tileGOs.AllAllies())
				{
					return false;
				}
			}
			return true;
		}

		public bool AreTheyDead(List<string> names)
		{
			foreach (TileGOs tileGOs in _grid)
			{
				if (tileGOs.AreTheyDead(names))
				{
					return true;
				}
			}
			return false;
		}
	}
}