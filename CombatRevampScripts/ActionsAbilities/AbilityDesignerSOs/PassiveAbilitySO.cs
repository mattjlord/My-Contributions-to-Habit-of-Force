using System;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.Abilities;
using CombatRevampScripts.ActionsAbilities.Abilities.PassiveAbilities;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using UnityEditor;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.AbilitySOs
{
    /// <summary>
    /// Represents an AAbilityDesignerSO that builds into a PassiveAbility.
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Passive Ability")]
    public class PassiveAbilitySO : AAbilityDesignerSO<PassiveAbility>
    {
        public string abilityName;
        public string abilityDescription = "Insert Description Here";
        public AbilityType type;
        [SerializeField] public PassiveVisualInfo passiveVisualInfo;
        [SerializeField] public List<EffectTriggerStruct> effectTriggers;
        [SerializeField] public List<CombatPassiveStruct> savedPassives;

        public override PassiveAbility Build(ITurnManager turnManager)
        {
            PassiveAbility newAbility = new PassiveAbility(abilityName, type);
            foreach (EffectTriggerStruct effectTriggerStruct in effectTriggers)
            {
                EffectTrigger effectTrigger = effectTriggerStruct.Build(turnManager);
                foreach (CombatPassiveStruct passive in savedPassives)
                {
                    effectTrigger.AddSavedPassive(passive);
                }
                newAbility.AddEffectTrigger(effectTrigger);
            }

            newAbility.SetVisualInfo(passiveVisualInfo);
            newAbility.SetDescription(abilityDescription);
            return newAbility;
        }
    }
}
