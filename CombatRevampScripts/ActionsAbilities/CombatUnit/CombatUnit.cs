using System;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.Abilities.ActivatedAbilities;
using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.CombatModifiers;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CustomEventArgs;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Attack;
using CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Defend;
using CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Move;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.CombatVisuals.Handler;
using CombatRevampScripts.CombatVisuals.OneShotAnim;
using CombatRevampScripts.Extensions;
using UnityEngine;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.ActionsAbilities.Structs.Events;
using UnityEngine.Networking.Types;

namespace CombatRevampScripts.ActionsAbilities.CombatUnit
{
    /// <summary>
    /// Represents a participant in a combat encounter, controlled by either a player or AI.
    /// </summary>
    public class CombatUnit : ICombatUnit
    {
        private ITurnManager _turnManager;
        private MechTilePiece _tilePiece;
        private IActionEconomy _actionEconomy;

        private MechSO _mechSO;
        private PilotSO _pilotSO;

        private ICombatVisualHandler _visualHandler;
        private DeathVisualInfo _deathVisualInfo;

        private List<ICombatAction> _actions;
        private List<ICombatPassive> _assignedPassives;
        private List<ICombatPassive> _ownedPassives;
        private Dictionary<string, ActivatedAbility> _activatedAbilities;
        private List<ICombatModifier> _modifiers;
        private List<StatModifier> _statModifiers;
        private List<PropertyModifier> _propertyModifiers;

        private ICombatAction _moveAction;
        private ICombatAction _attackAction;
        private ICombatAction _defendAction;
        private ICombatAction _hailAction;

        private bool isDead = false;
        private float initiative = 0f;

        private bool _isBusy;

        public static event EventHandler OnThisTurnStart;
        public static event EventHandler OnThisTurnEnd;

        public static event EventHandler OnMoveDistance;

        public static event EventHandler OnIncomingMechDamage;
        public static event EventHandler OnIncomingPilotDamage;
        public static event EventHandler OnTakeMechDamage;
        public static event EventHandler OnTakePilotDamage;
        public static event EventHandler OnTakeMechHealing;
        public static event EventHandler OnTakePilotHealing;

        public static event EventHandler OnDamageMech;
        public static event EventHandler OnDamagePilot;
        public static event EventHandler OnHealMech;
        public static event EventHandler OnHealPilot;

        public static event EventHandler OnKillMech;
        public static event EventHandler OnKillPilot;
        public static event EventHandler OnMechDeath;
        public static event EventHandler OnPilotDeath;
        public static event EventHandler OnMechKilled;
        public static event EventHandler OnPilotKilled;

        public static event EventHandler OnEnterTile;
        public static event EventHandler OnExitTile;
        public static event EventHandler OnStartTurnOnTile;
        public static event EventHandler OnEndTurnOnTile;


        public CombatUnit(MechTilePiece tilePiece)
        {
            tilePiece.SetCombatUnit(this);
            _tilePiece = tilePiece;
            _visualHandler = tilePiece.GetVisualHandler();
            Initialize();
        }

