using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender
{
    public class SenderEventArgProperty : AEffectProperty<object>
    {
        public SenderEventArgProperty(object value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, object> visitor)
        {
            return visitor.Visit(this);
        }
    }
}