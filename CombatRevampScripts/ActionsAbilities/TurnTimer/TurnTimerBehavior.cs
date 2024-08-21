namespace CombatRevampScripts.ActionsAbilities.TurnTimer
{
    /// <summary>
    /// Indicates to an IEffectTrigger whether it should call advance its ITurnTimer at the start or end of its
    /// ICombatUnit's turn.
    /// </summary>
    public enum TurnTimerBehavior
    {
        StartOfTurn,
        EndOfTurn
    }
}