using CombatRevampScripts.ActionsAbilities.CombatUnit;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    public enum KillMode
    {
        KillMech,
        KillPilot
    }

    /// <summary>
    /// Kills the subject unit's Mech or Pilot
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Kill Unit Mech or Pilot")]
    public class KillUnit : AEffectDelegate<ICombatUnit>
    {
        public KillMode killMode;
        public bool invokeKillEventOnAssignedUnit;

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            switch (killMode)
            {
                case KillMode.KillMech:
                    if (invokeKillEventOnAssignedUnit)
                    {
                        subject.KillMech(effect.GetOwner());
                    }
                    else
                    {
                        subject.KillMech();
                    }
                    break;
                case KillMode.KillPilot:
                    if (invokeKillEventOnAssignedUnit)
                    {
                        subject.KillPilot(effect.GetOwner());
                    }
                    else
                    {
                        subject.KillPilot();
                    }
                    break;
            }
        }
    }
}