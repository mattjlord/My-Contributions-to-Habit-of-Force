using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that sets the specified amount for the temp health for the subject unit.
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Health/Overheal/Set Unit Overheal")]
    public class SetUnitOverheal : AHealthEffectDelegate
    {
        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            SetupNode(subject, effect);
            ProcessDamageType(effect);

            switch (damageType)
            {
                case DamageType.LaserDamage:
                    subject.SetMechTempHealth(valueNode.Compute());
                    break;
                case DamageType.BallisticDamage:
                    subject.SetPilotTempHealth(valueNode.Compute());
                    break;
            }
        }
    }
}