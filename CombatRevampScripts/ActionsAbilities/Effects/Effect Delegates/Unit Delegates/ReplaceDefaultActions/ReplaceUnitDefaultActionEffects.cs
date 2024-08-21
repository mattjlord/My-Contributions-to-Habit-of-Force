using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    public enum DefaultActionType
    {
        Move,
        Attack,
        Defend
    }

    /// <summary>
    /// Replaces the effects of a default action on the subject unit with the given
    /// list of effects.
    /// </summary>
    [CreateAssetMenu (menuName = "Ability Designer/Effect Delegates/CombatUnit/Replace Effects for a Default Action of Unit Until Next Turn")]
    public class ReplaceUnitDefaultActionEffects : AEffectDelegate<ICombatUnit>
    {
        public DefaultActionType actionType;
        public List<ACombatEffect> newEffects;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            List<ICombatEffect> temp = new List<ICombatEffect>();
            foreach (ACombatEffect newEffect in newEffects)
            {
                temp.Add(newEffect);
            }

            switch (actionType)
            {
                case DefaultActionType.Attack:
                    subject.OverrideAttackEffects(temp);
                    return;
                case DefaultActionType.Defend:
                    subject.OverrideDefendEffects(temp);
                    return;
                case DefaultActionType.Move:
                    subject.OverrideMoveEffects(temp);
                    return;
            }
        }
    }
}