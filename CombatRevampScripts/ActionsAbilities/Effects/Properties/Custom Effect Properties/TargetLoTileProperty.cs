using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that targets a list of tiles on the board via user or
    /// AI input.
    /// <para>REPRESENTS: the targeted tile</para>
    /// <para>CONTAINS: a List<ITile></para>
    /// </summary>
    public class TargetLoTileProperty : AEffectProperty<List<ITile>>
    {
        public TargetLoTileProperty(List<ITile> value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, List<ITile>> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
