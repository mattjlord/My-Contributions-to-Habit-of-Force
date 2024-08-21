using CombatRevampScripts.ActionsAbilities.AbilitySOs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatRevampScripts.Board.TilePiece.MechPilot
{
	/// <summary>
	/// Represents the gameplay information related to a mech.
	/// </summary>
	/// <remarks>
	/// Public fields used because they ought to
	/// be able to be observed and written,
	/// any management of data (such as resetting a stat between battles)
	/// must be triggerred elsewhere 
	/// </remarks>
	[CreateAssetMenu(menuName = "Mech SO")]
	public class MechSO : ACoreStatManager
	{
		public string mechName;
		public float maxHealth;
		public float currHealth;
		public float tempHealth;
		public float speed;
		public float ballisticDamage;
		public float laserDamage;
		public float defense;
		public float clarity;
		public float accuracy;
		public int moveRange;
		public int ballisticAttackRange;
		public int laserAttackRange;
		public bool hasFlag;
		public bool hasFlagPending;
		public bool isDefending;
		public Sprite mechSprite;

		public List<ActivatedAbilitySO> activatedAbilitySOs;
		public List<PassiveAbilitySO> passiveAbilitySOs;

        public override void RestoreHealthToMax()
        {
			currHealth = maxHealth;
        }

		public override string[] GetStatNames()
		{
			string[] statNames = { "maxHealth", "currHealth", "tempHealth", "speed", "ballisticDamage", "laserDamage", "defense", "clarity", "accuracy",
			"moveRange", "ballisticAttackRange", "laserAttackRange" };
			return statNames;
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
				case "speed":
					return ApplyFloatStatModifiers("speed", speed);
				case "ballisticDamage":
					return ApplyFloatStatModifiers("ballisticDamage", ballisticDamage);
				case "laserDamage":
					return ApplyFloatStatModifiers("laserDamage", laserDamage);
				case "defense":
					return ApplyFloatStatModifiers("defense", defense);
				case "clarity":
					return ApplyFloatStatModifiers("clarity", clarity);
				case "accuracy":
					return ApplyFloatStatModifiers("accuracy", accuracy);
				default:
					throw new System.ArgumentException("A float stat with the given name does not exist!");
			}
        }

        public override int GetIntStatValue(string statName)
        {
            switch (statName)
            {
                case "moveRange":
					return ApplyIntStatModifiers("moveRange", moveRange);
				case "ballisticAttackRange":
					return ApplyIntStatModifiers("ballisticAttackRange", ballisticAttackRange);
				case "laserAttackRange":
					return ApplyIntStatModifiers("laserAttackRange", laserAttackRange);
				default:
                    throw new System.ArgumentException("An int stat with the given name does not exist!");
            }
        }
    }
}