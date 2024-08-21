using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Expressions;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Structs.Modifiers
{
    /// <summary>
    /// A serializable IModifierStruct that builds into a StatModifier
    /// </summary>
    [System.Serializable]
    public struct StatModifierStruct : IModifierStruct<StatModifier>
    {
        public StatType mechOrPilotStat;
        public string statName;
        public ModifierType mathOperation;
        [SerializeField] public UnitExpressionNode modifierValueNode;
        public int duration;
        public TurnTimerBehavior turnTimerBehavior;

        public StatModifier Build(IActionOrPassive source)
        {
            return new StatModifier(mechOrPilotStat, statName, modifierValueNode.Compute(), source, mathOperation, duration, turnTimerBehavior);
        }
    }
}