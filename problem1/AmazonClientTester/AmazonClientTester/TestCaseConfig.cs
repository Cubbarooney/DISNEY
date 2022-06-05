using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonClientTester
{
    public class TestCaseConfig
    {
        public string URL { get; set; }
        public string SearchTerm { get; set; }
        public List<AmazonItem> ExpectedResults { get; set; } = new List<AmazonItem>();
    }

    public class AmazonItem
    {
        public string Description { get; set; }
        public string ExpectedSymbol { get; set; }
        public string ExpectedDollars { get; set; }
        public string ExpectedCents { get; set; }
        public string Name { get; set; }
    }
}
