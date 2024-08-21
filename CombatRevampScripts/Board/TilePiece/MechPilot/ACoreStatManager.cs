using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatRevampScripts.Board.TilePiece.MechPilot
{
    /// <summary>
    /// Represents an ICoreStatManager as a ScriptableObject
    /// </summary>
    public abstract class ACoreStatManager : ScriptableObject, ICoreStatManager
    {
        // Should instances of this write back to the saved object after combat?
        public bool unique;

        public Dictionary<string, List<StatModifier>> statModifiers = new Dictionary<string, List<StatModifier>>();
        public List<StatModifier> statModifiersList = new List<StatModifier>();

        public void AddStatModifier(StatModifier statModifier)
        {
            string statName = statModifier.statName;

            if (!DoesStatExist(statName)) { return; }

            if (!statModifiers.ContainsKey(statName))
            {
                statModifiers[statName] = new List<StatModifier>();
            }

            List<StatModifier> copyLoModifiers = statModifiers[statName];
            

            copyLoModifiers.Add(statModifier);
            statModifiers[statName] = copyLoModifiers;

            statModifiersList.Add(statModifier);
        }

        public void RemoveStatModifier(StatModifier statModifier)
        {
            List<StatModifier> copyLoModifiers = statModifiers[statModifier.statName];
            if (copyLoModifiers != null)
            {
                copyLoModifiers.Remove(statModifier);
                statModifiers[statModifier.statName] = copyLoModifiers;
            }
            statModifiersList.Remove(statModifier);
        }

        /// <summary>
        /// Gets the value of the given stat, including modifiers, assuming that a stat with the given name exists, and the
        /// stat with the given name has a float value matching the baseValue argument.
        /// </summary>
        /// <param name="statName">the name of the stat</param>
        /// <param name="baseValue">the base float value of the stat</param>
        /// <returns>the value of the stat, as a float, after applying modifiers</returns>
        protected internal float ApplyFloatStatModifiers(string statName, float baseValue)
        {
            if (!statModifiers.ContainsKey(statName))
            {
                statModifiers[statName] = new List<StatModifier>();
            }

            List<StatModifier> copyLoModifiers = statModifiers[statName];

            if (copyLoModifiers == null) { return baseValue; }

            float copyValue = baseValue;
            
            foreach (StatModifier modifier in copyLoModifiers)
            {
                copyValue = modifier.ApplyToValue(copyValue);
            }

            if (copyValue < 0f) { copyValue = 0f; }

            return copyValue;
        }

        /// <summary>
        /// Gets the value of the given stat, including modifiers, assuming that a stat with the given name exists, and the
        /// stat with the given name has a value matching the baseValue argument.
        /// </summary>
        /// <param name="statName">the name of the stat</param>
        /// <param name="baseValue">the base int value of the stat</param>
        /// <returns></returns>
        protected internal int ApplyIntStatModifiers(string statName, int baseValue)
        {
            float baseValueAsFloat = baseValue;
            float finalValueAsFloat = ApplyFloatStatModifiers(statName, baseValueAsFloat);

            int finalValue = (int)finalValueAsFloat;
            return finalValue;
        }

        public bool DoesStatExist(string statName)
        {
            return GetStatNames().Contains(statName);
        }

        public abstract string[] GetStatNames();
        public abstract float GetFloatStatValue(string statName);
        public abstract int GetIntStatValue(string statName);
        public abstract void RestoreHealthToMax();
    }
}