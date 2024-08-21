using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that can deal damage to a Mech.
    /// <para>REPRESENTS: the value of the damage</para>
    /// <para>CONTAINS: a float</para>
    /// </summary>
    public class MechDamageProperty : AFloatProperty
    {
        public MechDamageProperty() : base() { }

        public MechDamageProperty(float value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, float> visitor)
        {
            return visitor.Visit(this);
        }
    }
}