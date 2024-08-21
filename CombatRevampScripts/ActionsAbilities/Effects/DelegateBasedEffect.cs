using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates.Effect_Delegate_Structs;
using System.Collections.Generic;
using System;
using CombatRevampScripts.Board.Tile;
using UnityEngine;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;

namespace CombatRevampScripts.ActionsAbilities.Effects
{
    /// <summary>
    /// Represents a type of combat effect that uses a list of delegate structs to perform
    /// its effect behavior
    /// </summary>
    [CreateAssetMenu (menuName = "Ability Designer/Combat Effects/Standard Delegate-Based Effect")]
    public class DelegateBasedEffect : ACombatEffect
    {
        [SerializeField] public List<UnitEffectDelegateStruct> effectsOnUnits;
        [SerializeField] public List<TileEffectDelegateStruct> effectsOnTiles;

        private Dictionary<string, ICombatPassive> _builtPassives;

        public override void Initialize()
        {
            base.Initialize();

            if (effectsOnUnits == null)
            {
                IActionOrPassive source = GetSource();
                Debug.Log("### " + source.GetName());
            }

            foreach (UnitEffectDelegateStruct effectOnUnit in effectsOnUnits)
            {
                effectOnUnit.effectDelegate.InitializeEffectProperties(this);
            }
            foreach (TileEffectDelegateStruct effectOnTile in effectsOnTiles)
            {
                effectOnTile.effectDelegate.InitializeEffectProperties(this);
            }

            _builtPassives = new Dictionary<string, ICombatPassive>();
        }

        public override void DoDefaultEffect()
        {
            PerformDelegates(effectsOnUnits, effectsOnTiles);
        }

        public void PerformDelegates(List<UnitEffectDelegateStruct> onUnits, List<TileEffectDelegateStruct> onTiles)
        {
            foreach (UnitEffectDelegateStruct effectOnUnit in onUnits)
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

            foreach (TileEffectDelegateStruct effectOnTile in onTiles)
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