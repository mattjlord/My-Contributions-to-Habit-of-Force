using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VFXController
{
    public abstract class AVFXController : MonoBehaviour, IVFXController
    {
        [SerializeField] private float _lifetime;

        private float _startTime;

        public virtual void Start()
        {
            _startTime = Time.time;
        }
        public virtual void FixedUpdate()
        {
            if (_lifetime > 0)
            {
                if (Time.fixedDeltaTime >= _startTime + _lifetime)
                {
                    Destroy();
                }
            }
        }

        public void SetTransformFromUnit(ICombatUnit unit)
        {
            Transform parent = unit.GetTilePiece().transform;
            if (parent != null)
            {
                transform.position = parent.position;
                transform.parent = parent;
            }
        }

        public abstract void Destroy();

        public abstract bool IsBusy();

        public abstract void OnBusyPeriodEnd();

        public abstract void SetupFromEffectHolder(IEffectHolder effectHolder);
    }
}