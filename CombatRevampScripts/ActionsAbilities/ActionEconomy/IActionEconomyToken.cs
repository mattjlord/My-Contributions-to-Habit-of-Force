namespace CombatRevampScripts.ActionsAbilities.ActionEconomy
{
    /// <summary>
    /// Represents a token that is expended from an IActionEconomy when an ICombatAction is taken in an ICombatUnit's turn.
    /// </summary>
    public interface IActionEconomyToken
    {
        /// <summary>
        /// Adds the given ActionType to this list of Allowed Action Types, if it isn't already there
        /// </summary>
        /// <param name="actionType">the ActionType to allow</param>
        public void AllowActionType(ActionType actionType);

        /// <summary>
        /// Removes the given ActionType from this list of Allowed Action Types, if it exists in said list
        /// </summary>
        /// <param name="actionType">the ActionType to disallow</param>
        public void DisallowActionType(ActionType actionType);

        /// <summary>
        /// Sets this list of Allowed Action Types to only include the given ActionType
        /// </summary>
        /// <param name="actionType">the ActionType to allow</param>
        public void OnlyAllowActionType(ActionType actionType);

        /// <summary>
        /// Sets this list of Allowed Action Types to all ActionTypes
        /// </summary>
        public void AllowAllActionTypes();

        /// <summary>
        /// Checks if the given ActionType is present in this list of Allowed Action Types
        /// </summary>
        /// <param name="actionType">the ActionType to check</param>
        /// <returns>whether or not the given ActionType is present</returns>
        public bool IsActionTypeAllowed(ActionType actionType);

        /// <summary>
        /// Returns whether this token only allows one type of action, that isn't an ActivatedAbility
        /// </summary>
        public bool IsOnlyOneActionAllowed();

        /// <summary>
        /// If IsOnlyOneActionAllowed() = true, returns the one allowed ActionType, otherwise throws an error.
        /// </summary>
        public ActionType GetSingleAllowedAction();
    }
}
