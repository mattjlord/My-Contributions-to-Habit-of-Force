using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.CombatVisuals.OneShotAnim;
using System;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.Effects.Properties;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;

namespace CombatRevampScripts.ActionsAbilities.EffectHolders
{
    /// <summary>
    /// Represents an object that contains a list of ICombatEffects that it can trigger with DoEffects, as well
    /// as specific combat visuals that can be triggered alongside the effect.
    /// </summary>
    public interface IEffectHolder
    {
        /// <summary>
        /// Sets the combat unit of this to the given one
        /// </summary>
        /// <param name="combatUnit">the ICombatUnit to set</param>
        public void SetCombatUnit(ICombatUnit combatUnit);

        /// <summary>
        /// Gets the combat unit of this effect trigger
        /// </summary>
        /// <returns>the ICombatUnit of this effect trigger</returns>
        public ICombatUnit GetCombatUnit();

        /// <summary>
        /// Does the effects of this
        /// </summary>
        public void DoEffects();

        /// <summary>
        /// Adds the given effect to this
        /// </summary>
        /// <param name="effects">the effect to add</param>
        public void AddEffect(ICombatEffect effect);

        /// <summary>
        /// Does the effects of this, then invokes the given action on completion.
        /// </summary>
        /// <param name="actionOnComplete">the action delegate to invoke</param>
        public void DoEffects(Action actionOnComplete);

        /// <summary>
        /// Gets the effects of this
        /// </summary>
        /// <returns>the list of ICombatEffect of this</returns>
        public List<ICombatEffect> GetEffects();

        /// <summary>
        /// Modifies the current ICombatEffect of this with the given visitor
        /// </summary>
        /// <param name="visitor">the IEffectPropertyVisitor used to modify this</param>
        public void ModifyEffects<T, U>(IEffectPropertyVisitor<T, U> visitor);

        /// <summary>
        /// Sets the visual info of this to the given.
        /// </summary>
        /// <param name="info">the info to set</param>
        public void SetEffectVisualInfo(EffectVisualInfo info);

        /// <summary>
        /// Returns the first instance of a property of the given type in the effects
        /// of this, otherwise returns null
        /// </summary>
        /// <typeparam name="U">the type of value stored in the property</typeparam>
        /// <param name="propertyType">the type of property to get</param>
        /// <returns>the property, in interface form</returns>
        public IEffectProperty<U> GetFirstPropertyOfType<U>(Type propertyType);

        /// <summary>
        /// Gets a stored temporary effect-generated value of the given type
        /// </summary>
        /// <param name="valueType">the type of value to retrieve</param>
        /// <returns>the value</returns>
        public float GetTempEffectValue(TempEffectValueType valueType);

        /// <summary>
        /// Sets the value of a stored temporary effect-generated value of the given type
        /// </summary>
        /// <param name="valueType">the type of value to set/param>
        /// <param name="value">the value to set</param>
        public void SetTempEffectValue(TempEffectValueType valueType, float value);

        public void AddSavedPassive(CombatPassiveStruct passive);
    }
}