using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Extensions;

namespace CombatRevampScripts.ActionsAbilities.Structs.Predicates.Custom_Predicates
{

    [System.Serializable]
    public struct UnitDistanceWithinPred : ICustomPredicate<ICombatUnit>
    {
        public ICombatUnit thisUnit;
        public int distance;

        public void SetOwnerUnit(ICombatUnit combatUnit)
        {
            thisUnit = combatUnit;
        }

        public void SetEffectHolder(IEffectHolder effectHolder) { }

        public bool Test(ICombatUnit obj)
        {
            ITile thisTile = thisUnit.GetTilePiece().GetComponentInParent<ITile>();
            ITile objTile = obj.GetTilePiece().GetComponentInParent<ITile>();

            if (!PathfindingExtension.DoesPathExist(thisUnit, thisTile, objTile, distance, PathfindingMode.Standard)) { return false; }
            
            return PathfindingExtension.GetBestPath(thisUnit, thisTile, objTile, distance, PathfindingMode.Standard).Count <= distance;
        }
    }
}
