using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Expressions;
using CombatRevampScripts.Board.TilePiece.MechPilot;

namespace CombatRevampScripts.ActionsAbilities.Structs.Predicates.Custom_Predicates
{
    public enum StatPredMode
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    public enum ValueType
    {
        Int,
        Float
    }

    [System.Serializable]
    public struct UnitStatPred : ICustomPredicate<ICombatUnit>
    {
        public ICombatUnit thisUnit;
        public IEffectHolder effectHolder;
        public StatType statType;
        public ValueType valueType;
        public string statName;
        public StatPredMode mode;
        public UnitExpressionNode valueNode;

        public void SetOwnerUnit(ICombatUnit combatUnit)
        {
            thisUnit = combatUnit;
        }

        public void SetEffectHolder(IEffectHolder effectHolder)
        {
            this.effectHolder = effectHolder;
        }

        public bool Test(ICombatUnit obj)
        {
            ICoreStatManager statManager;

            if (statType == StatType.Mech)
            {
                statManager = thisUnit.GetTilePiece().GetMechSO();
            }
            else
            {
                statManager = thisUnit.GetTilePiece().GetPilotSO();
            }

            float statValue;

            if (valueType == ValueType.Float)
            {
                statValue = statManager.GetFloatStatValue(statName);
            }
            else
            {
                statValue = statManager.GetIntStatValue(statName);
            }

            valueNode.Setup(obj, thisUnit, thisUnit, effectHolder);
            float value = valueNode.Compute();
            
            switch (mode)
            {
                case StatPredMode.GreaterThan:
                    return statValue > value;
                case StatPredMode.GreaterThanOrEqual:
                    return statValue <= value;
                case StatPredMode.LessThan:
                    return statValue < value;
                case StatPredMode.LessThanOrEqual:
                    return statValue <= value;
                default:
                    return false;
            }
        }
    }
}
