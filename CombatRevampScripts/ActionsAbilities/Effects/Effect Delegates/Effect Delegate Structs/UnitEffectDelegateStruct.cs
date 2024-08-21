using CombatRevampScripts.ActionsAbilities.CombatUnit;

namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Effect_Delegate_Structs
{
    public enum UnitSubjectType
    {
        AssignedUnit,
        OwnerUnit,
        TargetUnit,
        TargetListOfUnits,
        EventArgUnit,
        EventSenderUnit
    }

    [System.Serializable]
    public struct UnitEffectDelegateStruct
    {
        public UnitSubjectType subject;
        public AEffectDelegate<ICombatUnit> effectDelegate;
    }
}