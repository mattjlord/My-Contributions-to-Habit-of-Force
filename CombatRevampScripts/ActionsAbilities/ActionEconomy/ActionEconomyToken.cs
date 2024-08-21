using System.Collections.Generic;

namespace CombatRevampScripts.ActionsAbilities.ActionEconomy
{
    /// <summary>
    /// Represents a token that is expended from an IActionEconomy when an ICombatAction is taken in an ICombatUnit's turn.
    /// </summary>
    public class ActionEconomyToken : IActionEconomyToken
    {
        private List<ActionType> _allowedActionTypes;

        public ActionEconomyToken(List<ActionType> allowedActionTypes)
        {
            _allowedActionTypes = allowedActionTypes;
        }

        public ActionEconomyToken() 
        {
            _allowedActionTypes = new List<ActionType>();
        }

        public void AllowActionType(ActionType actionType)
        {
            if (_allowedActionTypes.Contains(actionType))
            {
                return;
            }
            _allowedActionTypes.Add(actionType);
        }

        public void DisallowActionType(ActionType actionType)
        {
            if (!_allowedActionTypes.Contains(actionType))
            {
                return;
            }
            _allowedActionTypes.Remove(actionType);
        }

        public void OnlyAllowActionType(ActionType actionType)
        {
            List<ActionType> newList = new List<ActionType>();
            newList.Add(actionType);
            _allowedActionTypes = newList;
        }

        public void AllowAllActionTypes()
        {
            List<ActionType> newList = new List<ActionType>();
            newList.Add(ActionType.Move);
            newList.Add(ActionType.Attack);
            newList.Add(ActionType.Defend);
            newList.Add(ActionType.Hail);
            newList.Add(ActionType.ActivatedAbility);
            _allowedActionTypes = newList;
        }

        public bool IsActionTypeAllowed(ActionType actionType)
        {
            return _allowedActionTypes.Contains(actionType);
        }

        public bool IsOnlyOneActionAllowed()
        {
            if (_allowedActionTypes.Count == 1)
            {
                 return _allowedActionTypes[0] != ActionType.ActivatedAbility;
            }
            return false;
        }

        public ActionType GetSingleAllowedAction()
        {
            if (IsOnlyOneActionAllowed())
            {
                return _allowedActionTypes[0];
            }
            throw new System.Exception("This ActionToken allows more than one action!");
        }
    }
}
