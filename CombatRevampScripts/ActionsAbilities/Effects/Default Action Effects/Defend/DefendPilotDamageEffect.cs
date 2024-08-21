using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Defend
{
    public class DefendPilotDamageEffect : ACombatEffect
    {
        public override void DoDefaultEffect()
        {
            float defense = assignedUnit.GetMechSO().GetFloatStatValue("defense");
            float speed = assignedUnit.GetMechSO().GetFloatStatValue("speed");
            float reduction = 65 + ((2 * defense + speed / 2) / 4);
            //Debug.Log("* Defend triggered -- reducing damage by " + reduction + "%");
            float amountToHeal = (reduction / 100) * eventFloat;
            //Debug.Log("* Adding back health to this unit: " + amountToHeal);
            assignedUnit.AddPilotHealth(amountToHeal, true);

        }
    }
}