        /// <summary>
        /// Initializes default values for this unit
        /// </summary>
        private void Initialize()
        {
            _actionEconomy = new ActionEconomy.ActionEconomy();
            _actions = new List<ICombatAction>();
            _assignedPassives = new List<ICombatPassive>();
            _ownedPassives = new List<ICombatPassive>();
            _activatedAbilities = new Dictionary<string, ActivatedAbility>();
            _modifiers = new List<ICombatModifier>();
            _statModifiers = new List<StatModifier>();
            _propertyModifiers = new List<PropertyModifier>();

            _mechSO = ScriptableObject.Instantiate(_tilePiece.GetMechSO());
            _pilotSO = ScriptableObject.Instantiate(_tilePiece.GetPilotSO());

            _deathVisualInfo = _tilePiece.GetDeathVisualInfo();

            _moveAction = new CombatAction("Move", ActionType.Move, 1, 0, TargetingRangeSource.UseMoveRange, TargetingRules.TargetEmptyTile, false);
            _attackAction = new CombatAction("Attack", ActionType.Attack, 1, 0, TargetingRangeSource.UseAttackRangeFromDamageType, TargetingRules.TargetAnyHostileUnit, true);
            _defendAction = new CombatAction("Defend", ActionType.Defend, 0, 0, TargetingRangeSource.None, TargetingRules.None, false);

            _moveAction.SetClarityGain(5);
            _attackAction.SetClarityGain(20);
            _defendAction.SetClarityGain(10);

            ACombatEffect moveEffect = ScriptableObject.CreateInstance<MoveDefault>();
            moveEffect.Initialize();
            _moveAction.AddEffect(moveEffect);
            ACombatEffect attackEffect = ScriptableObject.CreateInstance<AttackDefault>();
            attackEffect.Initialize();
            _attackAction.AddEffect(attackEffect);
            ACombatEffect defendEffect = ScriptableObject.CreateInstance<DefendDefault>();
            defendEffect.Initialize();
            _defendAction.AddEffect(defendEffect);

            AttackVisualInfo attackVisuals = _tilePiece.GetAttackVisualInfo();

            EffectVisualInfo ballisticAttackVisuals = new EffectVisualInfo(new OneShotAnimInfo("BallisticAttack", attackVisuals.ballisticAttackBusyPeriod), 
                new OneShotAnimInfo(), attackVisuals.ballisticAttackPreVFX, attackVisuals.ballisticAttackPostVFX);
            EffectVisualInfo laserAttackVisuals = new EffectVisualInfo(new OneShotAnimInfo("LaserAttack", attackVisuals.laserAttackBusyPeriod), 
                new OneShotAnimInfo(), attackVisuals.laserAttackPreVFX, attackVisuals.laserAttackPostVFX);

            _attackAction.SetDamageTypeVisualVariant(DamageType.BallisticDamage, ballisticAttackVisuals);
            _attackAction.SetDamageTypeVisualVariant(DamageType.LaserDamage, laserAttackVisuals);

            #region DEFEND ACTION INITIALIZATION
            // TODO: SIMPLIFY THIS LATER
            DefendVisualInfo defendVisuals = _tilePiece.GetDefendVisualInfo();

            PassiveVisualInfo defendPassiveVisuals = new PassiveVisualInfo();
            defendPassiveVisuals.animBoolName = "Defending";
            defendPassiveVisuals.ongoingVFX = defendVisuals.ongoingVFX;
            defendPassiveVisuals.releaseVFX = defendVisuals.releaseVFX;

            CombatPassiveStruct defendPassive = new CombatPassiveStruct();
            defendPassive.passiveName = "Defending";
            defendPassive.passiveDescription = "This Mech is currently Defending. Incoming damage will be reduced.";
            defendPassive.duration = 1;
            defendPassive.turnTimerBehavior = TurnTimerBehavior.StartOfTurn;
            defendPassive.passiveVisualInfo = defendPassiveVisuals;
            defendPassive.effectTriggers = new List<EffectTriggerStruct>();

            EffectTriggerStruct mechDefend = new EffectTriggerStruct();
            mechDefend.eventTriggersFromThisUnit = new List<CombatUnitEventType>
            {
                CombatUnitEventType.OnIncomingMechDamage
            };
            mechDefend.effects = new List<ACombatEffect> { ScriptableObject.CreateInstance<DefendMechDamageEffect>() };
            defendPassive.effectTriggers.Add(mechDefend);

            EffectTriggerStruct pilotDefend = new EffectTriggerStruct();
            pilotDefend.eventTriggersFromThisUnit = new List<CombatUnitEventType>
            {
                CombatUnitEventType.OnIncomingPilotDamage
            };
            pilotDefend.effects = new List<ACombatEffect> { ScriptableObject.CreateInstance<DefendPilotDamageEffect>() };
            defendPassive.effectTriggers.Add(pilotDefend);

            _defendAction.AddSavedPassive(defendPassive);
            #endregion

            string moveDescription = "Move a distance equal to this Mech's Move Range stat!";
            string attackDescription = "Damage an enemy's Mech Integrity or Pilot Health with a Laser or Ballistic Attack!";
            string defendDescription = "Until this Mechs next turn, reduce incoming Ballistic or Laser damage. The amount of damage nullified is calculated using this Mechs Defense and Speed stat.";
            string hailDescription = "Reach out to an enemy over your coms. Convince them to defect and join your team!";

            _moveAction.SetDescription(moveDescription);
            _attackAction.SetDescription(attackDescription);
            _defendAction.SetDescription(defendDescription);

            AddAction(_moveAction);
            AddAction(_attackAction);
            AddAction(_defendAction);

            if (_pilotSO.leadership > 0)
            {
                _hailAction = new CombatAction("Hail", ActionType.Hail, 1, 30, TargetingRangeSource.None, TargetingRules.TargetAnyHostileUnit, false);
                _hailAction.SetDescription(hailDescription);
                ACombatEffect hailEffect = ScriptableObject.CreateInstance<HailDefault>();
                hailEffect.Initialize();
                _hailAction.AddEffect(hailEffect);
                AddAction(_hailAction);
            }

            _isBusy = false;
        }

