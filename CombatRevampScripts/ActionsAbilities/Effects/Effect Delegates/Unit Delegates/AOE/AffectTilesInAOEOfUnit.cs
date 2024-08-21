using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that gets and then affects tiles in an AOE around the subject unit
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/AOE/Affect Tiles In AOE of Unit")]
    public class AffectTilesInAOEOfUnit : ATileAOEDelegate<ICombatUnit>
    {
        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            ICombatUnit source = effect.GetOwner();
            List<ITile> aoeTiles = subject.GetTilesInAOE(source, GetAOERange(), selfInclusive, relativeAffiliation, tileStatus, pathfindingMode);
            InvokeOnSubjects(aoeTiles, effect);
        }
    }
}