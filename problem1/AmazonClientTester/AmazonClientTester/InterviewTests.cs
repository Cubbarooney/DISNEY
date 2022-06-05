﻿using System;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Reflection;
using System.IO;

namespace AmazonClientTester
{
    [TestFixture]
    public class InterviewTests
    {
        string _url = "https://www.amazon.com";

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

        /// <summary>
        /// Test: Add a test case for doing a simple search on Amaxon.com and verifying that correct results are shown
        /// </summary>
        [Test]
        public void SimpleSearch()
        {
            var xpathToResultSpan = "//span[@data-component-type='s-search-results']";
            var xpathToAllResults = $"//div[contains(@cel_widget_id, 'MAIN-SEARCH_RESULTS')]";
            var configDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Likely would have a config file that defines all the input files for tests.
            // For simplicity sake, it is hardcoded in this example.
            var configFile = "SimpleSearchConfig.xml";
            var config = TestCaseConfigReader.OpenTestCaseConfig(Path.Combine(configDirectory, configFile));

            // Navigate to the website
            _driver.Url = config.URL;
            // Search for the param
            SearchAmazon(config.SearchTerm);

            // Wait until the results are visible. This span contains all of the results, as well as other data...
            var wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(2));
            var result = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathToResultSpan)));

            // Get the list of Results
            // Note, this includes some (presumably) un-desirable items, such as the header "RESULTS"
            var results = _driver.FindElements(By.XPath(xpathToAllResults));

            // We have a few conditions to check, so we do so within a Assert.Multiple
            // This allows the test to complete and check for all cases
            Assert.Multiple(() =>
            {
                foreach (var expected in config.ExpectedResults)
                {
                    // There is probably a more efficient way of doing this...
                    // Instead of looping through the entire list each time.
                    var foundDescription = false;
                    foreach (var r in results)
                    {
                        try
                        {
                            var description = r.FindElement(By.ClassName("a-text-normal"));
                            if (description.Text.Equals(expected.Description))
                            {
                                Assert.That(description.Displayed, $"Result <{expected.Name}> is not Displayed");
                                foundDescription = true;
                            }
                        }
                        catch (NoSuchElementException) { }
                        // If we found the result, then we do not need to continue after we check for the price
                        if (foundDescription)
                        {
                            try
                            {
                                var symbol = r.FindElement(By.ClassName("a-price-symbol")).Text;
                                var whole = r.FindElement(By.ClassName("a-price-whole")).Text;
                                var fraction = r.FindElement(By.ClassName("a-price-fraction")).Text;

                                Assert.That(symbol, Is.EqualTo(expected.ExpectedSymbol), $"Result <{expected.Name}> did not have the expected monetary symbol");
                                Assert.That(whole, Is.EqualTo(expected.ExpectedDollars), $"Result <{expected.Name}> did not have the expected dollar ammount");
                                Assert.That(fraction, Is.EqualTo(expected.ExpectedCents), $"Result <{expected.Name}> did not have the expected cents ammount");
                            }
                            catch (NoSuchElementException) { }

                            break;
                        }
                    }

                    Assert.That(foundDescription, $"Result <{expected.Name}> could not be found");
                }
            });
        }

        /// <summary>
        /// Test: Add a test case for adding an item to the cart and verifying that item has been added correctly
        /// </summary>
        [Test]
        public void ItemInCart()
        {

        }

        /// <summary>
        /// Add a test case for invalid password entry and verify correct error message is shown
        /// </summary>
        [Test]
        public void InvalidPassword()
        {

        }

        private void SearchAmazon(string searchTerm)
        {
            var searchBar = _driver.FindElement(By.Id("twotabsearchtextbox"));
            searchBar.SendKeys(searchTerm);
            searchBar.Submit();
            //var searchButton = _driver.FindElement(By.Id("nav-search-submit-button"));
            //searchButton.Click();
        }

        [TearDown]
        public void CloseBrowser()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}