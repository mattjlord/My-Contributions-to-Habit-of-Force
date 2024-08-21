using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.CombatVisuals.VFXController;
using System;
using System.Collections;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.Handler
{
    public class CombatVisualHandler : MonoBehaviour, ICombatVisualHandler
    {
        private Animator _animator;

        private bool _isBusy;

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _isBusy = false;
        }

        public bool IsBusy()
        {
            return _isBusy;
        }

        public void PlayAnim(string name, float busyPeriod)
        {
            _animator.Play(name, 0);
            if (busyPeriod > 0)
            {
                _isBusy = true;
                StartCoroutine(WaitForAnimBusyPeriod(busyPeriod));
            }
        }

        public void PlayAnimIntoAction(string name, float busyPeriod, Action actionOnComplete)
        {
            _animator.Play(name, 0);
            if (busyPeriod > 0)
            {
                _isBusy = true;
                StartCoroutine(WaitForAnimBusyPeriodIntoAction(busyPeriod, actionOnComplete));
            }
            else
            {
                actionOnComplete.Invoke();
            }
        }

        public IVFXController PlayVFXObject(GameObject vfxObject, IEffectHolder effectHolder)
        {
            IVFXController vfxController = vfxObject.GetComponent<IVFXController>();
            if (vfxController != null)
            {
                _isBusy = true;
                GameObject vfxObjectInstance = Instantiate(vfxObject);
                IVFXController vfxControllerInstance = vfxObjectInstance.GetComponent<IVFXController>();
                vfxControllerInstance.SetupFromEffectHolder(effectHolder);
                StartCoroutine(WaitForVFXBusyPeriod(vfxControllerInstance));
                return vfxControllerInstance;
            }
            return null;
        }

        public IVFXController PlayVFXObjectIntoAction(GameObject vfxObject, IEffectHolder effectHolder, Action actionOnComplete)
        {
            IVFXController vfxController = vfxObject.GetComponent<IVFXController>();
            if (vfxController != null)
            {
                Debug.Log("Playing VFX: " + vfxObject.name);
                _isBusy = true;
                GameObject vfxObjectInstance = Instantiate(vfxObject);
                IVFXController vfxControllerInstance = vfxObjectInstance.GetComponent<IVFXController>();
                vfxControllerInstance.SetupFromEffectHolder(effectHolder);
                StartCoroutine(WaitForVFXBusyPeriodIntoAction(vfxControllerInstance, actionOnComplete));
                return vfxControllerInstance;
            }
            return null;
        }

        public IVFXController PlayVFXObject(GameObject vfxObject, ICombatUnit unit)
        {
            IVFXController vfxController = vfxObject.GetComponent<IVFXController>();
            if (vfxController != null)
            {
                GameObject vfxObjectInstance = Instantiate(vfxObject);
                IVFXController vfxControllerInstance = vfxObjectInstance.GetComponent<IVFXController>();
                vfxControllerInstance.SetTransformFromUnit(unit);
                return vfxControllerInstance;
            }
            return null;
        }

        public void SetAnimatorBool(string name, bool value)
        {
            _animator.SetBool(name, value);
        }

        private IEnumerator WaitForAnimBusyPeriod(float busyPeriod)
        {
            //Debug.Log("Animation Starting Busy Period with duration " + busyPeriod);
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < busyPeriod)
            {
                //Debug.Log("Playing Animation: BUSY");
                yield return null;
            }
            //Debug.Log("Animation Ending Busy Period");
            _isBusy = false;
        }

        private IEnumerator WaitForAnimBusyPeriodIntoAction(float busyPeriod, Action actionOnComplete)
        {
            // Debug.Log("Animation Starting Busy Period with duration " + busyPeriod);
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < busyPeriod)
            {
                // Debug.Log("Playing Animation: BUSY");
                yield return null;
            }
            // Debug.Log("Animation Ending Busy Period");
            _isBusy = false;
            actionOnComplete.Invoke();
        }

        private IEnumerator WaitForVFXBusyPeriod(IVFXController vfxController)
        {
            while (vfxController.IsBusy())
            {
                yield return null;
            }

            vfxController.OnBusyPeriodEnd();
            _isBusy = false;
        }

        private IEnumerator WaitForVFXBusyPeriodIntoAction(IVFXController vfxController, Action actionOnComplete)
        {
            while (vfxController.IsBusy())
            {
                yield return null;
            }

            vfxController.OnBusyPeriodEnd();
            _isBusy = false;
            actionOnComplete.Invoke();
        }
    }
}