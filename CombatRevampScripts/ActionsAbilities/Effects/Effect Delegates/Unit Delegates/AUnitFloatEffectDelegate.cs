using CombatRevampScripts.ActionsAbilities.Expressions;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    public abstract class AUnitFloatEffectDelegate : AEffectDelegate<ICombatUnit>
    {
        [SerializeField] public UnitExpressionNode valueNode;

        public void SetupNode(ICombatUnit subject, ICombatEffect effect)
        {
            valueNode.Setup(subject, effect);
        }
    }
}