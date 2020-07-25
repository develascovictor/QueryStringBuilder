using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using Expressions;

// Leave namespace as is (required to override IOperation validation on ExpressionBuilder nuget library)
namespace ExpressionBuilder.Operations
{
    public static class CustomExpressionOperations
    {
        public static IOperation EqualToNoTrim => new EqualToWithNoTrim();
        public static IOperation NotEqualToNoTrim => new NotEqualToWithNoTrim();
        public static IOperation ContainsNoTrim => new ContainsWithNoTrim();
        public static IOperation DoesNotContainNoTrim => new DoesNotContainWithNoTrim();
        public static IOperation IsNullNoTrim => new IsNullWithNoTrim();
        public static IOperation IsNotNullNoTrim => new IsNotNullWithNoTrim();
        public static IOperation IsNullOrEmpty => new IsNullOrWhiteSpaceWithNoTrim();
        public static IOperation IsNotNullNorEmpty => new IsNotNullNorWhiteSpaceWithNoTrim();

        internal class EqualToWithNoTrim : OperationBase
        {
            public EqualToWithNoTrim()
                : base(nameof(EqualToWithNoTrim), 1, TypeGroup.Text | TypeGroup.Number | TypeGroup.Boolean | TypeGroup.Date, true, false, false)
            {
            }

            public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
            {
                var constant = (Expression)constant1;

                if (member.Type != typeof(string))
                {
                    return Expression.Equal(member, constant);
                }

                constant = constant1.ToLower();
                return Expression.Equal(member.ToLower(), constant)
                    .AddNullCheck(member);
            }
        }

        internal class NotEqualToWithNoTrim : OperationBase
        {
            public NotEqualToWithNoTrim()
                : base(nameof(NotEqualToWithNoTrim), 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text)
            {
            }

            public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
            {
                var constant = (Expression)constant1;

                if (member.Type != typeof(string))
                {
                    return Expression.NotEqual(member, constant);
                }

                constant = constant1.ToLower();
                return Expression.NotEqual(member.ToLower(), constant)
                    .AddNullCheck(member);
            }
        }

        internal class ContainsWithNoTrim : OperationBase
        {
            private readonly MethodInfo _stringContainsMethod = typeof(string).GetMethod(nameof(Contains), new[] { typeof(string) });

            public ContainsWithNoTrim()
                : base(nameof(ContainsWithNoTrim), 1, TypeGroup.Text)
            {
            }

            public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
            {
                var lower = constant1.ToLower();

                return Expression.Call(member.ToLower(), _stringContainsMethod, lower)
                    .AddNullCheck(member);
            }
        }

        internal class DoesNotContainWithNoTrim : OperationBase
        {
            private readonly MethodInfo _stringContainsMethod = typeof(string).GetMethod(nameof(ExpressionBuilder.Operations.Contains), new[] { typeof(string) });

            public DoesNotContainWithNoTrim()
                : base(nameof(DoesNotContainWithNoTrim), 1, TypeGroup.Text)
            {
            }

            public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
            {
                var lower = constant1.ToLower();

                return Expression.Not(Expression.Call(member.ToLower(), _stringContainsMethod, lower))
                    .AddNullCheck(member);
            }
        }

        internal class IsNullWithNoTrim : OperationBase
        {
            public IsNullWithNoTrim()
                : base(nameof(IsNullWithNoTrim), 1, TypeGroup.Text)
            {
            }

            public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
            {
                return null;
            }
        }

        internal class IsNotNullWithNoTrim : OperationBase
        {
            public IsNotNullWithNoTrim()
                : base(nameof(IsNotNullWithNoTrim), 1, TypeGroup.Text)
            {
            }

            public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
            {
                return null;
            }
        }

        internal class IsNullOrWhiteSpaceWithNoTrim : OperationBase
        {
            public IsNullOrWhiteSpaceWithNoTrim()
                : base(nameof(IsNullOrWhiteSpaceWithNoTrim), 0, TypeGroup.Text, true, false, true)
            {
            }

            public override Expression GetExpression(
                MemberExpression member,
                ConstantExpression constant1,
                ConstantExpression constant2)
            {
                var right1 = (Expression)Expression.Constant(null);
                var right2 = (Expression)Expression.Constant(string.Empty);
                var isNull = (Expression)Expression.Equal(member, right1);
                var isNotNull = (Expression)Expression.NotEqual(member, right1);
                var isEmpty = (Expression)Expression.Equal(member.ToLower(), right2);
                var isNotNullAndEmpty = (Expression)Expression.AndAlso(isNotNull, isEmpty);
                var final = (Expression)Expression.OrElse(isNull, isNotNullAndEmpty);

                return final;
            }
        }

        internal class IsNotNullNorWhiteSpaceWithNoTrim : OperationBase
        {
            public IsNotNullNorWhiteSpaceWithNoTrim()
                : base(nameof(IsNotNullNorWhiteSpaceWithNoTrim), 0, TypeGroup.Text, true, false, false)
            {
            }

            public override Expression GetExpression(
                MemberExpression member,
                ConstantExpression constant1,
                ConstantExpression constant2)
            {
                var right1 = (Expression)Expression.Constant(null);
                var right2 = (Expression)Expression.Constant(string.Empty);
                var isNotNull = (Expression)Expression.NotEqual(member, right1);
                var isNotEmpty = (Expression)Expression.NotEqual(member.ToLower(), right2);
                var final = (Expression)Expression.AndAlso(isNotNull, isNotEmpty);

                return final;
            }
        }
    }
}