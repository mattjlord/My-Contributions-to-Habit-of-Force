using System;
using System.Collections.Generic;
using System.Diagnostics;
using CombatRevampScripts.ActionsAbilities.ActionEconomy;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors.Custom_Visitors;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.Extensions;

namespace CombatRevampScripts.ActionsAbilities.CombatActions
{
    /// <summary>
    /// Represents a type of IEffectHolder that triggers its ICombatEffect from AI or player inputs.
    /// </summary>
    public class CombatAction : AEffectHolder, ICombatAction
    {
        private string _name;
        private string _description;
        private List<ICombatEffect> _defaultEffects;
        private ActionType _actionType;
        private int _numTargets;
        private int _range;
        private TargetingRangeSource _targetingRangeSource;
        private TargetingRules _targetingRules;
        private bool _askForDamageType;
        private bool _allTargetsSelected;

        private float _clarityGain;

        private List<ITile> _selectedTargetTiles;
        private List<ICombatUnit> _selectedTargetUnits;

        private Dictionary<DamageType, EffectVisualInfo> _damageTypeVisualVariants;

        public CombatAction(string name, ActionType actionType, int numTargets, int range, TargetingRangeSource targetingRangeSource, 
            TargetingRules targetingRules, bool askForDamageType) : base()
        {
            if (numTargets < 0)
            {
                throw new ArgumentOutOfRangeException("The value for numTargets must be a positive integer!");
            }
            if (numTargets > 0 && range < 1 && targetingRangeSource == TargetingRangeSource.None)
            {
                throw new ArgumentException("You must assign a non-zero range value if there are one or more targets, unless you set a targetingRangeSource that can override it!");
            }
            if (numTargets < 1 && range > 0)
            {
                throw new ArgumentException("There must be one or more target if you have a non-zero range value!");
            }
            if (range < 0)
            {
                throw new ArgumentOutOfRangeException("The value for range must be a positive integer!");
            }
            if (numTargets > 0 && targetingRules == TargetingRules.None)
            {
                throw new ArgumentException("You must set targetingRules to a value other than None if your Action has one or more targets!");
            }
            if (targetingRangeSource == TargetingRangeSource.UseAttackRangeFromDamageType && !askForDamageType)
            {
                throw new ArgumentException("Cannot set targetingRangeSource to UseAttackRangeFromDamageType if askForDamageType is false!");
            }

            _name = name;
            _actionType = actionType;
            _numTargets = numTargets;
            _range = range;
            _targetingRangeSource = targetingRangeSource;
            _targetingRules = targetingRules;
            _askForDamageType = askForDamageType;
            _allTargetsSelected = false;

            _damageTypeVisualVariants = new Dictionary<DamageType, EffectVisualInfo>();

            ResetSelectedTargets();
        }

        public override void InitializeEffectProperties(ICombatEffect effect)
        {
            if (_askForDamageType)
            {
                effect.AddProperty(new DamageTypeProperty(DamageType.None));
            }

            if (_targetingRules == TargetingRules.TargetAnyTile || _targetingRules == TargetingRules.TargetEmptyTile
                || _targetingRules == TargetingRules.TargetOtherTile || _targetingRules == TargetingRules.TargetOtherOccupiedTile
                || _targetingRules == TargetingRules.TargetAnyOccupiedTile || _targetingRules == TargetingRules.TargetAnyNonFriendlyTile
                || _targetingRules == TargetingRules.TargetAnyNonHostileTile || _targetingRules == TargetingRules.TargetOtherNonHostileTile)
            {
                if (_numTargets == 1)
                {
                    effect.AddProperty(new TargetTileProperty(null));
                }
                if (_numTargets > 1)
                {
                    effect.AddProperty(new TargetLoTileProperty(null));
                }
            }

            if (_targetingRules == TargetingRules.TargetAnyUnit || _targetingRules == TargetingRules.TargetOtherUnit
                || _targetingRules == TargetingRules.TargetAnyFriendlyUnit || _targetingRules == TargetingRules.TargetOtherFriendlyUnit
                || _targetingRules == TargetingRules.TargetAnyHostileUnit)
            {
                if (_numTargets == 1)
                {
                    effect.AddProperty(new TargetUnitProperty(null));
                }
                if (_numTargets > 1)
                {
                    effect.AddProperty(new TargetLoUnitProperty(null));
                }
            }
        }

