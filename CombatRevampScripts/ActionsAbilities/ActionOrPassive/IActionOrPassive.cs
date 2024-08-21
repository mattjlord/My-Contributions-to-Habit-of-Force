using CombatRevampScripts.ActionsAbilities.CombatUnit;

namespace CombatRevampScripts.ActionsAbilities.ActionOrPassive
{
    /// <summary>
    /// Represents an action or passive with a string name, description, and an ICombatUnit owner.
    /// </summary>
    public interface IActionOrPassive
    {
        /// <summary>
        /// Gets the name of this
        /// </summary>
        /// <returns>the name of this, as a string</returns>
        public string GetName();
       
        
        /// <summary>
        /// Gets the description of this
        /// </summary>
        /// <returns>the description of this, as a string</returns>
        public string GetDescription();

        /// <summary>
        /// Sets the description of this to the given
        /// </summary>
        /// <param name="description">the description to set</param>
        public void SetDescription(string description);

        /// <summary>
        /// Gets the owner of this.
        /// </summary>
        /// <returns>the ICombatUnit owner</returns>
        public ICombatUnit GetOwnerUnit();
    }
}