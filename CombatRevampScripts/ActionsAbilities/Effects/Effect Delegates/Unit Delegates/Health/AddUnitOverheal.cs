using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that adjusts the specified amount to the temp health for the subject unit.
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Health/Overheal/Add Overheal to Unit")]
    public class AddUnitOverheal : AHealthEffectDelegate
    {
        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            SetupNode(subject, effect);
            ProcessDamageType(effect);

            switch (damageType)
            {
                case DamageType.LaserDamage:
                    subject.AddMechTempHealth(valueNode.Compute());
                    break;
                case DamageType.BallisticDamage:
                    subject.AddPilotTempHealth(valueNode.Compute());
                    break;
            }
        }
    }
}