using System;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.ActionOrPassive;
using CombatRevampScripts.ActionsAbilities.CombatActions;
using CombatRevampScripts.ActionsAbilities.CombatPassives;
using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.EffectHolders;
using CombatRevampScripts.ActionsAbilities.Effects.Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.ActionsAbilities.Enums;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;
using CombatRevampScripts.ActionsAbilities.TurnManager;
using CombatRevampScripts.Board.Tile;
using UnityEngine;

namespace CombatRevampScripts.ActionsAbilities.Effects
{
    /// <summary>
    /// Represents a behavior in combat that is triggered by IEffectHolders of an ICombatUnit and contains IEffectProperties.
    /// </summary>
    public abstract class ACombatEffect : ScriptableObject, ICombatEffect
    {
        private Dictionary<string, object> _properties;

        protected ICombatUnit assignedUnit;
        protected ICombatUnit ownerUnit;
        protected IActionOrPassive effectSource;
        protected IEffectHolder effectHolder;

        // CombatAction-inherited property values
        protected ICombatUnit targetUnit;
        protected List<ICombatUnit> targetLoUnits;
        protected ITile targetTile;
        protected List<ITile> targetLoTiles;
        protected DamageType damageType;

        // EffectTrigger-inherited property values
        protected object eventSender;
        protected ICombatUnit eventUnit;
        protected ITile eventTile;
        protected float eventFloat;
        protected int eventInt;

        // Casted versions of eventSender
        protected ICombatUnit senderUnit;
        protected ITurnManager senderTurnManager;
        protected ITile senderTile; // Never used

        // Callback action to send to methods (currently only used by MoveDefault)
        protected Action callbackAction;
        // Flag to tell callback system to send callbacks from DoEffect to this effect rather than have the IEffectHolder that
        // holds this effect execute them. Default value is always false (currently only true for MoveDefault, where it is triggered
        // after real-time movement is complete).
        protected bool routeCallbackToEffect = false;
        // Flag to indicate whether the default behavior of performing DoAdditionalEffect immediately after DoDefaultEffect should
        // be enforced, or whether it should be handled in a unique manner by this effect (such as with the Move action, where
        // the additional effect happens after the real-time movement is complete).
        protected bool overrideAdditionalEffectBehavior = false;

        // A dictionary of all Passives that this effect or any effects involved in the execution of this effect should have access to.
        // Passives are stored in struct form, so that getter methods on them return a new copy each time they are called.
        private Dictionary<string, CombatPassiveStruct> _savedPassives;

        public ACombatEffect()
        {
            //Initialize();
        }

        public virtual void Initialize()
        {
            _properties = new Dictionary<string, object>();
            _savedPassives = new Dictionary<string, CombatPassiveStruct>();
        }

        public void ModifyProperty<T, U>(IEffectPropertyVisitor<T, U> visitor)
        {
            string propertyName = typeof(T).Name;
            if (!_properties.ContainsKey(propertyName))
            {
                return;
            }
            IEffectProperty<U> property = (IEffectProperty<U>)_properties[propertyName];
            property.Accept(visitor);
        }

        public IEffectProperty<U> AddProperty<U>(IEffectProperty<U> property)
        {
            UnityEngine.Debug.Log("Adding Property: " + property.GetType().Name);
            string propertyName = property.GetType().Name;
            _properties[propertyName] = property;
            return property;
        }

        public IEffectProperty<U> GetPropertyOfType<U>(Type propertyType)
        {
            string propertyName = propertyType.Name;
            if (_properties.ContainsKey(propertyName))
            {
                return (IEffectProperty<U>)_properties[propertyName];
            }
            return null;
        }

        public IEffectProperty<U> GetOrAddPropertyOfType<U>(Type propertyType)
        {
            IEffectProperty<U> existingProperty = GetPropertyOfType<U>(propertyType);
            if (existingProperty != null)
            {
                return existingProperty;
            }

            IEffectProperty<U> newProperty = Activator.CreateInstance(propertyType) as IEffectProperty<U>;

            return AddProperty(newProperty);
        }

        public ICombatPassive GetPassiveByName(string name)
        {
            if (_savedPassives.ContainsKey(name))
            {
                CombatPassiveStruct passiveStruct = _savedPassives[name];
                ICombatPassive passive = passiveStruct.Build(ownerUnit.GetTurnManager());
                return passive;
            }
            return null;
        }

        public void AddSavedPassive(CombatPassiveStruct passive)
        {
            string name = passive.passiveName;
            _savedPassives[name] = passive;
        }

        public virtual ICombatPassive GetBuiltPassiveByName(string name)
        {
            return null;
        }

        public virtual void AddBuiltPassive(ICombatPassive passive) { }

        public void DoEffect()
        {
            SetInheritedPropertyValues();
            CastSenderValues();

            if (ownerUnit == null)
            {
                ownerUnit = assignedUnit;
            }

            DoDefaultEffect();
            if (!overrideAdditionalEffectBehavior)
            {
                DoAdditionalEffect();
            }
        }

