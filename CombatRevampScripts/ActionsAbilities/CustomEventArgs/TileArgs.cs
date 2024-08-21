using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.Board.Tile;
using System;

namespace CombatRevampScripts.ActionsAbilities.CustomEventArgs
{
    /// <summary>
    /// EventArgs for containing a single ITile Value, as well as an option to
    /// override the sender property with another ICombatUnit instead of the default.
    /// </summary>
    public class TileArgs : EventArgs
    {
        public ITile Value { get; private set; }
        public ICombatUnit OverrideSender { get; private set; }

        public TileArgs(ITile value)
        {
            Value = value;
        }

        public TileArgs(ITile value, ICombatUnit overrideSender) : this(value)
        {
            OverrideSender = overrideSender;
        }
    }
}
