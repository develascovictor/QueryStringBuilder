using Filtering.Helpers;
using Filtering.Unit.Tests.Models;
using NUnit.Framework;

namespace Filtering.Unit.Tests.Helpers
{
    [TestFixture]
    public class PropertyCacheTests
    {
        private const string Id = nameof(Account.Id);
        private const string Active = nameof(Account.Active);
        private const string CompanyName = nameof(Account.CompanyName);
        private const string ContactName = nameof(Account.ContactName);
        private const string CustomerName = nameof(Account.CustomerName);
        private const string CreatedOn = nameof(Account.CreatedOn);
        private const string Language = nameof(Account.Language);
        private const string AccountValue = nameof(Account.AccountValue);

        [TestCase(Id)]
        [TestCase(Active)]
        [TestCase(CompanyName)]
        [TestCase(ContactName)]
        [TestCase(CustomerName)]
        [TestCase(CreatedOn)]
        [TestCase(Language)]
        [TestCase(AccountValue)]
        public void ShouldGetPropertyCache(string propertyName)
        {
            var obtainedPropertyName = PropertyCache<Account>.Get(propertyName.ToLower());
            Assert.AreEqual(obtainedPropertyName, propertyName);
        }

        [TestCase(Id)]
        [TestCase(Active)]
        [TestCase(CompanyName)]
        [TestCase(ContactName)]
        [TestCase(CustomerName)]
        [TestCase(CreatedOn)]
        [TestCase(Language)]
        [TestCase(AccountValue)]
        public void ShouldGetPropertyCacheCaseInsensitive(string propertyName)
        {
            var obtainedPropertyName = PropertyCache<Account>.Get(propertyName.ToLower());
            Assert.AreEqual(obtainedPropertyName, propertyName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(Id + " ")]
        [TestCase(" " + Id + " ")]
        [TestCase("2" + Id)]
        public void ShouldGetPropertyCacheReturnNull(string propertyName)
        {
            var obtainedPropertyName = PropertyCache<Account>.Get(propertyName);
            Assert.IsNull(obtainedPropertyName);
        }
    }
}