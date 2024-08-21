using System;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.Abilities;
using CombatRevampScripts.ActionsAbilities.Abilities.ActivatedAbilities;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using UnityEditor;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.AbilitySOs
{
    /// <summary>
    /// Represents an AAbilityDesignerSO that builds into an ActivatedAbility.
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Activated Ability")]
    public class ActivatedAbilitySO : AAbilityDesignerSO<ActivatedAbility>
    {
        public string abilityName;
        public string abilityDescription = "Insert Description Here";
        public AbilityType type;
        public int clarityCost;
        public int numberOfTargets;
        public int targetingRange;
        public TargetingRangeSource targetingRangeSource;
        public TargetingRules targetingRules;
        public bool askForDamageType;
        public List<ACombatEffect> effects;
        [SerializeField] public EffectVisualInfo visualInfo;
        [SerializeField] public List<CombatPassiveStruct> savedPassives;

        public override ActivatedAbility Build(ITurnManager turnManager)
        {
            ActivatedAbility ability = new ActivatedAbility(name, numberOfTargets, targetingRange, targetingRangeSource, targetingRules, 
                askForDamageType, type, clarityCost);

            foreach (ACombatEffect effect in effects)
            {
                effect.Initialize();
                ability.AddEffect(effect);
            }

            ability.SetEffectVisualInfo(visualInfo);

            foreach (CombatPassiveStruct passive in savedPassives)
            {
                ability.AddSavedPassive(passive);
            }
            ability.SetDescription(abilityDescription);

            return ability;
        }
    }
}
