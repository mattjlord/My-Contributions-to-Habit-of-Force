using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectTriggers;
using CombatRevampScripts.ActionsAbilities.TurnTimer;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using System;
using System.Collections.Generic;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.ActionsAbilities.Effects;
using UnityEngine;
using CombatRevampScripts.CombatVisuals.VFXController;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.CombatVisuals.Handler;

namespace CombatRevampScripts.ActionsAbilities.CombatPassives
{
    /// <summary>
    /// Represents an object on a CombatUnit that contains EffectTriggers that are triggered by
    /// various events.
    /// </summary>
    public class CombatPassive : ATurnTimer, ICombatPassive
    {
        private string _name;
        private string _description;


        private List<IEffectTrigger> _effectTriggers;
        private List<ICombatEffect> _effectsOnDeactivate;

        private ICombatUnit _assignee;
        private ICombatUnit _owner;

        private string _animBoolName;
        private GameObject _ongoingVFX;
        private IVFXController _ongoingVFXController;
        private GameObject _releaseVFX;

        private int _charges;

        public CombatPassive(string name) : base(0, TurnTimerBehavior.EndOfTurn)
        {
            _name = name;
            _effectTriggers = new List<IEffectTrigger>();
            _effectsOnDeactivate = new List<ICombatEffect>();
        }

        public CombatPassive(string name, int charges) : base(0, TurnTimerBehavior.EndOfTurn)
        {
            _name = name;
            _charges = charges;
            _effectTriggers = new List<IEffectTrigger>();
            _effectsOnDeactivate = new List<ICombatEffect>();
        }

        public CombatPassive(string name, int duration, TurnTimerBehavior turnTimerBehavior) : base(duration, turnTimerBehavior)
        {
            _name = name;
            _charges = 0;
            _effectTriggers = new List<IEffectTrigger>();
            _effectsOnDeactivate = new List<ICombatEffect>();
        }

        public CombatPassive(string name, int duration, int charges, TurnTimerBehavior turnTimerBehavior) : base(duration, turnTimerBehavior)
        {
            _name = name;
            _charges = charges;
            _effectTriggers = new List<IEffectTrigger>();
            _effectsOnDeactivate = new List<ICombatEffect>();
        }

        public ICombatUnit GetOwnerUnit()
        {
            return _owner;
        }

        public void SetAssignee(ICombatUnit combatUnit)
        {
            _assignee = combatUnit;
            foreach (IEffectTrigger effectTrigger in _effectTriggers)
            {
                effectTrigger.SetCombatUnit(combatUnit);
            }

            ICombatVisualHandler visualHandler = _assignee.GetVisualHandler();

            if (visualHandler != null && _ongoingVFX != null)
            {
                IVFXController controllerInstance = visualHandler.PlayVFXObject(_ongoingVFX, _assignee);
                _ongoingVFXController = controllerInstance;
            }

            if (visualHandler != null && _animBoolName != null && _animBoolName != "")
            {
                visualHandler.SetAnimatorBool(_animBoolName, true);
            }

            foreach (ICombatEffect effectOnDeactivate in _effectsOnDeactivate)
            {
                effectOnDeactivate.SetAssignee(_assignee);
            }
        }

        public void SetOwner(ICombatUnit combatUnit)
        {
            foreach (IEffectTrigger effectTrigger in _effectTriggers)
            {
                effectTrigger.SetEffectOwner(combatUnit);
            }
            _owner = combatUnit;

            foreach (ICombatEffect effectOnDeactivate in _effectsOnDeactivate)
            {
                effectOnDeactivate.SetOwner(_owner);
            }
        }

        public ICombatUnit GetAssignee()
        {
            return _assignee;
        }

        public ICombatUnit GetOwner()
        {
            return _owner;
        }

        public void AddEffectTrigger(IEffectTrigger effectTrigger)
        {
            effectTrigger.SetEffectSource(this);
            effectTrigger.SetEffectOwner(_owner);
            _effectTriggers.Add(effectTrigger);
        }

        public void ModifyEffectTriggers<T, U>(IEffectPropertyVisitor<T, U> visitor)
        {
            foreach (IEffectTrigger effectTrigger in _effectTriggers)
            {
                effectTrigger.ModifyEffects(visitor);
            }

            foreach (ICombatEffect effectOnDeactivate in _effectsOnDeactivate)
            {
                effectOnDeactivate.ModifyProperty(visitor);
            }
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


        public void ExpendCharge()
        {
            if (_charges > 1) { _charges -= 1; }
            else if (_charges == 1) { Deactivate(); }
        }

        public override void OnTimerEnd()
        {
            Deactivate();
        }

        /// <summary>
        /// Deactivates this passive
        /// </summary>
        private void Deactivate()
        {
            // Debug.Log("Deactivating passive: " + _name);
            if (_ongoingVFXController != null)
            {
                _ongoingVFXController.Destroy();
            }

            foreach (ICombatEffect effectOnDeactivate in _effectsOnDeactivate)
            {
                effectOnDeactivate.DoEffect();
            }

            foreach (IEffectTrigger effectTrigger in _effectTriggers)
            {
                effectTrigger.OnDeactivate();
            }

            ICombatVisualHandler visualHandler = _assignee.GetVisualHandler();

            if (visualHandler != null && _releaseVFX != null)
            {
                visualHandler.PlayVFXObject(_releaseVFX, _assignee);
            }

            if (visualHandler != null && _animBoolName != null && _animBoolName != "")
            {
                visualHandler.SetAnimatorBool(_animBoolName, false);
            }

            _assignee.RemoveAssignedPassive(this);
        }

        public void SubscribeToUnits(List<ICombatUnit> units)
        {
            foreach (IEffectTrigger trigger in _effectTriggers)
            {
                trigger.SubscribeToUnits(units);
            }
        }

        public void SubscribeToUnit(ICombatUnit unit)
        {
            foreach (IEffectTrigger trigger in _effectTriggers)
            {
                trigger.SubscribeToUnit(unit);
            }
        }

        public void SubscribeToTiles(List<ITile> tiles)
        {
            foreach (IEffectTrigger trigger in _effectTriggers)
            {
                trigger.SubscribeToTiles(tiles);
            }
        }

        public void SubscribeToTile(ITile tile)
        {
            foreach (IEffectTrigger trigger in _effectTriggers)
            {
                trigger.SubscribeToTile(tile);
            }
        }

        public void SetVisualInfo(PassiveVisualInfo info)
        {
            _ongoingVFX = info.ongoingVFX;
            _releaseVFX = info.releaseVFX;
        }

        public void AddReleaseEffect(ICombatEffect effect)
        {
            _effectsOnDeactivate.Add(effect);
        }
    }
}