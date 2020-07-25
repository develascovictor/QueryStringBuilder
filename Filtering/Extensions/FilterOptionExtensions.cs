using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Operations;
using Expressions;
using Filtering.Constants;
using Filtering.Exceptions;
using Filtering.Helpers;

namespace Filtering.Extensions
{
    public static class FilterOptionExtensions
    {
        public static Expression<Func<T, bool>> GetFilterExpression<T>(string filter, IReadOnlyDictionary<string, string> whitelist = null, bool disableTrim = false) where T : class
        {
            whitelist = whitelist ?? new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                return x => true;
            }

            var filters = filter.Split(new[] { LogicalOperators.AndOperator }, StringSplitOptions.RemoveEmptyEntries);
            var expression = ResolveFilters<T>(filters, whitelist, disableTrim);

            return expression;
        }

        private static Expression<Func<T, bool>> ResolveFilters<T>(IEnumerable<string> filters, IReadOnlyDictionary<string, string> whitelist, bool disableTrim) where T : class
        {
            var list = new List<Expression<Func<T, bool>>>();

            foreach (var filter in filters)
            {
                var filterExp = new Filter<T>();

                if (filter.Contains(LogicalOperators.OrOperator))
                {
                    AddOrFilter<T>(filterExp, filter, disableTrim, whitelist);
                }

                else
                {
                    AddFilter<T>(filterExp, filter, disableTrim, false, whitelist);
                }

                list.Add(filterExp);
            }

            if (!list.Any())
            {
                return new Filter<T>();
            }

            var expression = list.First();

            for (var i = 1; i < list.Count; i++)
            {
                expression = expression.And(list[i]);
            }

            return expression;
        }

        private static void AddOrFilter<T>(IFilter filter, string filterString, bool disableTrim, IReadOnlyDictionary<string, string> whitelist) where T : class
        {
            var filters = filterString.Split(new[] { LogicalOperators.OrOperator }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var f in filters)
            {
                AddFilter<T>(filter, f, disableTrim, true, whitelist);
            }
        }

        private static void AddFilter<T>(IFilter filter, string filterString, bool disableTrim, bool useOr, IReadOnlyDictionary<string, string> whitelist) where T : class
        {
            var paramArray = GetParams(filterString);
            AddFilterByParams<T>(filter, paramArray, useOr, disableTrim, whitelist);
        }

        private static void AddFilterByParams<T>(IFilter filter, IList<string> paramArray, bool useOr, bool disableTrim, IReadOnlyDictionary<string, string> whitelist) where T : class
        {
            var propertyName = paramArray[0];
            var propertyValue = paramArray[1];
            var operatorType = paramArray[2];
            var filterStatementConnector = useOr ? Connector.Or : Connector.And;

            var entityPropertyName = propertyName;

            if (whitelist.Any())
            {
                if (!whitelist.TryGetValue(propertyName, out entityPropertyName))
                {
                    throw new UnsupportedFilterPropertyException(propertyName, typeof(T));
                }
            }

            //TODO: Verify if this is reachable
            if (string.IsNullOrWhiteSpace(entityPropertyName))
            {
                throw new EntityPropertyNameNotDefinedException(propertyName);
            }

            var unescapedPropertyValue = propertyValue != null ? Uri.UnescapeDataString(propertyValue) : null;

            switch (operatorType)
            {
                case FilterOperators.EqualsOperator:
                    {
                        AddEqualOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, disableTrim ? CustomExpressionOperations.EqualToNoTrim : Operation.EqualTo, filterStatementConnector);
                        break;
                    }

                case FilterOperators.ContainsOperator:
                    {
                        AddContainOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, disableTrim ? CustomExpressionOperations.ContainsNoTrim : Operation.Contains, filterStatementConnector);
                        break;
                    }

