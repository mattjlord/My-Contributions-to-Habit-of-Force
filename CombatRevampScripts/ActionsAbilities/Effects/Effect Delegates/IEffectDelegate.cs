using System;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates
{
    /// <summary>
    /// A delegate used inside a combat effect with a specific type of
    /// subject that is affected by it.
    /// </summary>
    /// <typeparam name="T">the type of subject affected by this delegate</typeparam>
    public interface IEffectDelegate<T>
    {
        /// <summary>
        /// Attempts to invoke the delegate
        /// </summary>
        /// <param name="subject">the subject to affect with this delegate</param>
        /// <param name="effect">the combat effect that is using this delegate</param>
        public void TryInvoke(T subject, ICombatEffect effect);
    }
}