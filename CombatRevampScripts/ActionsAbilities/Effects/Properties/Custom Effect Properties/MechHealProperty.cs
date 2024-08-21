using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that can heal a Mech.
    /// <para>REPRESENTS: the value of the healing</para>
    /// <para>CONTAINS: a float</para>
    /// </summary>
    public class MechHealProperty : AFloatProperty
    {
        public MechHealProperty() : base() { }

        public MechHealProperty(float value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, float> visitor)
        {
            return visitor.Visit(this);
        }
    }
}