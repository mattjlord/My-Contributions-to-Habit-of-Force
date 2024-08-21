using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// Adds a saved CombatPassive to the subject unit
    /// </summary>
    [CreateAssetMenu (menuName = "Ability Designer/Effect Delegates/CombatUnit/Passives/Add a Saved Passive to Unit")]
    public class AddSavedPassiveToUnit : AEffectDelegate<ICombatUnit>
    {
        public string savedPassiveName;
        public bool allowStacking;

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
                effect.AddBuiltPassive(newPassive);
                builtPassive = newPassive;
            }

            subject.AddAssignedPassive(builtPassive, effect.GetOwner(), allowStacking);
        }
    }
}