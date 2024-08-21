using CombatRevampScripts.ActionsAbilities.TurnManager;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.AbilitySOs
{
    /// <summary>
    /// Represents an IAbilityDesignerSO that serializes as a ScriptableObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AAbilityDesignerSO<T> : ScriptableObject, IAbilityDesignerSO<T>
    {
        public abstract T Build(ITurnManager turnManager);
    }
}
