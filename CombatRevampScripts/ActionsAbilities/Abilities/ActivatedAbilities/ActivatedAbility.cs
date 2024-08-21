using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.Effects;

namespace CombatRevampScripts.ActionsAbilities.Abilities.ActivatedAbilities
{
    /// <summary>
    /// Represents a type of CombatAction that is also an IAbility with a Clarity Cost. The Clarity Cost is applied when the ICombatEffect is triggered.
    /// </summary>
    public class ActivatedAbility : CombatAction, IAbility
    {
        private AbilityType _abilityType;
        private float _clarityCost;
        public ActivatedAbility(string name, int numTargets, int range, TargetingRangeSource targetingRangeSource, 
            TargetingRules targetingRules, bool askForDamageType, AbilityType abilityType, float clarityCost)
            : base(name, ActionType.ActivatedAbility, numTargets, range, targetingRangeSource, targetingRules, askForDamageType)
        {
            _abilityType = abilityType;
            _clarityCost = clarityCost;
        }

        public override bool IsClarityCostMet()
        {
            float currClarity = GetCombatUnit().GetMechSO().GetFloatStatValue("clarity");
            return currClarity >= _clarityCost;
        }

        public override float GetClarityCost()
        {
            return _clarityCost;
        }

        /// <summary>
        /// Does the effect of this, and additionally does the Clarity cost
        /// </summary>
        public override void DoEffectsOnly()
        {
            DoClarityCost();
            base.DoEffectsOnly();
        }

        /// <summary>
        /// Performs the required Clarity cost of this ability
        /// </summary>
        public void DoClarityCost()
        {
            GetCombatUnit().GetMechSO().clarity -= _clarityCost;
        }

        public ICombatAction GetThisAsAction()
        {
            return this;
        }


        public AbilityType GetAbilityType()
        {
            return _abilityType;
        }

        public void SetAbilityType(AbilityType abilityType)
        {
            _abilityType = abilityType;
        }
    }
}
