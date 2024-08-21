using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_OnTargetUnit : AVFXController
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
            TargetUnitProperty targetUnitProperty = (TargetUnitProperty)effectHolder.GetFirstPropertyOfType<ICombatUnit>(typeof(TargetUnitProperty));
            if (targetUnitProperty != null)
            {
                ICombatUnit unit = targetUnitProperty.GetValue();
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
}