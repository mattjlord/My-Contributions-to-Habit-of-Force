using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using System.Collections.Generic;
using CombatRevampScripts.Extensions;

namespace CombatRevampScripts.ActionsAbilities.AOEBoardUtils
{
    /// <summary>
    /// Utility interface containing methods for getting lists of tiles or units in an area of effect.
    /// </summary>
    public interface IAOEBoardUtils
    {
        /// <summary>
        /// Gets a list of all tiles in an area of effect that match the given criteria.
        /// </summary>
        /// <param name="source">the unit that created this area of effect</param>
        /// <param name="radius">the radius of the area of effect</param>
        /// <param name="selfInclusive">whether or not to include the Tile at the center of the AOE in the area of effect</param>
        /// <param name="relativeAffiliation">if a MechTilePiece occupies a Tile in this area of effect, what should its affiliation relative to the source parameter's
        /// affiliation be in order for it to be included in the area of effect?</param>
        /// <param name="tileStatus">whether this area of effect should include occupied Tiles, empty Tiles, or both</param>
        /// <param name="pathfindingMode">the pathfinding mode to use for determing which tiles are reachable by this AOE</param>
        /// <returns>A list of ITiles that should be affected by this.</returns>
        public List<ITile> GetTilesInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation, 
            TileStatus tileStatus, PathfindingMode pathfindingMode);

        /// <summary>
        /// Gets a list of all tiles in an area of effect that match the given criteria.
        /// </summary>
        /// <param name="source">the unit that created this area of effect</param>
        /// <param name="radius">the radius of the area of effect</param>
        /// <param name="selfInclusive">whether or not to include the Tile at the center of the AOE in the area of effect</param>
        /// <param name="relativeAffiliation">what should the affiliation of a unit have to be relative to the affiliation of the source parameter
        /// in order for it to be included in the area of effect?</param>
        /// <returns>A list of ICombatUnits that should be affected by this.</returns>
        public List<ICombatUnit> GetUnitsInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation);
    }
}
