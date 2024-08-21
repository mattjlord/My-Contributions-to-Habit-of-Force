using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that gets and then affects tiles in an AOE around the subject tile
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/Tile/AOE/Affect Tiles In AOE of Tile")]
    public class AffectTilesInAOEOfTile : ATileAOEDelegate<ITile>
    {
        public override void Invoke(ITile subject, ICombatEffect effect)
        {
            ICombatUnit source = effect.GetOwner();
            List<ITile> aoeTiles = subject.GetTilesInAOE(source, GetAOERange(), selfInclusive, relativeAffiliation, tileStatus, pathfindingMode);
            InvokeOnSubjects(aoeTiles, effect);
        }
    }
}