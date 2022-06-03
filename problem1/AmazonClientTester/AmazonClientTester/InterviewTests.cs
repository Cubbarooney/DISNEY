using System;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;

namespace AmazonClientTester
{
    [TestFixture]
    public class InterviewTests
    {
        string _url = "https://www.amazon.com";
        string _search = "Steam Train";

        IWebDriver _driver;

        [SetUp]
        public void OpenBrowser()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Test: Simply open the browser to Amazon, wait 2 seconds, and then
        /// end the test. Serves as an alternative to "Hello, World!"
        /// Fail Condition: Only if an error is thrown
        /// </summary>
        [Test]
        public void OpenAmazon()
        {
            _driver.Url = _url;

            System.Threading.Thread.Sleep(2000); //2secs
        }

        [TearDown]
        public void CloseBrowser()
        {
            if(_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}