using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors.PropertyModifierVisitors;
using CombatRevampScripts.ActionsAbilities.TurnTimer;

namespace CombatRevampScripts.ActionsAbilities.CombatModifiers
{
    public enum PropertyType
    {
        MechDamage,
        PilotDamage,
        MechHeal,
        PilotHeal,
        AOERange
    }

    /// <summary>
    /// Represents an ACombatModifier that modifies a property
    /// </summary>
    public class PropertyModifier : ACombatModifier
    {
        private PropertyType _propertyType;

        public PropertyModifier(PropertyType propertyType, float value, IActionOrPassive source, ModifierType type) : base(value, source, type) 
        {
            _propertyType = propertyType;
        }

        public PropertyModifier(PropertyType propertyType, float value, IActionOrPassive source, ModifierType type, int duration,
            TurnTimerBehavior timerBehavior) : base(value, source, type, duration, timerBehavior)
        {
            _propertyType = propertyType;
        }

        public override void AdditionalUnitSetup()
        {
            switch (_propertyType)
            {
                case PropertyType.MechDamage:
                    combatUnit.ModifyOwnedEffects(new MechDamageModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.PilotDamage:
                    combatUnit.ModifyOwnedEffects(new PilotDamageModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.MechHeal:
                    combatUnit.ModifyOwnedEffects(new MechHealModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.PilotHeal:
                    combatUnit.ModifyOwnedEffects(new PilotHealModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.AOERange:
                    combatUnit.ModifyOwnedEffects(new AOERangeModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
            }
        }

        public override void RemoveModifier()
        {
            switch (_propertyType)
            {
                case PropertyType.MechDamage:
                    combatUnit.ModifyOwnedEffects(new MechDamageModifierVisitor(this, PropertyModifierVisitorMode.RemoveModifier));
                    break;
                case PropertyType.PilotDamage:
                    combatUnit.ModifyOwnedEffects(new PilotDamageModifierVisitor(this, PropertyModifierVisitorMode.RemoveModifier));
                    break;
                case PropertyType.MechHeal:
                    combatUnit.ModifyOwnedEffects(new MechHealModifierVisitor(this, PropertyModifierVisitorMode.RemoveModifier));
                    break;
                case PropertyType.PilotHeal:
                    combatUnit.ModifyOwnedEffects(new PilotHealModifierVisitor(this, PropertyModifierVisitorMode.RemoveModifier));
                    break;
                case PropertyType.AOERange:
                    combatUnit.ModifyOwnedEffects(new AOERangeModifierVisitor(this, PropertyModifierVisitorMode.RemoveModifier));
                    break;
            }

            combatUnit.RemoveModifier(this);
        }

        /// <summary>
        /// Modifies a single passive
        /// </summary>
        /// <param name="passive">The ICombatPassive to modify</param>
        public void ModifyPassive(ICombatPassive passive)
        {
            switch (_propertyType)
            {
                case PropertyType.MechDamage:
                    passive.ModifyEffectTriggers(new MechDamageModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.PilotDamage:
                    passive.ModifyEffectTriggers(new PilotDamageModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.MechHeal:
                    passive.ModifyEffectTriggers(new MechHealModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.PilotHeal:
                    passive.ModifyEffectTriggers(new PilotHealModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
                case PropertyType.AOERange:
                    passive.ModifyEffectTriggers(new AOERangeModifierVisitor(this, PropertyModifierVisitorMode.AddModifier));
                    break;
            }
        }
    }
}