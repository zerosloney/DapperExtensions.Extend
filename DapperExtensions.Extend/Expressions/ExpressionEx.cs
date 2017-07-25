using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DapperExtensions.Extend
{
    internal static class ExpressionEx
    {

        /// <summary>
        /// Combines two expressions (a -> b) and (a -> c) with a combining expression ((b,c) -> d) to form an expression (a -> d)
        /// </summary>
        /// <typeparam name="T">The input type of both primary expressions</typeparam>
        /// <typeparam name="T1">The result type of the first primary expression</typeparam>
        /// <typeparam name="T2">The result type of the second primary expression</typeparam>
        /// <typeparam name="TResult">The result type of the combining expression</typeparam>
        /// <param name="first">The first primary expression</param>
        /// <param name="second">The second primary expression</param>
        /// <param name="combineExpr">The combining expression</param>
        /// <returns>A composite of all three expressions, with the first two substituted in place of the third's parameters</returns>
        public static Expression<Func<T, TResult>> Combine<T, T1, T2, TResult>(
            this Expression<Func<T, T1>> first,
            Expression<Func<T, T2>> second,
            Expression<Func<T1, T2, TResult>> combineExpr)
        {
            // replace second's parameter with first's
            var inputParameter = first.Parameters[0];
            var expr2Body = second.Body.Replace(second.Parameters[0], inputParameter);

            // then replace both expressions in combining func
            var map = new Dictionary<Expression, Expression>
            {
                { combineExpr.Parameters[0], first.Body },
                { combineExpr.Parameters[1], expr2Body }
            };
            var replaced = combineExpr.Body.Replace(map);
            return Expression.Lambda<Func<T, TResult>>(replaced, inputParameter);
        }

        /// <summary>
        /// Composes two expressions (a -> b) and (b -> c) to form an expression (a -> c)
        /// </summary>
        /// <typeparam name="T1">The input to the first expression</typeparam>
        /// <typeparam name="T2">The output of the first expression and input to the second</typeparam>
        /// <typeparam name="T3">The output of the second expression</typeparam>
        /// <param name="first">The first expression</param>
        /// <param name="second">The second expression</param>
        /// <returns>A composite of the two expressions, with the first substituted in place of the second's parameter</returns>
        public static Expression<Func<T1, T3>> Compose<T1, T2, T3>(
            this Expression<Func<T1, T2>> first,
            Expression<Func<T2, T3>> second)
        {
            var replaced = second.Body.Replace(second.Parameters[0], first.Body);
            return Expression.Lambda<Func<T1, T3>>(replaced, first.Parameters[0]);
        }

        /// <summary>
        /// Applies null coalescence to an expression resulting in a nullable type
        /// </summary>
        /// <typeparam name="T">The input to the expression</typeparam>
        /// <typeparam name="TResult">The nullable result</typeparam>
        /// <param name="expr">The expression</param>
        /// <param name="defaultValue">The default value with which to replace a null value. Defaults to default(TResult)</param>
        /// <returns>An expression in which a null result is replaced with the given value</returns>
        public static Expression<Func<T, TResult>> Coalesce<T, TResult>(this Expression<Func<T, TResult?>> expr, TResult defaultValue = default(TResult))
            where TResult : struct
        {
            return expr.Compose(x => x ?? defaultValue);
        }

        /// <summary>
        /// 从属性表达式获取属性元数据访问实例
        /// </summary>
        /// <typeparam name="T1">属性表达式参数类型</typeparam>
        /// <typeparam name="T2">属性表达式返回类型</typeparam>
        /// <param name="propertyGetter">属性表达式</param>
        /// <returns>属性元数据访问实例</returns>
        public static PropertyInfo GetPropertyInfo<T1, T2>(this Expression<Func<T1, T2>> propertyGetter)
        {
            var memberExpr = propertyGetter.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("Expression should be property getter: " + propertyGetter);
            var propInfo = memberExpr.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException("Expression should be property getter: " + propertyGetter);
            return propInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberInit"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static MemberInitExpression AddBinding(this MemberInitExpression memberInit, MemberBinding binding)
        {
            var bindings = new List<MemberBinding>(memberInit.Bindings)
            {
                binding
            };
            return Expression.MemberInit(memberInit.NewExpression, bindings);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="toReplace"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static Expression Replace(this Expression expression, Expression toReplace, Expression replacement)
        {
            return ExpressionReplacer.Replace(expression, toReplace.Equals, x => replacement);
        }

        public static Expression Replace(this Expression expression, IDictionary<Expression, Expression> map)
        {
            return ExpressionReplacer.Replace(expression, map.ContainsKey, x => map[x]);
        }

        class ExpressionReplacer : ExpressionVisitor
        {
            private readonly Func<Expression, bool> _match;
            private readonly Func<Expression, Expression> _replace;

            private ExpressionReplacer(Func<Expression, bool> match, Func<Expression, Expression> replace)
            {
                _match = match;
                _replace = replace;
            }

            public override Expression Visit(Expression node)
            {
                return _match(node) ? _replace(node) : base.Visit(node);
            }

            public static Expression Replace(Expression expression, Func<Expression, bool> match,
                Func<Expression, Expression> replace)
            {
                return new ExpressionReplacer(match, replace).Visit(expression);
            }
        }
    }
}
