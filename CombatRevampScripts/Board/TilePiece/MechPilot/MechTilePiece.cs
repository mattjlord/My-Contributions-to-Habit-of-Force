using CombatRevampScripts.ActionsAbilities.Abilities.ActivatedAbilities;
using CombatRevampScripts.ActionsAbilities.Abilities.PassiveAbilities;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.ActionsAbilities.AbilitySOs;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.GeneralVisitors;
using CombatRevampScripts.CombatVisuals.Handler;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
#if UNITY_EDITOR
#endif

namespace CombatRevampScripts.Board.TilePiece.MechPilot
{
	/// <summary>
	/// The tile piece component for the mech.
	/// </summary>
	public class MechTilePiece : MonoBehaviour, ITilePiece
	{
		[SerializeField] private GameObject _allyParticles;
		[SerializeField] private GameObject _enemyParticles;
		
		[SerializeField] private MechSO _mechSO;
		[SerializeField] private PilotSO _pilotSO;
		[SerializeField] private AttackVisualInfo _attackVisualInfo;
		[SerializeField] private DefendVisualInfo _defendVisualInfo;
		[SerializeField] private DeathVisualInfo _deathVisualInfo;
		[SerializeField] private Transform _specialVFXOrigin;

        private ICombatUnit combatUnit;

		private float _realTimeMoveSpeed = 0.1f;

		private ICombatVisualHandler _visualHandler;

        /*private void OnEnable()
        {
			CombatUnit.OnMechDeath += ProcessMechDeath;
            CombatUnit.OnPilotDeath += ProcessPilotDeath;
        }

        private void OnDisable()
        {
            CombatUnit.OnMechDeath -= ProcessMechDeath;
            CombatUnit.OnPilotDeath -= ProcessPilotDeath;
        }*/

        /// <summary>
        /// Returns this mech scriptable object.
        /// </summary>
        /// <returns>the mech scriptable object</returns>
        public MechSO GetMechSO()
		{
			return _mechSO;
		}
		
		/// <summary>
		/// Sets this mech scriptable object to the given.
		/// </summary>
		/// <param name="mechSO">The given mech scriptable object</param>
		public void SetMechSO(MechSO mechSO)
		{
			_mechSO = mechSO;
		}

		/// <summary>
		/// Returns this pilot scriptable object.
		/// </summary>
		/// <returns>the pilot scriptable object</returns>
		public PilotSO GetPilotSO()
		{
			return _pilotSO;
		}

		/// <summary>
		/// Sets this pilot scriptable object to the given.
		/// </summary>
		/// <param name="pilotSO">The given pilot scriptable object</param>
		public void SetPilotSO(PilotSO pilotSO)
		{
			_pilotSO = pilotSO;
		}

		/// <summary>
		/// Sets this ICombatUnit to the given.
		/// </summary>
		/// <param name="combatUnit">the given ICombatUnit</param>
		public void SetCombatUnit(ICombatUnit combatUnit)
		{
			this.combatUnit = combatUnit;
		}

		/// <summary>
		/// Gets this ICombatUnit
		/// </summary>
		/// <returns>this ICombatUnit</returns>
		public ICombatUnit GetCombatUnit()
		{
			if (combatUnit == null)
			{
				Debug.LogError("DevError : this mech tile piece does not have a combat unit");
			}
			return combatUnit;
		}

		public Transform GetSpecialVFXOrigin()
		{
			return _specialVFXOrigin;
		}

		public ICombatVisualHandler GetVisualHandler()
		{
			if (_visualHandler == null)
			{
				_visualHandler = GetComponent<ICombatVisualHandler>();
			}

			if (_visualHandler == null)
			{
				Debug.LogWarning("Could not find a CombatVisualHandler component on this tile piece!");
			}

			return _visualHandler;
		}

        public void Initialize()
		{
			Transform tf = transform;

			switch (_pilotSO.affiliation)
			{
				case Affiliation.ally:
					Instantiate(_allyParticles, tf.position + new Vector3(0, 0.4f, 0), 
						Quaternion.identity, tf);
					break;
				case Affiliation.enemy:
					Instantiate(_enemyParticles, tf.position + new Vector3(0, 0.4f, 0), 
						Quaternion.identity, tf);
					break;
			}
		}

		public void ShutDown()
		{
			// TODO: Destroy the scriptable object as well, depending on settings...
			#if UNITY_EDITOR
				Undo.DestroyObjectImmediate(gameObject);
			#else
				DestroyImmediate(gameObject);
			#endif
		}

		public void SetPosition(Vector2Int pos)
		{
			//var tf = transform;
			transform.position = new Vector3(pos.x, transform.position.y, pos.y);
		}

		public Vector2Int GetPosition()
		{
			var position = transform.position;
			return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
		}

		public T Accept<T>(ITilePieceVisitor<T> visitor)
		{
			return visitor.Visit(this);
		}

        public List<ITile> GetTilesInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation, 
			TileStatus tileStatus, PathfindingMode pathfindingMode)
        {
            ITile tile = transform.GetComponentInParent<ITile>();
			if (tile == null)
			{
				throw new System.Exception("A TilePiece GameObject must have a parent Tile GameObject for AOE methods to work!");
			}
			return tile.GetTilesInAOE(source, radius, selfInclusive, relativeAffiliation, tileStatus, pathfindingMode);
        }

