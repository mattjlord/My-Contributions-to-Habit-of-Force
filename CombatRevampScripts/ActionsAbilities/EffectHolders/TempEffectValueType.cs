namespace CombatRevampScripts.ActionsAbilities.EffectHolders
{
    /// <summary>
    /// The types of temporary, effect-generated values that can be stored in an AEffectHolder over the
    /// course of its execution.
    /// </summary>
    public enum TempEffectValueType
    {
        LastDamageSent,
        LastDamageDealt,
        LastHealingSent,
        LastHealingDealt,
        LastDistanceMoved
    }
}