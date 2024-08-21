using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatActions;

namespace CombatRevampScripts.ActionsAbilities.ActionEconomy
{
    /// <summary>
    /// Represents the rules for what ICombatActions can be taken in a turn by an ICombatUnit.
    /// </summary>
    public interface IActionEconomy
    {
        /// <summary>
        /// Adds the given IActionEconomyToken to the Actions list for this
        /// </summary>
        /// <param name="turnAction">the IActionEconomyToken to add</param>
        public void AddActionEconomyToken(IActionEconomyToken turnAction);

        /// <summary>
        /// Checks if the Actions list for this contains any IActionEconomyTokens that allow the ActionType of the given action
        /// </summary>
        /// <param name="action">the ICombatAction to check</param>
        /// <returns>whether or not the Actions list for this contains any IActionEconomyTokens that allow the given action</returns>
        public bool CanDoAction(ICombatAction action);

        /// <summary>
        /// Produces necessary results in this IActionEconomy for when the given action is performed.
        /// </summary>
        /// <param name="action">the ICombatAction which is performed</param>
        public void DoAction(ICombatAction action);

        /// <summary>
        /// Allows the given ActionType for all IActionEconomyTokens in this economy.
        /// </summary>
        /// <param name="actionType">the ActionType to allow</param>
        public void AllowActionType(ActionType actionType);

        /// <summary>
        /// Disallows the given ActionType for all IActionEconomyTokens in this economy.
        /// </summary>
        /// <param name="actionType">the ActionType to disallow</param>
        public void DisallowActionType(ActionType actionType);

        /// <summary>
        /// Only allows the given ActionType for all IActionEconomyTokens in this economy.
        /// </summary>
        /// <param name="actionType">the ActionType to allow</param>
        public void OnlyAllowActionType(ActionType actionType);

        /// <summary>
        /// Allows all ActionTypes for all IActionEconomyTokens in this economy.
        /// </summary>
        public void AllowAllActionTypes();

        /// <summary>
        /// Resets this to its default values.
        /// </summary>
        public void Reset();

        /// <summary>
        /// Clears the list of tokens in this action economy.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Returns true if there are no tokens
        /// </summary>
        /// <returns>whether there are no tokens</returns>
        public bool IsEmpty();

        public bool CanDoActionAfterNextTurn(ICombatAction action);

        /// <summary>
        /// Returns the token at index 0
        /// </summary>
        /// <returns>the token at index 0</returns>
        public IActionEconomyToken GetNextToken();

        /// <summary>
        /// Removes the token at index 0
        /// </summary>
        public void RemoveNextToken();

        /// <summary>
        /// Returns the full list of tokens in this action economy
        /// </summary>
        /// <returns>a list of tokens in this action economy</returns>
        public List<IActionEconomyToken> GetAllTokens();
    }
}