        public MechTilePiece GetTilePiece()
        {
            return _tilePiece;
        }

        public MechSO GetMechSO()
        {
            return _mechSO;
        }

        public PilotSO GetPilotSO()
        {
            return _pilotSO;
        }

        public void SetTurnManager(ITurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        public ITurnManager GetTurnManager()
        {
            return _turnManager;
        }

        public IActionEconomy GetActionEconomy()
        {
            return _actionEconomy;
        }

        public ICombatVisualHandler GetVisualHandler()
        {
            return _visualHandler;
        }

        public void SetBusyStatus(bool value)
        {
            _isBusy = value;
        }

        public bool IsBusy()
        {
            if (_visualHandler != null && _visualHandler.IsBusy())
            {
                return true;
            }

            return _isBusy;
        }

        public List<ICombatAction> GetActions()
        {
            return _actions;
        }

        public List<ICombatAction> GetLegalActions()
        {
            List<ICombatAction> legalActions = new List<ICombatAction>();

            foreach (ICombatAction action in _actions)
            {
                if (CanDoAction(action))
                {
                    legalActions.Add(action);
                }
            }

            return legalActions;
        }

        public ICombatAction GetMoveAction()
        {
            return _moveAction;
        }

        public ICombatAction GetAttackAction()
        {
            return _attackAction;
        }

        public ICombatAction GetDefendAction()
        {
            return _defendAction;
        }

        public bool IsActionEconomyEmpty()
        {
            return _actionEconomy.IsEmpty();
        }

        public void AddAction(ICombatAction action)
        {
            if (_actions.Contains(action))
            {
                return;
            }
            action.SetCombatUnit(this);
            _actions.Add(action);
        }

        public void AddAssignedPassive(ICombatPassive passive, ICombatUnit owner, bool allowStacking)
        {
            //Debug.Log("Adding passive: " + passive.GetName());
            if (!allowStacking)
            {
                for (int i = _assignedPassives.Count - 1; i >= 0; i--)
                {
                    ICombatPassive existingPassive = _assignedPassives[i];
                    if (passive.GetName() == existingPassive.GetName())
                    {
                        RemoveAssignedPassive(existingPassive);
                    }
                }
            }

            passive.SetAssignee(this);
            passive.SetOwner(owner);
            
            foreach (PropertyModifier modifier in _propertyModifiers)
            {
                modifier.ModifyPassive(passive);
            }

            _assignedPassives.Add(passive);

            owner.AddOwnedPassive(passive);
        }

        public void AddOwnedPassive(ICombatPassive passive)
        {
            if (_ownedPassives.Contains(passive))
            {
                _assignedPassives.Remove(passive);
            }

            _ownedPassives.Add(passive);
        }

        public void RemoveAssignedPassive(ICombatPassive passive)
        {
            if (_assignedPassives.Contains(passive))
            {
                Debug.Log("Removing passive: " + passive.GetName());
                _assignedPassives.Remove(passive);
                Debug.Log("Remaining passives: " + _assignedPassives.Count);
                passive.GetOwner().RemoveOwnedPassive(passive);
            }
        }

        public void RemoveOwnedPassive(ICombatPassive passive)
        {
            if (_ownedPassives.Contains(passive))
            {
                _ownedPassives.Remove(passive);
            }
        }

        public void AddModifier(ICombatModifier modifier)
        {
            _modifiers.Add(modifier);

            if (modifier.GetType() == typeof(StatModifier))
            {
                _statModifiers.Add((StatModifier) modifier);
            }

            if (modifier.GetType() == typeof(PropertyModifier))
            {
                _propertyModifiers.Add((PropertyModifier)modifier);
            }

            modifier.SetupUnit(this);
        }

        public void RemoveModifier(ICombatModifier modifier)
        {
            _modifiers.Remove(modifier);

            if (modifier.GetType() == typeof(StatModifier))
            {
                _statModifiers.Remove((StatModifier)modifier);
            }

            if (modifier.GetType() == typeof(PropertyModifier))
            {
                _propertyModifiers.Remove((PropertyModifier)modifier);
            }
        }

        public void AddActivatedAbility(ActivatedAbility activatedAbility)
        {
            string abilityName = activatedAbility.GetName();
            if (_activatedAbilities.ContainsKey(abilityName))
            {
                return;
            }
            _activatedAbilities.Add(abilityName, activatedAbility);
            AddAction(activatedAbility);
        }

        public void ModifyOwnedEffects<T, U>(IEffectPropertyVisitor<T, U> visitor)
        {
            foreach (ICombatAction action in _actions)
            {
                action.ModifyEffects(visitor);
            }

            foreach (ICombatPassive passive in _ownedPassives)
            {
                passive.ModifyEffectTriggers(visitor);
            }
        }

        public void ModifyAllEffects<T, U>(IEffectPropertyVisitor<T, U> visitor)
        {
            foreach (ICombatAction action in _actions)
            {
                action.ModifyEffects(visitor);
            }

            List<ICombatPassive> allPassives = _ownedPassives;

            foreach (ICombatPassive passive in _assignedPassives)
            {
                if (!_ownedPassives.Contains(passive))
                {
                    allPassives.Add(passive);
                }
            }

            foreach (ICombatPassive passive in allPassives)
            {
                passive.ModifyEffectTriggers(visitor);
            }
        }

        public void OverrideMoveEffects(List<ICombatEffect> effects)
        {
            _moveAction.OverrideEffects(effects);
        }

        public void OverrideAttackEffects(List<ICombatEffect> effects)
        {
            _attackAction.OverrideEffects(effects);
        }

        public void OverrideDefendEffects(List<ICombatEffect> effects)
        {
            _defendAction.OverrideEffects(effects);
        }

        public bool CanDoAction(string actionName)
        {
            switch (actionName)
            {
                case "Move":
                    return CanDoAction(_moveAction);
                case "Attack":
                    return CanDoAction(_attackAction);
                case "Defend":
                    return CanDoAction(_defendAction);
                case "Hail":
                    return CanDoAction(_hailAction);
            }

            ICombatAction activatedAbility = _activatedAbilities[actionName];
            if (activatedAbility != null)
            {
                return CanDoAction(activatedAbility);
            }

            return false;
        }

        public bool CanDoAction(ICombatAction action)
        {
            return _actionEconomy.CanDoAction(action) && action.IsClarityCostMet();
        }

        public void DoAction(string actionName)
        {
            switch (actionName)
            {
                case "Move":
                    DoAction(_moveAction);
                    return;
                case "Attack":
                    DoAction(_attackAction);
                    return;
                case "Defend":
                    DoAction(_defendAction);
                    return;
                case "Hail":
                    DoAction(_hailAction);
                    return;
            }

            ICombatAction activatedAbility = _activatedAbilities[actionName];
            if (activatedAbility != null)
            {
                DoAction(activatedAbility);
            }
        }

        public void DoAction(ICombatAction action)
        {
            Action actionEconomyChange = () =>
            {
                _actionEconomy.DoAction(action);
            };
            
            action.DoEffects(actionEconomyChange);
        }

        public void DoAction(ICombatAction action, Action callbackAction)
        {
            Action callbackPlusActionEconomyEvent = () =>
            {
                _actionEconomy.DoAction(action);
                callbackAction.Invoke();
            };
            
            action.DoEffects(callbackPlusActionEconomyEvent);
        }

        public void AddTurnAction(IActionEconomyToken turnAction)
        {
            _actionEconomy.AddActionEconomyToken(turnAction);
        }

        public void OnTurnStart()
        {
            UpdateActions();
            _actionEconomy.Reset();
            OnThisTurnStart?.Invoke(this, EventArgs.Empty);
            _turnManager.OnUnitStartOfTurn(this);

            ITile thisTile = _tilePiece.GetComponentInParent<ITile>();
            if (thisTile == null)
            {
                throw new Exception("The Tile Piece GO of this unit must be a child object of valid Tile GO to perform OnTurnStart functionality!");
            }
            thisTile.OnUnitStartTurnOn(this);
            OnStartTurnOnTile?.Invoke(this, new TileArgs(thisTile));
            _turnManager.OnUnitTurnStartOnTile(this, thisTile);

            for (int i = _assignedPassives.Count - 1; i >= 0; i--)
            {
                ICombatPassive passive = _assignedPassives[i];
                passive.OnTurnStart();
            }

            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                ICombatModifier modifier = _modifiers[i];
                modifier.OnTurnStart();
            }
        }

