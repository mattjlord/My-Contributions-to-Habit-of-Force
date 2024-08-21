using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender;
using CombatRevampScripts.ActionsAbilities.Enums;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Unit_Delegates
{
    public enum DamageOrHealType
    {
        Mech,
        Pilot,
        UserSelection
    }

    public enum DamageOrHealSource
    {
        OwnerUnit,
        AssignedUnit,
        SubjectUnit,
        EventSenderUnit,
        EventArgUnit
    }

    /// <summary>
    /// Represents a delegate that modifies the health of a unit
    /// </summary>
    public abstract class AHealthEffectDelegate : AUnitFloatEffectDelegate
    {
        public DamageOrHealType type;
        protected DamageType damageType;

        public void ProcessDamageType(ICombatEffect effect)
        {
            switch (type)
            {
                case DamageOrHealType.UserSelection:
                    DamageTypeProperty property = (DamageTypeProperty)effect.GetPropertyOfType<DamageType>(typeof(DamageTypeProperty));
                    if (property == null)
                    {
                        throw new System.Exception("The type field for this damage/heal delegate was set to UserSelection, but a DamageTypeProperty on the effect this belongs to " +
                            "could not be found. To solve this, check the Ask For Damage Type box on the scriptable object for the Activated Ability the effect belongs to.");
                    }
                    damageType = property.GetValue();
                    return;
                case DamageOrHealType.Mech:
                    damageType = DamageType.LaserDamage;
                    return;
                case DamageOrHealType.Pilot:
                    damageType = DamageType.BallisticDamage;
                    return;
                default:
                    damageType = DamageType.None;
                    return;
            }
        }

        public ICombatUnit GetUnitFromDamageOrHealSource(DamageOrHealSource damageOrHealSource, ICombatUnit subject, ICombatEffect effect)
        {
            switch (damageOrHealSource)
            {
                case DamageOrHealSource.OwnerUnit:
                    return effect.GetOwner();
                case DamageOrHealSource.AssignedUnit:
                    return effect.GetAssignee();
                case DamageOrHealSource.SubjectUnit:
                    return subject;
                case DamageOrHealSource.EventArgUnit:
                    CombatUnitEventArgProperty unitArgProperty = (CombatUnitEventArgProperty)effect.GetPropertyOfType<ICombatUnit>(typeof(CombatUnitEventArgProperty));
                    if (unitArgProperty == null)
                    {
                        throw new System.Exception("The effect that this delegate belongs to does not have a CombatUnitEventArgProperty. Double check to make sure the " +
                            "EffectTrigger this belongs to is subscribed to an event that sends a CombatUnit argument. If the effect belongs to a CombatAction rather than " +
                            "an EffectTrigger, avoid referencing any event arguments.");
                    }
                    return unitArgProperty.GetValue();
                case DamageOrHealSource.EventSenderUnit:
                    SenderEventArgProperty unitSenderProperty = (SenderEventArgProperty)effect.GetPropertyOfType<object>(typeof(SenderEventArgProperty));
                    if (unitSenderProperty != null)
                    {
                        throw new System.Exception("The effect that this delegate belongs to does not have a SenderEventArgProperty. Double check to make sure the " +
                            "EffectTrigger this belongs to is subscribed to at least one event. If the effect belongs to a CombatAction rather than an EffectTrigger, " +
                            "avoid referencing any event arguments or senders.");
                    }
                    object senderObject = unitSenderProperty.GetValue();
                    if (senderObject.GetType() == typeof(ICombatUnit))
                    {
                        return (ICombatUnit)senderObject;
                    }
                    else
                    {
                        throw new System.Exception("The effect that this delegate belongs to does not have a SenderEventArgProperty that contains an ICombatUnit value. " +
                            "To fix this issue, make sure the EffectTrigger the effect belongs to is subscribed to at least one event that has a sender of type ICombatUnit.");
                    }
                default:
                    return null;
            }
        }
    }
}