        public List<ICombatUnit> GetUnitsInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation)
        {
            ITile tile = transform.GetComponentInParent<ITile>();
            if (tile == null)
            {
                throw new System.Exception("A TilePiece GameObject must have a parent Tile GameObject for AOE methods to work!");
			}
            return tile.GetUnitsInAOE(source, radius, selfInclusive, relativeAffiliation);
		}

		public AttackVisualInfo GetAttackVisualInfo()
		{
			return _attackVisualInfo;
		}

		public DefendVisualInfo GetDefendVisualInfo()
		{
			return _defendVisualInfo;
		}

		public DeathVisualInfo GetDeathVisualInfo()
		{
			return _deathVisualInfo;
		}

		/// <summary>
		/// Initializes the CombatUnit of this for a combat encounter.
		/// </summary>
		/// <param name="turnManager">the ITurnManager of the current combat encounter</param>
		public void InitializeCombatUnit(ITurnManager turnManager)
		{
            ICombatUnit combatUnit = new CombatUnit(this);
			List<ActivatedAbilitySO> activatedAbilitySOs = new List<ActivatedAbilitySO>();
			activatedAbilitySOs.AddRange(_mechSO.activatedAbilitySOs);
			activatedAbilitySOs.AddRange(_pilotSO.activatedAbilitySOs);
			List<PassiveAbilitySO> passiveAbilitySOs = new List<PassiveAbilitySO>();
			passiveAbilitySOs.AddRange(_mechSO.passiveAbilitySOs);
			passiveAbilitySOs.AddRange(_pilotSO.passiveAbilitySOs);

            foreach (ActivatedAbilitySO activatedAbilitySO in activatedAbilitySOs)
			{
                ActivatedAbility activatedAbility = activatedAbilitySO.Build(turnManager);
                combatUnit.AddActivatedAbility(activatedAbility);
            }
            foreach (PassiveAbilitySO passiveAbilitySO in passiveAbilitySOs)
            {
                PassiveAbility passiveAbility = passiveAbilitySO.Build(turnManager);
                combatUnit.AddAssignedPassive(passiveAbility, combatUnit, false);
            }

            SetCombatUnit(combatUnit);
            turnManager.AddCombatUnit(combatUnit);
        }

		/// <summary>
		/// Deletes the CombatUnit of this and restores default SO values.
		/// </summary>
		public void DeleteCombatUnit()
		{
			MechSO mechSOInstance = combatUnit.GetMechSO();
			PilotSO pilotSOInstance = combatUnit.GetPilotSO();

			if (_mechSO.unique)
			{
				_mechSO.currHealth = mechSOInstance.currHealth;
			}
			if (_pilotSO.unique)
			{
                _pilotSO.currHealth = pilotSOInstance.currHealth;
				_pilotSO.leadership = pilotSOInstance.leadership;
            }

			SetCombatUnit(null);
		}

		public void MoveOnPath(List<ITile> path, Action actionOnComplete)
		{
			StartCoroutine(MoveOnPathInRealTime(path, actionOnComplete));
		}

		private IEnumerator MoveOnPathInRealTime(List<ITile> path, Action actionOnComplete)
		{
			if (GetVisualHandler() != null)
			{
				GetVisualHandler().SetAnimatorBool("Moving", true);
			}
            
			while (path.Count > 0)
			{
                ITile nextTile = path[0];
				path.RemoveAt(0);
                yield return StartCoroutine(MoveToTileInRealTime(nextTile));
            }

			if (GetVisualHandler() != null)
			{
				GetVisualHandler().SetAnimatorBool("Moving", false);
			}

			combatUnit.SetBusyStatus(false);

			actionOnComplete.Invoke();
        }

		private IEnumerator MoveToTileInRealTime(ITile tile)
		{
			ITile startTile = GetComponentInParent<ITile>();
            startTile.OnUnitExit(combatUnit);
			combatUnit.OnTileExited(tile);

            Vector3 startPos = transform.position;
			Vector3 endPos = tile.GetWorldPosition();
			float lerpValue = 0;

			FaceToward(tile.GetBoardPosition());

			while (Vector3.Distance(transform.position, endPos) > 0.2f)
			{
				transform.position = Vector3.Lerp(startPos, endPos, lerpValue);
				lerpValue += _realTimeMoveSpeed;
				yield return new WaitForFixedUpdate();
			}

			tile.OnUnitEnter(combatUnit);
			startTile.MoveTilePieceTo(tile);
			combatUnit.OnTileEntered(tile);
			// Debug.Log("MOVED TO:" + tile.GetBoardPosition());

			if (GetVisualHandler() != null)
			{
				while (GetVisualHandler().IsBusy())
				{
					yield return null;
				}
			}
        }

		public void FaceToward(Vector2 pos)
		{
			Vector3 pos3D = new Vector3(pos.x, 0, pos.y);
			Quaternion rot = Quaternion.LookRotation(pos3D - transform.position);
			transform.rotation = rot;
		}

/*		private void ProcessMechDeath(object sender, EventArgs e)
		{
			if (sender != this) { return; }
			DeactivateParticles();
        }

		private void ProcessPilotDeath(object sender, EventArgs e)
		{
            DeactivateParticles();
        }*/
		public bool IsDead()
		{
			return GetCombatUnit().IsDead();
		}

        public void DeactivateParticles()
		{
			VisualEffect[] visualEffects = this.gameObject.GetComponentsInChildren<VisualEffect>();
			foreach (VisualEffect v in visualEffects)
			{
				v.enabled = false;
			}
		}
	}
}