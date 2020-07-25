using Filtering.Extensions;
using Filtering.Unit.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Filtering.Unit.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void ShouldCreateEqualExpression(ITester tester)
        {
            tester.Run();
        }

        #region Test Cases
        public static IEnumerable<ITester> TestCases()
        {
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.Account),
                ExpectedType = typeof(Account)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.Id),
                ExpectedType = typeof(long)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.ContactName),
                ExpectedType = typeof(string)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.ExpirationDate),
                ExpectedType = typeof(DateTime?)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.DollarValue),
                ExpectedType = typeof(decimal?)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.AssignedEmployeeId),
                ExpectedType = typeof(int?)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.MessageId),
                ExpectedType = typeof(short?)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.Approved),
                ExpectedType = typeof(bool?)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.CreatedOn),
                ExpectedType = typeof(DateTime)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = nameof(Opportunity.EmailId),
                ExpectedType = typeof(long?)
            };
            yield return new Tester<Opportunity>
            {
                PropertyName = $"{nameof(Opportunity.Id)}foo!",
                ExpectedType = null
            };
        }
        #endregion

        #region Tester Interfaces
        public interface ITester
        {
            void Run();
        }
        #endregion

        #region Tester Classes
        private sealed class Tester<TClass> : ITester
            where TClass : class
        {
            public string PropertyName { private get; set; }

            public Type ExpectedType { private get; set; }

            public void Run()
            {
                var type = PropertyName.GetPropertyType<TClass>();
                Assert.AreEqual(ExpectedType, type);
            }
        }
        #endregion
    }
}