using System;
using System.Collections.Generic;
using System.Reflection;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.CustomEventArgs;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors.EventArg_Visitors.Sender;
using CombatRevampScripts.ActionsAbilities.Structs.Events;
using CombatRevampScripts.ActionsAbilities.Structs.Predicates;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.CombatVisuals.Handler;
using CombatRevampScripts.CombatVisuals.VFXController;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.EffectTriggers
{

    /// <summary>
    /// Represents a type of IEffectHolder that listens to Events and uses a PredicateSO<ICombatUnit> to filter them in order to trigger its ICombatEffect.
    /// </summary>
    public class EffectTrigger : AEffectHolder, IEffectTrigger
    {
        private ICombatPassive _passive;

        private List<ICustomPredicate<ICombatUnit>> combatUnitPredicates;

        private List<CombatUnitEventType> _cachedThisUnitEvents;
        private List<CombatUnitEventType> _cachedUnitEvents;
        private List<TileEventType> _cachedTileEvents;

        private GameObject _ongoingVFX;
        private IVFXController _ongoingVFXController;
        
        private List<ITile> _subscribedTiles;
        private List<ICombatUnit> _subscribedUnits;

        private Dictionary<object, List<Delegate>> _eventSubscriptions;
        private List<string> _eventNames;

        public EffectTrigger() : base()
        {
            combatUnitPredicates = new List<ICustomPredicate<ICombatUnit>>();
            _cachedThisUnitEvents = new List<CombatUnitEventType>();
            _cachedUnitEvents = new List<CombatUnitEventType>();
            _cachedTileEvents = new List<TileEventType>();
            _subscribedTiles = new List<ITile>();
            _subscribedUnits = new List<ICombatUnit>();
            _eventSubscriptions = new Dictionary<object, List<Delegate>>();
            _eventNames = new List<string>();
        }

        public override void InitializeEffectProperties(ICombatEffect effect)
        {
            //UnityEngine.Debug.Log("Initializing EffectTrigger properties:");
            effect.AddProperty(new SenderEventArgProperty(null));
            effect.AddProperty(new FloatEventArgProperty(0));
            effect.AddProperty(new IntEventArgProperty(0));
            effect.AddProperty(new CombatUnitEventArgProperty(null));
        }

        public override void AfterFullEffectResolve()
        {
            base.AfterFullEffectResolve();
            _passive.ExpendCharge();
        }

        private void DoEffects(object sender)
        {
            ModifyEffects(new SenderEventArgVisitor(sender));
            DoEffects();
        }

        private void DoEffects(object sender, float value)
        {
            ModifyEffects(new FloatEventArgVisitor(value));
            DoEffects(sender);
        }

        private void DoEffects(object sender, int value)
        {
            ModifyEffects(new IntEventArgVisitor(value));
            DoEffects(sender);
        }

        private void DoEffects(object sender, ICombatUnit value)
        {
            ModifyEffects(new CombatUnitEventArgVisitor(value));
            DoEffects(sender);
        }

        private void DoEffects(object sender, ITile value)
        {
            ModifyEffects(new TileEventArgVisitor(value));
            DoEffects(sender);
        }

        private void DoEffects(object sender, ICombatUnit value1, float value2)
        {
            ModifyEffects(new CombatUnitEventArgVisitor(value1));
            ModifyEffects(new FloatEventArgVisitor(value2));
            DoEffects(sender);
        }

        private void DoEffects(object sender, ICombatUnit value1, int value2)
        {
            ModifyEffects(new CombatUnitEventArgVisitor(value1));
            ModifyEffects(new IntEventArgVisitor(value2));
            DoEffects(sender);
        }

        public override void SetCombatUnit(ICombatUnit combatUnit)
        {
            base.SetCombatUnit(combatUnit);
            foreach (var pred in combatUnitPredicates)
            {
                pred.SetOwnerUnit(combatUnit);
            }

            if (_ongoingVFX != null && _ongoingVFXController == null)
            {
                ICombatVisualHandler visualHandler = combatUnit.GetVisualHandler();
                if (visualHandler != null)
                {
                    _ongoingVFXController = visualHandler.PlayVFXObject(_ongoingVFX, this);
                }
            }

            foreach (CombatUnitEventType type in _cachedThisUnitEvents)
            {
                SubscribeToThisUnitEvent(type.ToString());
            }
        }

        public void SetEffectOwner(ICombatUnit owner)
        {
            foreach (ICombatEffect effect in effects)
            {
                effect.SetOwner(owner);
            }
        }

        public void SetEffectSource(IActionOrPassive source)
        {
            _passive = (ICombatPassive)source;

            foreach (ICombatEffect effect in effects)
            {
                effect.SetSource(source);
                effect.SetEffectHolder(this);
            }
        }

        public void SetOngoingVFX(GameObject vfxObject)
        {
            _ongoingVFX = vfxObject;
        }

        public void SubscribeToEvent(object eventSource, string eventName)
        {
            EventInfo eventInfo = eventSource.GetType().GetEvent(eventName);
            if (eventInfo != null)
            {
                MethodInfo eventHandler = GetType().GetMethod(nameof(HandleEvent), BindingFlags.Instance | BindingFlags.NonPublic);
                Delegate handlerDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, eventHandler);
                eventInfo.AddEventHandler(eventSource, handlerDelegate);

                if (!_eventSubscriptions.ContainsKey(eventSource))
                {
                    _eventSubscriptions[eventSource] = new List<Delegate>();
                }
                _eventSubscriptions[eventSource].Add(handlerDelegate);
                _eventNames.Add(eventName);
            }
        }
        
        public void SubscribeToThisUnitEvent(string eventName)
        {
            SubscribeToEvent(combatUnit, eventName);
        }

        public void CacheThisUnitEventType(CombatUnitEventType type)
        {
            _cachedThisUnitEvents.Add(type);
        }

        public void CacheUnitEventType(CombatUnitEventType type)
        {
            _cachedUnitEvents.Add(type);
        }

        public void CacheTileEventType(TileEventType type)
        {
            _cachedTileEvents.Add(type);
        }

        public void SubscribeToUnits(List<ICombatUnit> units)
        {
            foreach(ICombatUnit unit in units)
            {
                SubscribeToUnit(unit);
            }
        }

        public void SubscribeToUnit(ICombatUnit unit)
        {
            foreach (CombatUnitEventType eventType in _cachedUnitEvents)
            {
                SubscribeToEvent(unit, eventType.ToString());
            }
            _subscribedUnits.Add(unit);
        }

        public void SubscribeToTiles(List<ITile> tiles)
        {
            foreach (ITile tile in tiles)
            {
                SubscribeToTile(tile);
            }
        }

        public void SubscribeToTile(ITile tile)
        {
            foreach (TileEventType eventType in _cachedTileEvents)
            {
                SubscribeToEvent(tile, eventType.ToString());
            }
            _subscribedTiles.Add(tile);
        }

        public List<ICombatUnit> GetSubscribedUnits()
        {
            return _subscribedUnits;
        }

        public List<ITile> GetSubscribedTiles()
        {
            return _subscribedTiles;
        }
        
        private void HandleEvent(object sender, EventArgs e)
        {
            // Test CombatUnitPredicates on sender if applicable
            if (sender.GetType() == typeof(ICombatUnit))
            {
                if (TestPredicatesOnUnit((ICombatUnit)sender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }
            // Test CombatUnitPredicates on the OverrideSender of e if e is of type UnitArgs
            if (e.GetType() == typeof(UnitArgs))
            {
                UnitArgs unitArgs = (UnitArgs)e;
                if (TestPredicatesOnUnit(unitArgs.OverrideSender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }

            // Test CombatUnitPredicates on the OverrideSender of e if e is of type UnitFloatArgs
            if (e.GetType() == typeof(UnitFloatArgs))
            {
                UnitFloatArgs unitFloatArgs = (UnitFloatArgs)e;
                if (TestPredicatesOnUnit(unitFloatArgs.OverrideSender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }

            // Test CombatUnitPredicates on the Override Sender of e if e is of type UnitIntArgs
            if (e.GetType() == typeof(UnitIntArgs))
            {
                UnitIntArgs unitIntArgs = (UnitIntArgs)e;
                if (TestPredicatesOnUnit(unitIntArgs.OverrideSender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }

            // Test CombatUnitPredicates on the Override Sender of e if e is of type FloatArgs
            if (e.GetType() == typeof(FloatArgs))
            {
                FloatArgs floatArgs = (FloatArgs)e;
                if (TestPredicatesOnUnit(floatArgs.OverrideSender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }

            // Test CombatUnitPredicates on the Override Sender of e if e is of type IntArgs
            if (e.GetType() == typeof(IntArgs))
            {
                IntArgs intArgs = (IntArgs)e;
                if (TestPredicatesOnUnit(intArgs.OverrideSender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }

            // Test CombatUnitPredicates on the Override Sender of e if e is of type OverrideSenderArgs
            if (e.GetType() == typeof(OverrideSenderArgs))
            {
                OverrideSenderArgs overrideSenderArgs = (OverrideSenderArgs)e;
                if (TestPredicatesOnUnit(overrideSenderArgs.OverrideSender))
                {
                    DoEventResponse(sender, e);
                }
                return;
            }

            DoEventResponse(sender, e);
        }

        private bool TestPredicatesOnUnit(ICombatUnit unit)
        {
            if (combatUnitPredicates.Count == 0 || unit == null)
            {
                return true;
            }

            foreach (ICustomPredicate<ICombatUnit> pred in combatUnitPredicates)
            {
                pred.SetEffectHolder(this);
                if (pred.Test(unit))
                {
                    return true;
                }
            }

            return false;
        }

        private void DoEventResponse(object sender, EventArgs e)
        {
            if (typeof(ITile).IsAssignableFrom(sender.GetType()))
            {
                ITile tile = (ITile)sender;
                
                bool subscribed = false;
                foreach (ITile subscribedTile in _subscribedTiles)
                {
                    if (tile == subscribedTile)
                    {
                        subscribed = true;
                        break;
                    }
                }

                if (!subscribed) { return; }
            }

            if (typeof(ICombatUnit).IsAssignableFrom(sender.GetType()))
            {
                ICombatUnit unit = (ICombatUnit)sender;

                bool subscribed = false;
                if (unit == combatUnit && _cachedThisUnitEvents.Count > 0)
                {
                    subscribed = true;
                }
                if (!subscribed)
                {
                    foreach (ICombatUnit subscribedUnit in _subscribedUnits)
                    {
                        if (unit == subscribedUnit)
                        {
                            subscribed = true;
                            break;
                        }
                    }
                }

                if (!subscribed) { return; }
            }

            switch (e)
            {
                case FloatArgs floatArgs:
                    if (floatArgs.OverrideSender != null)
                    {
                        DoEffects(floatArgs.OverrideSender, floatArgs.Value);
                        break;
                    }
                    DoEffects(sender, floatArgs.Value);
                    break;
                case IntArgs intArgs:
                    if (intArgs.OverrideSender != null)
                    {
                        DoEffects(intArgs.OverrideSender, intArgs.Value);
                        break;
                    }
                    DoEffects(sender, intArgs.Value);
                    break;
                case UnitArgs unitArgs:
                    if (unitArgs.OverrideSender != null)
                    {
                        DoEffects(unitArgs.OverrideSender, unitArgs.Value);
                        break;
                    }
                    DoEffects(sender, unitArgs.Value);
                    break;
                case TileArgs tileArgs:
                    if (tileArgs.OverrideSender != null)
                    {
                        DoEffects(tileArgs.OverrideSender, tileArgs.Value);
                        break;
                    }
                    DoEffects(sender, tileArgs.Value);
                    break;
                case UnitFloatArgs unitFloatArgs:
                    if (unitFloatArgs.OverrideSender != null)
                    {
                        DoEffects(unitFloatArgs.OverrideSender, unitFloatArgs.Value1, unitFloatArgs.Value2);
                        break;
                    }
                    DoEffects(sender, unitFloatArgs.Value1, unitFloatArgs.Value2);
                    break;
                case UnitIntArgs unitIntArgs:
                    if (unitIntArgs.OverrideSender != null)
                    {
                        DoEffects(unitIntArgs.OverrideSender, unitIntArgs.Value1, unitIntArgs.Value2);
                        break;
                    }
                    DoEffects(sender, unitIntArgs.Value1, unitIntArgs.Value2);
                    break;
                case OverrideSenderArgs overrideSenderArgs:
                    DoEffects(overrideSenderArgs.OverrideSender);
                    break;
                default:
                    DoEffects(sender);
                    break;
            }
        }

        public void AddUnitPredicate(ICustomPredicate<ICombatUnit> predicate)
        {
            combatUnitPredicates.Add(predicate);
        }

        private void UnsubscribeFromAllEvents()
        {
            foreach (string eventName in _eventNames)
            {
                foreach (var eventSource in _eventSubscriptions.Keys)
                {
                    foreach (var handlerDelegate in _eventSubscriptions[eventSource])
                    {
                        EventInfo eventInfo = eventSource.GetType().GetEvent(eventName);
                        if (eventInfo != null)
                        {
                            eventInfo.RemoveEventHandler(eventSource, handlerDelegate);
                        }
                    }
                }
            }
        }

        public void OnDeactivate()
        {
            if (_ongoingVFXController != null)
            {
                _ongoingVFXController.Destroy();
            }
            UnsubscribeFromAllEvents();
        }
    }
}