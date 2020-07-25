using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Utilities.Extensions;

namespace Utilities.Unit.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("                  ", "")]
        [TestCase("Regular Text", "Regular Text")]
        [TestCase(" Foo", "Foo")]
        public void ShouldReturnStringValue(string parameter, string expectedValue)
        {
            var newValue = parameter.NullTrim();
            Assert.AreEqual(newValue, expectedValue);
        }

        [TestCase("1, 2, 3, 4, -3, -50, -49", 1, 2, 3, 4, -3, -50, -49)]
        [TestCase("1", 1)]
        [TestCase("")]
        public void ShouldReturnCommaFormattedStringsFromInts(string expectedValue, params int[] parameters)
        {
            var list = parameters.ToList();
            var newValue = list.ToCommaFormat();
            Assert.IsNotNull(newValue);
            Assert.AreEqual(newValue, expectedValue);
        }

        [Test]
        public void ShouldReturnCommaFormattedStringsFromNullIntCollection()
        {
            var newValue = ((ICollection<int>)null).ToCommaFormat();
            Assert.IsNotNull(newValue);
            Assert.IsEmpty(newValue);
        }

        [TestCase("1233333846633221, 2, 3, 4, -3, -50, -49", 1233333846633221, 2, 3, 4, -3, -50, -49)]
        [TestCase("-1233333846633221", -1233333846633221)]
        [TestCase("")]
        public void ShouldReturnCommaFormattedStringsFromLongs(string expectedValue, params long[] parameters)
        {
            var list = parameters.ToList();
            var newValue = list.ToCommaFormat();
            Assert.IsNotNull(newValue);
            Assert.AreEqual(newValue, expectedValue);
        }

        [Test]
        public void ShouldReturnCommaFormattedStringsFromNullLongCollection()
        {
            var newValue = ((ICollection<long>)null).ToCommaFormat();
            Assert.IsNotNull(newValue);
            Assert.IsEmpty(newValue);
        }

        [TestCase("1, 2, 3, 4", "1", "2", "3", "4")]
        [TestCase("1", "1")]
        [TestCase("1, 2, ,,  2.5,, 3", " ", "1", null, "2", ",", " 2.5,", "3")]
        [TestCase("", " ")]
        [TestCase("")]
        public void ShouldReturnCommaFormattedStrings(string expectedValue, params string[] parameters)
        {
            var list = parameters.ToList();
            var newValue = list.ToCommaFormat();
            Assert.IsNotNull(newValue);
            Assert.AreEqual(newValue, expectedValue);
        }

        [Test]
        public void ShouldReturnCommaFormattedStringsFromNullStringCollection()
        {
            var newValue = ((ICollection<string>)null).ToCommaFormat();
            Assert.IsNotNull(newValue);
            Assert.IsEmpty(newValue);
        }

        [Test]
        public void ShouldReturnPipeFormattedStrings()
        {
            const char pipe = '|';

            var list = new List<string> { "1", "2", "3", "4" };
            var newValue = list.ToPipeFormat();
            Assert.IsNotNull(newValue);
            Assert.AreNotEqual(newValue.Substring(0, 1), pipe);
            Assert.AreNotEqual(newValue.Substring(newValue.Length - 1, 1), pipe);
            Assert.AreEqual(newValue.Count(x => x == pipe), list.Count - 1);
            Assert.AreEqual(newValue.Length, list.Count + list.Count - 1);

            list = new List<string> { "1" };
            newValue = list.ToPipeFormat();
            Assert.IsNotNull(newValue);
            Assert.AreNotEqual(newValue.Substring(0, 1), pipe);
            Assert.AreNotEqual(newValue.Substring(newValue.Length - 1, 1), pipe);
            Assert.AreEqual(newValue.Count(x => x == pipe), list.Count - 1);
            Assert.AreEqual(newValue.Length, list.Count + list.Count - 1);

            list = new List<string> { " ", "1", null, "2", pipe.ToString(), "3" };
            newValue = list.ToPipeFormat();
            list = list.Where(x => !string.IsNullOrWhiteSpace(x) && !x.Contains("|")).ToList();
            Assert.IsNotNull(newValue);
            Assert.AreNotEqual(newValue.Substring(0, 1), pipe);
            Assert.AreNotEqual(newValue.Substring(newValue.Length - 1, 1), pipe);
            Assert.AreEqual(newValue.Count(x => x == pipe), list.Count - 1);
            Assert.AreEqual(newValue.Length, list.Count + list.Count - 1);

            list = new List<string> { " ", pipe.ToString() };
            newValue = list.ToPipeFormat();
            Assert.IsNull(newValue);

            list = new List<string>();
            newValue = list.ToPipeFormat();
            Assert.IsNull(newValue);

            newValue = ((ICollection<string>)null).ToPipeFormat();
            Assert.IsNull(newValue);
        }

        [TestCase("\n- 1\n- 2\n- 3\n- 4\n- -3\n- -50\n- -49", 1, 2, 3, 4, -3, -50, -49)]
        [TestCase("\n- 1", 1)]
        [TestCase("")]
        public void ShouldReturnBulletFormattedStringsFromInts(string expectedValue, params int[] parameters)
        {
            var list = parameters.ToList();
            var newValue = list.ToBulletFormat();
            Assert.IsNotNull(newValue);
            Assert.AreEqual(expectedValue, newValue);
        }

        [Test]
        public void ShouldReturnBulletFormattedStringsFromNullIntCollection()
        {
            var newValue = ((ICollection<int>)null).ToBulletFormat();
            Assert.IsNotNull(newValue);
            Assert.IsEmpty(newValue);
        }

        [TestCase("\n- 1233333846633221\n- 2\n- 3\n- 4\n- -3\n- -50\n- -49", 1233333846633221, 2, 3, 4, -3, -50, -49)]
        [TestCase("\n- -1233333846633221", -1233333846633221)]
        [TestCase("")]
        public void ShouldReturnBulletFormattedStringsFromLongs(string expectedValue, params long[] parameters)
        {
            var list = parameters.ToList();
            var newValue = list.ToBulletFormat();
            Assert.IsNotNull(newValue);
            Assert.AreEqual(expectedValue, newValue);
        }

        [Test]
        public void ShouldReturnBulletFormattedStringsFromNullLongCollection()
        {
            var newValue = ((ICollection<long>)null).ToBulletFormat();
            Assert.IsNotNull(newValue);
            Assert.IsEmpty(newValue);
        }

        [TestCase("\n- 1\n- 2\n- 3\n- 4", "1", "2", "3", "4")]
        [TestCase("\n- 1", "1")]
        [TestCase("\n- 1\n- 2\n- *\n- 2.5*\n- 3", " ", "1", null, "2", "\n- ", " 2.5\n- ", "3")]
        [TestCase("", " ")]
        [TestCase("")]
        public void ShouldReturnBulletFormattedStrings(string expectedValue, params string[] parameters)
        {
            var list = parameters.ToList();
            var newValue = list.ToBulletFormat();
            Assert.IsNotNull(newValue);
            Assert.AreEqual(expectedValue, newValue);
        }

        [Test]
        public void ShouldReturnBulletFormattedStringsFromNullStringCollection()
        {
            var newValue = ((ICollection<string>)null).ToBulletFormat();
            Assert.IsNotNull(newValue);
            Assert.IsEmpty(newValue);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(10000)]
        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldGenerateRandomStrings(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var generatedValue = StringExtensions.RandomString(length);
            Assert.IsNotNull(generatedValue);
            Assert.AreEqual(generatedValue.Length, length >= 0 ? length : 0);

            var generatedChars = generatedValue.ToCharArray();

            for (var i = 0; i < length; i++)
            {
                var charValue = generatedChars[i];
                Assert.IsTrue(chars.Contains(charValue), $"'{charValue}' is not a valid char for string '{chars}'.");
            }
        }
    }
}