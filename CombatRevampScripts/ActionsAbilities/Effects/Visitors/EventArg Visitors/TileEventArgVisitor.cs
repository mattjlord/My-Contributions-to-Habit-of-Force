using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors
{
    public class TileEventArgVisitor : EffectPropertyVisitor<TileEventArgProperty, ITile>
    {
        private ITile _value;

        public TileEventArgVisitor(ITile value)
        {
            _value = value;
        }

        public override TileEventArgProperty Visit(TileEventArgProperty property)
        {
            return SetValue(property, _value);
        }
    }
}
