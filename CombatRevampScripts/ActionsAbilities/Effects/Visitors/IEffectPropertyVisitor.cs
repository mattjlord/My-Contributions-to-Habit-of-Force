using CombatRevampScripts.ActionsAbilities.Effects.Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.Custom_Effect_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties;
using CombatRevampScripts.ActionsAbilities.Effects.Properties.EventArg_Properties.Sender;

namespace CombatRevampScripts.ActionsAbilities.Effects.Visitors
{
    /// <summary>
    /// Performs modifying operations on the value of a specific IEffectProperty
    /// </summary>
    /// <typeparam name="T">the type of the IEffectProperty to modify</typeparam>
    /// <typeparam name="U">the type of value contained in T</typeparam>
    public interface IEffectPropertyVisitor<out T, U>
    {
        /// <summary>
        /// Visits the given IEffectProperty
        /// </summary>
        /// <param name="effectProperty">the IEffectProperty to visit</param>
        /// <returns>A generic value determined by this</returns>
        T Visit(IEffectProperty<U> property);

        // Custom Effect Properties
        T Visit(AOERangeProperty property);
        T Visit(DamageTypeProperty property);
        T Visit(TargetUnitProperty property);
        T Visit(TargetTileProperty property);
        T Visit(TargetLoUnitProperty property);
        T Visit(TargetLoTileProperty property);
        T Visit(MechDamageProperty property);
        T Visit(MechHealProperty property);
        T Visit(PilotDamageProperty property);
        T Visit(PilotHealProperty property);

        // EffectTrigger Properties
        T Visit(SenderEventArgProperty property);
        T Visit(FloatEventArgProperty property);
        T Visit(IntEventArgProperty property);
        T Visit(CombatUnitEventArgProperty property);
        T Visit(TileEventArgProperty property);
    }
}
