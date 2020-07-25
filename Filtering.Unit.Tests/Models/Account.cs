using System;
using System.Collections.Generic;

namespace Filtering.Unit.Tests.Models
{
    public class Account
    {
        public Account()
        {
            AccountValue = new List<AccountValue>();
        }
    
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public short? ContactLanguageId { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal TotalServiceValue { get; set; }
        public short LanguageId { get; set; }
        public bool Active { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<AccountValue> AccountValue { get; set; }
    }
}
