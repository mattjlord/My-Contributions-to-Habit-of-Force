namespace CombatRevampScripts.ActionsAbilities.CombatActions
{
    public enum TargetingRules
    {
        None,
        TargetAnyTile,
        TargetOtherTile,
        TargetAnyNonHostileTile,
        TargetOtherNonHostileTile,
        TargetAnyNonFriendlyTile,
        TargetEmptyTile,
        TargetAnyOccupiedTile,
        TargetOtherOccupiedTile,
        TargetAnyUnit,
        TargetOtherUnit,
        TargetAnyFriendlyUnit,
        TargetOtherFriendlyUnit,
        TargetAnyHostileUnit
    }
}