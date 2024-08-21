using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_OnTargetTile : AVFXController
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
            TargetTileProperty targetTileProperty = (TargetTileProperty)effectHolder.GetFirstPropertyOfType<ITile>(typeof(TargetTileProperty));
            if (targetTileProperty != null)
            {
                ITile tile = targetTileProperty.GetValue();
                Tile tileComponent = (Tile)tile;
                GameObject tileGO = tileComponent.gameObject;
                transform.position = tileGO.transform.position;
                if (_stickToParent)
                {
                    transform.SetParent(tileGO.transform);
                }
            }
        }
    }
}