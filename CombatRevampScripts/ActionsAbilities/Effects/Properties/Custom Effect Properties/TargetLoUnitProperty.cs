using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that targets a list of units on the board via user or
    /// AI input.
    /// <para>REPRESENTS: the targeted CombatUnit</para>
    /// <para>CONTAINS: a List<ICombatUnit></para>
    /// </summary>
    public class TargetLoUnitProperty : AEffectProperty<List<ICombatUnit>>
    {
        public TargetLoUnitProperty(List<ICombatUnit> value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, List<ICombatUnit>> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
