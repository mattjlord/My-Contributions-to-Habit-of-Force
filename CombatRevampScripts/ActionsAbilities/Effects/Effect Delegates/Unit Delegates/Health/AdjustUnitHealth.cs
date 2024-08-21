using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that adjusts the health of the unit by a specified amount.
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Health/Adjust Unit Health Without Events")]
    public class AdjustUnitHealth : AHealthEffectDelegate
    {
        public bool affectOverheal;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            SetupNode(subject, effect);
            ProcessDamageType(effect);

            switch (damageType)
            {
                case DamageType.LaserDamage:
                    subject.AddMechHealth(valueNode.Compute(), affectOverheal);
                    break;
                case DamageType.BallisticDamage:
                    subject.AddPilotHealth(valueNode.Compute(), affectOverheal);
                    break;
            }
        }
    }
}