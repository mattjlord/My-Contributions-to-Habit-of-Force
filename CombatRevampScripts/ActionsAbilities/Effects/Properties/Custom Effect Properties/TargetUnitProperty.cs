using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that targets a unit on the board via user or
    /// AI input.
    /// <para>REPRESENTS: the targeted unit</para>
    /// <para>CONTAINS: an ICombatUnit</para>
    /// </summary>
    public class TargetUnitProperty : AEffectProperty<ICombatUnit>
    {
        public TargetUnitProperty(ICombatUnit value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, ICombatUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
