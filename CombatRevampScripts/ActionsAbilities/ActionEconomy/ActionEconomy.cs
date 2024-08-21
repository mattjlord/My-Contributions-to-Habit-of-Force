using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.ActionEconomy
{
    /// <summary>
    /// Represents the rules for what ICombatActions can be taken in a turn by an ICombatUnit.
    /// </summary>
    public class ActionEconomy : IActionEconomy
    {
        private List<IActionEconomyToken> _actionEconomyTokens;

        public ActionEconomy()
        {
            Reset();
        }

        public void AddActionEconomyToken(IActionEconomyToken actionToken)
        {
            _actionEconomyTokens.Add(actionToken);
        }

        public bool CanDoAction(ICombatAction action)
        {
            ActionType actionType = action.GetActionType();

            List<IActionEconomyToken> newList = new List<IActionEconomyToken>();
            foreach (IActionEconomyToken actionToken in _actionEconomyTokens)
            {
                if (actionToken.IsActionTypeAllowed(actionType))
                {
                    newList.Add(actionToken);
                }
            }
            return newList.Count > 0;
        }

        public void DoAction(ICombatAction action)
        {
            ActionType actionType = action.GetActionType();
            for (int i = 0; i < _actionEconomyTokens.Count; i ++)
            {
                IActionEconomyToken actionToken = _actionEconomyTokens[i];
                if (actionToken.IsActionTypeAllowed(actionType))
                {
                    RemoveTokensAtAndBefore(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes the token at the given index from _actionEconomyTokens, as well as all tokens before it in the list.
        /// </summary>
        /// <param name="index">the index of the token to remove</param>
        private void RemoveTokensAtAndBefore(int index)
        {
            if (index == _actionEconomyTokens.Count - 1)
            {
                _actionEconomyTokens.Clear();
            }
            else
            {
                for (int i = index; i >= 0; i--)
                {
                    RemoveTokenAt(i);
                }
            }
        }

        /// <summary>
        /// If there is a value at the given index in _actionEconomyTokens, removes it
        /// </summary>
        /// <param name="index">the index to remove the value of</param>
        private void RemoveTokenAt(int index)
        {
            if (index < 0 || index >= _actionEconomyTokens.Count)
            {
                return;
            }

            if (_actionEconomyTokens[index] != null)
            {
                _actionEconomyTokens.RemoveAt(index);
            }
        }

        public void AllowActionType(ActionType actionType)
        {
            foreach (IActionEconomyToken token in _actionEconomyTokens)
            {
                token.AllowActionType(actionType);
            }
        }

        public void DisallowActionType(ActionType actionType)
        {
            foreach (IActionEconomyToken token in _actionEconomyTokens)
            {
                token.DisallowActionType(actionType);
            }
        }

        public void OnlyAllowActionType(ActionType actionType)
        {
            foreach (IActionEconomyToken token in _actionEconomyTokens)
            {
                token.OnlyAllowActionType(actionType);
            }
        }

        public void AllowAllActionTypes()
        {
            foreach (IActionEconomyToken token in _actionEconomyTokens)
            {
                token.AllowAllActionTypes();
            }
        }

        public void Reset()
        {
            _actionEconomyTokens = new List<IActionEconomyToken>();
            IActionEconomyToken moveTurnAction = new ActionEconomyToken();
            IActionEconomyToken mainTurnAction = new ActionEconomyToken();
            moveTurnAction.OnlyAllowActionType(ActionType.Move);
            mainTurnAction.AllowAllActionTypes();
            AddActionEconomyToken(moveTurnAction);
            AddActionEconomyToken(mainTurnAction);
        }

        public void Clear()
        {
            _actionEconomyTokens.Clear();
        }

        public bool IsEmpty()
        {
            return _actionEconomyTokens.Count == 0;
        }

        public bool CanDoActionAfterNextTurn(ICombatAction action)
        {
            List<IActionEconomyToken> newList = new List<IActionEconomyToken>();
            for (int i = 1; i < _actionEconomyTokens.Count; i++)
            {
                if (_actionEconomyTokens[i].IsActionTypeAllowed(action.GetActionType()))
                {
                    newList.Add(_actionEconomyTokens[i]);
                }
            }
            return newList.Count > 0;
        }

        public IActionEconomyToken GetNextToken()
        {
            if (!IsEmpty())
            {
                return _actionEconomyTokens[0];
            }
            return null;
        }

        public void RemoveNextToken()
        {
            if (!IsEmpty())
            {
                RemoveTokenAt(0);
            }
        }

        public List<IActionEconomyToken> GetAllTokens()
        {
            return _actionEconomyTokens;
        }
    }
}
