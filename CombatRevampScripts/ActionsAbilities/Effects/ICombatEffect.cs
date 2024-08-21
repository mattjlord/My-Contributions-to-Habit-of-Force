using System;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;

namespace CombatRevampScripts.ActionsAbilities.Effects
{
    /// <summary>
    /// Represents a behavior in combat that is triggered by IEffectHolders of an ICombatUnit and contains IEffectProperties.
    /// </summary>
    public interface ICombatEffect
    {
        /// <summary>
        /// Performs this effect
        /// </summary>
        public void DoEffect();

        /// <summary>
        /// If the Dictionary of IEffectProperties for this contains a property of type T,
        /// calls the Accept function on that property using the given visitor as its argument,
        /// otherwise does nothing
        /// </summary>
        /// <param name="visitor">the visitor to use</param>
        public void ModifyProperty<T, U>(IEffectPropertyVisitor<T, U> visitor);

        /// <summary>
        /// Adds the given property to this effect's Dictionary of IEffectProperties, if it is not already present, 
        /// otherwise does nothing
        /// </summary>
        /// <param name="property">the IEffectProperty to add</param
        /// <returns>the property to add</returns>
        public IEffectProperty<U> AddProperty<U>(IEffectProperty<U> property);

        /// <summary>
        /// Gets the IEffectProperty of the given type for this effect,
        /// returns null if it can't
        /// </summary>
        /// <param name="propertyType">the Type of the property to look for</param>
        /// <returns>the instance of the property used by this effect</returns>
        public IEffectProperty<U> GetPropertyOfType<U>(Type propertyType);

        /// <summary>
        /// If a property of the given type exists, returns that property. Otherwise,
        /// adds the property, then returns it.
        /// </summary>
        /// <typeparam name="U">the value type contained in the property to get/add</typeparam>
        /// <param name="propertyType">the property type to get/add</param>
        /// <returns></returns>
        public IEffectProperty<U> GetOrAddPropertyOfType<U>(Type propertyType);
        
        /// <summary>
        /// Searches for the named passive in this effect's passives dictionary and
        /// builds it if found, returning a new instance of an ICombatPassive
        /// </summary>
        /// <param name="name">the name of the passive to get</param>
        /// <returns>the built passive, if it is found, otherwise null</returns>
        public ICombatPassive GetPassiveByName(string name);

        /// <summary>
        /// If this isn't a delegate-based effect, returns null,
        /// otherwise searches for a built passive with the given name
        /// and returns it if found.
        /// </summary>
        /// <param name="name">the name of the passive to search for</param>
        /// <returns>the passive, if found, otherwise null</returns>
        public ICombatPassive GetBuiltPassiveByName(string name);

        /// <summary>
        /// If this isn't a delegate-based effect, does nothing,
        /// otherwise creates an entry for the given passive
        /// </summary>
        /// <param name="passive">the passive to add</param>
        public void AddBuiltPassive(ICombatPassive passive);

        /// <summary>
        /// Creates a dictionary entry for the given passive struct.
        /// </summary>
        /// <param name="passive">the passive struct to add</param>
        public void AddSavedPassive(CombatPassiveStruct passive);

        /// <summary>
        /// Sets the owner unit of this to the given one. If this isn't set, it will automatically be set to the value
        /// of combatUnit when DoEffect is called.
        /// </summary>
        /// <param name="owner">the owner unit to set</param>
        public void SetOwner(ICombatUnit owner);

        /// <summary>
        /// Gets the owner of this effect
        /// </summary>
        /// <returns>the combat unit owner of this effect</returns>
        public ICombatUnit GetOwner();

        /// <summary>
        /// Sets the assigned unit of this to the given one. If this isn't set, DoEffect will not work.
        /// </summary>
        /// <param name="owner">the owner unit to set</param>
        public void SetAssignee(ICombatUnit assignee);

        /// <summary>
        /// Gets the assignee of this effect
        /// </summary>
        /// <returns>the combat unit assignee of this effect</returns>
        public ICombatUnit GetAssignee();

        /// <summary>
        /// Sets the source action or passive of this effect
        /// </summary>
        /// <param name="source">the IActionOrPassive source of this effect</param>
        public void SetSource(IActionOrPassive source);

        /// <summary>
        /// Overloaded variant of SetSource that also sets the EffectHolder 
        /// (for actions, the source and holder are the same).
        /// </summary>
        /// <param name="source">the ICombatAction source of this effect</param>
        public void SetSource(ICombatAction source);

        /// <summary>
        /// Sets the effect holder of this effect
        /// </summary>
        /// <param name="holder">the IEffectHolder that holds this effect</param>
        public void SetEffectHolder(IEffectHolder holder);

        /// <summary>
        /// Gets the source action or passive of this effect
        /// </summary>
        /// <returns>the IActionOrPassive source of this effect</returns>
        public IActionOrPassive GetSource();

        /// <summary>
        /// Gets the effect holder of this effect
        /// </summary>
        /// <returns>the IEffectHolder holding this effect</returns>
        public IEffectHolder GetEffectHolder();

        /// <summary>
        /// Sets the callback action field of this to the given
        /// </summary>
        /// <param name="callbackAction">the System.Action to set</param>
        public void SetCallbackAction(Action callbackAction);

        /// <summary>
        /// Returns whether to send callback actions to this effect rather than use standard IEffectHolder
        /// callback handling
        /// </summary>
        /// <returns>whether or not to route incoming Action callbacks to this effect</returns>
        public bool ShouldRouteCallbackToEffect();
    }
}