        public void OnTurnEnd()
        {
            _actionEconomy.Clear();
            OnThisTurnEnd?.Invoke(this, EventArgs.Empty);
            _turnManager.OnUnitEndOfTurn(this);

            ITile thisTile = _tilePiece.GetComponentInParent<ITile>();
            if (thisTile == null)
            {
                throw new Exception("The Tile Piece GO of this unit must be a child object of valid Tile GO to perform OnTurnEnd functionality!");
            }
            thisTile.OnUnitEndTurnOn(this);
            OnEndTurnOnTile?.Invoke(this, new TileArgs(thisTile));
            _turnManager.OnUnitTurnEndOnTile(this, thisTile);

            for (int i = _assignedPassives.Count - 1; i >= 0; i--)
            {
                ICombatPassive passive = _assignedPassives[i];
                passive.OnTurnEnd();
            }

            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                ICombatModifier modifier = _modifiers[i];
                modifier.OnTurnEnd();
            }
        }

        public void MoveTo(ITile targetTile, int range)
        {
            Action emptyAction = () => { };
            MoveTo(targetTile, range, emptyAction);
        }

        public void MoveTo(ITile targetTile, int range, Action actionOnComplete)
        {
            ITile thisTile = _tilePiece.GetComponentInParent<ITile>();
            if (thisTile == null)
            {
                throw new Exception("The Tile Piece GO of this unit must be a child object of valid Tile GO to perform MoveTo functionality!");
            }
            if (targetTile.GetTilePiece() != null)
            {
                throw new Exception("The target tile of the MoveTo method must be unoccupied!");
            }

            List<ITile> path = PathfindingExtension.GetBestPath(this, thisTile, targetTile, range, PathfindingMode.Movement);
            int distance = path.Count;
            Action afterMoveComplete = () =>
            {
                OnMoveDistance?.Invoke(this, new IntArgs(distance));
                _turnManager.OnUnitMove(this, distance);
                actionOnComplete.Invoke();
            };

            _tilePiece.MoveOnPath(path, afterMoveComplete);
        }
        public void AddClarity(float value)
        {
            float currClarity = _mechSO.GetFloatStatValue("clarity");
            float newClarity = currClarity + value;
            if (newClarity > 100)
            {
                _mechSO.clarity = 100;
            }
            else
            {
                _mechSO.clarity = newClarity;
            }
        }

