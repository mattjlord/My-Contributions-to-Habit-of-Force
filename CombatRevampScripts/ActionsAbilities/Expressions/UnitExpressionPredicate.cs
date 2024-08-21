using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Expressions
{
    public enum ComparisonMode
    {
        Equals,
        IsLessThan,
        IsLessThanOrEqualTo,
        IsGreaterThan,
        IsGreaterThanOrEqualTo
    }

    [System.Serializable]
    public struct UnitExpressionPredicate : IExpressionPredicate
    {
        [SerializeField] public UnitExpressionNode firstValueNode;
        public ComparisonMode comparisonMode;
        [SerializeField] public UnitExpressionNode secondValueNode;

        public void Setup(ICombatUnit subject, ICombatEffect effect)
        {
            firstValueNode.Setup(subject, effect);
            secondValueNode.Setup(subject, effect);
        }

        public bool Test()
        {
            switch (comparisonMode)
            {
                case ComparisonMode.Equals:
                    return firstValueNode.Compute() == secondValueNode.Compute();
                case ComparisonMode.IsLessThan:
                    return firstValueNode.Compute() < secondValueNode.Compute();
                case ComparisonMode.IsLessThanOrEqualTo:
                    return firstValueNode.Compute() <= secondValueNode.Compute();
                case ComparisonMode.IsGreaterThan:
                    return firstValueNode.Compute() > secondValueNode.Compute();
                case ComparisonMode.IsGreaterThanOrEqualTo:
                    return firstValueNode.Compute() >= secondValueNode.Compute();
                default: 
                    return false;
            }
        }
    }
}