using System.Collections.Generic;

namespace Expressions.Unit.Tests.Models
{
    public class Account
    {
        public Account()
        {
            Opportunity = new List<Opportunity>();
        }

        public long Id { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Opportunity> Opportunity { get; set; }
    }
}