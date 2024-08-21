using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;
using UnityEngine.VFX;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_AOEOnThisUnit : AVFXController
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

            ICombatUnit unit = effectHolder.GetCombatUnit();
            if (unit != null)
            {
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