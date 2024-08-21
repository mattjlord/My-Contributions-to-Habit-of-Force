namespace CombatRevampScripts.ActionsAbilities.Structs.Events
{
    public enum TurnManagerEventType
    {
        OnCombatStart,

        OnUnitTurnStart,
        OnUnitTurnEnd,

        OnUnitMoveDistance,

        OnUnitIncomingMechDamage,
        OnUnitIncomingPilotDamage,
        OnUnitTakeMechDamage,
        OnUnitTakePilotDamage,
        OnUnitTakeMechHealing,
        OnUnitTakePilotHealing,

        OnUnitDamageMech,
        OnUnitDamagePilot,
        OnUnitHealMech,
        OnUnitHealPilot,

        OnUnitKillMech,
        OnUnitKillPilot,
        OnUnitMechDeath,
        OnUnitPilotDeath,
        OnUnitMechKilled,
        OnUnitPilotKilled,

        OnUnitEnterTile,
        OnUnitExitTile,
        OnUnitStartTurnOnTile,
        OnUnitEndTurnOnTile
    }
}