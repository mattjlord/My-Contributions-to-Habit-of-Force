using System.Collections.Generic;
using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors.Custom_Visitors
{
    /// <summary>
    /// VISITS: a TargetLoTileProperty
    /// <para>Sets the List<ITile> value</para>
    /// </summary>
    public class SetTargetLoTileVisitor : EffectPropertyVisitor<Properties.Custom_Effect_Properties.TargetLoTileProperty, List<ITile>>
    {
        private List<ITile> _targetTiles;

        public SetTargetLoTileVisitor(List<ITile> targetTiles)
        {
            _targetTiles = targetTiles;
        }

        public override Properties.Custom_Effect_Properties.TargetLoTileProperty Visit(Properties.Custom_Effect_Properties.TargetLoTileProperty property)
        {
            property.SetValue(_targetTiles);
            return property;
        }
    }
}