        public float DamageMech(float damage, ICombatUnit source)
        {
            // Reject non-positive values
            if (damage <= 0) { return 0; }

            // Get initial value for comparison later on
            float initialHealth = _mechSO.currHealth;
            float initialTempHealth = _mechSO.tempHealth;
            float initialHealthCombined = _mechSO.currHealth + _mechSO.tempHealth;
            //Debug.Log("* Damaging Mech:");
            //Debug.Log("* Initial health is " + _mechSO.currHealth + " and temp health is " + _mechSO.tempHealth);
            //Debug.Log("* Incoming damage: " + damage);

            // Invoke the event for incoming damage BEFORE damage actually gets processed
            OnIncomingMechDamage?.Invoke(this, new UnitFloatArgs(source, damage));
            _turnManager.OnUnitIncomingMechDamageFromUnit(this, source, damage);
            //Debug.Log("* After processing events, health is now " + _mechSO.currHealth + " and temp health is now " + _mechSO.tempHealth);

            // Apply damage in a crude way, allowing for negative values (these negatives will be addressed later in this method)
            if (_mechSO.tempHealth > 0)
            {
                _mechSO.tempHealth -= damage;
            }
            else
            {
                _mechSO.currHealth -= damage;
            }

            // Get the end value for comparison
            float endHealthCombined = _mechSO.currHealth + _mechSO.tempHealth;
            //Debug.Log("* After taking damage, health is now " + _mechSO.currHealth + " and temp health is now " + _mechSO.tempHealth);

            // Check that damage has actually been received by comparing initial values to end values
            if (endHealthCombined < initialHealthCombined)
            {
                float defense = _mechSO.defense / 100 * damage;
                //Debug.Log("* Defense stat is " + defense + "%. Based on initial damage " + damage + ", this reduction should equal " + defense);

                // Check that the damage is greater than the defense stat, otherwise we should just treat things
                // as if no damage has been taken, resetting health values to their initial values
                if (damage > defense)
                {
                    // Add back health to represent damage blocked by the defense stat
                    AddMechHealth(defense, true);
                    //Debug.Log("* Adding back health from the defense stat reduction: " + defense);
                    //Debug.Log("* Health is now " + _mechSO.currHealth + " and temp health is now " + _mechSO.tempHealth);

                    // Fix negative temp health values by rolling over the damage onto regular health
                    if (_mechSO.tempHealth < 0)
                    {
                        _mechSO.currHealth += _mechSO.tempHealth;
                        _mechSO.tempHealth = 0;
                        //Debug.Log("* After fixing negative temp health values, health is now " + _mechSO.currHealth);
                    }

                    endHealthCombined = _mechSO.currHealth + _mechSO.tempHealth;

                    // Return value + value to send to damage events
                    float adjustedDamage = initialHealthCombined - endHealthCombined;

                    // Invoke the actual damage events, this time using our adjusted damage value that includes the defense stat
                    source.OnDoMechDamage(this, adjustedDamage);
                    OnTakeMechDamage?.Invoke(this, new UnitFloatArgs(source, adjustedDamage));
                    _turnManager.OnUnitTakeMechDamageFromUnit(this, source, adjustedDamage);
                    //Debug.Log("* Sending the reduced damage value to events: " + adjustedDamage);

                    // Address negative values for current health (a.k.a. death).
                    if (_mechSO.currHealth <= 0)
                    {
                        KillMech(source);
                    }
                    // Play the "TakeDamage" animation if the damage taken wasn't lethal (lethal damage has a unique animation)
                    else
                    {
                        if (_visualHandler != null)
                        {
                            _visualHandler.PlayAnim("TakeDamage", 1);
                        }
                    }

                    return adjustedDamage;
                }
                // Reset health values to initial values if damage is not greater than defense (no damage has been taken)
                else
                {
                    _mechSO.currHealth = initialHealth;
                    _mechSO.tempHealth = initialTempHealth;
                }
            }

            return 0;
        }

