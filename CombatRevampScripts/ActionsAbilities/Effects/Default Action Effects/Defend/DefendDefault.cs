using CombatRevampScripts.ActionsAbilities.CombatPassives;

namespace CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Defend
{
    public class DefendDefault : ACombatEffect
    {
        public override void DoDefaultEffect()
        {
            ICombatPassive defendPassive = GetPassiveByName("Defending");
            assignedUnit.AddAssignedPassive(defendPassive, ownerUnit, false);
        }
    }
}