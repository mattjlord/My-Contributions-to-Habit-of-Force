using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using System.Collections.Generic;

namespace CombatRevampScripts.Board.TilePiece.MechPilot
{
    /// <summary>
    /// Represents an object that stores core stats for a unit in combat.
    /// </summary>
    public interface ICoreStatManager
    {
        /// <summary>
        /// Restores the current health value of this to the max health value of this.
        /// </summary>
        public void RestoreHealthToMax();

        /// <summary>
        /// Adds the given modifier to the current modifiers
        /// </summary>
        /// <param name="statModifier">the StatModifier to add</param>
        public void AddStatModifier(StatModifier statModifier);

        /// <summary>
        /// Removes the given modifier from the current modifiers
        /// </summary>
        /// <param name="statModifier">the StatModifier to remove</param>
        public void RemoveStatModifier(StatModifier statModifier);

        /// <summary>
        /// Returns whether or not this contains an integer or float stat with the given name.
        /// </summary>
        /// <param name="statName">the name of the stat to check for</param>
        /// <returns>whether the given stat exists.</returns>
        public bool DoesStatExist(string statName);

        /// <summary>
        /// Returns an array containing the names of all the stats in this.
        /// </summary>
        /// <returns>an array containing the names of all the stats in this.</returns>
        public string[] GetStatNames();

        /// <summary>
        /// Returns the value of a stat containing a float value; throws an error if it can't find one.
        /// </summary>
        /// <param name="statName">the name of the stat</param>
        /// <returns>the value of the stat, after applying modifiers</returns>
        public float GetFloatStatValue(string statName);

        /// <summary>
        /// Returns the value of a stat containing an int value; throws an error if it can't find one.
        /// </summary>
        /// <param name="statName">the name of the stat</param>
        /// <returns>the value of the stat, after applying modifiers</returns>
        public int GetIntStatValue(string statName);
    }
}