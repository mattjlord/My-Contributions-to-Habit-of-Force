using UnityEngine;

namespace CombatRevampScripts.Extensions
{
    public static class MathExtension
    {
        public static int ManhattanDistance(Vector2Int first, Vector2Int second)
        {
            return Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y);
        }

        public static float ApplyModifiers(float baseVal, float addend, float multiplier)
        {
            return (baseVal * multiplier) + addend;
        }

        public static float CalculateHitChance(float accuracy, float precision, float reactionSpeed)
        {
            return (accuracy/100) + 96 - (10 * (reactionSpeed/100)) + (10 * (precision/100));
            //"(Mech's Accuracy/100) + 96 - (10 * (Enemy Pilot Reaction Speed/100)) + (10 * (Pilot Precision/100))
            
        }

        public static float CalculateCriticalHitChance(float accuracy, float precision)
        {
            return ((precision * 5 + accuracy) / 100) - 2f;
            //((Pilot Precision * 5 + Mech Accuracy) / 100) -2
        }

        public static bool GenerateChanceOutcome(float chance)
        {
            float rand = Random.Range(0, 100);
            return rand <= chance;
        }
    }
}