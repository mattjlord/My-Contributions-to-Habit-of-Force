using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors
{
    public class FloatEventArgVisitor : EffectPropertyVisitor<FloatEventArgProperty, float>
    {
        private float _value;

        public FloatEventArgVisitor(float value)
        {
            _value = value;
        }

        public override FloatEventArgProperty Visit(FloatEventArgProperty property)
        {
            return SetValue(property, _value);
        }
    }
}