        /// <summary>
        /// Performs the clarity gain, then the effects
        /// </summary>
        public override void DoEffectsOnly()
        {
            GetOwnerUnit().AddClarity(_clarityGain);
            base.DoEffectsOnly();
        }

        public string GetName()
        {
            return _name;
        } 
        
        public string GetDescription()
        {
            return _description;
        }
        
        public void SetDescription(string description)
        {
            _description = description;
        }

        public ICombatUnit GetOwnerUnit()
        {
            return combatUnit;
        }

        public override void AddEffect(ICombatEffect effect)
        {
            base.AddEffect(effect);
            effect.SetSource(this);
            _defaultEffects = effects;
        }

        public void OverrideEffects(List<ICombatEffect> effects)
        {
            this.effects = effects;
        }

        public virtual bool IsClarityCostMet()
        {
            return true;
        }

        public virtual float GetClarityCost()
        {
            return 0;
        }

        public float GetClarityGain()
        {
            return _clarityGain;
        }

        public void SetClarityGain(float value)
        {
            _clarityGain = value;
        }

        public ActionType GetActionType()
        {
            return _actionType;
        }

        public int GetRange()
        {
            return _range;
        }

        public bool ShouldAskForDamageType()
        {
            return _askForDamageType;
        }

        public void SetDamageType(DamageType damageType)
        {
            ModifyEffects(new SetDamageTypeVisitor(damageType));
            UpdateVisualsToDamageType(damageType);
            if (_targetingRangeSource == TargetingRangeSource.UseAttackRangeFromDamageType)
            {
                MechSO mechSO = combatUnit.GetMechSO();
                switch (damageType)
                {
                    case DamageType.BallisticDamage:
                        _range = mechSO.ballisticAttackRange;
                        return;
                    case DamageType.LaserDamage:
                        _range = mechSO.laserAttackRange;
                        return;
                }
            }
        }

        public void SetDamageTypeVisualVariant(DamageType damageType, EffectVisualInfo variantInfo)
        {
            _damageTypeVisualVariants[damageType] = variantInfo;
        }

        private void UpdateVisualsToDamageType(DamageType damageType)
        {
            if (_damageTypeVisualVariants.ContainsKey(damageType))
            {
                EffectVisualInfo variantInfo = _damageTypeVisualVariants[damageType];
                SetEffectVisualInfo(variantInfo);
            }
        }

