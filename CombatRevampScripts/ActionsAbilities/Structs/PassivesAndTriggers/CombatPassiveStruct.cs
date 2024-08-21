using CombatRevampScripts.ActionsAbilities.Abilities.PassiveAbilities;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers
{
    [System.Serializable]
    public struct CombatPassiveStruct
    {
        public string passiveName;
        public string passiveDescription;
        public int duration;
        public TurnTimerBehavior turnTimerBehavior;
        public int charges;
        [SerializeField] public PassiveVisualInfo passiveVisualInfo;
        [SerializeField] public List<EffectTriggerStruct> effectTriggers;
        public List<ACombatEffect> releaseEffects;
        [SerializeField] public List<CombatPassiveStruct> savedPassives;

        public CombatPassive Build(ITurnManager turnManager)
        {
            if (passiveDescription == null)
            {
                passiveDescription = "Insert Description Here";
            }

            CombatPassive newPassive = new CombatPassive(passiveName, duration, charges, turnTimerBehavior);

            foreach (EffectTriggerStruct effectTriggerStruct in effectTriggers)
            {
                EffectTrigger effectTrigger = effectTriggerStruct.Build(turnManager);
                foreach (CombatPassiveStruct passive in savedPassives)
                {
                    effectTrigger.AddSavedPassive(passive);
                }
                newPassive.AddEffectTrigger(effectTrigger);
            }

            newPassive.SetVisualInfo(passiveVisualInfo);

            foreach (ACombatEffect releaseEffect in releaseEffects)
            {
                releaseEffect.Initialize();
                newPassive.AddReleaseEffect(releaseEffect);
            }

            newPassive.SetDescription(passiveDescription);
            return newPassive;
        }
    }
}