using CombatRevampScripts.ActionsAbilities.CombatUnit;
using CombatRevampScripts.ActionsAbilities.Effects;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Visitors;
using CombatRevampScripts.Board.Tile;
using CombatRevampScripts.Board.TilePiece.MechPilot;
using CombatRevampScripts.CombatVisuals.VisualInfo;
using CombatRevampScripts.CombatVisuals.Handler;
using CombatRevampScripts.CombatVisuals.OneShotAnim;
using System;
using UnityEngine;
using System.Collections.Generic;
using CombatRevampScripts.ActionsAbilities.Effects.Properties;
using CombatRevampScripts.ActionsAbilities.Structs.PassivesAndTriggers;

namespace CombatRevampScripts.ActionsAbilities.EffectHolders
{
    public abstract class AEffectHolder : IEffectHolder
    {
        protected OneShotAnimInfo preEffectAnim;
        protected GameObject preEffectVFXObject;
        protected GameObject postEffectVFXObject;
        protected OneShotAnimInfo postEffectAnim;

        protected List<ICombatEffect> effects;
        protected ICombatUnit combatUnit;

        private float _lastDamageSent;
        private float _lastDamageDealt;
        private float _lastHealingSent;
        private float _lastHealingDealt;
        private int _lastDistanceMoved;

        public AEffectHolder()
        {
            effects = new List<ICombatEffect>();
        }

        public void DoEffects()
        {
            Action emptyAction = () => { };
            DoEffects(emptyAction);
        }

        public void DoEffects(Action actionOnComplete)
        {
            ICombatVisualHandler visualHandler = GetCombatUnit().GetVisualHandler();

            if (visualHandler == null) { }

            bool routeCallbackToEffect = false;

            foreach (ICombatEffect effect in effects)
            {
                routeCallbackToEffect = effect.ShouldRouteCallbackToEffect();

                if (routeCallbackToEffect)
                {
                    effect.SetCallbackAction(actionOnComplete);
                    break;
                }
            }

            Action afterPostAnim = () =>
            {
                if (!routeCallbackToEffect)
                {
                    GetCombatUnit().SetBusyStatus(false);
                    actionOnComplete.Invoke();
                    AfterFullEffectResolve();
                }
            };

            Action afterPostVFX = () =>
            {
                if (postEffectAnim.animationStateName != null && postEffectAnim.animationStateName != "")
                {
                    visualHandler.PlayAnimIntoAction(postEffectAnim.animationStateName, postEffectAnim.busyPeriod, afterPostAnim);
                }
                else
                {
                    afterPostAnim();
                }
            };

            Action afterEffect = () =>
            {
                if (postEffectVFXObject != null)
                {
                    visualHandler.PlayVFXObjectIntoAction(postEffectVFXObject, this, afterPostVFX);
                }
                else
                {
                    afterPostVFX();
                }
            };

            Action afterPreVFX = () =>
            {
                DoEffectsOnly();
                afterEffect();
            };

            Action afterPreAnim = () =>
            {
                if (preEffectVFXObject != null)
                {
                    visualHandler.PlayVFXObjectIntoAction(preEffectVFXObject, this, afterPreVFX);
                }
                else
                {
                    afterPreVFX();
                }
            };

            GetCombatUnit().SetBusyStatus(true);

            TargetUnitProperty targetUnitProperty = (TargetUnitProperty)GetFirstPropertyOfType<ICombatUnit>(typeof(TargetUnitProperty));
            TargetTileProperty targetTileProperty = (TargetTileProperty)GetFirstPropertyOfType<ITile>(typeof(TargetTileProperty));

            if (targetUnitProperty != null)
            {
                ICombatUnit targetUnit = targetUnitProperty.GetValue();
                MechTilePiece tilePiece = targetUnit.GetTilePiece();
                ITile tile = tilePiece.GetComponentInParent<ITile>();
                Vector2 targetPos = tile.GetBoardPosition();
                GetCombatUnit().GetTilePiece().FaceToward(targetPos);
            }
            if (targetTileProperty != null)
            {
                ITile tile = targetTileProperty.GetValue();
                Vector2 targetPos = tile.GetBoardPosition();
                GetCombatUnit().GetTilePiece().FaceToward(targetPos);
            }

            if (preEffectAnim.animationStateName != null && preEffectAnim.animationStateName != "")
            {
                visualHandler.PlayAnimIntoAction(preEffectAnim.animationStateName, preEffectAnim.busyPeriod, afterPreAnim);
            }
            else
            {
                afterPreAnim();
            }
        }

