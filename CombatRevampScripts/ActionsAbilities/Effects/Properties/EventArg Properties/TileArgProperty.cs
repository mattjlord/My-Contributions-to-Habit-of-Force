using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties
{
    public class TileEventArgProperty : AEffectProperty<ITile>
    {
        public TileEventArgProperty(ITile value) : base(value) { }

        public override T Accept<T>(IEffectPropertyVisitor<T, ITile> visitor)
        {
            return visitor.Visit(this);
        }
    }
}