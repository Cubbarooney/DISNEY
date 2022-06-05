using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonClientTester
{
    public abstract class TestCaseConfig
    {
        public string URL { get; set; }
    }

    public class ExpectedItemsTestCaseConfig : TestCaseConfig
    {
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

    public class ItemInCartConfig : TestCaseConfig
    {
        public string ItemName { get; set; }
        public string SearchTerm { get; set; }
    }

    public class PasswordTestCaseConfig : TestCaseConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
