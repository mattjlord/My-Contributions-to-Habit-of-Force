using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using UnityEngine;

public class HailDefault : ACombatEffect
{
    protected bool success;
    public override void DoDefaultEffect()
    {
        float thisPilotHealth = assignedUnit.GetPilotSO().GetFloatStatValue("currHealth");
        float thisMechHealth = assignedUnit.GetMechSO().GetFloatStatValue("currHealth");
        float targetPilotHealth = targetUnit.GetPilotSO().GetFloatStatValue("currHealth");
        float targetMechHealth = targetUnit.GetMechSO().GetFloatStatValue("currHealth");
        float leadership = assignedUnit.GetPilotSO().GetFloatStatValue("leadership");

        bool targetPilotNotDamaged = targetPilotHealth / targetUnit.GetPilotSO().GetFloatStatValue("maxHealth") >= 0.9;

        success = ((thisPilotHealth + thisMechHealth) * leadership > (targetMechHealth - targetPilotHealth) * 1000) && targetPilotNotDamaged;
        //
        //success = true;
        switch(success)
        {
            case true:
                targetUnit.GetPilotSO().affiliation = assignedUnit.GetPilotSO().affiliation;
                // TODO: Remaining success effects
                break;
            case false: 
                // TODO: Failure effect
                break;
        }
    }

    public bool GetSuccess()
    {
        return success;
    }
}
