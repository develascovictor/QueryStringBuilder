using Filtering.Extensions;
using Filtering.Unit.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;

namespace Filtering.Unit.Tests.Extensions
{
    [TestFixture]
    public class QueryableExtensionsTests
    {
        private const string Id = nameof(Account.Id);
        private const string Active = nameof(Account.Active);
        private const string CompanyName = nameof(Account.CompanyName);
        private const string ContactName = nameof(Account.ContactName);
        private const string CustomerName = nameof(Account.CustomerName);
        private const string CreatedOn = nameof(Account.CreatedOn);

        private static readonly IQueryable<Account> Accounts = GetCollection().AsQueryable();

        private static string GetExceptionMessage(string propertyName) => $"Invalid property name {propertyName} in Account.";

        [TestCaseSource(nameof(TestCasesApplySortByExpressions))]
        public void ShouldApplySortByExpressions(ITester tester)
        {
            tester.RunApplySortByExpressions();
        }

        [TestCaseSource(nameof(TestCasesApplySortByExpressionsEmptyData))]
        public void ShouldApplySortByExpressionsEmptyData(ITester tester)
        {
            tester.RunApplySortByExpressionsEmptyData();
        }

        [TestCaseSource(nameof(TestThrowInvalidDataExceptionApplySortByExpressions))]
        public void ShouldThrowParseExceptionApplySortByExpressions(ITester tester)
        {
            tester.RunThrowParseExceptionApplySortByExpressions();
        }

        #region Collections
        private static IEnumerable<Account> GetCollection()
        {
            var accountOne = new Account
            {
                Id = 10,
                CompanyName = "Some Company",
                CustomerName = "Someone",
                ContactName = "Mario",
                ContactLanguageId = 1,
                Active = true,
                CreatedOn = new DateTime(2019, 3, 12, 15, 45, 8)
            };

            var accountTwo = new Account
            {
                Id = 12,
                CompanyName = "Other Company",
                CustomerName = "Unknown",
                ContactName = "Luigi",
                ContactLanguageId = 1,
                Active = false,
                CreatedOn = new DateTime(2019, 3, 12, 15, 49, 12)
            };

            var accountThree = new Account
            {
                Id = 13,
                CompanyName = "Elsewhere",
                CustomerName = "Me",
                ContactName = "Peach",
                ContactLanguageId = 1,
                Active = false,
                CreatedOn = new DateTime(2019, 1, 29, 0, 0, 0)
            };

            var accountFour = new Account
            {
                Id = 14,
                CompanyName = "Far away",
                CustomerName = "You",
                ContactName = "Toad",
                ContactLanguageId = 1,
                Active = true,
                CreatedOn = new DateTime(2019, 3, 13, 15, 49, 12)
            };

            var accounts = new List<Account>
            {
                accountOne,
                accountTwo,
                accountThree,
                accountFour
            };

            return accounts;
        }
        #endregion

