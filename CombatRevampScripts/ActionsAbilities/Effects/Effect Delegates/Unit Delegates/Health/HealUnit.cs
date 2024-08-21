using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Enums;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    /// <summary>
    /// An effect delegate that heals the subject unit by a specified amount
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Effect Delegates/CombatUnit/Health/Heal Unit")]
    public class HealUnit : AHealthEffectDelegate
    {
        public DamageOrHealSource healSource;

        private MechHealProperty _mechHealProperty;
        private PilotHealProperty _pilotHealProperty;

        public override void InitializeEffectProperties(ICombatEffect effect)
        {
            if (type == DamageOrHealType.Mech || type == DamageOrHealType.UserSelection)
            {
                _mechHealProperty = (MechHealProperty)effect.AddProperty(new MechHealProperty(0));
            }
            if (type == DamageOrHealType.Pilot || type == DamageOrHealType.UserSelection)
            {
                _pilotHealProperty = (PilotHealProperty)effect.AddProperty(new PilotHealProperty(0));
            }
        }

        public override void Invoke(ICombatUnit subject, ICombatEffect effect)
        {
            ICombatUnit sourceUnit = GetUnitFromDamageOrHealSource(healSource, subject, effect);

            SetupNode(subject, effect);
            ProcessDamageType(effect);

            float healingSent = 0;
            float healingDealt = 0;

            switch (damageType)
            {
                case DamageType.LaserDamage:
                    _mechHealProperty.SetValue(valueNode.Compute());
                    healingSent = _mechHealProperty.GetValue();
                    healingDealt = subject.HealMech(healingSent, sourceUnit);
                    break;
                case DamageType.BallisticDamage:
                    _pilotHealProperty.SetValue(valueNode.Compute());
                    healingSent = _pilotHealProperty.GetValue();
                    healingDealt = subject.HealPilot(healingSent, sourceUnit);
                    break;
            }

            effect.GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastHealingSent, healingSent);
            effect.GetEffectHolder().SetTempEffectValue(TempEffectValueType.LastHealingDealt, healingDealt);
        }
    }
}