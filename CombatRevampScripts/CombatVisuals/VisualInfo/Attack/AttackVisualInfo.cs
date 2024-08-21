using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VisualInfo
{
    [System.Serializable]
    public struct AttackVisualInfo
    {
        [Range(0, 1)]
        public float ballisticAttackBusyPeriod;
        [Range(0, 1)]
        public float laserAttackBusyPeriod;
        public GameObject ballisticAttackPreVFX;
        public GameObject ballisticAttackPostVFX;
        public GameObject laserAttackPreVFX;
        public GameObject laserAttackPostVFX;
    }
}