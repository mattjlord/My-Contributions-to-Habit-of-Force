using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors
{
    public class IntEventArgVisitor : EffectPropertyVisitor<IntEventArgProperty, int>
    {
        private int _value;

        public IntEventArgVisitor(int value)
        {
            _value = value;
        }

        public override IntEventArgProperty Visit(IntEventArgProperty property)
        {
            return SetValue(property, _value);
        }
    }
}
