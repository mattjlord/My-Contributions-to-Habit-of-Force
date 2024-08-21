namespace CombatRevampScripts.ActionsAbilities.Expressions
{
    /// <summary>
    /// Represents a node in a binary expression tree
    /// </summary>
    public interface IExpressionNode
    {
        /// <summary>
        /// Computes the value represented by the entire tree
        /// </summary>
        /// <returns>the value computed for the entire tree</returns>
        public float Compute();
    }
}