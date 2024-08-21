using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;

namespace CombatRevampScripts.CombatVisuals.VFXController
{
    public interface IVFXController
    {
        /// <summary>
        /// Returns whether this controller is currently busy
        /// animating VFX
        /// </summary>
        /// <returns>whether this controller is busy</returns>
        public bool IsBusy();

        /// <summary>
        /// Performs necessary procedures for when the busy period
        /// of this ends, as called from a CombatVisualHandler.
        /// </summary>
        public void OnBusyPeriodEnd();

        /// <summary>
        /// Destroys this and its GameObject
        /// </summary>
        public void Destroy();

        /// <summary>
        /// Sets up Visual Effect values of this based on values that are
        /// attainable from the given effect holder that created this.
        /// </summary>
        /// <param name="effectHolder">the effect holder that created this</param>
        public void SetupFromEffectHolder(IEffectHolder effectHolder);

        /// <summary>
        /// Sets the transform of this to match and follow the transform of the given unit
        /// (used for Combat Passives only).
        /// </summary>
        /// <param name="unit">the unit to attach to</param>
        public void SetTransformFromUnit(ICombatUnit unit);
    }
}