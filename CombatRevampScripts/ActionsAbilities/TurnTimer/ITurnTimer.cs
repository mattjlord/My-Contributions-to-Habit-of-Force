namespace CombatRevampScripts.ActionsAbilities.TurnTimer
{
    /// <summary>
    /// Represents an object with a timer that advances each turn
    /// </summary>
    public interface ITurnTimer
    {
        /// <summary>
        /// Advances timers if the timer behavior is StartOfTurn.
        /// </summary>
        public void OnTurnStart();

        /// <summary>
        /// Advances timers if the timer behavior is EndOfTurn.
        /// </summary>
        public void OnTurnEnd();

        /// <summary>
        /// Returns the value of the timer
        /// </summary>
        /// <returns>the value of the timer</returns>
        public int GetTimerValue();

        /// <summary>
        /// Returns the behavior of this turn timer
        /// </summary>
        /// <returns>the behavior of the timer</returns>
        public TurnTimerBehavior GetTimerBehavior();

        /// <summary>
        /// Performs necessary actions for when this timer ends
        /// </summary>
        public void OnTimerEnd();
    }
}