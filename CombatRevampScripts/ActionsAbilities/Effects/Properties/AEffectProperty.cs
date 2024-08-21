using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties
{
    /// <summary>
    /// Represents a public value attached to an ICombatEffect that can be modified by IEffectPropertyVisitors.
    /// </summary>
    /// <typeparam name="U">the type of value contained in this</typeparam>
    public abstract class AEffectProperty<U> : IEffectProperty<U>
    {
        protected U value;

        public AEffectProperty (U value)
        {
            this.value = value;
        }

        public AEffectProperty()
        {
            value = default;
        }

        public abstract T Accept<T>(IEffectPropertyVisitor<T, U> visitor);

        public virtual U GetValue()
        {
            return value;
        }

        public void SetValue(U value)
        {
            this.value = value;
        }
    }
}
