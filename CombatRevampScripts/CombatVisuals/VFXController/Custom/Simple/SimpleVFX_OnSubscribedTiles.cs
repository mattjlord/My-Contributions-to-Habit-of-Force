using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.Board.Tile;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_OnSubscribedTiles : AVFXController
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
            List<ITile> subscribedTiles = effectTrigger.GetSubscribedTiles();
            if (subscribedTiles.Count == 0) { return; }
            createdObjects = new List<GameObject>();
            SetupObject(gameObject, subscribedTiles[0]);
            if (subscribedTiles.Count > 1)
            {
                for (int i = 1; i < subscribedTiles.Count; i++)
                {
                    GameObject copy = Instantiate(gameObject);
                    SetupObject(copy, subscribedTiles[i]);
                }
            }
        }

        private void SetupObject(GameObject obj, ITile tile)
        {
            createdObjects.Add(obj);
            obj.transform.position = tile.GetWorldPosition();
            if (_stickToParent)
            {
                Tile tileAsComponent = (Tile)tile;
                obj.transform.parent = tileAsComponent.transform;
            }    
        }
    }
}