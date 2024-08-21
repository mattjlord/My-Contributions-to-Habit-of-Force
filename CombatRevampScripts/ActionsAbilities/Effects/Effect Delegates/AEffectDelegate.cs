using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Expressions;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates
{
    /// <summary>
    /// An IEffectDelegate that serializes as a ScriptableObject
    /// </summary>
    /// <typeparam name="T">the type of subject affected by the delegate</typeparam>
    public abstract class AEffectDelegate<T> : ScriptableObject, IEffectDelegate<T>
    {
        [SerializeField] public List<UnitExpressionPredicate> requiredValueConditions;

        public void TryInvoke(T subject, ICombatEffect effect)
        {
            if (CanInvoke(subject, effect))
            {
                Invoke(subject, effect);
            }
        }

        public virtual bool CanInvoke(T subject, ICombatEffect effect) 
        {
            ICombatUnit subjectAsUnit = null;

            if (typeof(ICombatUnit).IsAssignableFrom(subject.GetType()))
            {
                subjectAsUnit = (ICombatUnit)subject;
            }

            for (int i = 0; i < requiredValueConditions.Count; i++)
            {
                UnitExpressionPredicate predicate = requiredValueConditions[i];
                predicate.Setup(subjectAsUnit, effect);
                requiredValueConditions[i] = predicate;
            }

            foreach (UnitExpressionPredicate predicate in requiredValueConditions)
            {
                if (!predicate.Test())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Initializes properties required by this delegate on the given effect (empty by default).
        /// </summary>
        /// <param name="effect"></param>
        public virtual void InitializeEffectProperties(ICombatEffect effect) { }

        /// <summary>
        /// Invokes this delegate
        /// </summary>
        public abstract void Invoke(T subject, ICombatEffect effect);
    }    
}