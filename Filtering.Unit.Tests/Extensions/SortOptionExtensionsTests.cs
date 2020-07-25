using Filtering.Exceptions;
using Filtering.Extensions;
using Filtering.Unit.Tests.Models;
using Filtering.Unit.Tests.Whitelists;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Extensions;

namespace Filtering.Unit.Tests.Extensions
{
    [TestFixture]
    public class SortOptionExtensionsTests
    {
        private static IReadOnlyDictionary<string, string> _whitelist;
        private const string ContactName = nameof(Opportunity.ContactName);
        private const string ContactEmail = nameof(Opportunity.ContactEmail);
        private const string ProductValue = "product.value";

        [OneTimeSetUp]
        public void SetUp()
        {
            var opportunityWhitelist = new OpportunityWhitelist();
            _whitelist = opportunityWhitelist.Whitelist;
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("-")]
        [TestCase(",")]
        [TestCase("- ,")]
        [TestCase(", -")]
        [TestCase("-,-")]
        public void ShouldBuildSortByExpressionReturnEmpty(string sortOptions)
        {
            var sortDictionary = SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions);
            Assert.IsEmpty(sortDictionary);
        }

        [TestCase(ContactName)]
        [TestCase("-" + ContactName)]
        [TestCase(ContactName + "," + ContactEmail)]
        [TestCase(ContactName + ",-" + ContactEmail)]
        [TestCase("-" + ContactName + ",-" + ContactEmail)]
        [TestCase(ContactName + ",")]
        [TestCase(",-" + ContactName)]
        public void ShouldBuildSortByExpressionWithWhitelist(string sortOptions)
        {
            var sortDictionary = SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions, _whitelist);
            ValidateDictionary(sortDictionary, sortOptions);
        }

        [TestCase(ContactName + "," + ContactName)]
        [TestCase(ContactName + ",-" + ContactName)]
        [TestCase("-" + ContactName + ",-" + ContactName)]
        [TestCase(ContactName + "," + ContactEmail + ",-" + ContactName)]
        public void ShouldBuildSortByExpressionWithWhitelistThrowArgumentException(string sortOptions)
        {
            var argumentException = Assert.Catch<ArgumentException>(() => SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions, _whitelist));
            Assert.IsNotNull(argumentException);
            Assert.IsTrue(argumentException.Message.Contains("An item with the same key has already been added."));
        }

        [TestCase(nameof(Opportunity.ManagerId))]
        [TestCase(nameof(Opportunity.AssignedEmployeeId))]
        public void ShouldBuildSortByExpressionWithWhitelistThrowUnsupportedWhitelistPropertyException(string sortOptions)
        {
            Assert.Throws<UnsupportedWhitelistPropertyException>(() => SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions, _whitelist));
        }

        [TestCase(ContactName)]
        [TestCase("-" + ContactName)]
        [TestCase(ContactName + "," + ContactEmail)]
        [TestCase(ContactName + ",-" + ContactEmail)]
        [TestCase("-" + ContactName + ",-" + ContactEmail)]
        [TestCase(ContactName + ",")]
        [TestCase(",-" + ContactName)]
        public void ShouldBuildSortByExpressionWithEmptyWhitelist(string sortOptions)
        {
            var sortDictionary = SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions);
            ValidateDictionary(sortDictionary, sortOptions);
        }

        [TestCase(ContactName + "," + ContactName)]
        [TestCase(ContactName + ",-" + ContactName)]
        [TestCase("-" + ContactName + ",-" + ContactName)]
        [TestCase(ContactName + "," + ContactEmail + ",-" + ContactName)]
        public void ShouldBuildSortByExpressionWithEmptyWhitelistThrowArgumentException(string sortOptions)
        {
            var argumentException = Assert.Catch<ArgumentException>(() => SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions));
            Assert.IsNotNull(argumentException);
            Assert.IsTrue(argumentException.Message.Contains("An item with the same key has already been added."));
        }

        [TestCase(ProductValue)]
        [TestCase("-" + ProductValue)]
        [TestCase(ContactName + "," + ProductValue)]
        public void ShouldBuildSortByExpressionWithEmptyWhitelistThrowEntityPropertyNameNotDefinedException(string sortOptions)
        {
            Assert.Throws<EntityPropertyNameNotDefinedException>(() => SortOptionExtensions.BuildSortByExpression<Opportunity>(sortOptions));
        }

        private static void ValidateDictionary(IReadOnlyDictionary<string, bool> sortDictionary, string sortOptions)
        {
            Assert.IsNotEmpty(sortDictionary);

            var array = sortOptions.Split(',').Where(x => !string.IsNullOrWhiteSpace(x) && x.NullTrim() != "-").ToList();
            Assert.AreEqual(sortDictionary.Count, array.Count);

            foreach (var iteration in array)
            {
                var propertyName = iteration;
                var asc = true;

                if (iteration.StartsWith("-", StringComparison.Ordinal))
                {
                    propertyName = iteration.Remove(0, 1);
                    asc = false;
                }

                if (_whitelist.Any() && _whitelist.TryGetValue(propertyName, out var value))
                {
                    propertyName = value;
                }

                Assert.IsTrue(sortDictionary.ContainsKey(propertyName));
                Assert.AreEqual(sortDictionary[propertyName], asc);
            }
        }
    }
}