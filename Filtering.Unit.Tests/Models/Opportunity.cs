using System;

namespace Filtering.Unit.Tests.Models
{
    public class Opportunity
    {
        public long Id { get; set; }
        public long ManagerId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public decimal? DollarValue { get; set; }
        public short? MessageId { get; set; }
        public long? EmailId { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime CreatedOn { get; set; }
    
        public virtual Account Account { get; set; }
    }
}