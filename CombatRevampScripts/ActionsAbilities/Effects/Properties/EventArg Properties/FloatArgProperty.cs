using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties
{
    public class FloatEventArgProperty : AEffectProperty<float>
    {
        public FloatEventArgProperty(float value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, float> visitor)
        {
            return visitor.Visit(this);
        }
    }
}