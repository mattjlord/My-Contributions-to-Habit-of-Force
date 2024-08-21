using CombatRevampScripts.ActionsAbilities.AbilitySOs;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.Board.TilePiece.MechPilot
{
	/// <summary>
	/// Represents the gameplay information related to a pilot.
	/// </summary>
	/// <remarks>
	/// Public fields used because they ought to
	/// be able to be observed and written
	/// </remarks>
	[CreateAssetMenu(menuName = "Pilot SO")]
	public class PilotSO : ACoreStatManager
    {
        public string pilotName;
		public float maxHealth;
		public float currHealth;
        public float tempHealth;
		
		public Affiliation affiliation;

		public float reactionSpeed; // Used for dodging
		public float precision; // Used for accuracy
		public float leadership = 0;

        public List<ActivatedAbilitySO> activatedAbilitySOs;
        public List<PassiveAbilitySO> passiveAbilitySOs;

        public override void RestoreHealthToMax()
        {
			currHealth = maxHealth;
        }

        public override string[] GetStatNames()
        {
            string[] validNames = { "maxHealth", "currHealth", "tempHealth", "leadership", "reactionSpeed", "precision" };
            return validNames;
        }

        public override float GetFloatStatValue(string statName)
        {
            switch (statName)
            {
                case "maxHealth":
                    return ApplyFloatStatModifiers("maxHealth", maxHealth);
                case "currHealth":
                    return ApplyFloatStatModifiers("currHealth", currHealth);
                case "tempHealth":
                    return ApplyFloatStatModifiers("tempHealth", tempHealth);
                case "leadership":
                    return ApplyFloatStatModifiers("leadership", leadership);
                case "reactionSpeed":
                    return ApplyFloatStatModifiers("reactionSpeed", reactionSpeed);
                case "precision":
                    return ApplyFloatStatModifiers("precision", precision);
                default:
                    throw new System.ArgumentException("A float stat with the given name does not exist!");
            }
        }

        public override int GetIntStatValue(string statName)
        {
            throw new System.ArgumentException("The PilotSO class does not contain any int stats!");
        }
    }
}