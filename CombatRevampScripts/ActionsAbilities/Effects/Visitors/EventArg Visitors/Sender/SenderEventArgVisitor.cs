using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors.Sender
{
    public class SenderEventArgVisitor : EffectPropertyVisitor<SenderEventArgProperty, object>
    {
        private object _value;

        public SenderEventArgVisitor(object value)
        {
            _value = value;
        }

        public override SenderEventArgProperty Visit(SenderEventArgProperty property)
        {
            return SetValue(property, _value);
        }
    }
}
