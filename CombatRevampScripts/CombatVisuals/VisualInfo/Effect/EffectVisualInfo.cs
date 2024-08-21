using CombatRevampScripts.CombatVisuals.OneShotAnim;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.VisualInfo
{
    [System.Serializable]
    public struct EffectVisualInfo
    {
        [SerializeField] public OneShotAnimInfo preEffectAnimation;
        [SerializeField] public OneShotAnimInfo postEffectAnimation;
        public GameObject preEffectVFX;
        public GameObject postEffectVFX;

        public EffectVisualInfo (OneShotAnimInfo preEffectAnim, OneShotAnimInfo postEffectAnim, GameObject preVFX, GameObject postVFX)
        {
            preEffectAnimation = preEffectAnim;
            postEffectAnimation = postEffectAnim;
            preEffectVFX = preVFX;
            postEffectVFX = postVFX;
        }
    }
}