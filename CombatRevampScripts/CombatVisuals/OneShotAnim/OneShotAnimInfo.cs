using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.OneShotAnim
{
    [System.Serializable]
    public struct OneShotAnimInfo
    {
        public string animationStateName;
        [Range(0, 1)]
        public float busyPeriod;

        public OneShotAnimInfo(string animationStateName, float busyPeriod)
        {
            this.animationStateName = animationStateName;
            this.busyPeriod = busyPeriod;
        }
    }
}