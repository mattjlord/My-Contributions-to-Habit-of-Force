using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.Board.Board;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.Extensions
{
    public enum PathfindingMode
    {
        Standard,
        Movement
    }
    
    public static class PathfindingExtension
    {
        private static List<(Vector2Int, int, int, Vector2Int)> _prevClosedList;

        public static bool DoesPathExist(ICombatUnit source, ITile start, ITile destination, int range, PathfindingMode mode)
        {
            if (start == destination) { return true; }

            Vector2Int startPos = start.GetBoardPosition();
            Vector2Int finalPos = destination.GetBoardPosition();

            ISerializableObservableBoard board = start.GetBoard();

            List<(Vector2Int, int, int, Vector2Int)> openList = new List<(Vector2Int, int, int, Vector2Int)>();
            List<(Vector2Int, int, int, Vector2Int)> closedList = new List<(Vector2Int, int, int, Vector2Int)>();

            openList.Add((startPos, 0, 0, startPos));

            bool pathFound = false;

            while (openList.Count > 0 && !pathFound)
            {
                openList.Sort((a, b) => (a.Item2 + a.Item3).CompareTo(b.Item2 + b.Item3));

                var storedNode = openList[0];

                openList.RemoveAt(0);

                ITile currTile = board.GetTileAt(storedNode.Item1).GetComponent<ITile>();

                List<ITile> neighbors;

                if (mode == PathfindingMode.Movement)
                {
                    // TODO: Fix this later
                    //neighbors = currTile.GetNeighbors(source, false, RelativeAffiliation.Friendly, TileStatus.Any);
                    neighbors = currTile.GetNeighbors(source, false, RelativeAffiliation.Any, TileStatus.Empty);
                }
                else
                {
                    neighbors = currTile.GetNeighbors(source, false, RelativeAffiliation.Any, TileStatus.Any);
                }

                foreach (ITile neighbor in neighbors)
                {
                    Vector2Int neighborPos = neighbor.GetBoardPosition();

                    int gScore = 1 + storedNode.Item2;

                    if (gScore > range) { continue; }

                    int hScore = MathExtension.ManhattanDistance(neighborPos, finalPos);

                    if (neighborPos == finalPos)
                    {
                        closedList.Add(storedNode);
                        closedList.Add((neighborPos, gScore, hScore, storedNode.Item1));
                        pathFound = true;
                        break;
                    }

                    bool skip = false;

                    foreach (var node in openList)
                    {
                        if (node.Item1 == neighborPos && node.Item2 + node.Item3 < gScore + hScore)
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip) { continue; }

                    foreach (var node in closedList)
                    {
                        if (node.Item1 == neighborPos && node.Item2 + node.Item3 < gScore + hScore)
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (!skip)
                    {
                        openList.Add((neighborPos, gScore, hScore, storedNode.Item1));
                    }
                }

                if (pathFound)
                {
                    break;
                }

                closedList.Add(storedNode);
            }

            _prevClosedList = closedList;

            /*
            if (!pathFound)
            {
                Debug.Log("A path could not be found between tiles " + startPos + " & " + finalPos);
                
                //for (int i = 0; i < closedList.Count; i++)
                //{
                //    Debug.Log("----" + i + ": " + closedList[i].Item1);
                //}
            }*/

            return pathFound;
        }

        public static List<ITile> GetBestPath(ICombatUnit source, ITile start, ITile destination, int range, PathfindingMode mode)
        {
            if (!DoesPathExist(source, start, destination, range, mode)) { return null; }

            ISerializableObservableBoard board = start.GetBoard();

            List<ITile> path = new List<ITile> { destination };
            Vector2Int parentPos = _prevClosedList[_prevClosedList.Count - 1].Item4;

            _prevClosedList.RemoveAt(_prevClosedList.Count - 1);

            bool pathReconstructed = false;

            while (!pathReconstructed)
            {
                int tempIndex = -1;
                for (int i = 0; i < _prevClosedList.Count; i++)
                {
                    var node = _prevClosedList[i];

                    if (node.Item1 == parentPos)
                    {
                        tempIndex = i;
                        parentPos = node.Item4;

                        if (node.Item1 == start.GetBoardPosition()) { pathReconstructed = true; break; }
                        path.Add(board.GetTileAt(node.Item1).GetComponent<ITile>());
                        break;
                    }
                }

                if (tempIndex >= 0) 
                { 
                    _prevClosedList.RemoveAt(tempIndex); 
                }
                else
                {
                    // If I programmed this right, this error should literally never occur
                    throw new System.Exception("The path ran into a dead end during reconstruction!");
                }
            }

            path.Reverse();
            return path;
        }
    }
}