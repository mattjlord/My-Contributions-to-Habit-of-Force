using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Abilities.PassiveAbilities
{
    /// <summary>
    /// Represents a CombatPassive that is also a type of IAbility
    /// </summary>
    public class PassiveAbility : CombatPassive, IAbility
    {
        private AbilityType _abilityType;

        public PassiveAbility(string name, AbilityType abilityType) : base(name)
        {
            _abilityType = abilityType;
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
