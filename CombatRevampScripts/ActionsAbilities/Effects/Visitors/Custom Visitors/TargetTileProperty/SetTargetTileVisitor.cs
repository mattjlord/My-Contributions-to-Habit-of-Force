using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.Custom_Visitors
{
    /// <summary>
    /// VISITS: a TargetTileProperty
    /// <para>Sets the ITile value</para>
    /// </summary>
    public class SetTargetTileVisitor : EffectPropertyVisitor<Properties.Custom_Effect_Properties.TargetTileProperty, ITile>
    {
        private ITile _targetTile;

        public SetTargetTileVisitor(ITile targetTile)
        {
            _targetTile = targetTile;
        }

        public override Properties.Custom_Effect_Properties.TargetTileProperty Visit(Properties.Custom_Effect_Properties.TargetTileProperty property)
        {
            property.SetValue(_targetTile);
            return property;
        }
    }
}