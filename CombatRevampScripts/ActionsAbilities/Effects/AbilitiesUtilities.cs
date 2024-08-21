using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Custom_Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AbilityDictionary
{
    // Aurelio
    Bulwark,
    Guardian,
    Guardian_Passive_Ballistic,
    Guardian_Passive_Laser,
    Repair_Drone,

    // Bell
    Blitz,
    Evasive,

    // Principality
    Adhesive_Compound,
    Buff_Ally_Damage,
    Buff_Ally_Defense,
    Debuff_Enemy_Defense,
    Explosive_Shotgun,
    Plasma_Mine,
    Plasma_Mine_Field,
    Triage
}

/// <summary>
/// This is a temporary solution
/// </summary>
public static class AbilitiesUtilities
{
    private static Dictionary<AbilityDictionary, ACombatEffect> abilities;

    static AbilitiesUtilities ()
    {
        abilities = new Dictionary<AbilityDictionary, ACombatEffect>
        {
            // Aurelio
            { AbilityDictionary.Bulwark, new Bulwark() },
            { AbilityDictionary.Guardian, new Guardian() },
            { AbilityDictionary.Guardian_Passive_Ballistic, new GuardianPassiveBallistic() },
            { AbilityDictionary.Guardian_Passive_Laser, new GuardianPassiveLaser() },
            { AbilityDictionary.Repair_Drone, new RepairDrone() },

            // Bell
            { AbilityDictionary.Blitz, new Blitz() },
            { AbilityDictionary.Evasive, new Evasive() },

            // Principality
            { AbilityDictionary.Adhesive_Compound, new AdhesiveCompound() },
            { AbilityDictionary.Buff_Ally_Damage, new BuffAllyDamage() },
            { AbilityDictionary.Buff_Ally_Defense, new BuffAllyDefense() },
            { AbilityDictionary.Debuff_Enemy_Defense, new DebuffEnemyDefense() },
            { AbilityDictionary.Explosive_Shotgun, new ExplosiveShotgun() },
            { AbilityDictionary.Plasma_Mine, new PlasmaMine() },
            { AbilityDictionary.Plasma_Mine_Field, new PlasmaMineField() },
            { AbilityDictionary.Triage, new Triage() }
        };
    }

    public static ACombatEffect FetchAbility(AbilityDictionary key)
    {
        if (abilities.ContainsKey(key))
        {
            return abilities[key];
        }
        else
        {
            throw new KeyNotFoundException($"Ability with key {key} not found.");
        }
    }
}
