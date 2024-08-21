using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Structs.Modifiers;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// Adds the given stat modifier to the subject unit
    /// </summary>
    [CreateAssetMenu (menuName = "Ability Designer/Effect Delegates/CombatUnit/Modifiers/Add Stat Modifier to Unit")]
    public class AddStatModifierToUnit : AEffectDelegate<ICombatUnit>
    {
        [SerializeField] public StatModifierStruct modifier;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            modifier.modifierValueNode.Setup(subject, effect);
            subject.AddModifier(modifier.Build(effect.GetSource()));
        }
    }
}