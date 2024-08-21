using System;
using CombatRevampScripts.ActionsAbilities.Effects.Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors
{
    /// <summary>
    /// Performs modifying operations on the value of a specific IEffectProperty
    /// </summary>
    /// <typeparam name="T">the type of the IEffectProperty to modify</typeparam>
    /// <typeparam name="U">the type of value contained in T</typeparam>
    public class EffectPropertyVisitor<T, U> : IEffectPropertyVisitor<T, U>
    {
        /// <summary>
        /// Utility function for all visitors.
        /// If the given effect property matches the type T of this visitor,
        /// sets the value of the given property to the given value,
        /// otherwise throws an exception.
        /// </summary>
        /// <param name="effectProperty">the given effect property, in interface form (IEffectProperty)</param>
        /// <param name="value">the value to set</param>
        /// <returns>the modified property, which is cast as T (this is safe because the property argument should always be of type T)</returns>
        /// <exception cref="ArgumentException">occurs if the given property is not of type T</exception>
        public T SetValue(IEffectProperty<U> effectProperty, U value)
        {
            if (effectProperty.GetType() != typeof(T))
            {
                throw new ArgumentException("The given effect property must match the type of this visitor: ", typeof(T).Name);
            }
            effectProperty.SetValue(value);
            return (T)effectProperty;
        }

        public T Visit(IEffectProperty<U> property)
        {
            return property.Accept(this);
        }

        // Custom Effect Properties
        public virtual T Visit(DamageTypeProperty property) { return default; }
        public virtual T Visit(TargetUnitProperty property) { return default; }
        public virtual T Visit(AOERangeProperty property) { return default; }
        public virtual T Visit(TargetTileProperty property) { return default; }
        public virtual T Visit(TargetLoUnitProperty property) { return default; }
        public virtual T Visit(TargetLoTileProperty property) { return default; }
        public virtual T Visit(MechDamageProperty property) { return default; }
        public virtual T Visit(PilotDamageProperty property) { return default; }
        public virtual T Visit(MechHealProperty property) { return default; }
        public virtual T Visit(PilotHealProperty property) { return default; }

        // EffectTrigger Properties
        public virtual T Visit(SenderEventArgProperty property) { return default; }
        public virtual T Visit(FloatEventArgProperty property) { return default; }
        public virtual T Visit(IntEventArgProperty property) { return default; }
        public virtual T Visit(CombatUnitEventArgProperty property) { return default; }
        public virtual T Visit(TileEventArgProperty property) { return default; }
    }
}
