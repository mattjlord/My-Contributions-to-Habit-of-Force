using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that has an area of effect.
    /// <para>REPRESENTS: the range, in Tiles, of the effect</para>
    /// <para>CONTAINS: an int</para>
    /// </summary>
    public class AOERangeProperty : AIntProperty
    {
        public AOERangeProperty() : base() { }

        public AOERangeProperty(int value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, int> visitor)
        {
            return visitor.Visit(this);
        }
    }
}