        public List<ITile> GetTargetableTiles()
        {
            PathfindingMode pathfindingMode;

            if (_actionType == ActionType.Move)
            {
                pathfindingMode = PathfindingMode.Movement;
            }
            else
            {
                pathfindingMode = PathfindingMode.Standard;
            }

            switch (_targetingRules)
            {
                case TargetingRules.None:
                    return new List<ITile>();
                case TargetingRules.TargetAnyTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, true, RelativeAffiliation.Any, TileStatus.Any, pathfindingMode);
                case TargetingRules.TargetOtherTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Any, TileStatus.Any, pathfindingMode);
                case TargetingRules.TargetEmptyTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Any, TileStatus.Empty, pathfindingMode);
                case TargetingRules.TargetAnyNonHostileTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, true, RelativeAffiliation.Friendly, TileStatus.Any, pathfindingMode);
                case TargetingRules.TargetOtherNonHostileTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Friendly, TileStatus.Any, pathfindingMode);
                case TargetingRules.TargetAnyNonFriendlyTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Hostile, TileStatus.Any, pathfindingMode);
                case TargetingRules.TargetAnyOccupiedTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, true, RelativeAffiliation.Any, TileStatus.Occupied, pathfindingMode);
                case TargetingRules.TargetOtherOccupiedTile:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Any, TileStatus.Occupied, pathfindingMode);
                case TargetingRules.TargetAnyUnit:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, true, RelativeAffiliation.Any, TileStatus.OccupiedByUnit, pathfindingMode);
                case TargetingRules.TargetOtherUnit:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Any, TileStatus.OccupiedByUnit, pathfindingMode);
                case TargetingRules.TargetAnyFriendlyUnit:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, true, RelativeAffiliation.Friendly, TileStatus.OccupiedByUnit, pathfindingMode);
                case TargetingRules.TargetOtherFriendlyUnit:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Friendly, TileStatus.OccupiedByUnit, pathfindingMode);
                case TargetingRules.TargetAnyHostileUnit:
                    return combatUnit.GetTilesInAOE(combatUnit, _range, false, RelativeAffiliation.Hostile, TileStatus.OccupiedByUnit, pathfindingMode);
                default:
                    return null;
            }
        }

        public void SelectTarget(ITile targetTile)
        {
            if (_targetingRules == TargetingRules.None) { return; }

            if (GetName() == "Hail")
            {
                if (targetTile.GetTilePiece() != null)
                {
                    MechTilePiece mech = (MechTilePiece)targetTile.GetTilePiece();
                    if (mech.GetCombatUnit().GetPilotSO().affiliation == Affiliation.enemy)
                    {
                        TargetUnit(mech.GetCombatUnit());
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            List<ITile> targetableTiles = GetTargetableTiles();

            if (targetableTiles.Contains(targetTile))
            {
                switch (_targetingRules)
                {
                    case TargetingRules.TargetAnyTile:
                    case TargetingRules.TargetOtherTile:
                    case TargetingRules.TargetEmptyTile:
                    case TargetingRules.TargetOtherOccupiedTile:
                    case TargetingRules.TargetAnyOccupiedTile:
                    case TargetingRules.TargetAnyNonFriendlyTile:
                    case TargetingRules.TargetAnyNonHostileTile:
                    case TargetingRules.TargetOtherNonHostileTile:
                        TargetTile(targetTile); 
                        return;
                    default:
                        ITilePiece tilePiece = targetTile.GetTilePiece();
                        MechTilePiece mechTilePiece = null;
                        ICombatUnit unit = null;
                        if (tilePiece != null && tilePiece.GetType() == typeof(MechTilePiece))
                        {
                            mechTilePiece = (MechTilePiece)tilePiece;
                        }
                        if (mechTilePiece != null)
                        {
                            unit = mechTilePiece.GetCombatUnit();
                        }
                        if (unit != null)
                        {
                            TargetUnit(unit);
                            return;
                        }
                        throw new Exception("A CombatUnit on the targeted tile could not be found!");
                }
            }
        }

        /// <summary>
        /// If this action has one target, uses a visitor to set this action's TargetTileProperty values to the given tile.
        /// If this action has more than one target, adds the given tile to a list of selected tiles.
        /// If the list of selected tiles exceeds this action's total number of targets, uses a visitor to set this action's
        /// TargetLoTileProperty values to the list of selected tiles and resets the list of selected tiles.
        /// </summary>
        /// <param name="targetTile">the targeted ITile</param>
        private void TargetTile(ITile targetTile)
        {
            if (_numTargets == 1)
            {
                ModifyEffects(new SetTargetTileVisitor(targetTile));
                _allTargetsSelected = true;
                return;
            }
            if (_numTargets > 1)
            {
                AddTargetTileToSelected(targetTile);
                return;
            }
        }

        /// <summary>
        /// If this action has one target, uses a visitor to set this action's TargetUnitProperty values to the given tile.
        /// If this action has more than one target, adds the given unit to a list of selected units.
        /// If the list of selected units exceeds this action's total number of targets, uses a visitor to set this action's
        /// TargetLoUnitProperty values to the list of selected units and resets the list of selected units.
        /// </summary>
        /// <param name="targetUnit">the targeted ICombatUnit</param>
        private void TargetUnit(ICombatUnit targetUnit)
        {
            if (_numTargets == 1)
            {
                ModifyEffects(new SetTargetUnitVisitor(targetUnit));
                _allTargetsSelected = true;
                return;
            }
            if (_numTargets > 1)
            {
                AddTargetUnitToSelected(targetUnit);
                return;
            }
        }

        /// <summary>
        /// Adds the given tile to a list of selected tiles.
        /// If the list of selected tiles exceeds this action's total number of targets, uses a visitor to set this action's
        /// TargetLoTileProperty values to the list of selected tiles and resets the list of selected tiles.
        /// </summary>
        /// <param name="targetTile">the targeted ITile</param>
        private void AddTargetTileToSelected(ITile targetTile)
        {
            _selectedTargetTiles.Add(targetTile);

            if (_selectedTargetTiles.Count >= _numTargets)
            {
                ModifyEffects(new SetTargetLoTileVisitor(_selectedTargetTiles));
                _allTargetsSelected = true;
                ResetSelectedTargets();
            }
        }

        /// <summary>
        /// Adds the given unit to a list of selected units.
        /// If the list of selected units exceeds this action's total number of targets, uses a visitor to set this action's
        /// TargetLoUnitProperty values to the list of selected units and resets the list of selected units.
        /// </summary>
        /// <param name="targetUnit">the selected ICombatUnit</param>
        private void AddTargetUnitToSelected(ICombatUnit targetUnit)
        {
            _selectedTargetUnits.Add(targetUnit);

            if (_selectedTargetUnits.Count >= _numTargets)
            {
                ModifyEffects(new SetTargetLoUnitVisitor(_selectedTargetUnits));
                _allTargetsSelected = true;
                ResetSelectedTargets();
            }
        }

        /// <summary>
        /// Resets both listed of selected targets.
        /// </summary>
        private void ResetSelectedTargets()
        {
            _selectedTargetTiles = new List<ITile>();
            _selectedTargetUnits = new List<ICombatUnit>();
            _allTargetsSelected = false;

            foreach (ICombatEffect effect in effects)
            {
                effect.ModifyProperty(new SetTargetLoTileVisitor(null));
                effect.ModifyProperty(new SetTargetLoUnitVisitor(null));
                effect.ModifyProperty(new SetTargetTileVisitor(null));
                effect.ModifyProperty(new SetTargetUnitVisitor(null));
            }
        }

        public bool AreAllTargetsSelected()
        {
            if (_numTargets == 0)
            {
                // Should always return true if there are zero targets
                return true;
            }
            if (_allTargetsSelected)
            {
                // Resets the value back to false for convenience, but returns true.
                _allTargetsSelected = false;
                return true;
            }
            return _allTargetsSelected;
        }

        public void UpdateInfo()
        {
            UpdateRangeOverride();
            RestoreDefaultEffect();
            ResetSelectedTargets();
        }

        /// <summary>
        /// Resets the default effect for this action if it isn't already the default.
        /// </summary>
        private void RestoreDefaultEffect()
        {
            if (effects != _defaultEffects)
            {
                effects = _defaultEffects;
            }
        }

        /// <summary>
        /// Updates the _range of this Action to match the latest value
        /// represented by _targetingRangeSource.
        /// </summary>
        private void UpdateRangeOverride()
        {
            if (_targetingRangeSource == TargetingRangeSource.None)
            {
                return;
            }
            MechSO mechSO = combatUnit.GetMechSO();
            switch (_targetingRangeSource)
            {
                case TargetingRangeSource.UseLaserAttackRange:
                    _range = mechSO.GetIntStatValue("laserAttackRange");
                    break;
                case TargetingRangeSource.UseBallisticAttackRange:
                    _range = mechSO.GetIntStatValue("ballisticAttackRange");
                    break;
                case TargetingRangeSource.UseMoveRange:
                    _range = mechSO.GetIntStatValue("moveRange");
                    break;
            }
        }
    }
}
