using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatUnit;

namespace CombatRevampScripts.ActionsAbilities.CombatModifiers
{
    public enum ModifierType
    {
        Add,
        Multiply
    }

    /// <summary>
    /// Represents a modifier to a numerical value in combat that can also use a turn timer
    /// </summary>
    public abstract class ACombatModifier : ATurnTimer, ICombatModifier
    {
        private float _value;
        private IActionOrPassive _source;
        private ModifierType _type;

        protected ICombatUnit combatUnit;

        public ACombatModifier(float value, IActionOrPassive source, ModifierType type) : base(0, TurnTimerBehavior.EndOfTurn)
        {
            _value = value;
            _source = source;
            _type = type;
        }

        public ACombatModifier(float value, IActionOrPassive source, ModifierType type, int duration, TurnTimerBehavior timerBehavior) : base(duration, timerBehavior)
        {
            _value = value;
            _source = source;
            _type = type;
        }

        public float GetModifierValue()
        {
            return _value;
        }

        public IActionOrPassive GetModifierSource()
        {
            return _source;
        }

        public ModifierType GetModifierType()
        {
            return _type;
        }

        public float ApplyToValue(float value)
        {
            switch (_type)
            {
                case ModifierType.Add:
                    return value + _value;
                case ModifierType.Multiply:
                    return value * _value;
                default:
                    return value;
            }
        }

        public void SetupUnit(ICombatUnit combatUnit)
        {
            this.combatUnit = combatUnit;
            AdditionalUnitSetup();
        }

        public override void OnTimerEnd()
        {
            RemoveModifier();
        }

        /// <summary>
        /// Performs additional setup procedure that isn't covered by the combat unit of this, or the SetupUnit method
        /// </summary>
        public abstract void AdditionalUnitSetup();

        public abstract void RemoveModifier();
    }
}