        /// <summary>
        /// Just performs the effect(s), without any accompanying visuals (called from inside the DoEffect method).
        /// </summary>
        public virtual void DoEffectsOnly()
        {
            foreach (ICombatEffect effect in effects)
            {
                effect.DoEffect();
            }
        }

        public ICombatUnit GetCombatUnit()
        {
            return combatUnit;
        }

        public List<ICombatEffect> GetEffects()
        {
            return effects;
        }

        public virtual void AddEffect(ICombatEffect effect)
        {
            effects.Add(effect);
            // Debug.Log("Adding effect: " + effect.GetType().Name);
            InitializeEffectProperties(effect);
        }

        /// <summary>
        /// Initializes effect properties for the given effect
        /// </summary>
        public abstract void InitializeEffectProperties(ICombatEffect effect);

        public void ModifyEffects<T, U>(IEffectPropertyVisitor<T, U> visitor)
        {
            foreach (ICombatEffect effect in effects)
            {
                effect.ModifyProperty(visitor);
            }
        }

        public virtual void SetCombatUnit(ICombatUnit combatUnit)
        {
            this.combatUnit = combatUnit;
            foreach (ICombatEffect effect in effects)
            {
                effect.SetAssignee(combatUnit);
            }
        }

        public void SetEffectVisualInfo(EffectVisualInfo info)
        {
            preEffectAnim = info.preEffectAnimation;
            postEffectAnim = info.postEffectAnimation;
            preEffectVFXObject = info.preEffectVFX;
            postEffectVFXObject = info.postEffectVFX;
        }

        public IEffectProperty<U> GetFirstPropertyOfType<U>(Type propertyType)
        {
            foreach (ICombatEffect effect in effects)
            {
                IEffectProperty<U> property = effect.GetPropertyOfType<U>(propertyType);
                if (property != null)
                {
                    return property;
                }
            }

            return null;
        }

        public float GetTempEffectValue(TempEffectValueType valueType)
        {
            switch (valueType)
            {
                case TempEffectValueType.LastDamageSent:
                    return _lastDamageSent;
                case TempEffectValueType.LastDamageDealt:
                    return _lastDamageDealt;
                case TempEffectValueType.LastHealingSent:
                    return _lastHealingSent;
                case TempEffectValueType.LastHealingDealt:
                    return _lastHealingDealt;
                case TempEffectValueType.LastDistanceMoved:
                    return _lastDistanceMoved;
                default:
                    return 0;
            }
        }

        public void SetTempEffectValue(TempEffectValueType valueType, float value)
        {
            switch (valueType)
            {
                case TempEffectValueType.LastDamageSent:
                    _lastDamageSent = value;
                    break;
                case TempEffectValueType.LastDamageDealt:
                    _lastDamageDealt = value;
                    break;
                case TempEffectValueType.LastHealingSent:
                    _lastHealingDealt = value;
                    break;
                case TempEffectValueType.LastHealingDealt:
                    _lastHealingDealt = value;
                    break;
                case TempEffectValueType.LastDistanceMoved:
                    _lastDistanceMoved = (int)value;
                    break;
            }
        }

        public void AddSavedPassive(CombatPassiveStruct passive)
        {
            foreach (ICombatEffect effect in effects)
            {
                effect.AddSavedPassive(passive);
            }
        }

        public virtual void AfterFullEffectResolve() { }
    }
}