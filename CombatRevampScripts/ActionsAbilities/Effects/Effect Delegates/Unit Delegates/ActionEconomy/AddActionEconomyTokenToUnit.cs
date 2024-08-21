using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// Adds a new token with the given allowed action types to the subject unit's Action Economy
    /// </summary>
    [CreateAssetMenu (menuName = "Ability Designer/Effect Delegates/CombatUnit/Action Economy/Add a Token to Unit's Action Economy")]
    public class AddActionEconomyTokenToUnit : AEffectDelegate<ICombatUnit>
    {
        public List<ActionType> allowedActionTypes;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            subject.GetActionEconomy().AddActionEconomyToken(new ActionEconomyToken(allowedActionTypes));
        }
    }
}