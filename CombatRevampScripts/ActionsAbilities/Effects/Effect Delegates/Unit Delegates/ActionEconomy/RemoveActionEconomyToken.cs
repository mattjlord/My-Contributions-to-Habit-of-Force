using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// Removes the first token from the Action Economy of subject unit that allows all of the given action types
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Action Economy/Remove the First Token from Unit's Action Economy that Allows Certain Action Types")]
    public class RemoveActionEconomyTokenFromUnit : AEffectDelegate<ICombatUnit>
    {
        public List<ActionType> allowedActionTypes;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            IActionEconomy economy = subject.GetActionEconomy();
            List<IActionEconomyToken> tokens = economy.GetAllTokens();

            foreach (IActionEconomyToken token in tokens)
            {
                bool skipToNext = false;
                foreach (ActionType actionType in allowedActionTypes)
                {
                    if (!token.IsActionTypeAllowed(actionType))
                    {
                        skipToNext = true;
                        break;
                    }
                }

                if (skipToNext) { continue; }

                tokens.Remove(token);
                break;
            }
        }
    }
}