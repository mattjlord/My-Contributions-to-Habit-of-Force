using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.CombatVisuals.VFXController;
using System;
using UnityEngine;

namespace CombatRevampScripts.CombatVisuals.Handler
{
    /// <summary>
    /// Handles a single unit's combat-related visuals (animations and VFX) and their interaction
    /// with general combat logic.
    /// </summary>
    public interface ICombatVisualHandler
    {
        /// <summary>
        /// Plays an animation with the given name for this unit's animator.
        /// </summary>
        /// <param name="name">the name of the animation state to play</param>
        /// <param name="busyPeriod">A float between 0 and 1, to represent what percent of the animation
        /// the visual handler should be "busy" for after the animation is played.</param>
        public void PlayAnim(string name, float busyPeriod);

        /// <summary>
        /// Plays an animation with the given name for this unit's animator, as well as the given Action on
        /// completion.
        /// </summary>
        /// <param name="name">the name of the animation state to play</param>
        /// <param name="busyPeriod">A float between 0 and 1, to represent what percent of the animation
        /// the visual handler should be "busy" for after the animation is played.</param>
        /// <param name="actionOnComplete">the System.Action to execute after the busy period ends</param>
        public void PlayAnimIntoAction(string name, float busyPeriod, Action actionOnComplete);

        /// <summary>
        /// Instantiates the given GameObject if it has a VFX controller, initializes the controller using
        /// the given AEffectHolder, and uses the IsBusy method on the VFX controller to set its busy period.
        /// </summary>
        /// <param name="vfxObject">the VFX object to instantiate</param>
        /// <param name="effectHolder">the AEffectHolder that is playing the VFX</param>
        /// <returns>the VFX controller of the instantiated object</returns>
        public IVFXController PlayVFXObject(GameObject vfxObject, IEffectHolder effectHolder);

        /// <summary>
        /// Instantiates the given GameObject if it has a VFX controller, and uses the IsBusy
        /// method on the VFX controller to set its busy period. After the busy period ends, performs
        /// the given callback Action
        /// </summary>
        /// <param name="vfxObject">the VFX object to instantiate</param>
        /// <param name="effectHolder">the AEffectHolder that is playing the VFX</param>
        /// <param name="actionOnComplete">the System,Action to execute after the busy period ends</param>
        /// <returns>the VFX controller of the instantiated object</returns>
        public IVFXController PlayVFXObjectIntoAction(GameObject vfxObject, IEffectHolder effectHolder, Action actionOnComplete);

        /// <summary>
        /// Instantiates the given GameObject if it has a VFX controller, initializes the controller using
        /// the given unit.
        /// </summary>
        /// <param name="vfxObject">the VFX object to instantiate</param>
        /// <param name="unit">the unit to initialize this with</param>
        /// <returns></returns>
        public IVFXController PlayVFXObject(GameObject vfxObject, ICombatUnit unit);

        /// <summary>
        /// Sets the given boolean parameter for this unit's animator.
        /// </summary>
        /// <param name="name">the name of the boolean parameter</param>
        /// <param name="value">the value to set</param>
        public void SetAnimatorBool(string name, bool value);

        /// <summary>
        /// Returns whether or not this is "busy", i.e. when it is playing an animation or
        /// VFX object that is pausing combat logic.
        /// </summary>
        /// <returns>whether or not this handler is busy</returns>
        public bool IsBusy();
    }
}