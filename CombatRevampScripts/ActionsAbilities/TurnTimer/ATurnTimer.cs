namespace CombatRevampScripts.ActionsAbilities.TurnTimer
{
    public abstract class ATurnTimer : ITurnTimer
    {
        private int _timer;
        private TurnTimerBehavior _timerBehavior;

        public ATurnTimer(int timer, TurnTimerBehavior timerBehavior)
        {
            _timer = timer;
            _timerBehavior = timerBehavior;
        }

        public TurnTimerBehavior GetTimerBehavior()
        {
            return _timerBehavior;
        }

        public int GetTimerValue()
        {
            return _timer;
        }

        public void OnTurnStart()
        {
            if (_timer > 0 && _timerBehavior == TurnTimerBehavior.StartOfTurn)
            {
                AdvanceTimer();
            }
        }

        public void OnTurnEnd()
        {
            if (_timer > 0 && _timerBehavior == TurnTimerBehavior.EndOfTurn)
            {
                AdvanceTimer();
            }
        }

        private void AdvanceTimer()
        {
            if (_timer > 1) { _timer -= 1; }
            else if (_timer == 1) { OnTimerEnd(); }
        }

        public abstract void OnTimerEnd();
    }
}
