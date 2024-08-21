using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Custom_Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Expressions
{
    public enum NodeType
    {
        Value,
        Add,
        Subtract,
        Multiply,
        Divide,
        Power
    }

    public enum SpecialValueType
    {
        Custom,

        // Mech stats
        // FLOAT
        MaxMechIntegrity,
        CurrentMechIntegrity,
        MechOverheal,
        Speed,
        BallisticDamage,
        LaserDamage,
        Defense,
        Clarity,
        Accuracy,
        // INT
        MoveRange,
        BallisticAttackRange,
        LaserAttackRange,

        // Pilot stats
        // FLOAT
        MaxPilotHealth,
        CurrentPilotHealth,
        PilotOverheal,
        ReactionSpeed,
        Precision,
        Leadership,

        // Other
        // INT
        DistanceFromAssignedUnit,
        DistanceFromOwnerUnit,

        // From previous delegates
        // FLOAT
        LastDamageSent,
        LastDamageDealt,
        LastHealingSent,
        LastHealingDealt,
        // INT
        LastDistanceMoved,

        // From events
        // FLOAT
        FloatValueSentByEvent,
        // INT
        IntValueSentByEvent
    }

    public enum SpecialUnitValueSource
    {
        None,
        Subject,
        AssignedUnit,
        OwnerUnit
    }

    /// <summary>
    /// Represents a serializable IExpressionNode with five operation types that can also contain
    /// special CombatUnit-specific special values.
    /// </summary>
    [System.Serializable]
    public struct UnitExpressionNode : IExpressionNode
    {
        public NodeType nodeType;
        public SpecialUnitValueSource valueSource;
        public SpecialValueType valueType;
        public float customValue;
        [SerializeField] public List<UnitExpressionNode> childNodes;

        private ICombatUnit _assignedUnit;
        private ICombatUnit _ownerUnit;
        private ICombatUnit _subjectUnit;
        private IEffectHolder _effectHolder;
        private ICombatEffect _effect;

        public void Setup(ICombatUnit subject, ICombatEffect effect)
        {
            if (valueSource == SpecialUnitValueSource.Subject && subject == null)
            {
                throw new System.Exception("The CombatUnit subject of this node is null, but the value source is set to subject. This error is most likely caused by adding a predicate to " +
                    "an EffectDelegate that has a non-CombatUnit subject, but setting the value source of an expression node within the predicate to Subject. Choose another value source to " +
                    "fix this issue.");
            }

            _subjectUnit = subject;
            _ownerUnit = effect.GetOwner();
            _assignedUnit = effect.GetAssignee();
            _effectHolder = effect.GetEffectHolder();
            _effect = effect;

            for (int i = 0; i < childNodes.Count; i++)
            {
                UnitExpressionNode node = childNodes[i];
                node.Setup(subject, effect);
                childNodes[i] = node;
            }
        }

        public void Setup(ICombatUnit subject, ICombatUnit assignedUnit, ICombatUnit ownerUnit, IEffectHolder effectHolder)
        {
            _subjectUnit = subject;
            _assignedUnit = assignedUnit;
            _ownerUnit = ownerUnit;
            _effectHolder = effectHolder;
            for (int i = 0; i < childNodes.Count; i++)
            {
                UnitExpressionNode node = childNodes[i];
                node.Setup(subject, assignedUnit, ownerUnit, effectHolder);
                childNodes[i] = node;
            }
        }

        public float Compute()
        {
            if (nodeType == NodeType.Value)
            {
                return GetValue();
            }

            bool firstLoop = true;
            float currValue = 0;

            foreach (UnitExpressionNode node in childNodes)
            {
                if (!firstLoop)
                {
                    currValue = Operate(currValue, node.Compute());
                }
                else
                {
                    currValue = node.Compute();
                }

                firstLoop = false;
            }

            return currValue;
        }

        private float Operate(float currValue, float modifierValue)
        {
            switch (nodeType)
            {
                case NodeType.Add:
                    return currValue + modifierValue;
                case NodeType.Subtract:
                    return currValue - modifierValue;
                case NodeType.Multiply:
                    return currValue * modifierValue;
                case NodeType.Divide:
                    return currValue / modifierValue;
                case NodeType.Power:
                    return Mathf.Pow(currValue, modifierValue);
                default:
                    return 0;
            }
        }

        private float GetValue()
        {
            ICombatUnit unit = null;

            switch (valueSource)
            {
                case SpecialUnitValueSource.None:
                    break;
                case SpecialUnitValueSource.Subject:
                    unit = _subjectUnit;
                    break;
                case SpecialUnitValueSource.AssignedUnit:
                    unit = _assignedUnit; 
                    break;
                case SpecialUnitValueSource.OwnerUnit:
                    unit = _ownerUnit;
                    break;
            }

            switch (valueType)
            {
                case SpecialValueType.MaxMechIntegrity:
                    return unit.GetMechSO().GetFloatStatValue("maxHealth");
                case SpecialValueType.CurrentMechIntegrity:
                    return unit.GetMechSO().GetFloatStatValue("currHealth");
                case SpecialValueType.MechOverheal:
                    return unit.GetMechSO().GetFloatStatValue("tempHealth");
                case SpecialValueType.Speed:
                    return unit.GetMechSO().GetFloatStatValue("speed");
                case SpecialValueType.BallisticDamage:
                    return unit.GetMechSO().GetFloatStatValue("ballisticDamage");
                case SpecialValueType.LaserDamage:
                    return unit.GetMechSO().GetFloatStatValue("laserDamage");
                case SpecialValueType.Defense:
                    return unit.GetMechSO().GetFloatStatValue("defense");
                case SpecialValueType.Clarity:
                    return unit.GetMechSO().GetFloatStatValue("clarity");
                case SpecialValueType.Accuracy:
                    return unit.GetMechSO().GetFloatStatValue("accuracy");
                case SpecialValueType.MoveRange:
                    return unit.GetMechSO().GetIntStatValue("moveRange");
                case SpecialValueType.BallisticAttackRange:
                    return unit.GetMechSO().GetIntStatValue("ballisticAttackRange");
                case SpecialValueType.LaserAttackRange:
                    return unit.GetMechSO().GetIntStatValue("laserAttackRange");
                case SpecialValueType.MaxPilotHealth:
                    return unit.GetPilotSO().GetFloatStatValue("maxHealth");
                case SpecialValueType.CurrentPilotHealth:
                    return unit.GetPilotSO().GetFloatStatValue("currHealth");
                case SpecialValueType.PilotOverheal:
                    return unit.GetPilotSO().GetFloatStatValue("tempHealth");
                case SpecialValueType.ReactionSpeed:
                    return unit.GetPilotSO().GetFloatStatValue("reactionSpeed");
                case SpecialValueType.Precision:
                    return unit.GetPilotSO().GetFloatStatValue("precision");
                case SpecialValueType.Leadership:
                    return unit.GetPilotSO().GetFloatStatValue("leadership");
                case SpecialValueType.DistanceFromAssignedUnit:
                    return MathExtension.ManhattanDistance(_assignedUnit.GetTilePiece().GetPosition(), unit.GetTilePiece().GetPosition());
                case SpecialValueType.DistanceFromOwnerUnit:
                    return MathExtension.ManhattanDistance(_ownerUnit.GetTilePiece().GetPosition(), unit.GetTilePiece().GetPosition());
                case SpecialValueType.LastDamageSent:
                    return _effectHolder.GetTempEffectValue(TempEffectValueType.LastDamageSent);
                case SpecialValueType.LastDamageDealt:
                    return _effectHolder.GetTempEffectValue(TempEffectValueType.LastDamageDealt);
                case SpecialValueType.LastHealingSent:
                    return _effectHolder.GetTempEffectValue(TempEffectValueType.LastHealingSent);
                case SpecialValueType.LastHealingDealt:
                    return _effectHolder.GetTempEffectValue(TempEffectValueType.LastHealingDealt);
                case SpecialValueType.LastDistanceMoved:
                    return _effectHolder.GetTempEffectValue(TempEffectValueType.LastDistanceMoved);
                case SpecialValueType.FloatValueSentByEvent:
                    FloatEventArgProperty floatArgProperty = (FloatEventArgProperty)_effect.GetPropertyOfType<float>(typeof(FloatEventArgProperty));
                    if (floatArgProperty == null)
                    {
                        throw new System.Exception("Cannot find a FloatEventArgProperty in the Effect using this node! This is caused by improper setup " +
                            "of the EffectTrigger events that the Effect belongs to. To fix this, make sure the EffectTrigger subscribes to at least one " +
                            "event that sends a float value. If this effect belongs to a CombatAction rather than an EffectTrigger, do not try and access " +
                            "event values with this node!");
                    }
                    return floatArgProperty.GetValue();
                case SpecialValueType.IntValueSentByEvent:
                    IntEventArgProperty intArgProperty = (IntEventArgProperty)_effect.GetPropertyOfType<int>(typeof(IntEventArgProperty));
                    if (intArgProperty == null)
                    {
                        throw new System.Exception("Cannot find an IntEventArgProperty in the Effect using this node! This is caused by improper setup " +
                            "of the EffectTrigger events that the Effect belongs to. To fix this, make sure the EffectTrigger subscribes to at least one " +
                            "event that sends a int value. If this effect belongs to a CombatAction rather than an EffectTrigger, do not try and access " +
                            "event values with this node!");
                    }
                    return intArgProperty.GetValue();
                default:
                    return customValue;
            }
        }
    }
}