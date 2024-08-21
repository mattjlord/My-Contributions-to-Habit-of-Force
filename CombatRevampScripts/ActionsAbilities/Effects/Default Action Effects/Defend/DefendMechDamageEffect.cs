using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Defend
{
    public class DefendMechDamageEffect : ACombatEffect
    {
        public override void DoDefaultEffect()
        {
            float defense = assignedUnit.GetMechSO().GetFloatStatValue("defense");
            float speed = assignedUnit.GetMechSO().GetFloatStatValue("speed");
            float reduction = 65 + ((2 * defense + speed / 2) / 4);
            float amountToHeal = (reduction / 100) * eventFloat;
            assignedUnit.AddMechHealth(amountToHeal, true);
        }
    }
}