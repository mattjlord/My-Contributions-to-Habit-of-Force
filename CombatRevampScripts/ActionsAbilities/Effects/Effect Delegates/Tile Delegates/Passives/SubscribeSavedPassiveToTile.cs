using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.Board.Tile;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Tile_Delegates
{
    /// <summary>
    /// Subscribes the given passive to the subject tile.
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/Tile/Subscribe a Saved Passive to Tile")]
    public class SubscribeSavedPassiveToTile : ASubscribeSavedPassiveDelegate<ITile>
    {
        public override void Invoke(ITile subject, ICombatEffect effect)
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
                        newPassive.SubscribeToTile(subject);
                        effect.GetAssignee().AddAssignedPassive(newPassive, effectOwner, true);
                        return;
                    case PassiveInstanceOptions.AddANewInstanceToOwnerUnit:
                        newPassive.SubscribeToTile(subject);
                        effectOwner.AddAssignedPassive(newPassive, effectOwner, true);
                        return;
                    default:
                        effect.AddBuiltPassive(newPassive);
                        builtPassive = newPassive;
                        break;
                }
            }

            builtPassive.SubscribeToTile(subject);
        }
    }
}