        public float DamagePilot(float damage, ICombatUnit source)
        {
            // Reject non-positive values
            if (damage <= 0) { return 0; }

            // Get initial value for comparison later on
            float initialHealth = _pilotSO.currHealth;
            float initialTempHealth = _pilotSO.tempHealth;
            float initialHealthCombined = _pilotSO.currHealth + _pilotSO.tempHealth;
            //Debug.Log("* Damaging Pilot:");
            //Debug.Log("* Initial health is " + _pilotSO.currHealth + " and temp health is " + _pilotSO.tempHealth);
            //Debug.Log("* Incoming damage: " + damage);

            // Invoke the event for incoming damage BEFORE damage actually gets processed
            OnIncomingPilotDamage?.Invoke(this, new UnitFloatArgs(source, damage));
            _turnManager.OnUnitIncomingPilotDamageFromUnit(this, source, damage);
            //Debug.Log("* After processing events, health is now " + _pilotSO.currHealth + " and temp health is now " + _pilotSO.tempHealth);

            // Apply damage in a crude way, allowing for negative values (these negatives will be addressed later in this method)
            if (_pilotSO.tempHealth > 0)
            {
                _pilotSO.tempHealth -= damage;
            }
            else
            {
                _pilotSO.currHealth -= damage;
            }

            // Get the end value for comparison
            float endHealthCombined = _pilotSO.currHealth + _pilotSO.tempHealth;
            //Debug.Log("* After taking damage, health is now " + _pilotSO.currHealth + " and temp health is now " + _pilotSO.tempHealth);

            // Check that damage has actually been received by comparing initial values to end values
            if (endHealthCombined < initialHealthCombined)
            {
                float defense = _mechSO.defense/100 * damage;
                //Debug.Log("* Defense stat is " + defense + "%. Based on initial damage " + damage + ", this reduction should equal " + defense);

                // Check that the damage is greater than the defense stat, otherwise we should just treat things
                // as if no damage has been taken, resetting health values to their initial values
                if (damage > defense)
                {
                    // Add back health to represent damage blocked by the defense stat
                    AddPilotHealth(defense, true);
                    //Debug.Log("* Adding back health from the defense stat reduction: " + defense);
                    //Debug.Log("* Health is now " + _pilotSO.currHealth + " and temp health is now " + _pilotSO.tempHealth);

                    // Fix negative temp health values by rolling over the damage onto regular health
                    if (_pilotSO.tempHealth < 0)
                    {
                        _pilotSO.currHealth += _pilotSO.tempHealth;
                        _pilotSO.tempHealth = 0;
                        //Debug.Log("* After fixing negative temp health values, health is now " + _pilotSO.currHealth);
                    }

                    endHealthCombined = _pilotSO.currHealth + _pilotSO.tempHealth;

                    // Return value + value to send to damage events
                    float adjustedDamage = initialHealthCombined - endHealthCombined;

                    // Invoke the actual damage events, this time using our adjusted damage value that includes the defense stat
                    source.OnDoPilotDamage(this, adjustedDamage);
                    OnTakePilotDamage?.Invoke(this, new UnitFloatArgs(source, adjustedDamage));
                    _turnManager.OnUnitTakePilotDamageFromUnit(this, source, adjustedDamage);
                    //Debug.Log("* Sending the reduced damage value to events: " + adjustedDamage);

                    // Address negative values for current health (a.k.a. death).
                    if (_pilotSO.currHealth <= 0)
                    {
                        KillPilot(source);
                    }
                    // Play the "TakeDamage" animation if the damage taken wasn't lethal (lethal damage has a unique animation)
                    else
                    {
                        if (_visualHandler != null)
                        {
                            _visualHandler.PlayAnim("TakeDamage", 1);
                        }
                    }

                    return adjustedDamage;
                }
                // Reset health values to initial values if damage is not greater than defense (no damage has been taken)
                else
                {
                    _pilotSO.currHealth = initialHealth;
                    _pilotSO.tempHealth = initialTempHealth;
                }
            }

            return 0;
        }

