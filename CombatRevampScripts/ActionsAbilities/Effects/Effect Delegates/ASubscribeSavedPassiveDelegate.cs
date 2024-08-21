namespace CombatRevampScripts.ActionsAbilities.Effects.Effect_Delegates
{
    public enum PassiveInstanceOptions
    {
        DoNotCreateANewInstance,
        AddANewInstanceToAssignedUnit,
        AddANewInstanceToOwnerUnit
    }

    /// <summary>
    /// An effect delegate that subscribes the subject to a passive with the given name,
    /// and offers options for creating a new instance of the passive, such as in the 
    /// case of a landmine ability, where a new passive would be created on the owner
    /// unit for each tile occupied by a mine.
    /// </summary>
    /// <typeparam name="T">the type of the subject</typeparam>
    public abstract class ASubscribeSavedPassiveDelegate<T> : AEffectDelegate<T>
    {
        public string savedPassiveName;
        public PassiveInstanceOptions instanceOptions;
    }
}