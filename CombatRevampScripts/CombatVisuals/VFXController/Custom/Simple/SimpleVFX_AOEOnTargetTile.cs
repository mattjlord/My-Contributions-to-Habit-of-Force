using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.Board.Tile;
using UnityEngine;
using UnityEngine.VFX;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_AOEOnTargetTile : AVFXController
    {
        [SerializeField] private VisualEffect _vfx;
        [SerializeField] private string _vfxAOEProperty;
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
            AOERangeProperty aoeRangeProperty = (AOERangeProperty)effectHolder.GetFirstPropertyOfType<int>(typeof(AOERangeProperty));

            if (aoeRangeProperty != null)
            {
                int aoeRange = aoeRangeProperty.GetValue();
                _vfx.SetFloat(_vfxAOEProperty, aoeRange);
            }

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