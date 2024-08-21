using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_OnThisUnit : AVFXController
    {
        [SerializeField] private bool _stickToParent;

        public override void Destroy()
        {
            transform.SetParent(null);
            Destroy(gameObject);
        }

        public override bool IsBusy()
        {
            return false;
        }

        public override void OnBusyPeriodEnd()
        {
            // Nothing (never busy)
        }

        public override void SetupFromEffectHolder(IEffectHolder effectHolder)
        {
            ICombatUnit unit = effectHolder.GetCombatUnit();
            MechTilePiece tilePiece = unit.GetTilePiece();
            GameObject tpGO = tilePiece.gameObject;
            transform.position = tpGO.transform.position;
            if (_stickToParent)
            {
                transform.SetParent(tpGO.transform);
            }
        }
    }
}