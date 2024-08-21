using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using UnityEngine;
using UnityEngine.VFX;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class BusyVFX_ProgressToTargetUnit : AVFXController
    {
        [SerializeField] private float _speed;
        [SerializeField] private string _progressPropertyName;
        [SerializeField] private string _originPropertyName;
        [SerializeField] private string _targetPropertyName;
        [SerializeField] private VisualEffect _vfx;

        private float _progress;

        public override void Start()
        {
            base.Start();
            _progress = 0;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            _vfx.SetFloat(_progressPropertyName, _progress);
            _progress += _speed;
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }

        public override bool IsBusy()
        {
            return _progress < 1;
        }

        public override void OnBusyPeriodEnd()
        {
            Destroy();
        }

        public override void SetupFromEffectHolder(IEffectHolder effectHolder)
        {
            ICombatUnit originUnit = effectHolder.GetCombatUnit();
            Vector3 originPos = originUnit.GetTilePiece().gameObject.transform.position;

            TargetUnitProperty targetUnitProperty = (TargetUnitProperty)effectHolder.GetFirstPropertyOfType<ICombatUnit>(typeof(TargetUnitProperty));

            ICombatUnit targetUnit = targetUnitProperty.GetValue();
            Vector3 targetPos = targetUnit.GetTilePiece().gameObject.transform.position;

            Transform overrideOrigin = originUnit.GetTilePiece().GetSpecialVFXOrigin();
            if (overrideOrigin != null)
            {
                originPos = overrideOrigin.position;
                targetPos.y = originPos.y;
            }

            _vfx.SetVector3(_originPropertyName, originPos);
            _vfx.SetVector3(_targetPropertyName, targetPos);
        }
    }
}