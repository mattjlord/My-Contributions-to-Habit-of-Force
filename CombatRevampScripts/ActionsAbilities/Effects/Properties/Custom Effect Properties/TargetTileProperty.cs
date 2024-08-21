using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties
{
    /// <summary>
    /// Should be used by any CombatEffect that targets a tile on the board via user or
    /// AI input.
    /// <para>REPRESENTS: the targeted tile</para>
    /// <para>CONTAINS: an ITile</para>
    /// </summary>
    public class TargetTileProperty : AEffectProperty<ITile>
    {
        public TargetTileProperty(ITile value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, ITile> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
