using CombatRevampScripts.ActionsAbilities.Enums;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.Custom_Visitors
{
    /// <summary>
    /// VISITS: a DamageTypeProperty
    /// <para>Sets the DamageType value</para>
    /// </summary>
    public class SetDamageTypeVisitor : EffectPropertyVisitor<Properties.Custom_Effect_Properties.DamageTypeProperty, DamageType>
    {
        private DamageType _damageType;

        public SetDamageTypeVisitor(DamageType damageType)
        {
            _damageType = damageType;
        }

        public override Properties.Custom_Effect_Properties.DamageTypeProperty Visit(Properties.Custom_Effect_Properties.DamageTypeProperty property)
        {
            property.SetValue(_damageType);
            return property;
        }
    }
}
