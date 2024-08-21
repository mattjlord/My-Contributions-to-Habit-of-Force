using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties
{
    public class IntEventArgProperty : AEffectProperty<int>
    {
        public IntEventArgProperty(int value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, int> visitor)
        {
            return visitor.Visit(this);
        }
    }
}