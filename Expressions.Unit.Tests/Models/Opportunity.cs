using System;

namespace Expressions.Unit.Tests.Models
{
    public class Opportunity
    {
        public long Id { get; set; }
        public long ManagerId { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public bool Excluded { get; set; }
        public virtual Account Account { get; set; }
    }
}