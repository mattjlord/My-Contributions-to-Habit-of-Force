using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Tile_Delegates
{
    /// <summary>
    /// An effect delegate that gets and then affects units in an AOE around the subject tile
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/Tile/AOE/Affect Units In AOE of Tile")]
    public class AffectUnitsInAOEOfTile : AAOEDelegate<ITile, ICombatUnit>
    {
        public override void Invoke(ITile subject, ICombatEffect effect)
        {
            ICombatUnit source = effect.GetOwner();
            List<ICombatUnit> aoeUnits = subject.GetUnitsInAOE(source, GetAOERange(), selfInclusive, relativeAffiliation);
            InvokeOnSubjects(aoeUnits, effect);
        }
    }
}