                case FilterOperators.NotEqualToOperator:
                    {
                        AddEqualOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, disableTrim ? CustomExpressionOperations.NotEqualToNoTrim : Operation.NotEqualTo, filterStatementConnector);
                        break;
                    }

                case FilterOperators.NotContainsOperator:
                    {
                        AddContainOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, disableTrim ? CustomExpressionOperations.DoesNotContainNoTrim : Operation.DoesNotContain, filterStatementConnector);
                        break;
                    }

                case FilterOperators.GreaterThanOperator:
                    {
                        AddRangeOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, Operation.GreaterThan, filterStatementConnector);
                        break;
                    }

                case FilterOperators.LessThanOperator:
                    {
                        AddRangeOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, Operation.LessThan, filterStatementConnector);
                        break;
                    }

                case FilterOperators.GreaterThanOrEqualToOperator:
                    {
                        AddRangeOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, Operation.GreaterThanOrEqualTo, filterStatementConnector);
                        break;
                    }

                case FilterOperators.LessThanOrEqualToOperator:
                    {
                        AddRangeOperationFilter<T>(filter, entityPropertyName, unescapedPropertyValue, Operation.LessThanOrEqualTo, filterStatementConnector);
                        break;
                    }

                default:
                    {
                        //TODO: Verify if this is unreachable
                        throw new InvalidDataException($"The filter operator [{operatorType}] is invalid.");
                    }
            }
        }

        private static void AddEqualOperationFilter<T>(IFilter filter, string propertyName, string propertyValue, IOperation operation, Connector filterStatementConnector) where T : class
        {
            if (!Equals(operation, Operation.EqualTo) && !Equals(operation, CustomExpressionOperations.EqualToNoTrim) && !Equals(operation, Operation.NotEqualTo) && !Equals(operation, CustomExpressionOperations.NotEqualToNoTrim))
            {
                throw new ArgumentException($"Operation [{operation}] not supported on AddEqualOperationFilter.");
            }

            dynamic filterValue1;

            if (propertyValue == null)
            {
                if (Equals(operation, Operation.EqualTo))
                {
                    operation = Operation.IsNull;
                }

                else if (Equals(operation, CustomExpressionOperations.EqualToNoTrim))
                {
                    operation = CustomExpressionOperations.IsNullNoTrim;
                }

                else if (Equals(operation, CustomExpressionOperations.NotEqualToNoTrim))
                {
                    operation = CustomExpressionOperations.IsNotNullNoTrim;
                }

                else
                {
                    operation = Operation.IsNotNull;
                }
            }

            var propertyType = propertyName.GetPropertyType<T>();

            if (propertyType == typeof(string))
            {
                filterValue1 = propertyValue;

                if (Equals(operation, Operation.IsNull))
                {
                    operation = Operation.IsNullOrWhiteSpace;
                }

                else if (Equals(operation, Operation.IsNotNull))
                {
                    operation = Operation.IsNotNullNorWhiteSpace;
                }

                else if (Equals(operation, CustomExpressionOperations.IsNullNoTrim))
                {
                    operation = CustomExpressionOperations.IsNullOrEmpty;
                }

                else if (Equals(operation, CustomExpressionOperations.IsNotNullNoTrim))
                {
                    operation = CustomExpressionOperations.IsNotNullNorEmpty;
                }

                filter.By<string>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(bool))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Bool);
                filter.By<bool>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(bool?))
            {
                filterValue1 = GetNullableFilterValue(propertyValue, TryParseDelegates.Delegates.Bool);
                filter.By<bool?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(short))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Short);
                filter.By<short>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(short?))
            {
                filterValue1 = GetNullableFilterValue(propertyValue, TryParseDelegates.Delegates.Short);
                filter.By<short?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(int))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Int);
                filter.By<int>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(int?))
            {
                filterValue1 = GetNullableFilterValue(propertyValue, TryParseDelegates.Delegates.Int);
                filter.By<int?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(long))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Long);
                filter.By<long>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(long?))
            {
                filterValue1 = GetNullableFilterValue(propertyValue, TryParseDelegates.Delegates.Long);
                filter.By<long?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(decimal))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Decimal);
                filter.By<decimal>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(decimal?))
            {
                filterValue1 = GetNullableFilterValue(propertyValue, TryParseDelegates.Delegates.Decimal);
                filter.By<decimal?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(DateTime))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.DateTime).Date;

                if (Equals(operation, Operation.EqualTo))
                {
                    var filterValue2 = filterValue1.AddDays(1).AddMilliseconds(-1);
                    filter.By<DateTime>(propertyName, Operation.Between, filterValue1, filterValue2, filterStatementConnector);
                }

                else
                {
                    var filterValue2 = filterValue1.AddDays(1);
                    filter.By<DateTime>(propertyName, Operation.LessThan, filterValue1, filterStatementConnector);
                    filter.By<DateTime>(propertyName, Operation.GreaterThanOrEqualTo, filterValue2, filterStatementConnector);
                }
            }

            else if (propertyType == typeof(DateTime?))
            {
                var nullableDateTime = GetNullableFilterValue(propertyValue, TryParseDelegates.Delegates.DateTime)?.Date;

                if (nullableDateTime != null)
                {
                    filterValue1 = (DateTime)nullableDateTime;

                    if (Equals(operation, Operation.EqualTo))
                    {
                        var filterValue2 = filterValue1.AddDays(1).AddMilliseconds(-1);
                        filter.By<DateTime>(propertyName, Operation.Between, filterValue1, filterValue2, filterStatementConnector);
                    }

                    else
                    {
                        var filterValue2 = filterValue1.AddDays(1);
                        filter.By<DateTime>(propertyName, Operation.LessThan, filterValue1, filterStatementConnector);
                        filter.By<DateTime>(propertyName, Operation.GreaterThanOrEqualTo, filterValue2, filterStatementConnector);
                    }
                }

                else
                {
                    filter.By(propertyName, operation, filterStatementConnector);
                }
            }

            else
            {
                throw new UnsupportedTypeOnOperationException(propertyType, operation);
            }
        }

        private static void AddContainOperationFilter<T>(IFilter filter, string propertyName, string propertyValue, IOperation operation, Connector filterStatementConnector) where T : class
        {
            if (!Equals(operation, Operation.Contains) && !Equals(operation, CustomExpressionOperations.ContainsNoTrim) && !Equals(operation, Operation.DoesNotContain) && !Equals(operation, CustomExpressionOperations.DoesNotContainNoTrim))
            {
                throw new ArgumentException($"Operation [{operation}] not supported on AddContainOperationFilter.");
            }

            var propertyType = propertyName.GetPropertyType<T>();

            if (propertyType == typeof(string))
            {
                filter.By(propertyName, operation, propertyValue, filterStatementConnector);
            }

            else
            {
                throw new UnsupportedTypeOnOperationException(propertyType, operation);
            }
        }

        private static void AddRangeOperationFilter<T>(IFilter filter, string propertyName, string propertyValue, IOperation operation, Connector filterStatementConnector) where T : class
        {
            if (!Equals(operation, Operation.GreaterThan) && !Equals(operation, Operation.LessThan) && !Equals(operation, Operation.GreaterThanOrEqualTo) && !Equals(operation, Operation.LessThanOrEqualTo))
            {
                throw new ArgumentException($"Operation [{operation}] not supported on AddRangeOperationFilter.");
            }

            dynamic filterValue1;
            var propertyType = propertyName.GetPropertyType<T>();

            if (propertyType == typeof(short) || propertyType == typeof(short?))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Short);
                filter.By<short?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(int) || propertyType == typeof(int?))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Int);
                filter.By<int?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(long) || propertyType == typeof(long?))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Long);
                filter.By<long?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
            {
                filterValue1 = GetFilterValue(propertyValue, TryParseDelegates.Delegates.Decimal);
                filter.By<decimal?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                var date = GetFilterValue(propertyValue, TryParseDelegates.Delegates.DateTime).Date;

                if (Equals(operation, Operation.GreaterThan) || Equals(operation, Operation.LessThanOrEqualTo))
                {
                    filterValue1 = date.AddDays(1).AddMilliseconds(-1);
                }

                else
                {
                    filterValue1 = date;
                }

                filter.By<DateTime?>(propertyName, operation, filterValue1, filterStatementConnector);
            }

            else
            {
                throw new UnsupportedTypeOnOperationException(propertyType, operation);
            }
        }

        private static TType GetFilterValue<TType>(string propertyValue, TryParseDelegates.TryParse<TType> tryParse) where TType : struct
        {
            if (propertyValue == null)
            {
                throw new UnsupportedNullValueException(typeof(TType));
            }

            var valid = tryParse(propertyValue, out var value);

            if (!valid)
            {
                throw new TypeParseException(propertyValue, typeof(TType));
            }

            return value;
        }

        private static TType? GetNullableFilterValue<TType>(string propertyValue, TryParseDelegates.TryParse<TType> tryParse) where TType : struct
        {
            if (propertyValue == null)
            {
                return null;
            }

            var valid = tryParse(propertyValue, out var value);

            if (!valid)
            {
                throw new TypeParseException(propertyValue, typeof(TType));
            }

            return value;
        }

        private static IList<string> GetParams(string filter)
        {
            string operation;

            if (filter.Contains(FilterOperators.EqualsOperator))
            {
                operation = FilterOperators.EqualsOperator;
            }

            else if (filter.Contains(FilterOperators.ContainsOperator))
            {
                operation = FilterOperators.ContainsOperator;
            }

            else if (filter.Contains(FilterOperators.NotEqualToOperator))
            {
                operation = FilterOperators.NotEqualToOperator;
            }

            else if (filter.Contains(FilterOperators.NotContainsOperator))
            {
                operation = FilterOperators.NotContainsOperator;
            }

            else if (filter.Contains(FilterOperators.GreaterThanOrEqualToOperator))
            {
                operation = FilterOperators.GreaterThanOrEqualToOperator;
            }

            else if (filter.Contains(FilterOperators.LessThanOrEqualToOperator))
            {
                operation = FilterOperators.LessThanOrEqualToOperator;
            }

            else if (filter.Contains(FilterOperators.GreaterThanOperator))
            {
                operation = FilterOperators.GreaterThanOperator;
            }

            else if (filter.Contains(FilterOperators.LessThanOperator))
            {
                operation = FilterOperators.LessThanOperator;
            }

            else
            {
                throw new UnsupportedOperatorException(filter);
            }

            var paramArray = (IList<string>)filter.Split(new[] { operation }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //Only EqualsOperator and NotEqualToOperator are allowed to have a null property value
            var valid = !filter.Trim().StartsWith(operation, StringComparison.Ordinal) && paramArray.Count == 1 && (operation == FilterOperators.EqualsOperator || operation == FilterOperators.NotEqualToOperator) || paramArray.Count == 2;

            if (!valid)
            {
                throw new MissingValueOnOperatorException(operation, filter);
            }

            if (paramArray.Count == 1)
            {
                paramArray.Add(null);
            }

            paramArray.Add(operation);
            return paramArray;
        }
    }
}