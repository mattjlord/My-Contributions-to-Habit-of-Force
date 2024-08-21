using CombatRevampScripts.Board.Tile;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Effect_Delegate_Structs
{
    public enum TileSubjectType
    {
        TargetTile,
        TargetListOfTiles,
        EventArgTile
    }

    [System.Serializable]
    public struct TileEffectDelegateStruct
    {
        public TileSubjectType subject;
        public AEffectDelegate<ITile> effectDelegate;
    }
}