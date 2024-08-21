using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.Expressions;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Structs.Modifiers
{
    /// <summary>
    /// A serializable IModifierStruct that builds into a PropertyModifier
    /// </summary>
    [System.Serializable]
    public struct PropertyModifierStruct : IModifierStruct<PropertyModifier>
    {
        public PropertyType modifiedProperty;
        public ModifierType mathOperation;
        [SerializeField] public UnitExpressionNode modifierValueNode;
        public int duration;
        public TurnTimerBehavior turnTimerBehavior;

        public PropertyModifier Build(IActionOrPassive source)
        {
            return new PropertyModifier(modifiedProperty, modifierValueNode.Compute(), source, mathOperation, duration, turnTimerBehavior);
        }
    }
}