        public void SetAssignee(ICombatUnit assignee)
        {
            assignedUnit = assignee;
        }

        public ICombatUnit GetAssignee()
        {
            return assignedUnit;
        }

        public void SetOwner(ICombatUnit owner)
        {
            ownerUnit = owner;
        }

        public ICombatUnit GetOwner()
        {
            return ownerUnit;
        }

        public void SetSource(IActionOrPassive source)
        {
            effectSource = source;
        }

        public void SetSource(ICombatAction source)
        {
            effectSource = source;
            SetEffectHolder(source);
        }

        public void SetEffectHolder(IEffectHolder holder)
        {
            effectHolder = holder;
        }

        public IActionOrPassive GetSource()
        {
            return effectSource;
        }

        public IEffectHolder GetEffectHolder()
        {
            return effectHolder;
        }

        public void SetCallbackAction(Action callbackAction)
        {
            this.callbackAction = callbackAction;
        }

        public bool ShouldRouteCallbackToEffect()
        {
            return routeCallbackToEffect;
        }

        /// <summary>
        /// Performs the default behavior for this effect
        /// </summary>
        public abstract void DoDefaultEffect();

        /// <summary>
        /// Performs any additional behavior for this effect (defaults to nothing)
        /// </summary>
        public virtual void DoAdditionalEffect() { }

        private bool DoesPropertyExist(Type propertyType)
        {
            string propertyName = propertyType.Name;
            return _properties.ContainsKey(propertyName);
        }

        /// <summary>
        /// Updates the local values for all inherited properties
        /// </summary>
        private void SetInheritedPropertyValues()
        {
            // CombatAction-inherited property values
            if (DoesPropertyExist(typeof(TargetUnitProperty)))
            {
                TargetUnitProperty targetUnitProperty = (TargetUnitProperty)GetPropertyOfType<ICombatUnit>(typeof(TargetUnitProperty));
                targetUnit = targetUnitProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(TargetLoUnitProperty)))
            {
                TargetLoUnitProperty targetLoUnitProperty = (TargetLoUnitProperty)GetPropertyOfType<List<ICombatUnit>>(typeof(TargetLoUnitProperty));
                targetLoUnits = targetLoUnitProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(TargetTileProperty)))
            {
                TargetTileProperty targetTileProperty = (TargetTileProperty)GetPropertyOfType<ITile>(typeof(TargetTileProperty));
                targetTile = targetTileProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(TargetLoTileProperty)))
            {
                TargetLoTileProperty targetLoTileProperty = (TargetLoTileProperty)GetPropertyOfType<List<ITile>>(typeof(TargetLoTileProperty));
                targetLoTiles = targetLoTileProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(DamageTypeProperty)))
            {
                DamageTypeProperty damageTypeProperty = (DamageTypeProperty)GetPropertyOfType<DamageType>(typeof(DamageTypeProperty));
                damageType = damageTypeProperty.GetValue();
            }

            // EffectTrigger-inherited property values
            if (DoesPropertyExist(typeof(SenderEventArgProperty)))
            {
                SenderEventArgProperty eventSenderProperty = (SenderEventArgProperty)GetPropertyOfType<object>(typeof(SenderEventArgProperty));
                eventSender = eventSenderProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(CombatUnitEventArgProperty)))
            {
                CombatUnitEventArgProperty eventUnitProperty = (CombatUnitEventArgProperty)GetPropertyOfType<ICombatUnit>(typeof(CombatUnitEventArgProperty));
                eventUnit = eventUnitProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(TileEventArgProperty)))
            {
                TileEventArgProperty eventTileProperty = (TileEventArgProperty)GetPropertyOfType<ITile>(typeof(TileEventArgProperty));
                eventTile = eventTileProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(FloatEventArgProperty)))
            {
                FloatEventArgProperty eventFloatProperty = (FloatEventArgProperty)GetPropertyOfType<float>(typeof(FloatEventArgProperty));
                eventFloat = eventFloatProperty.GetValue();
            }
            if (DoesPropertyExist(typeof(IntEventArgProperty)))
            {
                IntEventArgProperty eventIntProperty = (IntEventArgProperty)GetPropertyOfType<int>(typeof(IntEventArgProperty));
                eventInt = eventIntProperty.GetValue();
            }
        }

        private void CastSenderValues()
        {
            if (eventSender != null)
            {
                if (typeof(ITurnManager).IsAssignableFrom(eventSender.GetType()))
                {
                    senderTurnManager = (ITurnManager)eventSender;
                }
                if (typeof(ITile).IsAssignableFrom(eventSender.GetType()))
                {
                    senderTile = (ITile)eventSender;
                }
                if (typeof(ICombatUnit).IsAssignableFrom(eventSender.GetType()))
                {
                    senderUnit = (ICombatUnit)eventSender;
                }
            }
        }
    }
}
