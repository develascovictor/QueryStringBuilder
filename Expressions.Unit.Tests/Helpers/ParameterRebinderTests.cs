using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Expressions.Helpers;
using NUnit.Framework;

namespace Expressions.Unit.Tests.Helpers
{
    [TestFixture]
    public class ParameterRebinderTests
    {
        [Test]
        public void ShouldInstantiateConstructorWithParameter()
        {
            var parameterX = GetParameterExpression("x");
            var dictionary = new Dictionary<ParameterExpression, ParameterExpression> { { parameterX, parameterX } };
            var parameterRebinder = new ParameterRebinder(dictionary);
        }

        [Test]
        public void ShouldInstantiateConstructorWithNullParameter()
        {
            var parameterRebinder = new ParameterRebinder(null);
        }

        [Test]
        public void ShouldReplaceParameters()
        {
            var expressionX = GetExpressionX();
            var parameter = GetParameterExpression("z");
            var dictionary = new Dictionary<ParameterExpression, ParameterExpression> { { parameter, parameter } };

            var returnedExpression = ParameterRebinder.ReplaceParameters(dictionary, expressionX);
            var returnedExpressionString = returnedExpression.ToString();
            Assert.AreEqual(returnedExpressionString, expressionX.ToString());
        }

        [Test]
        public void ShouldReplaceParametersNullDictionary()
        {
            var expressionX = GetExpressionX();
            var returnedExpression = ParameterRebinder.ReplaceParameters(null, expressionX);
            var returnedExpressionString = returnedExpression.ToString();
            Assert.AreEqual(returnedExpressionString, expressionX.ToString());
        }

        [Test]
        public void ShouldReplaceParametersReturnNull()
        {
            var parameterRebinder = ParameterRebinder.ReplaceParameters(new Dictionary<ParameterExpression, ParameterExpression>(), null);
            Assert.IsNull(parameterRebinder);

            parameterRebinder = ParameterRebinder.ReplaceParameters(null, null);
            Assert.IsNull(parameterRebinder);
        }

        private static ParameterExpression GetParameterExpression(string name) => Expression.Parameter(typeof(string), name);
        private static Expression<Func<string, bool>> GetExpressionX() => x => x == "foo";
    }
}