        public float HealMech(float healing, ICombatUnit source)
        {
            if (healing < 0) { return 0; }
            _mechSO.currHealth += healing;
            source.OnDoMechHealing(this, healing);
            OnTakeMechHealing?.Invoke(this, new UnitFloatArgs(source, healing));
            _turnManager.OnUnitTakeMechHealingFromUnit(this, source, healing);

            if (_mechSO.currHealth > _mechSO.maxHealth)
            {
                _mechSO.currHealth = _mechSO.maxHealth;
            }

            return healing;
        }

        public float HealPilot(float healing, ICombatUnit source)
        {
            if (healing < 0) { return 0; }
            _pilotSO.currHealth += healing;
            source.OnDoPilotHealing(this, healing);
            OnTakePilotHealing?.Invoke(this, new UnitFloatArgs(source, healing));
            _turnManager.OnUnitTakePilotHealingFromUnit(this, source, healing);

            if (_pilotSO.currHealth > _pilotSO.maxHealth)
            {
                _pilotSO.currHealth = _pilotSO.maxHealth;
            }

            return healing;
        }

        public void AddMechHealth(float value, bool affectTempHealth)
        {
            if (affectTempHealth && _mechSO.currHealth == _mechSO.maxHealth)
            {
                _mechSO.tempHealth += value;

                // Fix negative temp health values by rolling over the damage onto regular health
                if (_mechSO.tempHealth < 0)
                {
                    _mechSO.currHealth += _mechSO.tempHealth;
                    _mechSO.tempHealth = 0;
                }
            }
            else
            {
                _mechSO.currHealth += value;
            }

            float overheal = _mechSO.currHealth - _mechSO.maxHealth;

            // Prevent currHealth from exceeding maxHealth
            if (overheal > 0)
            {
                // Allow overhealing into temp health
                if (affectTempHealth)
                {
                    _mechSO.tempHealth = overheal;
                }

                _mechSO.currHealth = _mechSO.maxHealth;
            }

            // Address negative values for current health (a.k.a. death).
            if (_mechSO.currHealth <= 0)
            {
                KillMech();
            }
        }

        public void AddPilotHealth(float value, bool affectTempHealth)
        {
            if (affectTempHealth && _pilotSO.currHealth == _pilotSO.maxHealth)
            {
                _pilotSO.tempHealth += value;

                // Fix negative temp health values by rolling over the damage onto regular health
                if (_pilotSO.tempHealth < 0)
                {
                    _pilotSO.currHealth += _pilotSO.tempHealth;
                    _pilotSO.tempHealth = 0;
                }
            }
            else
            {
                _pilotSO.currHealth += value;
            }

            float overheal = _pilotSO.currHealth - _pilotSO.maxHealth;

            // Prevent currHealth from exceeding maxHealth
            if (overheal > 0)
            {
                // Allow overhealing into temp health
                if (affectTempHealth)
                {
                    _pilotSO.tempHealth = overheal;
                }

                _pilotSO.currHealth = _pilotSO.maxHealth;
            }

            // Address negative values for current health (a.k.a. death).
            if (_pilotSO.currHealth <= 0)
            {
                KillPilot();
            }
        }

        public void AddMechTempHealth(float value)
        {
            if (value < 0)
            {
                return;
            }
            _mechSO.tempHealth += value;
        }

        public void AddPilotTempHealth(float value)
        {
            if (value < 0)
            {
                return;
            }
            _pilotSO.tempHealth += value;
        }

        public void SetMechTempHealth (float value)
        {
            if (value < 0)
            {
                return;
            }
            _mechSO.tempHealth = value;
        }

