using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.ActionsAbilities.Structs.Events;
using CombatRevampScripts.ActionsAbilities.Structs.Predicates;
using CombatRevampScripts.ActionsAbilities.Structs.Predicates.Custom_Predicates;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers
{
    [System.Serializable ]
    public struct EffectTriggerStruct
    {
        public List<TurnManagerEventType> eventTriggersFromTurnManager;
        public List<CombatUnitEventType> eventTriggersFromThisUnit;
        public List<CombatUnitEventType> eventTriggersFromSpecifiedUnits;
        public List<TileEventType> eventTriggersFromSpecifiedTiles;

        [SerializeField] public UnitRelativeAffiliationPred senderUnitAffiliationPredicate;
        [SerializeField] public UnitDistanceWithinPred senderUnitDistanceWithinPredicate;
        [SerializeField] public List<UnitStatPred> senderUnitStatPredicates;

        //public MonoScript effectScript;
        public List<ACombatEffect> effects;
        // public AbilityDictionary ability;

        [SerializeField] public EffectVisualInfo visualInfo;
        public GameObject ongoingVFX;

        public EffectTrigger Build(ITurnManager turnManager)
        {
            EffectTrigger newEffectTrigger = new EffectTrigger();

            foreach (ACombatEffect effect in effects)
            {
                effect.Initialize();
                newEffectTrigger.AddEffect(effect);
            }

            if (eventTriggersFromThisUnit != null)
            {
                foreach (CombatUnitEventType eventType in eventTriggersFromThisUnit)
                {
                    newEffectTrigger.CacheThisUnitEventType(eventType);
                }
            }
            if (eventTriggersFromTurnManager != null)
            {
                foreach (TurnManagerEventType eventType in eventTriggersFromTurnManager)
                {
                    newEffectTrigger.SubscribeToEvent(turnManager, eventType.ToString());
                }
            }
            if (eventTriggersFromSpecifiedUnits != null)
            {
                foreach (CombatUnitEventType eventType in eventTriggersFromSpecifiedUnits)
                {
                    newEffectTrigger.CacheUnitEventType(eventType);
                }
            }
            if (eventTriggersFromSpecifiedTiles != null)
            {
                foreach (TileEventType eventType in eventTriggersFromSpecifiedTiles)
                {
                    newEffectTrigger.CacheTileEventType(eventType);
                }
            }

            if (senderUnitAffiliationPredicate.relativeAffiliation != RelativeAffiliation.Any)
            {
                newEffectTrigger.AddUnitPredicate(senderUnitAffiliationPredicate);
            }
            if (senderUnitDistanceWithinPredicate.distance != 0)
            {
                newEffectTrigger.AddUnitPredicate(senderUnitDistanceWithinPredicate);
            }
            if (senderUnitStatPredicates != null)
            {
                foreach (UnitStatPred pred in senderUnitStatPredicates)
                {
                    newEffectTrigger.AddUnitPredicate(pred);
                }
            }

            newEffectTrigger.SetEffectVisualInfo(visualInfo);

            if (ongoingVFX != null)
            {
                newEffectTrigger.SetOngoingVFX(ongoingVFX);
            }

            return newEffectTrigger;
        }
    }
}