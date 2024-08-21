using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Extensions;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates
{
    /// <summary>
    /// An extension of the AAOEDelegate that includes the additional arguments needed
    /// for getting tiles in an AOE
    /// </summary>
    /// <typeparam name="T">the type of subject from which the AOE originates</typeparam>
    public abstract class ATileAOEDelegate<T> : AAOEDelegate<T, ITile>
    {
        public TileStatus tileStatus;
        public PathfindingMode pathfindingMode;
    }
}