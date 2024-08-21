using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.Board.TilePiece.MechPilot;

namespace CombatRevampScripts.ActionsAbilities.CombatModifiers
{
    /// <summary>
    /// Represents an ACombatModifier that modifies a Mech or Pilot stat
    /// </summary>
    public class StatModifier : ACombatModifier
    {
        public StatType statType;
        public string statName;

        public StatModifier(StatType statType, string statName, float value, IActionOrPassive source, ModifierType type) : base(value, source, type)
        {
            this.statType = statType;
            this.statName = statName;
        }

        public StatModifier(StatType statType, string statName, float value, IActionOrPassive source, ModifierType type, int duration, 
            TurnTimerBehavior timerBehavior) : base(value, source, type, duration, timerBehavior)
        {
            this.statType = statType;
            this.statName = statName;
        }

        public override void AdditionalUnitSetup()
        {
            switch (statType)
            {
                case StatType.Pilot:
                    combatUnit.GetPilotSO().AddStatModifier(this);
                    break;
                case StatType.Mech:
                    combatUnit.GetMechSO().AddStatModifier(this);
                    break;
                default:
                    break;
            }
        }

        public override void RemoveModifier()
        {
            switch (statType)
            {
                case StatType.Pilot:
                    combatUnit.GetPilotSO().RemoveStatModifier(this);
                    break;
                case StatType.Mech:
                    combatUnit.GetMechSO().RemoveStatModifier(this);
                    break;
                default:
                    break;
            }

            combatUnit.RemoveModifier(this);
        }
    }
}