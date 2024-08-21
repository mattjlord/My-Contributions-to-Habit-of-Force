using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Extensions;
using System;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Move
{
    public class MoveDefault : ACombatEffect
    {
        protected int distanceMoved;

        public override void Initialize()
        {
            base.Initialize();
            routeCallbackToEffect = true;
            overrideAdditionalEffectBehavior = true;
        }

        public override void DoDefaultEffect()
        {
            ITile thisTile = assignedUnit.GetTilePiece().GetComponentInParent<ITile>();

            Vector2Int targetPos = targetTile.GetBoardPosition();
            Vector2Int thisPos = thisTile.GetBoardPosition();
            distanceMoved = MathExtension.ManhattanDistance(thisPos, targetPos);
            GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastDistanceMoved, distanceMoved);

            Action afterMove = () =>
            {
                DoAdditionalEffect();
                callbackAction.Invoke();
            };

            assignedUnit.MoveTo(targetTile, assignedUnit.GetMechSO().GetIntStatValue("moveRange"), afterMove);
        }
    }
}
