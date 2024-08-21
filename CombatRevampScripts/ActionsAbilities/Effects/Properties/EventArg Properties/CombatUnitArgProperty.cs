using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties
{
    public class CombatUnitEventArgProperty : AEffectProperty<ICombatUnit>
    {
        public CombatUnitEventArgProperty(ICombatUnit value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, ICombatUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}