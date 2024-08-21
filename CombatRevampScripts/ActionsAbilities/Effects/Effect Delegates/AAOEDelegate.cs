using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Enums;
using System.Collections.Generic;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates
{
    /// <summary>
    /// An AEffectDelegate for applying additional EffectDelegates to all subjects in its
    /// AOE
    /// </summary>
    /// <typeparam name="T">the type of subject where the AOE originates from</typeparam>
    /// <typeparam name="U">the type of subject affected by the AOE</typeparam>
    public abstract class AAOEDelegate<T, U> : AEffectDelegate<T>
    {
        public List<AEffectDelegate<U>> effectsOnAOESubjects;
        public int radius;
        public bool selfInclusive;
        public RelativeAffiliation relativeAffiliation;

        private AOERangeProperty _aoeRangeProperty;

        public override void InitializeEffectProperties(ICombatEffect effect)
        {
            _aoeRangeProperty = (AOERangeProperty)effect.AddProperty(new AOERangeProperty(radius));
        }

        public int GetAOERange()
        {
            return _aoeRangeProperty.GetValue();
        }

        /// <summary>
        /// Invokes each delegate in the effectsOnAOESubjects for each subject in the given list
        /// </summary>
        /// <param name="subjects">the list of subjects affected by the AOE</param>
        /// <param name="effect">the effect that is using this delegate</param>
        public void InvokeOnSubjects(List<U> subjects, ICombatEffect effect)
        {
            foreach (U subject in subjects)
            {
                foreach (AEffectDelegate<U> effectOnSubject in effectsOnAOESubjects)
                {
                    effectOnSubject.TryInvoke(subject, effect);
                }
            }
        }
    }
}