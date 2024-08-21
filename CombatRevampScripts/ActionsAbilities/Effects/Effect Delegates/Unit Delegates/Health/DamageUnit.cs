using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Enums;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that damages the subject unit by a specified amount
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Health/Damage Unit")]
    public class DamageUnit : AHealthEffectDelegate
    {
        public DamageOrHealSource damageSource;
        public bool useDefaultAttackDamageValues;

        private MechDamageProperty _mechDamageProperty;
        private PilotDamageProperty _pilotDamageProperty;

        public override void InitializeEffectProperties(ICombatEffect effect)
        {
            if (type == DamageOrHealType.Mech || type == DamageOrHealType.UserSelection)
            {
                _mechDamageProperty = (MechDamageProperty)effect.AddProperty(new MechDamageProperty(0));
            }
            if (type == DamageOrHealType.Pilot || type == DamageOrHealType.UserSelection)
            {
                _pilotDamageProperty = (PilotDamageProperty)effect.AddProperty(new PilotDamageProperty(0));
            }
        }

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            ICombatUnit sourceUnit = GetUnitFromDamageOrHealSource(damageSource, subject, effect);

            SetupNode(subject, effect);
            ProcessDamageType(effect);

            float damageSent = 0;
            float damageDealt = 0;

            switch (damageType)
            {
                case DamageType.LaserDamage:
                    if (useDefaultAttackDamageValues)
                    {
                        _mechDamageProperty.SetValue(effect.GetOwner().GetMechSO().GetFloatStatValue("laserDamage"));
                    }
                    else
                    {
                        _mechDamageProperty.SetValue(valueNode.Compute());
                    }
                    damageSent = _mechDamageProperty.GetValue();
                    damageDealt = subject.DamageMech(damageSent, sourceUnit);
                    break;
                case DamageType.BallisticDamage:
                    if (useDefaultAttackDamageValues)
                    {
                        _pilotDamageProperty.SetValue(effect.GetOwner().GetMechSO().GetFloatStatValue("ballisticDamage"));
                    }
                    else
                    {
                        _pilotDamageProperty.SetValue(valueNode.Compute());
                    }
                    damageSent = _pilotDamageProperty.GetValue();
                    damageDealt = subject.DamagePilot(damageSent, sourceUnit);
                    break;
            }

            effect.GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastDamageSent, damageSent);
            effect.GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastDamageDealt, damageDealt);
        }
    }
}