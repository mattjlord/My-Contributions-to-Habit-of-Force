using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that gets and then affects units in an AOE around the subject unit
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/AOE/Affect Units In AOE of Unit")]
    public class AffectUnitsInAOEOfUnit : AAOEDelegate<ICombatUnit, ICombatUnit>
    {
        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            ICombatUnit source = effect.GetOwner();
            List<ICombatUnit> aoeUnits = subject.GetUnitsInAOE(source, GetAOERange(), selfInclusive, relativeAffiliation);
            InvokeOnSubjects(aoeUnits, effect);
        }
    }
}