using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_OnSubscribedUnits : AVFXController
    {
        [SerializeField] private bool _stickToParent;

        private List<GameObject> createdObjects;

        public override void Destroy()
        {
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                GameObject obj = createdObjects[i];
                //obj.transform.SetParent(null);
                Destroy(obj);
            }
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
            IEffectTrigger effectTrigger = (IEffectTrigger)effectHolder;
            List<ICombatUnit> subscribedUnits = effectTrigger.GetSubscribedUnits();
            if (subscribedUnits.Count == 0) { return; }
            createdObjects = new List<GameObject>();
            SetupObject(gameObject, subscribedUnits[0]);
            if (subscribedUnits.Count > 1)
            {
                for (int i = 1; i < subscribedUnits.Count; i++)
                {
                    GameObject copy = Instantiate(gameObject);
                    SetupObject(copy, subscribedUnits[i]);
                }
            }
        }

        private void SetupObject(GameObject obj, ICombatUnit unit)
        {
            createdObjects.Add(obj);
            MechTilePiece tilePiece = unit.GetTilePiece();
            obj.transform.position = tilePiece.transform.position;
            if (_stickToParent)
            {
                obj.transform.parent = tilePiece.transform;
            }
        }
    }
}