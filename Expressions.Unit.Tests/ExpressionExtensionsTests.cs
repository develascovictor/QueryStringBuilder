using Expressions.Unit.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Expressions.Unit.Tests
{
    [TestFixture]
    public class ExpressionExtensionsTests
    {
        [TestCaseSource(nameof(TestCreateEqualExpression))]
        public void ShouldCreateEqualExpression(ICreateEqualExpressionTester tester)
        {
            tester.RunCreateEqualExpression();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(nameof(Opportunity.Account) + " ")]
        [TestCase(nameof(Opportunity.Account) + "!")]
        public void ShouldCreateEqualExpressionThrowInvalidDataExceptionOnInvalidPropertyName(string propertyName)
        {
            var exceptionMessage = GetPropertyExceptionMessage(propertyName, nameof(Opportunity));
            var invalidDataException = Assert.Catch<InvalidDataException>(() => ExpressionExtensions.CreateEqualExpression<Opportunity, string>(propertyName, null));
            Assert.IsNotNull(invalidDataException);
            Assert.AreEqual(invalidDataException.Message, exceptionMessage);
        }

        [TestCaseSource(nameof(TestCreateEqualExpressionThrowInvalidDataExceptionOnInvalidValue))]
        public void ShouldCreateEqualExpressionThrowInvalidDataExceptionOnInvalidValue(IInvalidValueTester tester)
        {
            tester.RunCreateExpressionThrowInvalidDataExceptionOnInvalidPropertyName();
        }

        [TestCaseSource(nameof(TestCreateReturnExpression))]
        public void ShouldCreateReturnExpression(ICreateReturnExpressionTester tester)
        {
            tester.RunCreateReturnExpressionTester();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(nameof(Opportunity.Account) + " ")]
        [TestCase(nameof(Opportunity.Account) + "!")]
        public void ShouldCreateReturnExpressionThrowInvalidDataExceptionOnInvalidPropertyName(string propertyName)
        {
            var exceptionMessage = GetPropertyExceptionMessage(propertyName, nameof(Opportunity));
            var invalidDataException = Assert.Catch<InvalidDataException>(() => ExpressionExtensions.CreateReturnExpression<Opportunity, string>(propertyName));
            Assert.IsNotNull(invalidDataException);
            Assert.AreEqual(invalidDataException.Message, exceptionMessage);
        }

        [TestCaseSource(nameof(TestCreateReturnExpressionThrowInvalidDataExceptionOnInvalidValue))]
        public void ShouldCreateReturnExpressionThrowInvalidDataExceptionOnInvalidValue(IInvalidValueTester tester)
        {
            tester.RunCreateExpressionThrowInvalidDataExceptionOnInvalidPropertyName();
        }

        [TestCaseSource(nameof(TestCreateIsNullOrWhiteSpaceExpression))]
        public void ShouldCreateIsNullOrWhiteSpaceExpression(ICreateIsNullOrWhiteSpaceExpressionTester tester)
        {
            tester.RunCreateIsNullOrWhiteSpaceExpressionTester();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(nameof(Opportunity.Account) + " ")]
        [TestCase(nameof(Opportunity.Account) + "!")]
        public void ShouldCreateIsNullOrWhiteSpaceExpressionThrowInvalidDataExceptionOnInvalidPropertyName(string propertyName)
        {
            var exceptionMessage = GetPropertyExceptionMessage(propertyName, nameof(Opportunity));
            var invalidDataException = Assert.Catch<InvalidDataException>(() => ExpressionExtensions.CreateIsNullOrWhiteSpaceExpression<Opportunity>(propertyName, true));
            Assert.IsNotNull(invalidDataException);
            Assert.AreEqual(invalidDataException.Message, exceptionMessage);
        }

        [TestCaseSource(nameof(TestCreateIsNullOrWhiteSpaceExpressionThrowInvalidDataExceptionOnInvalidValue))]
        public void ShouldCreateIsNullOrWhiteSpaceExpressionThrowInvalidDataExceptionOnInvalidValue(IInvalidValueTester tester)
        {
            tester.RunCreateExpressionThrowInvalidDataExceptionOnInvalidPropertyName();
        }

        [TestCaseSource(nameof(TestCasesAnd))]
        public void ShouldConcatenateAnd(IConcatenateTester tester)
        {
            tester.RunAnd();
        }

        [TestCaseSource(nameof(TestCasesOr))]
        public void ShouldConcatenateOr(IConcatenateTester tester)
        {
            tester.RunOr();
        }

        #region Test Cases
        public static IEnumerable<ICreateEqualExpressionTester> TestCreateEqualExpression()
        {
            yield return new CreateEqualExpressionTester<Opportunity, string>
            {
                PropertyName = nameof(Opportunity.ContactName).ToLower(),
                Value = "foo",
                ParameterName = "opp",
                ExpectedExpression = opp => opp.ContactName == "foo"
            };
            yield return new CreateEqualExpressionTester<Opportunity, string>
            {
                PropertyName = nameof(Opportunity.ContactName),
                Value = "bar",
                ParameterName = null,
                ExpectedExpression = x => x.ContactName == "bar"
            };
            yield return new CreateEqualExpressionTester<Opportunity, long>
            {
                PropertyName = nameof(Opportunity.ManagerId).ToUpper(),
                Value = 1,
                ParameterName = "y",
                ExpectedExpression = y => y.ManagerId == 1
            };
            yield return new CreateEqualExpressionTester<Opportunity, int?>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId),
                Value = 1,
                ParameterName = "z",
                ExpectedExpression = GetExpression<Opportunity, int?>(nameof(Opportunity.AssignedEmployeeId), 1, "z")
            };
            yield return new CreateEqualExpressionTester<Opportunity, int?>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId),
                Value = null,
                ParameterName = "x",
                ExpectedExpression = x => x.AssignedEmployeeId == null
            };
            yield return new CreateEqualExpressionTester<Opportunity, DateTime?>
            {
                PropertyName = nameof(Opportunity.ExpirationDate),
                Value = new DateTime(2019, 3, 18, 14, 13, 23),
                ParameterName = "bar",
                ExpectedExpression = GetExpression<Opportunity, DateTime?>(nameof(Opportunity.ExpirationDate), new DateTime(2019, 3, 18, 14, 13, 23), "bar")
            };
            yield return new CreateEqualExpressionTester<Opportunity, DateTime?>
            {
                PropertyName = nameof(Opportunity.ExpirationDate),
                Value = null,
                ParameterName = "var",
                ExpectedExpression = var => var.ExpirationDate == null
            };
            yield return new CreateEqualExpressionTester<Opportunity, bool>
            {
                PropertyName = nameof(Opportunity.Excluded),
                Value = false,
                ParameterName = "param",
                ExpectedExpression = param => param.Excluded == false
            };
        }

        public static IEnumerable<IInvalidValueTester> TestCreateEqualExpressionThrowInvalidDataExceptionOnInvalidValue()
        {
            yield return new CreateEqualExpressionInvalidValueTester<string, int>
            {
                PropertyName = nameof(Opportunity.ContactName),
                Value = 1
            };
            yield return new CreateEqualExpressionInvalidValueTester<long, bool>
            {
                PropertyName = nameof(Opportunity.ManagerId),
                Value = true
            };
            yield return new CreateEqualExpressionInvalidValueTester<int?, string>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId),
                Value = "foo"
            };
            yield return new CreateEqualExpressionInvalidValueTester<DateTime?, long>
            {
                PropertyName = nameof(Opportunity.ExpirationDate),
                Value = 3
            };
            yield return new CreateEqualExpressionInvalidValueTester<bool, DateTime>
            {
                PropertyName = nameof(Opportunity.Excluded),
                Value = new DateTime(2019, 3, 18, 16, 48, 39)
            };
            yield return new CreateEqualExpressionInvalidValueTester<int?, int>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId),
                Value = 4
            };
            yield return new CreateEqualExpressionInvalidValueTester<DateTime?, DateTime>
            {
                PropertyName = nameof(Opportunity.ExpirationDate),
                Value = new DateTime(2019, 3, 18, 16, 53, 1)
            };
            yield return new CreateEqualExpressionInvalidValueTester<long, int>
            {
                PropertyName = nameof(Opportunity.ManagerId),
                Value = 5
            };
        }

        public static IEnumerable<ICreateReturnExpressionTester> TestCreateReturnExpression()
        {
            yield return new CreateReturnExpressionTester<Opportunity, string>
            {
                PropertyName = nameof(Opportunity.ContactName).ToLower(),
                ParameterName = "opp",
                ExpectedExpression = opp => opp.ContactName
            };
            yield return new CreateReturnExpressionTester<Opportunity, string>
            {
                PropertyName = nameof(Opportunity.ContactName),
                ParameterName = null,
                ExpectedExpression = x => x.ContactName
            };
            yield return new CreateReturnExpressionTester<Opportunity, long>
            {
                PropertyName = nameof(Opportunity.ManagerId).ToUpper(),
                ParameterName = "y",
                ExpectedExpression = y => y.ManagerId
            };
            yield return new CreateReturnExpressionTester<Opportunity, int?>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId),
                ParameterName = "z",
                ExpectedExpression = z => z.AssignedEmployeeId
            };
            yield return new CreateReturnExpressionTester<Opportunity, DateTime?>
            {
                PropertyName = nameof(Opportunity.ExpirationDate),
                ParameterName = "var",
                ExpectedExpression = var => var.ExpirationDate
            };
            yield return new CreateReturnExpressionTester<Opportunity, bool>
            {
                PropertyName = nameof(Opportunity.Excluded),
                ParameterName = "param",
                ExpectedExpression = param => param.Excluded
            };
        }

        public static IEnumerable<IInvalidValueTester> TestCreateReturnExpressionThrowInvalidDataExceptionOnInvalidValue()
        {
            yield return new CreateReturnExpressionInvalidValueTester<string, int>
            {
                PropertyName = nameof(Opportunity.ContactName)
            };
            yield return new CreateReturnExpressionInvalidValueTester<long, bool>
            {
                PropertyName = nameof(Opportunity.ManagerId)
            };
            yield return new CreateReturnExpressionInvalidValueTester<int?, string>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId)
            };
            yield return new CreateReturnExpressionInvalidValueTester<DateTime?, long>
            {
                PropertyName = nameof(Opportunity.ExpirationDate)
            };
            yield return new CreateReturnExpressionInvalidValueTester<bool, DateTime>
            {
                PropertyName = nameof(Opportunity.Excluded)
            };
            yield return new CreateReturnExpressionInvalidValueTester<int?, int>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId)
            };
            yield return new CreateReturnExpressionInvalidValueTester<DateTime?, DateTime>
            {
                PropertyName = nameof(Opportunity.ExpirationDate)
            };
            yield return new CreateReturnExpressionInvalidValueTester<long, int>
            {
                PropertyName = nameof(Opportunity.ManagerId)
            };
        }

        public static IEnumerable<ICreateIsNullOrWhiteSpaceExpressionTester> TestCreateIsNullOrWhiteSpaceExpression()
        {
            yield return new CreateIsNullOrWhiteSpaceExpressionTester<Opportunity>
            {
                PropertyName = nameof(Opportunity.ContactName).ToLower(),
                ParameterName = "opp",
                ComparisonValue = true,
                ExpectedExpression = opp => string.IsNullOrWhiteSpace(opp.ContactName) == true
            };
            yield return new CreateIsNullOrWhiteSpaceExpressionTester<Opportunity>
            {
                PropertyName = nameof(Opportunity.ContactName),
                ParameterName = null,
                ComparisonValue = true,
                ExpectedExpression = x => string.IsNullOrWhiteSpace(x.ContactName) == true
            };
            yield return new CreateIsNullOrWhiteSpaceExpressionTester<Opportunity>
            {
                PropertyName = nameof(Opportunity.ContactEmail),
                ParameterName = "var",
                ComparisonValue = false,
                ExpectedExpression = var => string.IsNullOrWhiteSpace(var.ContactEmail) == false
            };
            yield return new CreateIsNullOrWhiteSpaceExpressionTester<Opportunity>
            {
                PropertyName = nameof(Opportunity.ContactEmail),
                ParameterName = "param",
                ComparisonValue = false,
                ExpectedExpression = param => string.IsNullOrWhiteSpace(param.ContactEmail) == false
            };
        }

        public static IEnumerable<IInvalidValueTester> TestCreateIsNullOrWhiteSpaceExpressionThrowInvalidDataExceptionOnInvalidValue()
        {
            yield return new CreateReturnExpressionInvalidValueTester<long, bool>
            {
                PropertyName = nameof(Opportunity.ManagerId)
            };
            yield return new CreateReturnExpressionInvalidValueTester<int?, string>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId)
            };
            yield return new CreateReturnExpressionInvalidValueTester<DateTime?, long>
            {
                PropertyName = nameof(Opportunity.ExpirationDate)
            };
            yield return new CreateReturnExpressionInvalidValueTester<bool, DateTime>
            {
                PropertyName = nameof(Opportunity.Excluded)
            };
            yield return new CreateReturnExpressionInvalidValueTester<int?, int>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId)
            };
            yield return new CreateReturnExpressionInvalidValueTester<DateTime?, DateTime>
            {
                PropertyName = nameof(Opportunity.ExpirationDate)
            };
            yield return new CreateReturnExpressionInvalidValueTester<long, int>
            {
                PropertyName = nameof(Opportunity.ManagerId)
            };
        }

        public static IEnumerable<IConcatenateTester> TestCasesAnd()
        {
            yield return new ConcatenateTester<string>
            {
                Expressions = new List<Expression<Func<string, bool>>>
                {
                    x => !string.IsNullOrWhiteSpace(x),
                    y => y.Length == 3
                },
                ExpectedExpression = x => !string.IsNullOrWhiteSpace(x) && x.Length == 3
            };
            yield return new ConcatenateTester<int?>
            {
                Expressions = new List<Expression<Func<int?, bool>>>
                {
                    x => x != null,
                    y => y >= -10,
                    z => z <= 10
                },
                ExpectedExpression = x => x != null && x >= -10 && x <= 10
            };
            yield return new ConcatenateTester<bool?>
            {
                Expressions = new List<Expression<Func<bool?, bool>>>
                {
                    x => x != null,
                    y => y != true
                },
                ExpectedExpression = x => x != null && x != true
            };
            yield return new ConcatenateTester<DateTime>
            {
                Expressions = new List<Expression<Func<DateTime, bool>>>
                {
                    x => x.Year == 2019,
                    y => y.Month >= 1,
                    z => z.Month < 4
                },
                ExpectedExpression = x => x.Year == 2019 && x.Month >= 1 && x.Month < 4
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => x != null,
                    y => y.Id > 0,
                    y => y.Active
                },
                ExpectedExpression = x => x != null && x.Id > 0 && x.Active
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => true,
                    y => y != null
                },
                ExpectedExpression = x => x != null
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => x != null,
                    y => true
                },
                ExpectedExpression = x => x != null
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => true,
                    y => true
                },
                ExpectedExpression = x => true
            };
        }

        public static IEnumerable<IConcatenateTester> TestCasesOr()
        {
            yield return new ConcatenateTester<string>
            {
                Expressions = new List<Expression<Func<string, bool>>>
                {
                    x => string.IsNullOrWhiteSpace(x),
                    y => y.Length > 3
                },
                ExpectedExpression = x => string.IsNullOrWhiteSpace(x) || x.Length > 3
            };
            yield return new ConcatenateTester<int?>
            {
                Expressions = new List<Expression<Func<int?, bool>>>
                {
                    x => x == null,
                    y => y.ToString().Contains("1")
                },
                ExpectedExpression = x => x == null || x.ToString().Contains("1")
            };
            yield return new ConcatenateTester<bool?>
            {
                Expressions = new List<Expression<Func<bool?, bool>>>
                {
                    x => x == null,
                    y => y == false
                },
                ExpectedExpression = x => x == null || x == false
            };
            yield return new ConcatenateTester<DateTime>
            {
                Expressions = new List<Expression<Func<DateTime, bool>>>
                {
                    x => x.Year == 2019,
                    y => y.Month == 3
                },
                ExpectedExpression = x => x.Year == 2019 || x.Month == 3
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => x == null,
                    y => new List<long>{ 2, 3 }.Contains(y.Id),
                    z => !z.Active
                },
                ExpectedExpression = x => x == null || new List<long> { 2, 3 }.Contains(x.Id) || !x.Active
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => true,
                    y => y != null
                },
                ExpectedExpression = x => true || x != null
            };
            yield return new ConcatenateTester<Account>
            {
                Expressions = new List<Expression<Func<Account, bool>>>
                {
                    x => x != null,
                    y => true
                },
                ExpectedExpression = x => x != null || true
            };
        }
        #endregion

        #region Tester Interfaces
        public interface ICreateEqualExpressionTester
        {
            void RunCreateEqualExpression();
        }

        public interface ICreateReturnExpressionTester
        {
            void RunCreateReturnExpressionTester();
        }

        public interface ICreateIsNullOrWhiteSpaceExpressionTester
        {
            void RunCreateIsNullOrWhiteSpaceExpressionTester();
        }

        public interface IInvalidValueTester
        {
            void RunCreateExpressionThrowInvalidDataExceptionOnInvalidPropertyName();
        }

        public interface IConcatenateTester
        {
            void RunAnd();
            void RunOr();
        }
        #endregion

        #region Tester Classes
        private sealed class CreateEqualExpressionTester<TClass, TProperty> : Validator<TClass, bool>, ICreateEqualExpressionTester
            where TClass : class
        {
            public string PropertyName { private get; set; }

            public dynamic Value { private get; set; }

            public string ParameterName { private get; set; }

            public void RunCreateEqualExpression()
            {
                var expression = ExpressionExtensions.CreateEqualExpression<TClass, TProperty>(PropertyName, Value, ParameterName);
                ValidateExpression(expression);
            }
        }

        private sealed class CreateEqualExpressionInvalidValueTester<TProperty, TValue> : CreateExpressionInvalidValueTester<TProperty, TValue>, IInvalidValueTester
        {
            protected override TestDelegate RunCreateExpression() => () => ExpressionExtensions.CreateEqualExpression<Opportunity, TValue>(PropertyName, Value);
        }

        private sealed class CreateReturnExpressionTester<TClass, TProperty> : Validator<TClass, TProperty>, ICreateReturnExpressionTester
            where TClass : class
        {
            public string PropertyName { private get; set; }

            public string ParameterName { private get; set; }

            public void RunCreateReturnExpressionTester()
            {
                var expression = ExpressionExtensions.CreateReturnExpression<TClass, TProperty>(PropertyName, ParameterName);
                ValidateExpression(expression);
            }
        }

        private sealed class CreateReturnExpressionInvalidValueTester<TProperty, TValue> : CreateExpressionInvalidValueTester<TProperty, TValue>, IInvalidValueTester
        {
            protected override TestDelegate RunCreateExpression() => () => ExpressionExtensions.CreateReturnExpression<Opportunity, TValue>(PropertyName);
        }

        private sealed class CreateIsNullOrWhiteSpaceExpressionTester<TClass> : Validator<TClass, bool>, ICreateIsNullOrWhiteSpaceExpressionTester
            where TClass : class
        {
            public string PropertyName { private get; set; }

            public string ParameterName { private get; set; }

            public bool ComparisonValue { private get; set; }

            public void RunCreateIsNullOrWhiteSpaceExpressionTester()
            {
                var expression = ExpressionExtensions.CreateIsNullOrWhiteSpaceExpression<TClass>(PropertyName, ComparisonValue, ParameterName);
                ValidateExpression(expression);
            }
        }

        private sealed class CreateIsNullOrWhiteSpaceExpressionInvalidValueTester<TProperty, TValue> : CreateExpressionInvalidValueTester<TProperty, TValue>, IInvalidValueTester
        {
            protected override TestDelegate RunCreateExpression() => () => ExpressionExtensions.CreateIsNullOrWhiteSpaceExpression<Opportunity>(PropertyName, true);
        }

        private abstract class CreateExpressionInvalidValueTester<TProperty, TValue>
        {
            private TValue _value;
            private bool _valueWasSet;

            public string PropertyName { protected get; set; }

            public TValue Value
            {
                protected get => _value;
                set
                {
                    _value = value;
                    _valueWasSet = true;
                }
            }

            private static bool IsNullable
            {
                get
                {
                    var type = typeof(TValue);
                    var nullableType = Nullable.GetUnderlyingType(type);

                    return nullableType != null;
                }
            }

            private object ValueAsObject => IsNullable ? Value : (_valueWasSet ? (object)Value : null);

            public void RunCreateExpressionThrowInvalidDataExceptionOnInvalidPropertyName()
            {
                var propertyType = typeof(Opportunity).GetProperties().FirstOrDefault(p => string.Equals(p.Name, PropertyName, StringComparison.InvariantCultureIgnoreCase))?.PropertyType;
                Assert.IsNotNull(propertyType);

                var suggestedPropertyType = typeof(TProperty);
                Assert.AreEqual(propertyType, suggestedPropertyType);

                var valueType = typeof(TValue);
                Assert.AreNotEqual(propertyType, valueType);

                var exceptionMessage = GetValueExceptionMessage(PropertyName, propertyType.Name, valueType.Name, ValueAsObject);
                var invalidDataException = Assert.Catch<InvalidDataException>(RunCreateExpression());
                Assert.IsNotNull(invalidDataException);
                Assert.AreEqual(invalidDataException.Message, exceptionMessage);
            }

            protected abstract TestDelegate RunCreateExpression();
        }

        private sealed class ConcatenateTester<T> : Validator<T, bool>, IConcatenateTester
        {
            /// <summary>
            /// Test case
            /// </summary>
            public IList<Expression<Func<T, bool>>> Expressions { private get; set; }

            public void RunAnd()
            {
                RunTest((current, exp) => current.And(exp));
            }

            public void RunOr()
            {
                RunTest((current, exp) => current.Or(exp));
            }

            private void RunTest(Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> func)
            {
                ValidateProperties();

                var expression = Expressions.Aggregate((Expression<Func<T, bool>>)null, (current, exp) => current == null ? exp : func(current, exp));
                ValidateExpression(expression);
            }

            private void ValidateProperties()
            {
                Assert.IsNotEmpty(Expressions);
                Assert.IsTrue(Expressions.Count >= 2);

                Assert.IsNotNull(ExpectedExpression);
            }
        }

        private abstract class Validator<T, TType>
        {
            public Expression<Func<T, TType>> ExpectedExpression { protected get; set; }

            protected void ValidateExpression(Expression<Func<T, TType>> expression)
            {
                Assert.IsNotNull(expression);
                Assert.IsNotEmpty(expression.Parameters);
                Assert.AreEqual(expression.Parameters.Count, 1);

                var parameter = expression.Parameters.First();
                var expectedParameter = ExpectedExpression.Parameters.First();
                Assert.AreEqual(parameter.Name, expectedParameter.Name);
                Assert.AreEqual(parameter.Type, expectedParameter.Type);

                var expressionString = expression.ToString();
                var expectedExpressionString = ExpectedExpression.ToString();
                Assert.AreEqual(expression.Name, ExpectedExpression.Name);
                Assert.AreEqual(expression.ReturnType, ExpectedExpression.ReturnType);
                Assert.AreEqual(expression.Type, ExpectedExpression.Type);
                Assert.AreEqual(expressionString, expectedExpressionString);
            }
        }
        #endregion

        private static Expression<Func<TClass, bool>> GetExpression<TClass, TValue>(string propertyName, TValue value, string parameterName)
        {
            var parameter = Expression.Parameter(typeof(TClass), !string.IsNullOrWhiteSpace(parameterName) ? parameterName : "x");
            var member = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value, typeof(TValue));
            var predicate = Expression.Equal(member, constant);
            var expression = Expression.Lambda<Func<TClass, bool>>(predicate, parameter);

            return expression;
        }

        private static string GetPropertyExceptionMessage(string propertyName, string className) => $"Property [{propertyName}] not found on class [{className}].";

        private static string GetValueExceptionMessage(string propertyName, string typeName, string valueTypeName, object value = null) => $"Property [{propertyName}] of type [{typeName}] cannot take value {(value != null ? $"[{value}] " : string.Empty)}of type [{valueTypeName}].";
    }
}