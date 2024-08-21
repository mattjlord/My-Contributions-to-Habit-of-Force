namespace CombatRevampScripts.ActionsAbilities.Expressions
{
    /// <summary>
    /// A predicate that compares the value of two binary expression trees and returns the result
    /// </summary>
    public interface IExpressionPredicate
    {
        /// <summary>
        /// Tests the predicate
        /// </summary>
        /// <returns>the result of the test</returns>
        public bool Test();
    }
}