        public void SetPilotTempHealth(float value)
        {
            if (value < 0)
            {
                return;
            }
            _pilotSO.tempHealth = value;
        }

        public void KillMech(ICombatUnit source)
        {
            source.OnKillUnitMech(this);
            _turnManager.OnUnitMechKilledByUnit(this, source);
            KillMech();
            Debug.Log($"KillMech(ICombatUnit {source}) this = {this} ");
        }

        public void KillPilot(ICombatUnit source)
        {
            source.OnKillUnitPilot(this);
            _turnManager.OnUnitPilotKilledByUnit(this, source);
            KillPilot();
            Debug.Log($"KillPilot(ICombatUnit {source}) this = {this} ");
        }

        public void KillMech()
        {
            if (_visualHandler != null)
            {
                _visualHandler.SetAnimatorBool("MechDead", true);
            }
            _mechSO.currHealth = 0;
            _mechSO.tempHealth = 0;
            OnMechDeath?.Invoke(this, EventArgs.Empty);
            _turnManager.OnUnitMechDie(this);
            _tilePiece.DeactivateParticles();
            _visualHandler.PlayVFXObject(_deathVisualInfo.mechDeathGO, this);
            isDead = true;
            Debug.Log($"KillMech() this = {this} ");
        }

        public void KillPilot()
        {
            if (_visualHandler != null)
            {
                _visualHandler.SetAnimatorBool("PilotDead", true);
            }
            _pilotSO.currHealth = 0;
            _pilotSO.tempHealth = 0;
            OnPilotDeath?.Invoke(this, EventArgs.Empty);
            _turnManager.OnUnitPilotDie(this);
            _tilePiece.DeactivateParticles();
            _visualHandler.PlayVFXObject(_deathVisualInfo.pilotDeathGO, this);
            isDead = true;
            Debug.Log($"KillPilot() this = {this} ");
        }

        public void OnDoMechDamage(ICombatUnit target, float value)
        {
            OnDamageMech?.Invoke(this, new UnitFloatArgs(target, value));
            _turnManager.OnUnitDoMechDamage(this, target, value);
        }

        public void OnDoPilotDamage(ICombatUnit target, float value)
        {
            OnDamagePilot?.Invoke(this, new UnitFloatArgs(target, value));
            _turnManager.OnUnitDoPilotDamage(this, target, value);
        }

        public void OnDoMechHealing(ICombatUnit target, float value)
        {
            OnHealMech?.Invoke(this, new UnitFloatArgs(target, value));
            _turnManager.OnUnitDoMechHealing(this, target, value);
        }

        public void OnDoPilotHealing(ICombatUnit target, float value)
        {
            OnHealPilot?.Invoke(this, new UnitFloatArgs(target, value));
            _turnManager.OnUnitDoPilotHealing(this, target, value);
        }

        public void OnKillUnitMech(ICombatUnit unit)
        {
            OnKillMech?.Invoke(this, new UnitArgs(unit));
            _turnManager.OnUnitKillUnitMech(this, unit);
        }

        public void OnKillUnitPilot(ICombatUnit unit)
        {
            OnKillPilot?.Invoke(this, new UnitArgs(unit));
            _turnManager.OnUnitKillUnitPilot(this, unit);
        }

        public void OnTileEntered(ITile tile)
        {
            OnEnterTile?.Invoke(this, new TileArgs(tile));
            _turnManager.OnTileEnteredByUnit(this, tile);
        }

        public void OnTileExited(ITile tile)
        {
            OnExitTile?.Invoke(this, new TileArgs(tile));
            _turnManager.OnTileExitedByUnit(this, tile);
        }

        /// <summary>
        /// Updates important information for all actions of this unit.
        /// </summary>
        private void UpdateActions()
        {
            foreach (ICombatAction action in _actions)
            {
                action.UpdateInfo();
            }
        }

        public List<ITile> GetTilesInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation, TileStatus tileStatus,
            PathfindingMode pathfindingMode)
        {
            return _tilePiece.GetTilesInAOE(source, radius, selfInclusive, relativeAffiliation, tileStatus, pathfindingMode);
        }

        public List<ICombatUnit> GetUnitsInAOE(ICombatUnit source, int radius, bool selfInclusive, RelativeAffiliation relativeAffiliation)
        {
            return _tilePiece.GetUnitsInAOE(source, radius, selfInclusive, relativeAffiliation);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float InitializeInitiative()
        {
            initiative = UnityEngine.Random.Range(0f, 100f) + _mechSO.speed;
            return initiative;
        }

        public float GetInitiative()
        {
            return initiative;
        }
    }
}
