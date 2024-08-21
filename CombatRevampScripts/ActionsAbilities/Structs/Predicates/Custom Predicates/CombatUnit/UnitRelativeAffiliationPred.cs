using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.Board.TilePiece;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Structs.Predicates.Custom_Predicates
{
    [System.Serializable]
    public struct UnitRelativeAffiliationPred : ICustomPredicate<ICombatUnit>
    {
        public ICombatUnit thisUnit;
        public RelativeAffiliation relativeAffiliation;

        public void SetOwnerUnit(ICombatUnit combatUnit)
        {
            thisUnit = combatUnit;
        }

        public void SetEffectHolder(IEffectHolder effectHolder) { }

        public bool Test(ICombatUnit obj)
        {
            Affiliation thisAffil = thisUnit.GetPilotSO().affiliation;
            Affiliation objAffil = obj.GetPilotSO().affiliation;

            bool objIsFriendly = (thisAffil == objAffil);

            switch (relativeAffiliation)
            {
                case RelativeAffiliation.Any:
                    return true;
                case RelativeAffiliation.Friendly:
                    return objIsFriendly;
                case RelativeAffiliation.Hostile:
                    return !objIsFriendly;
                default:
                    return true;
            }
        }
    }
}