        #region Test Cases
        public static IEnumerable<ITester> TestCasesApplySortByExpressions()
        {
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { Id, true } },
                ExpectedOrderedAccounts = Accounts.OrderBy(x => x.Id).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { Id, false } },
                ExpectedOrderedAccounts = Accounts.OrderByDescending(x => x.Id).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { Active, false } },
                ExpectedOrderedAccounts = Accounts.OrderByDescending(x => x.Active).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { CompanyName, true } },
                ExpectedOrderedAccounts = Accounts.OrderBy(x => x.CompanyName).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { CompanyName, true }, { ContactName, true } },
                ExpectedOrderedAccounts = Accounts.OrderBy(x => x.CompanyName).ThenBy(x => x.ContactName).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { CompanyName, false }, { CustomerName, true } },
                ExpectedOrderedAccounts = Accounts.OrderByDescending(x => x.CompanyName).ThenBy(x => x.CustomerName).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { CompanyName, true }, { CreatedOn, false } },
                ExpectedOrderedAccounts = Accounts.OrderBy(x => x.CompanyName).ThenBy(x => x.CreatedOn).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { CompanyName, false }, { CustomerName, true }, { Id, false } },
                ExpectedOrderedAccounts = Accounts.OrderByDescending(x => x.CompanyName).ThenBy(x => x.CustomerName).ThenByDescending(x => x.Id).ToList()
            };
        }

        public static IEnumerable<ITester> TestCasesApplySortByExpressionsEmptyData()
        {
            yield return new Tester
            {
                Dictionary = null,
                ExpectedOrderedAccounts = Accounts.OrderBy(x => x.Id).ToList()
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool>(),
                ExpectedOrderedAccounts = Accounts.OrderBy(x => x.Id).ToList()
            };
        }

        public static IEnumerable<ITester> TestThrowInvalidDataExceptionApplySortByExpressions()
        {
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { "", true } },
                ExpectedOrderedAccounts = null
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { " ", true } },
                ExpectedOrderedAccounts = null
            };
            //yield return new Tester
            //{
            //    Dictionary = new Dictionary<string, bool> { { $"{ContactName} ", true } },
            //    ExpectedOrderedAccounts = null
            //};
            //yield return new Tester
            //{
            //    Dictionary = new Dictionary<string, bool> { { $" {ContactName} ", true } },
            //    ExpectedOrderedAccounts = null
            //};
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { $"{ContactName}foobar", true } },
                ExpectedOrderedAccounts = null
            };
            yield return new Tester
            {
                Dictionary = new Dictionary<string, bool> { { "", true }, { $"{ContactName}foobar", true } },
                ExpectedOrderedAccounts = null
            };
        }
        #endregion

        #region Tester
        public interface ITester
        {
            void RunApplySortByExpressions();
            void RunApplySortByExpressionsEmptyData();
            void RunThrowParseExceptionApplySortByExpressions();
        }

        public class Tester : ITester
        {
            /// <summary>
            /// Test case
            /// </summary>
            public Dictionary<string, bool> Dictionary { private get; set; }

            /// <summary>
            /// Expected result
            /// </summary>
            public IList<Account> ExpectedOrderedAccounts { private get; set; }

            public void RunApplySortByExpressions()
            {
                ValidateDictionary();
                ValidateExpectedOrderedAccounts();

                var orderedAccounts = Accounts.ApplySortByExpressions(Dictionary);
                ValidateExpression(orderedAccounts);
            }

            public void RunApplySortByExpressionsEmptyData()
            {
                if (Dictionary != null)
                {
                    Assert.IsEmpty(Dictionary);
                }

                ValidateExpectedOrderedAccounts();

                var orderedAccounts = Accounts.ApplySortByExpressions(Dictionary);
                ValidateExpression(orderedAccounts);
            }

            public void RunThrowParseExceptionApplySortByExpressions()
            {
                ValidateDictionary();
                Assert.Throws<ParseException>(() => Accounts.ApplySortByExpressions(Dictionary));
            }

            private void ValidateDictionary()
            {
                Assert.IsNotEmpty(Dictionary);
                Assert.IsTrue(Dictionary.Any());
            }

            private void ValidateExpectedOrderedAccounts()
            {
                Assert.IsNotEmpty(ExpectedOrderedAccounts);
                Assert.AreEqual(ExpectedOrderedAccounts.Count, Accounts.Count(), $"[ExpectedOrderedAccounts.Count: {ExpectedOrderedAccounts.Count}] [Accounts.Count(): {Accounts.Count()}]");
            }

            private void ValidateExpression(IQueryable<Account> orderedAccounts)
            {
                Assert.IsNotNull(orderedAccounts);

                var serialized = orderedAccounts.ToList();
                Assert.IsNotEmpty(serialized);
                Assert.AreEqual(serialized.Count, ExpectedOrderedAccounts.Count, $"[serialized.Count: {serialized.Count}] [ExpectedOrderedAccounts.Count: {ExpectedOrderedAccounts.Count}]");

                for (var i = 0; i < serialized.Count; i++)
                {
                    Assert.AreEqual(serialized[i].Id, ExpectedOrderedAccounts[i].Id, $"[serialized[{i}].Id: {serialized[i].Id}] [ExpectedOrderedAccounts[{i}].Id: {ExpectedOrderedAccounts[i].Id}]");
                }
            }
        }
        #endregion
    }
}