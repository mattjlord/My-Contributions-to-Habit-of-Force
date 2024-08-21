using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Effect_Delegate_Structs;
using CombatRevampScripts.Board.Tile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects.Default_Action_Effects.Move
{
    /// <summary>
    /// A variant of the MoveDefault effect with additional effect delegates that can be triggered by DoAdditionalEffect
    /// </summary>
    [CreateAssetMenu(menuName = "Ability Designer/Combat Effects/Move Effect Variant")]
    public class MoveDelegateVariant : MoveDefault
    {
        [SerializeField] public List<UnitEffectDelegateStruct> additionalEffectsOnUnits;
        [SerializeField] public List<TileEffectDelegateStruct> additionalEffectsOnTiles;

        private Dictionary<string, ICombatPassive> _builtPassives;

        public override void Initialize()
        {
            base.Initialize();
            foreach (UnitEffectDelegateStruct effectOnUnit in additionalEffectsOnUnits)
            {
                effectOnUnit.effectDelegate.InitializeEffectProperties(this);
            }
            foreach (TileEffectDelegateStruct effectOnTile in additionalEffectsOnTiles)
            {
                effectOnTile.effectDelegate.InitializeEffectProperties(this);
            }

            _builtPassives = new Dictionary<string, ICombatPassive>();
        }

        public override void DoAdditionalEffect()
        {
            foreach (UnitEffectDelegateStruct effectOnUnit in additionalEffectsOnUnits)
            {
                switch (effectOnUnit.subject)
                {
                    case UnitSubjectType.TargetListOfUnits:
                        ProcessMultiUnitEffectDelegate(effectOnUnit);
                        break;
                    default:
                        ProcessSingleUnitEffectDelegate(effectOnUnit);
                        break;
                }
            }

            foreach (TileEffectDelegateStruct effectOnTile in additionalEffectsOnTiles)
            {
                switch (effectOnTile.subject)
                {
                    case TileSubjectType.TargetListOfTiles:
                        ProcessMultiTileEffectDelegate(effectOnTile);
                        break;
                    default:
                        ProcessSingleTileEffectDelegate(effectOnTile);
                        break;
                }
            }
        }

        private void ProcessMultiUnitEffectDelegate(UnitEffectDelegateStruct unitEffectDelegate)
        {
            List<ICombatUnit> subjects = null;

            switch (unitEffectDelegate.subject)
            {
                case UnitSubjectType.TargetListOfUnits:
                    subjects = targetLoUnits;
                    break;
            }

            if (subjects == null)
            {
                throw new Exception("Unit effect delegate subject type " + unitEffectDelegate.subject.ToString() + " could not be found in this effect!");
            }

            foreach (ICombatUnit subject in subjects)
            {
                unitEffectDelegate.effectDelegate.TryInvoke(subject, this);
            }
        }

        private void ProcessSingleUnitEffectDelegate(UnitEffectDelegateStruct unitEffectDelegate)
        {
            ICombatUnit subject = null;

            switch (unitEffectDelegate.subject)
            {
                case UnitSubjectType.AssignedUnit:
                    subject = assignedUnit;
                    break;
                case UnitSubjectType.OwnerUnit:
                    subject = ownerUnit;
                    break;
                case UnitSubjectType.TargetUnit:
                    subject = targetUnit;
                    break;
                case UnitSubjectType.EventArgUnit:
                    subject = eventUnit;
                    break;
                case UnitSubjectType.EventSenderUnit:
                    subject = senderUnit;
                    break;
            }

            if (subject == null)
            {
                throw new Exception("Unit effect delegate subject type " + unitEffectDelegate.subject.ToString() + " could not be found in this effect!");
            }

            unitEffectDelegate.effectDelegate.TryInvoke(subject, this);
        }

        private void ProcessMultiTileEffectDelegate(TileEffectDelegateStruct tileEffectDelegate)
        {
            List<ITile> subjects = null;

            switch (tileEffectDelegate.subject)
            {
                case TileSubjectType.TargetListOfTiles:
                    subjects = targetLoTiles;
                    break;
            }

            if (subjects == null)
            {
                throw new Exception("Unit effect delegate subject type " + tileEffectDelegate.subject.ToString() + " could not be found in this effect!");
            }

            foreach (ITile subject in subjects)
            {
                tileEffectDelegate.effectDelegate.TryInvoke(subject, this);
            }
        }

        private void ProcessSingleTileEffectDelegate(TileEffectDelegateStruct tileEffectDelegate)
        {
            ITile subject = null;

            switch (tileEffectDelegate.subject)
            {
                case TileSubjectType.TargetTile:
                    subject = targetTile;
                    break;
                case TileSubjectType.EventArgTile:
                    subject = eventTile;
                    break;
            }

            if (subject == null)
            {
                throw new Exception("Tile effect delegate subject type " + tileEffectDelegate.subject.ToString() + " could not be found in this effect!");
            }

            tileEffectDelegate.effectDelegate.TryInvoke(subject, this);
        }

        public override ICombatPassive GetBuiltPassiveByName(string name)
        {
            if (_builtPassives.ContainsKey(name))
            {
                return _builtPassives[name];
            }

            return null;
        }

        public override void AddBuiltPassive(ICombatPassive passive)
        {
            _builtPassives[passive.GetName()] = passive;
        }
    }
}