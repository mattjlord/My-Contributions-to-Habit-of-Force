using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// Subscribes the given passive to the subject unit.
    /// </summary>
    [CreateAssetMenu (menuName = "Ability Designer/Effect Delegates/CombatUnit/Passives/Subscribe a Saved Passive to Unit")]
    public class SubscribeSavedPassiveToUnit : ASubscribeSavedPassiveDelegate<ICombatUnit>
    {
        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            ICombatPassive builtPassive = effect.GetBuiltPassiveByName(savedPassiveName);
            if (builtPassive == null)
            {
                ICombatPassive newPassive = effect.GetPassiveByName(savedPassiveName);
                if (newPassive == null)
                {
                    throw new System.Exception("A saved passive with the name " + savedPassiveName + " could not be found for the ability " +
                        effect.GetSource().GetName());
                }

                ICombatUnit effectOwner = effect.GetOwner();

                switch (instanceOptions)
                {
                    case PassiveInstanceOptions.AddANewInstanceToAssignedUnit:
                        newPassive.SubscribeToUnit(subject);
                        effect.GetAssignee().AddAssignedPassive(newPassive, effectOwner, true);
                        return;
                    case PassiveInstanceOptions.AddANewInstanceToOwnerUnit:
                        newPassive.SubscribeToUnit(subject);
                        effectOwner.AddAssignedPassive(newPassive, effectOwner, true);
                        return;
                    default:
                        effect.AddBuiltPassive(newPassive);
                        builtPassive = newPassive;
                        break;
                }
            }

            builtPassive.SubscribeToUnit(subject);
        }
    }
}