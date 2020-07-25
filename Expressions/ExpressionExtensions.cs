using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Expressions.Helpers;

namespace Expressions
{
    public static class ExpressionExtensions
    {
        private static readonly MethodInfo ToLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);

        public static Expression<Func<TClass, bool>> CreateEqualExpression<TClass, TValue>(string propertyName, TValue value, string parameterName = null) where TClass : class
        {
            var valueType = typeof(TValue);
            var classType = typeof(TClass);
            ValidateClass<TValue>(classType, propertyName, value);

            var parameter = GetParameterExpression(classType, parameterName);
            var member = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value, valueType);
            var predicate = Expression.Equal(member, constant);
            var expression = Expression.Lambda<Func<TClass, bool>>(predicate, parameter);

            return expression;
        }

        public static Expression<Func<TClass, TValue>> CreateReturnExpression<TClass, TValue>(string propertyName, string parameterName = null) where TClass : class
        {
            var classType = typeof(TClass);
            ValidateClass<TValue>(classType, propertyName);

            var parameter = GetParameterExpression(classType, parameterName);
            var predicate = Expression.Property(parameter, propertyName);
            var expression = Expression.Lambda<Func<TClass, TValue>>(predicate, parameter);

            return expression;
        }

        public static Expression<Func<TClass, bool>> CreateIsNullOrWhiteSpaceExpression<TClass>(string propertyName, bool comparisonValue, string parameterName = null) where TClass : class
        {
            var classType = typeof(TClass);
            var method = ValidateMethod(typeof(string), "IsNullOrWhiteSpace");
            ValidateClass<string>(classType, propertyName);

            var parameter = GetParameterExpression(classType, parameterName);
            var obtainPropertyPredicate = Expression.Property(parameter, propertyName);
            var methodPredicate = Expression.Call(method, obtainPropertyPredicate);
            var comparisonPredicate = Expression.Equal(methodPredicate, Expression.Constant(comparisonValue));
            var expression = Expression.Lambda<Func<TClass, bool>>(comparisonPredicate, parameter);

            return expression;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression ToLower(this MemberExpression member)
        {
            return Expression.Call(member, ToLowerMethod);
        }

        public static Expression ToLower(this ConstantExpression constant)
        {
            return Expression.Call(constant, ToLowerMethod);
        }

        private static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // if andalso and second expression is true, then ignore it
            if (merge == Expression.AndAlso && second.ToString() == $"{second.Parameters.FirstOrDefault()?.Name} => True")
            {
                return first;
            }

            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // if andalso and first expression is true, then send only the second expression with the first expression's parameters
            if (merge == Expression.AndAlso && first.ToString() == $"{first.Parameters.FirstOrDefault()?.Name} => True")
            {
                return Expression.Lambda<T>(secondBody, first.Parameters);
            }

            // apply composition of lambda expression bodies to parameters from the first expression
            var expression = Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
            return expression;
        }

        private static ParameterExpression GetParameterExpression(Type type, string parameterName = null)
        {
            var parameter = Expression.Parameter(type, !string.IsNullOrWhiteSpace(parameterName) ? parameterName : "x");
            return parameter;
        }

        private static void ValidateClass<TValue>(Type type, string propertyName, object value = null)
        {
            var propertyType = type.GetProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.InvariantCultureIgnoreCase))?.PropertyType;

            if (propertyType == null)
            {
                throw new InvalidDataException($"Property [{propertyName}] not found on class [{type.Name}].");
            }

            var valueType = typeof(TValue);

            if (propertyType != valueType)
            {
                throw new InvalidDataException($"Property [{propertyName}] of type [{propertyType.Name}] cannot take value {(value != null ? $"[{value}] " : string.Empty)}of type [{valueType.Name}].");
            }
        }

        private static MethodInfo ValidateMethod(Type type, string methodName)
        {
            var method = type.GetMethod(methodName);

            if (method == null)
            {
                throw new InvalidOperationException($"Method [{methodName}] not found on type [{type.Name}].");
            }

            return method;
        }
    }
}