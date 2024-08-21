using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Structs.Modifiers;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// Adds the given property modifier to the subject unit
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Modifiers/Add Property Modifier to Unit")]
    public class AddPropertyModifierToUnit : AEffectDelegate<ICombatUnit>
    {
        [SerializeField] public PropertyModifierStruct modifier;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            modifier.modifierValueNode.Setup(subject, effect);
            subject.AddModifier(modifier.Build(effect.GetSource()));
        }
    }
}