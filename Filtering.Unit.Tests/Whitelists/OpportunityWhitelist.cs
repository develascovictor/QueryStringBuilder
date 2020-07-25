using Filtering.Unit.Tests.Models;
using System;
using System.Collections.Generic;

namespace Filtering.Unit.Tests.Whitelists
{
    public class OpportunityWhitelist
    {
        public IReadOnlyDictionary<string, string> Whitelist => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"contactName", nameof(Opportunity.ContactName)},
            {"contactEmail", nameof(Opportunity.ContactEmail)}
        };
    }
}