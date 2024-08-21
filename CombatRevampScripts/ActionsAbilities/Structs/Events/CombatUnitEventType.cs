namespace CombatRevampScripts.ActionsAbilities.Structs.Events
{
    public enum CombatUnitEventType
    {
        OnThisTurnStart,
        OnThisTurnEnd,

        OnMoveDistance,

        OnIncomingMechDamage,
        OnIncomingPilotDamage,
        OnTakeMechDamage,
        OnTakePilotDamage,
        OnTakeMechHealing,
        OnTakePilotHealing,

        OnDamageMech,
        OnDamagePilot,
        OnHealMech,
        OnHealPilot,

        OnKillMech,
        OnKillPilot,
        OnMechDeath,
        OnPilotDeath,
        OnMechKilled,
        OnPilotKilled,

        OnEnterTile,
        OnExitTile,
        OnStartTurnOnTile,
        OnEndTurnOnTile
    }
}