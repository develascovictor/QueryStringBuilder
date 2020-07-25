using Filtering.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Filtering.Unit.Tests.Helpers
{
    [TestFixture]
    public class TryParseDelegatesTests
    {
        [TestCaseSource(nameof(TestTryParse))]
        public void ShouldTryParse(ITryParseTester tester)
        {
            tester.RunTryParse();
        }

        [TestCaseSource(nameof(TestValidateDelegates))]
        public void ShouldValidateDelegates(IDelegateTester tester)
        {
            tester.Compare();
        }

        #region Test Cases
        public static IEnumerable<ITryParseTester> TestTryParse()
        {
            yield return new TryParseTester<string>
            {
                Delegate = delegate (string s, out string outParam)
                {
                    outParam = "foo";
                    return true;
                },
                ExpectedOutParameter = "foo",
                ExpectedBoolean = true
            };
            yield return new TryParseTester<bool>
            {
                Delegate = delegate (string s, out bool outParam)
                {
                    outParam = false;
                    return true;
                },
                ExpectedOutParameter = false,
                ExpectedBoolean = true
            };
            yield return new TryParseTester<short>
            {
                Delegate = delegate (string s, out short outParam)
                {
                    outParam = 2;
                    return false;
                },
                ExpectedOutParameter = 2,
                ExpectedBoolean = false
            };
            yield return new TryParseTester<int>
            {
                Delegate = delegate (string s, out int outParam)
                {
                    outParam = -5;
                    return false;
                },
                ExpectedOutParameter = -5,
                ExpectedBoolean = false
            };
            yield return new TryParseTester<long>
            {
                Delegate = delegate (string s, out long outParam)
                {
                    outParam = 4233232231123;
                    return true;
                },
                ExpectedOutParameter = 4233232231123,
                ExpectedBoolean = true
            };
            yield return new TryParseTester<decimal>
            {
                Delegate = delegate (string s, out decimal outParam)
                {
                    outParam = 2891.2M;
                    return true;
                },
                ExpectedOutParameter = 2891.2M,
                ExpectedBoolean = true
            };
            yield return new TryParseTester<DateTime>
            {
                Delegate = delegate (string s, out DateTime outParam)
                {
                    outParam = new DateTime(2019, 3, 29, 14, 23, 6);
                    return false;
                },
                ExpectedOutParameter = new DateTime(2019, 3, 29, 14, 23, 6),
                ExpectedBoolean = false
            };
        }

        public static IEnumerable<IDelegateTester> TestValidateDelegates()
        {
            yield return new DelegateTester<bool>
            {
                TryParseDelegate = TryParseDelegates.Delegates.Bool,
                ExpectedTryParseDelegate = bool.TryParse
            };
            yield return new DelegateTester<short>
            {
                TryParseDelegate = TryParseDelegates.Delegates.Short,
                ExpectedTryParseDelegate = short.TryParse
            };
            yield return new DelegateTester<int>
            {
                TryParseDelegate = TryParseDelegates.Delegates.Int,
                ExpectedTryParseDelegate = int.TryParse
            };
            yield return new DelegateTester<long>
            {
                TryParseDelegate = TryParseDelegates.Delegates.Long,
                ExpectedTryParseDelegate = long.TryParse
            };
            yield return new DelegateTester<decimal>
            {
                TryParseDelegate = TryParseDelegates.Delegates.Decimal,
                ExpectedTryParseDelegate = decimal.TryParse
            };
            yield return new DelegateTester<DateTime>
            {
                TryParseDelegate = TryParseDelegates.Delegates.DateTime,
                ExpectedTryParseDelegate = DateTime.TryParse
            };
        }
        #endregion

        #region Tester Interfaces
        public interface ITryParseTester
        {
            void RunTryParse();
        }

        public interface IDelegateTester
        {
            void Compare();
        }
        #endregion

        #region Tester Classes
        private sealed class TryParseTester<T> : ITryParseTester
        {
            public TryParseDelegates.TryParse<T> Delegate { private get; set; }
            public T ExpectedOutParameter { private get; set; }
            public bool ExpectedBoolean { private get; set; }

            public void RunTryParse()
            {
                Assert.IsNotNull(Delegate);

                var result = Delegate("", out var outPut);
                Assert.AreEqual(result, ExpectedBoolean);
                Assert.AreEqual(outPut, ExpectedOutParameter);
            }
        }

        private sealed class DelegateTester<T> : IDelegateTester
            where T : struct
        {
            public TryParseDelegates.TryParse<T> TryParseDelegate { private get; set; }
            public TryParseDelegates.TryParse<T> ExpectedTryParseDelegate { private get; set; }

            public void Compare()
            {
                Assert.AreEqual(TryParseDelegate, ExpectedTryParseDelegate);
            }
        }
        #endregion
    }
}