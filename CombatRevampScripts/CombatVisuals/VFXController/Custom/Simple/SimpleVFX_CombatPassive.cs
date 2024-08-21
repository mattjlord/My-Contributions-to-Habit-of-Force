using CombatRevampScripts.ActionsAbilities.EffectHolders;

namespace CombatRevampScripts.CombatVisuals.VFXController.Custom
{
    public class SimpleVFX_CombatPassive : AVFXController
    {
        public override void Destroy()
        {
            transform.parent = null;
            Destroy(gameObject);
        }

        public override bool IsBusy()
        {
            return false;
        }

        public override void OnBusyPeriodEnd()
        {
            // DO NOTHING
        }

        public override void SetupFromEffectHolder(IEffectHolder effectHolder)
        {
            // Can't be set up this way
        }
    }
}