using System;
using System.Collections.Generic;
using System.Linq;

namespace Amigo.Tenant.CrossCutting.Entities.Rules
{
    [Serializable]
    public class BrokenRulesCollection : List<BrokenRule>
    {
        private readonly object _syncRoot = new object();

        public int ErrorCount { get; private set; }

        public int WarningCount { get; private set; }

        public int InformationCount { get; private set; }

        public BrokenRulesCollection(IEnumerable<BrokenRule> brokenRules) : base(brokenRules)
        {
        }

        public BrokenRulesCollection()
        {
        }

        private void ClearRules()
        {
            lock (_syncRoot)
            {
                Clear();
                RecalculateCounts();
            }
        }

        private void RecalculateCounts()
        {
            ErrorCount = this.Count(c => c.Severity == RuleSeverity.Error);
            WarningCount = this.Count(c => c.Severity == RuleSeverity.Warning);
            InformationCount = this.Count(c => c.Severity == RuleSeverity.Information);
        }

        private BrokenRule GetFirstBrokenRule(string property)
            => GetFirstMessage(property, RuleSeverity.Error);

        private BrokenRule GetFirstMessage(string property, RuleSeverity severity)
            => this.FirstOrDefault(c => c.Property == property && c.Severity == severity);

        public override string ToString()
            => ToString(Environment.NewLine);

        private string ToString(RuleSeverity severity)
            => ToString(Environment.NewLine, severity);

        private string ToString(string separator)
            => string.Join(separator, this.Select(c => c.Description));

        private string ToString(string separator, RuleSeverity severity)
        {
            var descriptions = this.Where(c => c.Severity == severity).Select(c => c.Description);
            return string.Join(separator, descriptions);
        }

        private new string[] ToArray()
        {
            return (from c in this
                    select c.Description).ToArray();
        }

        private string[] ToArray(RuleSeverity severity)
            => this.Where(c => c.Severity == severity)
                    .Select(c => c.Description)
            .ToArray();

        public void AddItem(BrokenRule broken)
        {
            Add(broken);
            RecalculateCounts();
        }
    }
}
