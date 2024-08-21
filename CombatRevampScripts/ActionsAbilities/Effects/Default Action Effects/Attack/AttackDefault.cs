using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.Extensions;
using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using CombatRevampScripts.ActionsAbilities.EffectHolders;

namespace CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Attack
{
    public class AttackDefault : ACombatEffect
    {
        public PilotDamageProperty pilotDamageProperty;
        public MechDamageProperty mechDamageProperty;

        protected float damageDealt;
        protected bool hitSuccess;

        public override void Initialize()
        {
            base.Initialize();
            pilotDamageProperty = (PilotDamageProperty)AddProperty(new PilotDamageProperty(0));
            mechDamageProperty = (MechDamageProperty)AddProperty(new MechDamageProperty(0));
        }

        public override void DoDefaultEffect()
        {
            float accuracy = assignedUnit.GetMechSO().GetFloatStatValue("accuracy");
            float precision = assignedUnit.GetPilotSO().GetFloatStatValue("precision");
            float reactionSpeed = targetUnit.GetPilotSO().GetFloatStatValue("reactionSpeed");
            float hitChance = MathExtension.CalculateHitChance(accuracy, precision, reactionSpeed);
            hitSuccess = MathExtension.GenerateChanceOutcome(hitChance);

            if (hitSuccess)
            {
                pilotDamageProperty.SetValue(assignedUnit.GetMechSO().ballisticDamage);
                mechDamageProperty.SetValue(assignedUnit.GetMechSO().laserDamage);
                float pilotDamage = pilotDamageProperty.GetValue();
                float mechDamage = mechDamageProperty.GetValue();

                switch (damageType)
                {
                    case DamageType.BallisticDamage:
                        GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastDamageSent, pilotDamage);
                        damageDealt = targetUnit.DamagePilot(pilotDamage, assignedUnit);
                        break;
                    case DamageType.LaserDamage:
                        GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastDamageSent, mechDamage);
                        damageDealt = targetUnit.DamageMech(mechDamage, assignedUnit);
                        break;
                }

                GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastDamageDealt, damageDealt);

                float critChance = MathExtension.CalculateCriticalHitChance(accuracy, precision);
                bool critSuccess = MathExtension.GenerateChanceOutcome(critChance);
                if (critSuccess)
                {
                    targetUnit.AddModifier(new StatModifier(StatType.Mech, "moveRange", 0.5f, effectSource, 
                        ModifierType.Multiply, 1, TurnTimer.TurnTimerBehavior.EndOfTurn));
                    targetUnit.AddModifier(new StatModifier(StatType.Mech, "ballisticAttackRange", 0.5f, effectSource,
                        ModifierType.Multiply, 1, TurnTimer.TurnTimerBehavior.EndOfTurn));
                    targetUnit.AddModifier(new StatModifier(StatType.Mech, "laserAttackRange", 0.5f, effectSource,
                        ModifierType.Multiply, 1, TurnTimer.TurnTimerBehavior.EndOfTurn));
                    targetUnit.AddModifier(new StatModifier(StatType.Mech, "defense", 0.25f, effectSource,
                        ModifierType.Multiply, 1, TurnTimer.TurnTimerBehavior.EndOfTurn));
                }
            }
        }
    }
}
