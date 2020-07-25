using Filtering.Constants;
using Filtering.Extensions;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Extensions
{
    [TestFixture]
    public class RequestExtensionsTests
    {
        private const string Or = LogicalOperators.OrOperator;
        private const string And = LogicalOperators.AndOperator;

        [TestCase("filter=name==john", "name==john")]
        [TestCase("filter=name==john,name=@jane", "name==john" + Or + "name=@jane")]
        [TestCase("filter=name==john&filter=lastname!=doe", "name==john" + And + "lastname!=doe")]
        [TestCase("filter=name==john,name=@jane&filter=lastname!=doe&filter=age>10,age<=12", "name==john" + Or + "name=@jane" + And + "lastname!=doe" + And + "age>10" + Or + "age<=12")]
        [TestCase("filter=name==john,name=@jane&age>10,age<=12", "name==john" + Or + "name=@jane")]
        public void ShouldGetFilterRequest(string queryString, string expectedValue)
        {
            var filterRequest = RequestExtensions.GetFilterRequest(queryString);
            Assert.AreEqual(expectedValue, filterRequest);
        }

        [TestCase("opportunityFilter=name==john", "name==john")]
        [TestCase("opportunityFilter=name==john,name=@jane", "name==john" + Or + "name=@jane")]
        [TestCase("opportunityFilter=name==john&opportunityFilter=lastname!=doe", "name==john" + And + "lastname!=doe")]
        [TestCase("opportunityFilter=name==john,name=@jane&opportunityFilter=lastname!=doe&opportunityFilter=age>10,age<=12", "name==john" + Or + "name=@jane" + And + "lastname!=doe" + And + "age>10" + Or + "age<=12")]
        [TestCase("opportunityFilter=name==john,name=@jane&age>10,age<=12", "name==john" + Or + "name=@jane")]
        public void ShouldGetFilterRequestWithAlternateParamName(string queryString, string expectedValue)
        {
            var filterRequest = RequestExtensions.GetFilterRequest(queryString, "opportunityFilter");
            Assert.AreEqual(expectedValue, filterRequest);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("name==john")]
        [TestCase("filter=")]
        public void ShouldGetFilterRequestGetEmptyString(string queryString)
        {
            var filterRequest = RequestExtensions.GetFilterRequest(queryString);
            Assert.IsEmpty(filterRequest);
        }
    }
}