using Expressions;
using Filtering.Constants;
using Filtering.Exceptions;
using Filtering.Extensions;
using Filtering.Unit.Tests.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Filtering.Unit.Tests.Extensions
{
    [TestFixture]
    public class FilterOptionExtensionsTests
    {
        #region Const Values
        private const string CompanyName = nameof(Account.CompanyName);//string
        private const string ContactName = nameof(Account.ContactName);//string
        private const string CustomerName = nameof(Account.CustomerName);//string
        private const string Active = nameof(Account.Active);//bool
        private const string Approved = nameof(Opportunity.Approved);//bool?
        private const string LanguageId = nameof(Account.LanguageId);//short
        private const string ContactLanguageId = nameof(Account.ContactLanguageId);//short?
        private const string Id = nameof(Models.AccountValue.Id);//int
        private const string AssignedEmployeeId = nameof(Opportunity.AssignedEmployeeId);//int?
        private const string ManagerId = nameof(Opportunity.ManagerId);//long
        private const string EmailId = nameof(Opportunity.EmailId);//long?
        //A decimal example is missing
        private const string TotalServiceValue = nameof(Account.TotalServiceValue);//decimal?
        private const string CreatedOn = nameof(Account.CreatedOn);//DateTime
        private const string ExpirationDate = nameof(Opportunity.ExpirationDate);//DateTime?
        private const string Language = nameof(Account.Language);//class
        private const string AccountValue = nameof(Account.AccountValue);//ICollection
        private static readonly string LanguageName = $"{nameof(Account.Language)}.{nameof(Models.Language.Name)}";//Navigation Property

        private const string Parameter1 = "Red Inc";
        private const string Parameter2 = "Acme Inc";
        private const string Parameter3 = "ui";
        private const string Parameter4 = "ar";
        private const string Parameter5 = "Tru";
        private const string Parameter6 = "File Source";
        private const long Parameter7 = 10;
        private const bool Parameter8 = true;
        private const decimal Parameter10 = -7331;
        private const int Parameter11 = 23;
        private const short Parameter12 = 1;
        private const string Parameter13 = "en-us";
        private const int Year = 2019;
        private const int Month = 3;
        private const int Day = 20;

        private const string CompanyNameOr = CompanyName + FilterOperators.EqualsOperator + Parameter1 + LogicalOperators.OrOperator + CompanyName + FilterOperators.EqualsOperator + Parameter2;
        private const string CustomerNameContains = CustomerName + FilterOperators.ContainsOperator + Parameter5;
        private const string CustomerNameNotContains = CustomerName + FilterOperators.NotContainsOperator + Parameter5;

        private const string LambdaInit = "x => ";
        private const string TwelveAm = "12:00:00 AM";
        private const string ElevenPm = "11:59:59 PM";
        #endregion

        #region Private Values
        private static readonly DateTime Date = new DateTime(Year, Month, Day);

        private static readonly string ContactNameOr = $"{ContactName}{FilterOperators.ContainsOperator}{Parameter3}{LogicalOperators.OrOperator}{ContactName}{FilterOperators.ContainsOperator}{Parameter4}";

        private static readonly string ActiveEquals = $"{Active}{FilterOperators.EqualsOperator}{Parameter8}";
        private static readonly string ActiveEqualsNull = $"{Active}{FilterOperators.EqualsOperator}";
        private static readonly string ActiveContains = $"{Active}{FilterOperators.ContainsOperator}{Parameter8}";
        private static readonly string ActiveNotEqualTo = $"{Active}{FilterOperators.NotEqualToOperator}{Parameter8}";
        private static readonly string ActiveNotEqualToNull = $"{Active}{FilterOperators.NotEqualToOperator}";
        private static readonly string ActiveNotContains = $"{Active}{FilterOperators.NotContainsOperator}{Parameter8}";
        private static readonly string ActiveGreaterThan = $"{Active}{FilterOperators.GreaterThanOperator}{Parameter8}";
        private static readonly string ActiveLessThan = $"{Active}{FilterOperators.LessThanOperator}{Parameter8}";
        private static readonly string ActiveGreaterThanOrEqualTo = $"{Active}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter8}";
        private static readonly string ActiveLessThanOrEqualTo = $"{Active}{FilterOperators.LessThanOrEqualToOperator}{Parameter8}";

        private static readonly string ApprovedEquals = $"{Approved}{FilterOperators.EqualsOperator}{Parameter8}";
        private static readonly string ApprovedEqualsNull = $"{Approved}{FilterOperators.EqualsOperator}";
        private static readonly string ApprovedContains = $"{Approved}{FilterOperators.ContainsOperator}{Parameter8}";
        private static readonly string ApprovedNotEqualTo = $"{Approved}{FilterOperators.NotEqualToOperator}{Parameter8}";
        private static readonly string ApprovedNotEqualToNull = $"{Approved}{FilterOperators.NotEqualToOperator}";
        private static readonly string ApprovedNotContains = $"{Approved}{FilterOperators.NotContainsOperator}{Parameter8}";
        private static readonly string ApprovedGreaterThan = $"{Approved}{FilterOperators.GreaterThanOperator}{Parameter8}";
        private static readonly string ApprovedLessThan = $"{Approved}{FilterOperators.LessThanOperator}{Parameter8}";
        private static readonly string ApprovedGreaterThanOrEqualTo = $"{Approved}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter8}";
        private static readonly string ApprovedLessThanOrEqualTo = $"{Approved}{FilterOperators.LessThanOrEqualToOperator}{Parameter8}";

        private static readonly string LanguageIdEquals = $"{LanguageId}{FilterOperators.EqualsOperator}{Parameter12}";
        private static readonly string LanguageIdEqualsNull = $"{LanguageId}{FilterOperators.EqualsOperator}";
        private static readonly string LanguageIdContains = $"{LanguageId}{FilterOperators.ContainsOperator}{Parameter12}";
        private static readonly string LanguageIdNotEqualTo = $"{LanguageId}{FilterOperators.NotEqualToOperator}{Parameter12}";
        private static readonly string LanguageIdNotEqualToNull = $"{LanguageId}{FilterOperators.NotEqualToOperator}";
        private static readonly string LanguageIdNotContains = $"{LanguageId}{FilterOperators.NotContainsOperator}{Parameter12}";
        private static readonly string LanguageIdGreaterThan = $"{LanguageId}{FilterOperators.GreaterThanOperator}{Parameter12}";
        private static readonly string LanguageIdLessThan = $"{LanguageId}{FilterOperators.LessThanOperator}{Parameter12}";
        private static readonly string LanguageIdGreaterThanOrEqualTo = $"{LanguageId}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter12}";
        private static readonly string LanguageIdLessThanOrEqualTo = $"{LanguageId}{FilterOperators.LessThanOrEqualToOperator}{Parameter12}";

        private static readonly string ContactLanguageIdEquals = $"{ContactLanguageId}{FilterOperators.EqualsOperator}{Parameter12}";
        private static readonly string ContactLanguageIdEqualsNull = $"{ContactLanguageId}{FilterOperators.EqualsOperator}";
        private static readonly string ContactLanguageIdContains = $"{ContactLanguageId}{FilterOperators.ContainsOperator}{Parameter12}";
        private static readonly string ContactLanguageIdNotEqualTo = $"{ContactLanguageId}{FilterOperators.NotEqualToOperator}{Parameter12}";
        private static readonly string ContactLanguageIdNotEqualToNull = $"{ContactLanguageId}{FilterOperators.NotEqualToOperator}";
        private static readonly string ContactLanguageIdNotContains = $"{ContactLanguageId}{FilterOperators.NotContainsOperator}{Parameter12}";
        private static readonly string ContactLanguageIdGreaterThan = $"{ContactLanguageId}{FilterOperators.GreaterThanOperator}{Parameter12}";
        private static readonly string ContactLanguageIdLessThan = $"{ContactLanguageId}{FilterOperators.LessThanOperator}{Parameter12}";
        private static readonly string ContactLanguageIdGreaterThanOrEqualTo = $"{ContactLanguageId}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter12}";
        private static readonly string ContactLanguageIdLessThanOrEqualTo = $"{ContactLanguageId}{FilterOperators.LessThanOrEqualToOperator}{Parameter12}";

        private static readonly string IdEquals = $"{Id}{FilterOperators.EqualsOperator}{Parameter11}";
        private static readonly string IdEqualsNull = $"{Id}{FilterOperators.EqualsOperator}";
        private static readonly string IdContains = $"{Id}{FilterOperators.ContainsOperator}{Parameter11}";
        private static readonly string IdNotEqualTo = $"{Id}{FilterOperators.NotEqualToOperator}{Parameter11}";
        private static readonly string IdNotEqualToNull = $"{Id}{FilterOperators.NotEqualToOperator}";
        private static readonly string IdNotContains = $"{Id}{FilterOperators.NotContainsOperator}{Parameter11}";
        private static readonly string IdGreaterThan = $"{Id}{FilterOperators.GreaterThanOperator}{Parameter11}";
        private static readonly string IdLessThan = $"{Id}{FilterOperators.LessThanOperator}{Parameter11}";
        private static readonly string IdGreaterThanOrEqualTo = $"{Id}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter11}";
        private static readonly string IdLessThanOrEqualTo = $"{Id}{FilterOperators.LessThanOrEqualToOperator}{Parameter11}";

        private static readonly string AssignedEmployeeIdEquals = $"{AssignedEmployeeId}{FilterOperators.EqualsOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdEqualsNull = $"{AssignedEmployeeId}{FilterOperators.EqualsOperator}";
        private static readonly string AssignedEmployeeIdContains = $"{AssignedEmployeeId}{FilterOperators.ContainsOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdNotEqualTo = $"{AssignedEmployeeId}{FilterOperators.NotEqualToOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdNotEqualToNull = $"{AssignedEmployeeId}{FilterOperators.NotEqualToOperator}";
        private static readonly string AssignedEmployeeIdNotContains = $"{AssignedEmployeeId}{FilterOperators.NotContainsOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdGreaterThan = $"{AssignedEmployeeId}{FilterOperators.GreaterThanOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdLessThan = $"{AssignedEmployeeId}{FilterOperators.LessThanOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdGreaterThanOrEqualTo = $"{AssignedEmployeeId}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter11}";
        private static readonly string AssignedEmployeeIdLessThanOrEqualTo = $"{AssignedEmployeeId}{FilterOperators.LessThanOrEqualToOperator}{Parameter11}";

        private static readonly string ManagerIdEquals = $"{ManagerId}{FilterOperators.EqualsOperator}{Parameter7}";
        private static readonly string ManagerIdEqualsNull = $"{ManagerId}{FilterOperators.EqualsOperator}";
        private static readonly string ManagerIdContains = $"{ManagerId}{FilterOperators.ContainsOperator}{Parameter7}";
        private static readonly string ManagerIdNotEqualTo = $"{ManagerId}{FilterOperators.NotEqualToOperator}{Parameter7}";
        private static readonly string ManagerIdNotEqualToNull = $"{ManagerId}{FilterOperators.NotEqualToOperator}";
        private static readonly string ManagerIdNotContains = $"{ManagerId}{FilterOperators.NotContainsOperator}{Parameter7}";
        private static readonly string ManagerIdGreaterThan = $"{ManagerId}{FilterOperators.GreaterThanOperator}{Parameter7}";
        private static readonly string ManagerIdLessThan = $"{ManagerId}{FilterOperators.LessThanOperator}{Parameter7}";
        private static readonly string ManagerIdGreaterThanOrEqualTo = $"{ManagerId}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter7}";
        private static readonly string ManagerIdLessThanOrEqualTo = $"{ManagerId}{FilterOperators.LessThanOrEqualToOperator}{Parameter7}";

        private static readonly string EmailIdEquals = $"{EmailId}{FilterOperators.EqualsOperator}{Parameter7}";
        private static readonly string EmailIdEqualsNull = $"{EmailId}{FilterOperators.EqualsOperator}";
        private static readonly string EmailIdContains = $"{EmailId}{FilterOperators.ContainsOperator}{Parameter7}";
        private static readonly string EmailIdNotEqualTo = $"{EmailId}{FilterOperators.NotEqualToOperator}{Parameter7}";
        private static readonly string EmailIdNotEqualToNull = $"{EmailId}{FilterOperators.NotEqualToOperator}";
        private static readonly string EmailIdNotContains = $"{EmailId}{FilterOperators.NotContainsOperator}{Parameter7}";
        private static readonly string EmailIdGreaterThan = $"{EmailId}{FilterOperators.GreaterThanOperator}{Parameter7}";
        private static readonly string EmailIdLessThan = $"{EmailId}{FilterOperators.LessThanOperator}{Parameter7}";
        private static readonly string EmailIdGreaterThanOrEqualTo = $"{EmailId}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter7}";
        private static readonly string EmailIdLessThanOrEqualTo = $"{EmailId}{FilterOperators.LessThanOrEqualToOperator}{Parameter7}";

        private static readonly string TotalServiceValueEquals = $"{TotalServiceValue}{FilterOperators.EqualsOperator}{Parameter10}";
        private static readonly string TotalServiceValueEqualsNull = $"{TotalServiceValue}{FilterOperators.EqualsOperator}";
        private static readonly string TotalServiceValueContains = $"{TotalServiceValue}{FilterOperators.ContainsOperator}{Parameter10}";
        private static readonly string TotalServiceValueNotEqualTo = $"{TotalServiceValue}{FilterOperators.NotEqualToOperator}{Parameter10}";
        private static readonly string TotalServiceValueNotEqualToNull = $"{TotalServiceValue}{FilterOperators.NotEqualToOperator}";
        private static readonly string TotalServiceValueNotContains = $"{TotalServiceValue}{FilterOperators.NotContainsOperator}{Parameter10}";
        private static readonly string TotalServiceValueGreaterThan = $"{TotalServiceValue}{FilterOperators.GreaterThanOperator}{Parameter10}";
        private static readonly string TotalServiceValueLessThan = $"{TotalServiceValue}{FilterOperators.LessThanOperator}{Parameter10}";
        private static readonly string TotalServiceValueGreaterThanOrEqualTo = $"{TotalServiceValue}{FilterOperators.GreaterThanOrEqualToOperator}{Parameter10}";
        private static readonly string TotalServiceValueLessThanOrEqualTo = $"{TotalServiceValue}{FilterOperators.LessThanOrEqualToOperator}{Parameter10}";


        private static readonly string CreatedOnEquals = $"{CreatedOn}{FilterOperators.EqualsOperator}{Date}";
        private static readonly string CreatedOnEqualsNull = $"{CreatedOn}{FilterOperators.EqualsOperator}";
        private static readonly string CreatedOnContains = $"{CreatedOn}{FilterOperators.ContainsOperator}{Date}";
        private static readonly string CreatedOnNotEqualTo = $"{CreatedOn}{FilterOperators.NotEqualToOperator}{Date}";
        private static readonly string CreatedOnNotEqualToNull = $"{CreatedOn}{FilterOperators.NotEqualToOperator}";
        private static readonly string CreatedOnNotContains = $"{CreatedOn}{FilterOperators.NotContainsOperator}{Date}";
        private static readonly string CreatedOnGreaterThan = $"{CreatedOn}{FilterOperators.GreaterThanOperator}{Date}";
        private static readonly string CreatedOnLessThan = $"{CreatedOn}{FilterOperators.LessThanOperator}{Date}";
        private static readonly string CreatedOnGreaterThanOrEqualTo = $"{CreatedOn}{FilterOperators.GreaterThanOrEqualToOperator}{Date}";
        private static readonly string CreatedOnLessThanOrEqualTo = $"{CreatedOn}{FilterOperators.LessThanOrEqualToOperator}{Date}";

        private static readonly string ExpirationDateOnEquals = $"{ExpirationDate}{FilterOperators.EqualsOperator}{Date}";
        private static readonly string ExpirationDateOnEqualsNull = $"{ExpirationDate}{FilterOperators.EqualsOperator}";
        private static readonly string ExpirationDateContains = $"{ExpirationDate}{FilterOperators.ContainsOperator}{Date}";
        private static readonly string ExpirationDateNotEqualTo = $"{ExpirationDate}{FilterOperators.NotEqualToOperator}{Date}";
        private static readonly string ExpirationDateNotEqualToNull = $"{ExpirationDate}{FilterOperators.NotEqualToOperator}";
        private static readonly string ExpirationDateNotContains = $"{ExpirationDate}{FilterOperators.NotContainsOperator}{Date}";
        private static readonly string ExpirationDateGreaterThan = $"{ExpirationDate}{FilterOperators.GreaterThanOperator}{Date}";
        private static readonly string ExpirationDateLessThan = $"{ExpirationDate}{FilterOperators.LessThanOperator}{Date}";
        private static readonly string ExpirationDateGreaterThanOrEqualTo = $"{ExpirationDate}{FilterOperators.GreaterThanOrEqualToOperator}{Date}";
        private static readonly string ExpirationDateLessThanOrEqualTo = $"{ExpirationDate}{FilterOperators.LessThanOrEqualToOperator}{Date}";

        private static readonly string LanguageNameOnEquals = $"{LanguageName}{FilterOperators.EqualsOperator}{Parameter13}";
        private static readonly string LanguageNameContains = $"{LanguageName}{FilterOperators.ContainsOperator}{Parameter13}";
        private static readonly string LanguageNameNotContains = $"{LanguageName}{FilterOperators.NotContainsOperator}{Parameter13}";

        private static readonly string ExpressionStringApprovedEquals = $"{LambdaInit}(x.{Approved}.HasValue AndAlso (x.{Approved}.Value == {Parameter8}))";
        private static readonly string ExpressionStringApprovedNotEqualTo = $"{LambdaInit}(x.{Approved}.HasValue AndAlso (x.{Approved}.Value != {Parameter8}))";

        private static readonly string ExpressionStringLanguageIdEquals = $"{LambdaInit}(x.{LanguageId} == {Parameter12})";
        private static readonly string ExpressionStringLanguageIdNotEqualTo = $"{LambdaInit}(x.{LanguageId} != {Parameter12})";
        private static readonly string ExpressionStringLanguageIdGreaterThan = $"{LambdaInit}(x.{LanguageId} > {Parameter12})";
        private static readonly string ExpressionStringLanguageIdLessThan = $"{LambdaInit}(x.{LanguageId} < {Parameter12})";
        private static readonly string ExpressionStringLanguageIdGreaterThanOrEqualTo = $"{LambdaInit}(x.{LanguageId} >= {Parameter12})";
        private static readonly string ExpressionStringLanguageIdLessThanOrEqualTo = $"{LambdaInit}(x.{LanguageId} <= {Parameter12})";

        private static readonly string ExpressionStringContactLanguageIdEquals = $"{LambdaInit}(x.{ContactLanguageId}.HasValue AndAlso (x.{ContactLanguageId}.Value == {Parameter12}))";
        private static readonly string ExpressionStringContactLanguageIdNotEqualTo = $"{LambdaInit}(x.{ContactLanguageId}.HasValue AndAlso (x.{ContactLanguageId}.Value != {Parameter12}))";
        private static readonly string ExpressionStringContactLanguageIdGreaterThan = $"{LambdaInit}(x.{ContactLanguageId}.HasValue AndAlso (x.{ContactLanguageId}.Value > {Parameter12}))";
        private static readonly string ExpressionStringContactLanguageIdLessThan = $"{LambdaInit}(x.{ContactLanguageId}.HasValue AndAlso (x.{ContactLanguageId}.Value < {Parameter12}))";
        private static readonly string ExpressionStringContactLanguageIdGreaterThanOrEqualTo = $"{LambdaInit}(x.{ContactLanguageId}.HasValue AndAlso (x.{ContactLanguageId}.Value >= {Parameter12}))";
        private static readonly string ExpressionStringContactLanguageIdLessThanOrEqualTo = $"{LambdaInit}(x.{ContactLanguageId}.HasValue AndAlso (x.{ContactLanguageId}.Value <= {Parameter12}))";

        private static readonly string ExpressionStringAssignedEmployeeIdEquals = $"{LambdaInit}(x.{AssignedEmployeeId}.HasValue AndAlso (x.{AssignedEmployeeId}.Value == {Parameter11}))";
        private static readonly string ExpressionStringAssignedEmployeeIdNotEqualTo = $"{LambdaInit}(x.{AssignedEmployeeId}.HasValue AndAlso (x.{AssignedEmployeeId}.Value != {Parameter11}))";
        private static readonly string ExpressionStringAssignedEmployeeIdGreaterThan = $"{LambdaInit}(x.{AssignedEmployeeId}.HasValue AndAlso (x.{AssignedEmployeeId}.Value > {Parameter11}))";
        private static readonly string ExpressionStringAssignedEmployeeIdLessThan = $"{LambdaInit}(x.{AssignedEmployeeId}.HasValue AndAlso (x.{AssignedEmployeeId}.Value < {Parameter11}))";
        private static readonly string ExpressionStringAssignedEmployeeIdGreaterThanOrEqualTo = $"{LambdaInit}(x.{AssignedEmployeeId}.HasValue AndAlso (x.{AssignedEmployeeId}.Value >= {Parameter11}))";
        private static readonly string ExpressionStringAssignedEmployeeIdLessThanOrEqualTo = $"{LambdaInit}(x.{AssignedEmployeeId}.HasValue AndAlso (x.{AssignedEmployeeId}.Value <= {Parameter11}))";

        private static readonly string ExpressionStringEmailIdEquals = $"{LambdaInit}(x.{EmailId}.HasValue AndAlso (x.{EmailId}.Value == {Parameter7}))";
        private static readonly string ExpressionStringEmailIdNotEqualTo = $"{LambdaInit}(x.{EmailId}.HasValue AndAlso (x.{EmailId}.Value != {Parameter7}))";
        private static readonly string ExpressionStringEmailIdGreaterThan = $"{LambdaInit}(x.{EmailId}.HasValue AndAlso (x.{EmailId}.Value > {Parameter7}))";
        private static readonly string ExpressionStringEmailIdLessThan = $"{LambdaInit}(x.{EmailId}.HasValue AndAlso (x.{EmailId}.Value < {Parameter7}))";
        private static readonly string ExpressionStringEmailIdGreaterThanOrEqualTo = $"{LambdaInit}(x.{EmailId}.HasValue AndAlso (x.{EmailId}.Value >= {Parameter7}))";
        private static readonly string ExpressionStringEmailIdLessThanOrEqualTo = $"{LambdaInit}(x.{EmailId}.HasValue AndAlso (x.{EmailId}.Value <= {Parameter7}))";

        private static readonly string ExpressionStringCreatedOnEquals = GetDateExpressionString(Date);
        private static readonly string ExpressionStringCreatedOnNotEqualTo = $"{LambdaInit}((x.{CreatedOn} < {GetDateString(Date)} {TwelveAm}) AndAlso (x.{CreatedOn} >= {GetDateString(Date.AddDays(1))} {TwelveAm}))";
        private static readonly string ExpressionStringCreatedOnGreaterThan = $"{LambdaInit}(x.{CreatedOn} > {GetDateString(Date)} {ElevenPm})";
        private static readonly string ExpressionStringCreatedOnLessThan = $"{LambdaInit}(x.{CreatedOn} < {GetDateString(Date)} {TwelveAm})";
        private static readonly string ExpressionStringCreatedOnGreaterThanOrEqualTo = $"{LambdaInit}(x.{CreatedOn} >= {GetDateString(Date)} {TwelveAm})";
        private static readonly string ExpressionStringCreatedOnLessThanOrEqualTo = $"{LambdaInit}(x.{CreatedOn} <= {GetDateString(Date)} {ElevenPm})";

        private static readonly string ExpressionStringExpirationDateEquals = $"{LambdaInit}(x.{ExpirationDate}.HasValue AndAlso ((x.{ExpirationDate}.Value >= {GetDateString(Date)} {TwelveAm}) AndAlso (x.{ExpirationDate}.Value <= {GetDateString(Date)} {ElevenPm})))";
        private static readonly string ExpressionStringExpirationDateNotEqualTo = $"{LambdaInit}((x.{ExpirationDate}.HasValue AndAlso (x.{ExpirationDate}.Value < {GetDateString(Date)} {TwelveAm})) AndAlso (x.{ExpirationDate}.HasValue AndAlso (x.{ExpirationDate}.Value >= {GetDateString(Date.AddDays(1))} {TwelveAm})))";
        private static readonly string ExpressionStringExpirationDateGreaterThan = $"{LambdaInit}(x.{ExpirationDate}.HasValue AndAlso (x.{ExpirationDate}.Value > {GetDateString(Date)} {ElevenPm}))";
        private static readonly string ExpressionStringExpirationDateLessThan = $"{LambdaInit}(x.{ExpirationDate}.HasValue AndAlso (x.{ExpirationDate}.Value < {GetDateString(Date)} {TwelveAm}))";
        private static readonly string ExpressionStringExpirationDateGreaterThanOrEqualTo = $"{LambdaInit}(x.{ExpirationDate}.HasValue AndAlso (x.{ExpirationDate}.Value >= {GetDateString(Date)} {TwelveAm}))";
        private static readonly string ExpressionStringExpirationDateLessThanOrEqualTo = $"{LambdaInit}(x.{ExpirationDate}.HasValue AndAlso (x.{ExpirationDate}.Value <= {GetDateString(Date)} {ElevenPm}))";

        private static readonly string ExpressionStringLanguageNameEquals = $"{LambdaInit}((x.{Language} != null) AndAlso ((x.{LanguageName} != null) AndAlso (x.{LanguageName}.Trim().ToLower() == \"{Parameter13}\".Trim().ToLower())))";
        private static readonly string ExpressionStringLanguageNameContains = $"{LambdaInit}((x.{Language} != null) AndAlso ((x.{LanguageName} != null) AndAlso x.{LanguageName}.Trim().ToLower().Contains(\"{Parameter13}\".Trim().ToLower())))";
        private static readonly string ExpressionStringLanguageNameNotContains = $"{LambdaInit}((x.{Language} != null) AndAlso ((x.{LanguageName} != null) AndAlso Not(x.{LanguageName}.Trim().ToLower().Contains(\"{Parameter13}\".Trim().ToLower()))))";
        #endregion

        #region Expressions
        private static readonly Expression<Func<Account, bool>> ExpressionCompanyNameOr = x => x.CompanyName != null && x.CompanyName.Trim().ToLower() == Parameter1.Trim().ToLower() || x.CompanyName != null && x.CompanyName.Trim().ToLower() == Parameter2.Trim().ToLower();
        private static readonly Expression<Func<Account, bool>> ExpressionCompanyNameOrNoTrim = x => x.CompanyName != null && x.CompanyName.ToLower() == Parameter1.ToLower() || x.CompanyName != null && x.CompanyName.ToLower() == Parameter2.ToLower();
        private static readonly Expression<Func<Account, bool>> ExpressionContactNameOr = x => x.ContactName != null && x.ContactName.Trim().ToLower().Contains(Parameter3.Trim().ToLower()) || x.ContactName != null && x.ContactName.Trim().ToLower().Contains(Parameter4.Trim().ToLower());
        private static readonly Expression<Func<Account, bool>> ExpressionContactNameOrNoTrim = x => x.ContactName != null && x.ContactName.ToLower().Contains(Parameter3.ToLower()) || x.ContactName != null && x.ContactName.ToLower().Contains(Parameter4.ToLower());
        private static readonly Expression<Func<Account, bool>> ExpressionCustomerNameContains = x => x.CustomerName != null && x.CustomerName.Trim().ToLower().Contains(Parameter5.Trim().ToLower());
        private static readonly Expression<Func<Account, bool>> ExpressionCustomerNameContainsNoTrim = x => x.CustomerName != null && x.CustomerName.ToLower().Contains(Parameter5.ToLower());
        private static readonly Expression<Func<Account, bool>> ExpressionCustomerNameNotContains = x => x.CustomerName != null && !x.CustomerName.Trim().ToLower().Contains(Parameter5.Trim().ToLower());
        private static readonly Expression<Func<Account, bool>> ExpressionCustomerNameNotContainsNoTrim = x => x.CustomerName != null && !x.CustomerName.ToLower().Contains(Parameter5.ToLower());

        private static readonly Expression<Func<Account, bool>> ExpressionActiveEquals = x => x.Active == Parameter8;
        private static readonly Expression<Func<Account, bool>> ExpressionActiveNotEqualTo = x => x.Active != Parameter8;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionApprovedEquals = x => x.Approved.HasValue && x.Approved == Parameter8;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionApprovedEqualsNull = x => x.Approved == null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionApprovedNotEqualTo = x => x.Approved.HasValue && x.Approved != Parameter8;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionApprovedNotEqualToNull = x => x.Approved != null;

        private static readonly Expression<Func<Account, bool>> ExpressionLanguageIdEquals = x => x.LanguageId == Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageIdNotEqualTo = x => x.LanguageId != Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageIdGreaterThan = x => x.LanguageId > Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageIdLessThan = x => x.LanguageId < Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageIdGreaterThanOrEqualTo = x => x.LanguageId >= Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageIdLessThanOrEqualTo = x => x.LanguageId <= Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdEquals = x => x.ContactLanguageId.HasValue && x.ContactLanguageId == Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdEqualsNull = x => x.ContactLanguageId == null;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdNotEqualTo = x => x.ContactLanguageId.HasValue && x.ContactLanguageId != Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdNotEqualToNull = x => x.ContactLanguageId != null;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdGreaterThan = x => x.ContactLanguageId.HasValue && x.ContactLanguageId > Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdLessThan = x => x.ContactLanguageId.HasValue && x.ContactLanguageId < Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdGreaterThanOrEqualTo = x => x.ContactLanguageId.HasValue && x.ContactLanguageId >= Parameter12;
        private static readonly Expression<Func<Account, bool>> ExpressionContactLanguageIdLessThanOrEqualTo = x => x.ContactLanguageId.HasValue && x.ContactLanguageId <= Parameter12;

        private static readonly Expression<Func<AccountValue, bool>> ExpressionIdEquals = x => x.Id == Parameter11;
        private static readonly Expression<Func<AccountValue, bool>> ExpressionIdNotEqualTo = x => x.Id != Parameter11;
        private static readonly Expression<Func<AccountValue, bool>> ExpressionIdGreaterThan = x => x.Id > Parameter11;
        private static readonly Expression<Func<AccountValue, bool>> ExpressionIdLessThan = x => x.Id < Parameter11;
        private static readonly Expression<Func<AccountValue, bool>> ExpressionIdGreaterThanOrEqualTo = x => x.Id >= Parameter11;
        private static readonly Expression<Func<AccountValue, bool>> ExpressionIdLessThanOrEqualTo = x => x.Id <= Parameter11;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdEquals = x => x.AssignedEmployeeId.HasValue && x.AssignedEmployeeId == Parameter11;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdEqualsNull = x => x.AssignedEmployeeId == null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdNotEqualTo = x => x.AssignedEmployeeId.HasValue && x.AssignedEmployeeId != Parameter11;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdNotEqualToNull = x => x.AssignedEmployeeId != null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdGreaterThan = x => x.AssignedEmployeeId.HasValue && x.AssignedEmployeeId > Parameter11;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdLessThan = x => x.AssignedEmployeeId.HasValue && x.AssignedEmployeeId < Parameter11;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdGreaterThanOrEqualTo = x => x.AssignedEmployeeId.HasValue && x.AssignedEmployeeId >= Parameter11;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionAssignedEmployeeIdLessThanOrEqualTo = x => x.AssignedEmployeeId.HasValue && x.AssignedEmployeeId <= Parameter11;

        private static readonly Expression<Func<Opportunity, bool>> ExpressionManagerIdEquals = x => x.ManagerId == Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionManagerIdNotEqualTo = x => x.ManagerId != Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionManagerIdGreaterThan = x => x.ManagerId > Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionManagerIdLessThan = x => x.ManagerId < Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionManagerIdGreaterThanOrEqualTo = x => x.ManagerId >= Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionManagerIdLessThanOrEqualTo = x => x.ManagerId <= Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdEquals = x => x.EmailId.HasValue && x.EmailId != Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdEqualsNull = x => x.EmailId == null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdNotEqualTo = x => x.EmailId.HasValue && x.EmailId != Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdNotEqualToNull = x => x.EmailId != null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdGreaterThan = x => x.EmailId.HasValue && x.EmailId > Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdLessThan = x => x.EmailId.HasValue && x.EmailId < Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdGreaterThanOrEqualTo = x => x.EmailId.HasValue && x.EmailId >= Parameter7;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionEmailIdLessThanOrEqualTo = x => x.EmailId.HasValue && x.EmailId <= Parameter7;

        private static readonly Expression<Func<Account, bool>> ExpressionTotalServiceValueEquals = x => x.TotalServiceValue == Parameter10;
        private static readonly Expression<Func<Account, bool>> ExpressionTotalServiceValueNotEqualTo = x => x.TotalServiceValue != Parameter10;
        private static readonly Expression<Func<Account, bool>> ExpressionTotalServiceValueGreaterThan = x => x.TotalServiceValue > Parameter10;
        private static readonly Expression<Func<Account, bool>> ExpressionTotalServiceValueLessThan = x => x.TotalServiceValue < Parameter10;
        private static readonly Expression<Func<Account, bool>> ExpressionTotalServiceValueGreaterThanOrEqualTo = x => x.TotalServiceValue >= Parameter10;
        private static readonly Expression<Func<Account, bool>> ExpressionTotalServiceValueLessThanOrEqualTo = x => x.TotalServiceValue <= Parameter10;

        private static readonly Expression<Func<Account, bool>> ExpressionCreatedOnEquals = x => x.CreatedOn >= new DateTime(Year, Month, Day) && x.CreatedOn <= new DateTime(Year, Month, Day).AddDays(1).AddMilliseconds(-1);
        private static readonly Expression<Func<Account, bool>> ExpressionCreatedOnNotEqualTo = x => x.CreatedOn < new DateTime(Year, Month, Day) && x.CreatedOn >= new DateTime(Year, Month, Day).AddDays(1);
        private static readonly Expression<Func<Account, bool>> ExpressionCreatedOnGreaterThan = x => x.CreatedOn > new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Account, bool>> ExpressionCreatedOnLessThan = x => x.CreatedOn < new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Account, bool>> ExpressionCreatedOnGreaterThanOrEqualTo = x => x.CreatedOn >= new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Account, bool>> ExpressionCreatedOnLessThanOrEqualTo = x => x.CreatedOn <= new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateOnEquals = x => x.ExpirationDate.HasValue && x.ExpirationDate.Value >= new DateTime(Year, Month, Day) && x.ExpirationDate.Value <= new DateTime(Year, Month, Day).AddDays(1).AddMilliseconds(-1);
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateOnEqualsNull = x => x.ExpirationDate == null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateOnNotEqualTo = x => x.ExpirationDate.HasValue && x.ExpirationDate.Value < new DateTime(Year, Month, Day) && x.ExpirationDate.Value >= new DateTime(Year, Month, Day).AddDays(1);
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateOnNotEqualToNull = x => x.ExpirationDate != null;
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateGreaterThan = x => x.ExpirationDate.HasValue && x.ExpirationDate > new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateLessThan = x => x.ExpirationDate.HasValue && x.ExpirationDate < new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateGreaterThanOrEqualTo = x => x.ExpirationDate.HasValue && x.ExpirationDate >= new DateTime(Year, Month, Day);
        private static readonly Expression<Func<Opportunity, bool>> ExpressionExpirationDateLessThanOrEqualTo = x => x.ExpirationDate.HasValue && x.ExpirationDate <= new DateTime(Year, Month, Day);

        private static readonly Expression<Func<Account, bool>> ExpressionLanguageNameEquals = x => x.Language != null && x.Language.Name != null && x.Language.Name.Trim().ToLower() == Parameter13.Trim().ToLower();
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageNameContains = x => x.Language != null && x.Language.Name != null && x.Language.Name.Trim().ToLower().Contains(Parameter13.Trim().ToLower());
        private static readonly Expression<Func<Account, bool>> ExpressionLanguageNameNotContains = x => x.Language != null && x.Language.Name != null && !x.Language.Name.Trim().ToLower().Contains(Parameter13.Trim().ToLower());
        #endregion

        [TestCaseSource(nameof(TestCasesGetFilterExpressionEquals))]
        public void ShouldGetFilterExpressionEquals(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionEqualsNull))]
        public void ShouldGetFilterExpressionEqualsNull(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionEqualsNullThrowException))]
        public void ShouldGetFilterExpressionEqualsNullThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedNullValueException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionContains))]
        public void ShouldGetFilterExpressionContains(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionContainsThrowException))]
        public void ShouldGetFilterExpressionContainsThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedTypeOnOperationException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionNotEqualTo))]
        public void ShouldGetFilterExpressionNotEqualTo(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionNotEqualToNull))]
        public void ShouldGetFilterExpressionNotEqualToNull(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionNotEqualToNullThrowException))]
        public void ShouldGetFilterExpressionNotEqualToNullThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedNullValueException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionNotContains))]
        public void ShouldGetFilterExpressionNotContains(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionNotContainsThrowException))]
        public void ShouldGetFilterExpressionNotContainsThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedTypeOnOperationException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionGreaterThan))]
        public void ShouldGetFilterExpressionGreaterThan(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionGreaterThanThrowException))]
        public void ShouldGetFilterExpressionGreaterThanThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedTypeOnOperationException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionLessThan))]
        public void ShouldGetFilterExpressionLessThan(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionLessThanThrowException))]
        public void ShouldGetFilterExpressionLessThanThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedTypeOnOperationException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionGreaterThanOrEqualTo))]
        public void ShouldGetFilterExpressionGreaterThanOrEqualTo(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionGreaterThanOrEqualToThrowException))]
        public void ShouldGetFilterExpressionGreaterThanOrEqualToThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedTypeOnOperationException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionLessThanOrEqualTo))]
        public void ShouldGetFilterExpressionLessThanOrEqualTo(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionLessThanOrEqualToThrowException))]
        public void ShouldGetFilterExpressionLessThanOrEqualToThrowException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<UnsupportedTypeOnOperationException>();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionMultipleExpressions))]
        public void ShouldGetFilterExpressionMultipleExpressions(ITester tester)
        {
            tester.RunGetFilterExpression();
        }

        [TestCaseSource(nameof(TestCasesGetFilterExpressionThrowTypeParseException))]
        public void ShouldGetFilterExpressionThrowTypeParseException(ITester tester)
        {
            tester.RunThrowExceptionOnGetFilterExpression<TypeParseException>();
        }

        [TestCase(2019, 3, 12, 17, 5, 58)]
        [TestCase(2019, 3, 12, 17, 30, 34)]
        [TestCase(2019, 3, 12, 0, 0, 0)]
        public void ShouldGetFilterExpressionDateTimeConvertedToDate(int year, int month, int day, int hour, int minute, int second)
        {
            var dateTime = new DateTime(year, month, day, hour, minute, second);
            var expectedValue = GetDateExpressionString(dateTime);

            var filterExpression = FilterOptionExtensions.GetFilterExpression<Account>($"{CreatedOn}=={dateTime}");
            Assert.IsNotNull(filterExpression);

            var expressionString = filterExpression.ToString();
            Assert.AreEqual(expressionString, expectedValue);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldGetFilterExpressionReturnDefault(string parameter)
        {
            Expression<Func<Account, bool>> expectedExpression = x => true;

            var filterExpression = FilterOptionExtensions.GetFilterExpression<Account>(parameter);
            Assert.IsNotNull(filterExpression);

            var filterExpressionString = filterExpression.ToString();
            var expectedExpressionString = expectedExpression.ToString();
            Assert.AreEqual(filterExpressionString, expectedExpressionString);
        }

        [TestCase("foo")]
        [TestCase("foo=|bar")]
        [TestCase("foo**bar")]
        [TestCase("foo=&bar")]
        [TestCase(CompanyNameOr + LogicalOperators.AndOperator + "foo")]
        public void ShouldGetFilterExpressionThrowUnsupportedOperatorException(string parameter)
        {
            Assert.Throws<UnsupportedOperatorException>(() => FilterOptionExtensions.GetFilterExpression<Account>(parameter));
        }

        [TestCase(FilterOperators.EqualsOperator)]
        [TestCase(FilterOperators.EqualsOperator + "bar")]
        [TestCase(FilterOperators.ContainsOperator)]
        [TestCase("foo" + FilterOperators.ContainsOperator)]
        [TestCase(FilterOperators.ContainsOperator + "bar")]
        [TestCase(FilterOperators.NotEqualToOperator)]
        [TestCase(FilterOperators.NotEqualToOperator + "bar")]
        [TestCase(FilterOperators.NotContainsOperator)]
        [TestCase("foo" + FilterOperators.NotContainsOperator)]
        [TestCase(FilterOperators.NotContainsOperator + "bar")]
        [TestCase(FilterOperators.GreaterThanOperator)]
        [TestCase("foo" + FilterOperators.GreaterThanOperator)]
        [TestCase(FilterOperators.GreaterThanOperator + "bar")]
        [TestCase(FilterOperators.LessThanOperator)]
        [TestCase("foo" + FilterOperators.LessThanOperator)]
        [TestCase(FilterOperators.LessThanOperator + "bar")]
        [TestCase(FilterOperators.GreaterThanOrEqualToOperator)]
        [TestCase("foo" + FilterOperators.GreaterThanOrEqualToOperator)]
        [TestCase(FilterOperators.GreaterThanOrEqualToOperator + "bar")]
        [TestCase(FilterOperators.LessThanOrEqualToOperator)]
        [TestCase("foo" + FilterOperators.LessThanOrEqualToOperator)]
        [TestCase(FilterOperators.LessThanOrEqualToOperator + "bar")]
        [TestCase(CompanyNameOr + LogicalOperators.AndOperator + FilterOperators.EqualsOperator)]
        [TestCase(CompanyNameOr + LogicalOperators.OrOperator + FilterOperators.EqualsOperator)]
        [TestCase("foo" + FilterOperators.EqualsOperator + "bar" + FilterOperators.ContainsOperator + "bar" + FilterOperators.EqualsOperator + "foo")]
        [TestCase("foo" + FilterOperators.NotEqualToOperator + "bar" + FilterOperators.EqualsOperator + "bar" + FilterOperators.EqualsOperator + "foo")]
        [TestCase("foo" + FilterOperators.GreaterThanOperator + "bar" + FilterOperators.GreaterThanOrEqualToOperator + "bar" + FilterOperators.GreaterThanOrEqualToOperator + "foo")]
        public void ShouldGetFilterExpressionThrowMissingValueOnOperatorException(string parameter)
        {
            Assert.Throws<MissingValueOnOperatorException>(() => FilterOptionExtensions.GetFilterExpression<Account>(parameter));
        }

        [TestCase(Id)]
        [TestCase(Active)]
        [TestCase(CompanyName)]
        [TestCase(ContactName)]
        public void ShouldGetFilterExpressionThrowUnsupportedFilterPropertyException(string propertyName)
        {
            var filter = $"{propertyName}{FilterOperators.EqualsOperator}foo";
            var whitelist = new Dictionary<string, string>
            {
                { "foo", propertyName }
            };
            var exceptionMessage = $"The property [{propertyName}] for type [{nameof(Account)}] is invalid.";
            var unsupportedFilterPropertyException = Assert.Catch<UnsupportedFilterPropertyException>(() => FilterOptionExtensions.GetFilterExpression<Account>(filter, whitelist));
            Assert.IsNotNull(unsupportedFilterPropertyException);
            Assert.AreEqual(unsupportedFilterPropertyException.Message, exceptionMessage);
        }

        #region Test Cases
        public static IEnumerable<ITester> TestCasesGetFilterExpressionEquals()
        {
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionManagerIdEquals,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdEquals,
                ExpectedExpressionString = ExpressionStringEmailIdEquals
            };
            yield return new Tester<Account>
            {
                Filters = ActiveEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionActiveEquals,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionApprovedEquals,
                ExpectedExpressionString = ExpressionStringApprovedEquals
            };
            //TODO: Create test case for decimal? type (couldn't find an entity with decimal? type property)
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionTotalServiceValueEquals,
                ExpectedExpressionString = null
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionIdEquals,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdEquals,
                ExpectedExpressionString = ExpressionStringAssignedEmployeeIdEquals
            };
            yield return new Tester<Account>
            {
                Filters = LanguageIdEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageIdEquals,
                ExpectedExpressionString = ExpressionStringLanguageIdEquals
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdEquals,
                ExpectedExpressionString = ExpressionStringContactLanguageIdEquals
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionCreatedOnEquals,
                ExpectedExpressionString = ExpressionStringCreatedOnEquals
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateOnEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateOnEquals,
                ExpectedExpressionString = ExpressionStringExpirationDateEquals
            };
            yield return new Tester<Account>
            {
                Filters = LanguageNameOnEquals,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageNameEquals,
                ExpectedExpressionString = ExpressionStringLanguageNameEquals
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionEqualsNull()
        {
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedEqualsNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionApprovedEqualsNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdEqualsNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdEqualsNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdEqualsNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdEqualsNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdEqualsNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdEqualsNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateOnEqualsNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateOnEqualsNull,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionEqualsNullThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveEqualsNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageIdEqualsNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = IdEqualsNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueEqualsNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnEqualsNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionContains()
        {
            yield return new Tester<Account>
            {
                Filters = CustomerNameContains,
                DisableTrim = false,
                ExpectedExpression = ExpressionCustomerNameContains,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CustomerNameContains,
                DisableTrim = true,
                ExpectedExpression = ExpressionCustomerNameContainsNoTrim,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageNameContains,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageNameContains,
                ExpectedExpressionString = ExpressionStringLanguageNameContains
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionContainsThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageIdContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ManagerIdContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionNotEqualTo()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionActiveNotEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionApprovedNotEqualTo,
                ExpectedExpressionString = ExpressionStringApprovedNotEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = LanguageIdNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageIdNotEqualTo,
                ExpectedExpressionString = ExpressionStringLanguageIdNotEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdNotEqualTo,
                ExpectedExpressionString = ExpressionStringContactLanguageIdNotEqualTo
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionIdNotEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdNotEqualTo,
                ExpectedExpressionString = ExpressionStringAssignedEmployeeIdNotEqualTo
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionManagerIdNotEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdNotEqualTo,
                ExpectedExpressionString = ExpressionStringEmailIdNotEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionTotalServiceValueNotEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionCreatedOnNotEqualTo,
                ExpectedExpressionString = ExpressionStringCreatedOnNotEqualTo
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateNotEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateOnNotEqualTo,
                ExpectedExpressionString = ExpressionStringExpirationDateNotEqualTo
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionNotEqualToNull()
        {
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionApprovedNotEqualToNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdNotEqualToNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdNotEqualToNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdNotEqualToNull,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateOnNotEqualToNull,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionNotEqualToNullThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageIdNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = IdNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueNotEqualToNull,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionNotContains()
        {
            yield return new Tester<Account>
            {
                Filters = CustomerNameNotContains,
                DisableTrim = false,
                ExpectedExpression = ExpressionCustomerNameNotContains,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CustomerNameNotContains,
                DisableTrim = true,
                ExpectedExpression = ExpressionCustomerNameNotContainsNoTrim,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageNameNotContains,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageNameNotContains,
                ExpectedExpressionString = ExpressionStringLanguageNameNotContains
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionNotContainsThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageIdNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateNotContains,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionGreaterThan()
        {
            yield return new Tester<Account>
            {
                Filters = LanguageIdGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageIdGreaterThan,
                ExpectedExpressionString = ExpressionStringLanguageIdGreaterThan
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdGreaterThan,
                ExpectedExpressionString = ExpressionStringContactLanguageIdGreaterThan
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionIdGreaterThan,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdGreaterThan,
                ExpectedExpressionString = ExpressionStringAssignedEmployeeIdGreaterThan
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionManagerIdGreaterThan,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdGreaterThan,
                ExpectedExpressionString = ExpressionStringEmailIdGreaterThan
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionTotalServiceValueGreaterThan,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionCreatedOnGreaterThan,
                ExpectedExpressionString = ExpressionStringCreatedOnGreaterThan
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateGreaterThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateGreaterThan,
                ExpectedExpressionString = ExpressionStringExpirationDateGreaterThan
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionGreaterThanThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveGreaterThan,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedGreaterThan,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionLessThan()
        {
            yield return new Tester<Account>
            {
                Filters = LanguageIdLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageIdLessThan,
                ExpectedExpressionString = ExpressionStringLanguageIdLessThan
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdLessThan,
                ExpectedExpressionString = ExpressionStringContactLanguageIdLessThan
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionIdLessThan,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdLessThan,
                ExpectedExpressionString = ExpressionStringAssignedEmployeeIdLessThan
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionManagerIdLessThan,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdLessThan,
                ExpectedExpressionString = ExpressionStringEmailIdLessThan
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionTotalServiceValueLessThan,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionCreatedOnLessThan,
                ExpectedExpressionString = ExpressionStringCreatedOnLessThan
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateLessThan,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateLessThan,
                ExpectedExpressionString = ExpressionStringExpirationDateLessThan
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionLessThanThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveLessThan,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedLessThan,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionGreaterThanOrEqualTo()
        {
            yield return new Tester<Account>
            {
                Filters = LanguageIdGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageIdGreaterThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringLanguageIdGreaterThanOrEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdGreaterThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringContactLanguageIdGreaterThanOrEqualTo
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionIdGreaterThanOrEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdGreaterThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringAssignedEmployeeIdGreaterThanOrEqualTo
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionManagerIdGreaterThanOrEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdGreaterThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringEmailIdGreaterThanOrEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionTotalServiceValueGreaterThanOrEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionCreatedOnGreaterThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringCreatedOnGreaterThanOrEqualTo
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateGreaterThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringExpirationDateGreaterThanOrEqualTo
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionGreaterThanOrEqualToThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedGreaterThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionLessThanOrEqualTo()
        {
            yield return new Tester<Account>
            {
                Filters = LanguageIdLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionLanguageIdLessThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringLanguageIdLessThanOrEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageIdLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactLanguageIdLessThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringContactLanguageIdLessThanOrEqualTo
            };
            yield return new Tester<AccountValue>
            {
                Filters = IdLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionIdLessThanOrEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeIdLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionAssignedEmployeeIdLessThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringAssignedEmployeeIdLessThanOrEqualTo
            };
            yield return new Tester<Opportunity>
            {
                Filters = ManagerIdLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionManagerIdLessThanOrEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailIdLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionEmailIdLessThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringEmailIdLessThanOrEqualTo
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValueLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionTotalServiceValueLessThanOrEqualTo,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOnLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionCreatedOnLessThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringCreatedOnLessThanOrEqualTo
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDateLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = ExpressionExpirationDateLessThanOrEqualTo,
                ExpectedExpressionString = ExpressionStringExpirationDateLessThanOrEqualTo
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionLessThanOrEqualToThrowException()
        {
            yield return new Tester<Account>
            {
                Filters = ActiveLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ApprovedLessThanOrEqualTo,
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionMultipleExpressions()
        {
            yield return new Tester<Account>
            {
                Filters = CompanyNameOr,
                DisableTrim = false,
                ExpectedExpression = ExpressionCompanyNameOr,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CompanyNameOr,
                DisableTrim = true,
                ExpectedExpression = ExpressionCompanyNameOrNoTrim,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactNameOr,
                DisableTrim = false,
                ExpectedExpression = ExpressionContactNameOr,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactNameOr,
                DisableTrim = true,
                ExpectedExpression = ExpressionContactNameOrNoTrim,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{ContactNameOr}",
                DisableTrim = false,
                ExpectedExpression = ExpressionCompanyNameOr.And(ExpressionContactNameOr),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{ContactNameOr}",
                DisableTrim = true,
                ExpectedExpression = ExpressionCompanyNameOrNoTrim.And(ExpressionContactNameOrNoTrim),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{CustomerNameContains}",
                DisableTrim = false,
                ExpectedExpression = ExpressionCompanyNameOr.And(ExpressionCustomerNameContains),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{CustomerNameContains}",
                DisableTrim = true,
                ExpectedExpression = ExpressionCompanyNameOrNoTrim.And(ExpressionCustomerNameContainsNoTrim),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{ActiveEquals}",
                DisableTrim = false,
                ExpectedExpression = ExpressionCompanyNameOr.And(ExpressionActiveEquals),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{ActiveEquals}",
                DisableTrim = true,
                ExpectedExpression = ExpressionCompanyNameOrNoTrim.And(ExpressionActiveEquals),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{CustomerNameContains}{LogicalOperators.AndOperator}{ContactNameOr}",
                DisableTrim = false,
                ExpectedExpression = ExpressionCompanyNameOr.And(ExpressionCustomerNameContains).And(ExpressionContactNameOr),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{CustomerNameContains}{LogicalOperators.AndOperator}{ContactNameOr}",
                DisableTrim = true,
                ExpectedExpression = ExpressionCompanyNameOrNoTrim.And(ExpressionCustomerNameContainsNoTrim).And(ExpressionContactNameOrNoTrim),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{CustomerNameContains}{LogicalOperators.AndOperator}{ActiveEquals}{LogicalOperators.AndOperator}{ContactNameOr}",
                DisableTrim = false,
                ExpectedExpression = ExpressionCompanyNameOr.And(ExpressionCustomerNameContains).And(ExpressionActiveEquals).And(ExpressionContactNameOr),
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = $"{CompanyNameOr}{LogicalOperators.AndOperator}{CustomerNameContains}{LogicalOperators.AndOperator}{ActiveEquals}{LogicalOperators.AndOperator}{ContactNameOr}",
                DisableTrim = true,
                ExpectedExpression = ExpressionCompanyNameOrNoTrim.And(ExpressionCustomerNameContainsNoTrim).And(ExpressionActiveEquals).And(ExpressionContactNameOrNoTrim),
                ExpectedExpressionString = null
            };
        }

        public static IEnumerable<ITester> TestCasesGetFilterExpressionThrowTypeParseException()
        {
            yield return new Tester<Opportunity>
            {
                Filters = ManagerId + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = EmailId + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = Active + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = Approved + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = TotalServiceValue + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<AccountValue>
            {
                Filters = Id + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = AssignedEmployeeId + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = LanguageId + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = ContactLanguageId + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Account>
            {
                Filters = CreatedOn + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
            yield return new Tester<Opportunity>
            {
                Filters = ExpirationDate + FilterOperators.EqualsOperator + "foo",
                DisableTrim = false,
                ExpectedExpression = null,
                ExpectedExpressionString = null
            };
        }
        #endregion

        #region Tester Interfaces
        public interface ITester
        {
            void RunThrowExceptionOnGetFilterExpression<TException>() where TException : Exception;

            void RunGetFilterExpression();
        }
        #endregion

        #region Tester Classes
        private sealed class Tester<T> : Validator<T>, ITester where T : class
        {
            public string Filters { private get; set; }

            public bool DisableTrim { private get; set; }

            public void RunThrowExceptionOnGetFilterExpression<TException>() where TException : Exception
            {
                Assert.Throws<TException>(() => FilterOptionExtensions.GetFilterExpression<T>(Filters, disableTrim: DisableTrim), $"[Filters: \"{Filters}\"] [DisableTrim: \"{DisableTrim}\"]");
            }

            public void RunGetFilterExpression()
            {
                ValidateProperties();
                var filterExpression = FilterOptionExtensions.GetFilterExpression<T>(Filters, disableTrim: DisableTrim);
                ValidateExpression(filterExpression);
            }

            private void ValidateProperties()
            {
                Assert.IsNotEmpty(Filters);
                Assert.IsNotNull(ExpectedExpression);
            }
        }

        private abstract class Validator<T> where T : class
        {
            public Expression<Func<T, bool>> ExpectedExpression { protected get; set; }

            /// <summary>
            /// In case the ExpectedExpression cannot be converted to the appropriate string, explicitely specify it here
            /// </summary>
            public string ExpectedExpressionString { private get; set; }

            protected void ValidateExpression(Expression<Func<T, bool>> expression)
            {
                Assert.IsNotNull(expression);
                Assert.IsNotEmpty(expression.Parameters);
                Assert.AreEqual(expression.Parameters.Count, 1, $"[expression.Parameters.Count: \"{expression.Parameters.Count}\"] [Expected: \"{1}\"]");

                var parameter = expression.Parameters.First();
                var expectedParameter = ExpectedExpression.Parameters.First();
                Assert.AreEqual(parameter.Name, expectedParameter.Name, $"[parameter.Name: \"{parameter.Name}\"] [expectedParameter.Name: \"{expectedParameter.Name}\"]");
                Assert.AreEqual(parameter.Type, expectedParameter.Type, $"[parameter.Type: \"{parameter.Type}\"] [expectedParameter.Type: \"{expectedParameter.Type}\"]");

                var expressionString = expression.ToString();
                var expectedExpressionString = ExpectedExpressionString ?? ExpectedExpression.ToString();
                Assert.AreEqual(expression.Name, ExpectedExpression.Name, $"[expression.Name: \"{expression.Name}\"] [ExpectedExpression.Name: \"{ExpectedExpression.Name}\"]");
                Assert.AreEqual(expression.ReturnType, ExpectedExpression.ReturnType, $"[expression.ReturnType: \"{expression.ReturnType}\"] [ExpectedExpression.ReturnType: \"{ExpectedExpression.ReturnType}\"]");
                Assert.AreEqual(expression.Type, ExpectedExpression.Type, $"[expression.Type: \"{expression.Type}\"] [ExpectedExpression.Type: \"{ExpectedExpression.Type}\"]");
                Assert.AreEqual(expressionString, expectedExpressionString, $"[expressionString: \"{expressionString}\"] [expectedExpressionString: \"{expectedExpressionString}\"]");
            }
        }
        #endregion

        private static string GetDateString(DateTime? dateTime) => dateTime?.ToString("M/dd/yyyy");

        private static string GetDateExpressionString(DateTime dateTime) => $"{LambdaInit}((x.{CreatedOn} >= {GetDateString(dateTime)} {TwelveAm}) AndAlso (x.{CreatedOn} <= {GetDateString(dateTime)} {ElevenPm}))";
    }
}