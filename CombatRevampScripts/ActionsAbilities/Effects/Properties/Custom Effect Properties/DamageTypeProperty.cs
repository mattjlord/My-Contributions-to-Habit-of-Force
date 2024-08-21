using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.ActionsAbilities.Enums;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that deals or heals damage to Mechs or Pilots, where the damage type
    /// is determined by the user prior to the CombatEffect happening.
    /// <para>REPRESENTS: the damage type of the effect</para>
    /// <para>CONTAINS: a DamageType</para>
    /// </summary>
    public class DamageTypeProperty : AEffectProperty<DamageType>
    {
        public DamageTypeProperty() : base() { }

        public DamageTypeProperty(DamageType value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, DamageType> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
