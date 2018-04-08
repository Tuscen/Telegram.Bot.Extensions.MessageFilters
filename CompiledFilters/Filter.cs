using System;
using System.Linq.Expressions;

namespace CompiledFilters
{
    /// <summary>
    /// Delegate for the compiled filtering functions created
    /// with the <see cref="Filter{T}"/> class and its derivatives.
    /// </summary>
    /// <typeparam name="T">The type of the items that are filtered.</typeparam>
    /// <param name="item">The item to be filtered.</param>
    /// <returns>Whether the given item satisfies the filter conditions.</returns>
    public delegate bool CompiledFilter<T>(T item);

    /// <summary>
    /// The base class for all filters.
    /// <para/>
    /// Allows one to build easily readable expressions and compile them
    /// into a function that determines if they hold for a given input.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public abstract class Filter<T>
    {
        protected static ConstantExpression falseExpr = Expression.Constant(false, typeof(bool));
        protected static ConstantExpression nullExpr = Expression.Constant(null);
        protected static ParameterExpression parameter = Expression.Parameter(typeof(T));
        protected static ConstantExpression trueExpr = Expression.Constant(true, typeof(bool));

        public static Filter<T> operator |(Filter<T> lhs, Filter<T> rhs)
        {
            return new OrFilter<T>(lhs, rhs);
        }

        /// <summary>
        /// Compiles the current filter construction into a function
        /// that determines if the conditions hold for a given input.
        /// </summary>
        /// <returns>Whether the conditions are satisfied.</returns>
        public CompiledFilter<T> Compile()
        {
            return Expression.Lambda<CompiledFilter<T>>(GetFilterExpression(), parameter).Compile();
        }

        /// <summary>
        /// Helper method to get the <see cref="Expression"/>s from other <see cref="Filter{T}"/>s in derived classes.
        /// </summary>
        /// <param name="filter">The <see cref="Filter{T}"/> to get the <see cref="Expression"/> from.</param>
        /// <returns></returns>
        protected static Expression GetFilterExpression(Filter<T> filter)
        {
            return filter.GetFilterExpression();
        }

        /// <summary>
        /// Must return the <see cref="Expression"/> that represents the Filter.
        /// </summary>
        /// <returns>The <see cref="Expression"/> representing the Filter.</returns>
        protected abstract Expression GetFilterExpression();
    }
}