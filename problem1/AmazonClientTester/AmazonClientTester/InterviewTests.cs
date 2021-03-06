using System;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;

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
        /// Test: Add a test case for doing a simple search on Amazon.com and verifying that correct results are shown
        /// </summary>
        [Test]
        public void SimpleSearch_SomeFailures()
        {
            // Likely would have a config file that defines all the input files for tests.
            // For simplicity sake, it is hardcoded in this example.
            var configFile = "SimpleSearchConfig.xml";
            DoSearchTest(configFile);
        }

        /// <summary>
        /// Test: Same as SimpleSearch, but has inputs that do not fail
        /// </summary>
        [Test]
        public void SimpleSearch_AllPassing()
        {
            // Likely would have a config file that defines all the input files for tests.
            // For simplicity sake, it is hardcoded in this example.
            var configFile = "SimpleSearchConfig_AllPassing.xml";
            DoSearchTest(configFile);
        }

        /// <summary>
        /// Test: Add a test case for adding an item to the cart and verifying that item has been added correctly
        /// </summary>
        [Test]
        public void ItemInCart()
        {
            var configFile = "ItemInCartConfig.xml";
            var configFilepath = GetConfigFilepath(configFile);
            var config = TestCaseConfigReader.OpenTestCaseConfig<ItemInCartConfig>(configFilepath);

            _driver.Url = config.URL;
            var wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(1));
            var searchTerm = config.SearchTerm;
            SearchAmazon(searchTerm);

            var results = WaitForResults(wait);
            var found = false;
            foreach (var result in results)
            {
                try
                {
                    var item = result.FindElement(By.XPath($".//span[contains(text(),'{searchTerm}')]"));
                    item.Click();
                    found = true;
                    break;
                }
                catch (NoSuchElementException) { }
            }

            // If the item couldn't be found, it is not safe to continue
            Assert.IsTrue(found, $"Could not find item: {searchTerm}");

            // Add To Cart
            var addButton = _driver.FindElement(By.Id("add-to-cart-button"));
            addButton.Click();

            // Open Cart
            var cartButton = _driver.FindElement(By.Id("nav-cart"));
            cartButton.Click();

            // View Cart
            var cart = _driver.FindElement(By.Id("activeCartViewForm"));
            var activeItems = cart.FindElement(By.XPath(".//div[@data-name='Active Items']"));
            var cartContents = activeItems.FindElements(By.XPath(".//div[@data-item-index]"));
            var inCart = false;
            foreach (var content in cartContents)
            {
                if (content.Text.Contains(searchTerm))
                {
                    inCart = true;
                    break;
                }
            }
            Assert.IsTrue(inCart, $"Item was not in the cart: {searchTerm}");
        }

        /// <summary>
        /// Add a test case for invalid password entry and verify correct error message is shown
        /// </summary>
        [Test]
        public void InvalidPassword()
        {
            var configFile = "IncorrectPasswordConfig.xml";
            var configFilepath = GetConfigFilepath(configFile);
            var config = TestCaseConfigReader.OpenTestCaseConfig<PasswordTestCaseConfig>(configFilepath);
            var wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(1));

            _driver.Url = config.URL;
            // In my experiments, this button usually needs to be clicked first. But sometimes it doesn't appear.
            var yourAccountElement = _driver.FindElements(By.XPath(".//a[contains(text(),'Your Account')]"));
            if (yourAccountElement.Count > 0)
            {
                // If we found any <a> tags with the text "Your Account", click it. Otherwise, carry on.
                yourAccountElement[0].Click();
            }

            var signInPageLink = _driver.FindElement(By.Id("nav-link-accountList"));
            signInPageLink.Click();

            var usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ap_email")));
            usernameField.SendKeys(config.Username);
            usernameField.Submit();

            var passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ap_password")));
            passwordField.SendKeys(config.Password);
            passwordField.Submit();


            IWebElement authErrorBox = null;
            try
            {
                authErrorBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("auth-error-message-box")));
            }
            catch (WebDriverTimeoutException)
            {
                // This might be because there is a security box. Wait another minute and let the user deal with it, hopefully.
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));
                var captcha = _driver.FindElement(By.Id("auth-captcha-guess"));
                if (!string.IsNullOrEmpty(captcha.GetAttribute("value")))
                {
                    passwordField = _driver.FindElement(By.Id("ap_password"));
                    passwordField.SendKeys(config.Password);
                    passwordField.Submit();

                    var temp = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(),'Your password is incorrect')]")));
                    authErrorBox = _driver.FindElement(By.Id("auth-error-message-box"));
                }
            }

            Assert.NotNull(authErrorBox, "Could not find the Authentication Error Box");
            Assert.Multiple(() =>
            {
                var header = authErrorBox.FindElement(By.ClassName("a-alert-heading")).Text;
                var content = authErrorBox.FindElement(By.ClassName("a-alert-content")).Text;

                Assert.That(header, Is.EqualTo("There was a problem"));
                Assert.That(content, Is.EqualTo("Your password is incorrect"));
            });
        }

        /// <summary>
        /// Given a string, searches for it on Amazon.com
        /// Assumes the browser is already at the website.
        /// </summary>
        /// <param name="searchTerm">The term to search</param>
        private void SearchAmazon(string searchTerm)
        {
            var searchBar = _driver.FindElement(By.Id("twotabsearchtextbox"));
            searchBar.SendKeys(searchTerm);
            searchBar.Submit();
            //var searchButton = _driver.FindElement(By.Id("nav-search-submit-button"));
            //searchButton.Click();
        }

        /// <summary>
        /// After a search, wait for the results to return
        /// </summary>
        /// <param name="wait">The wait to be used</param>
        private ReadOnlyCollection<IWebElement> WaitForResults(WebDriverWait wait)
        {
            var xpathToResultSpan = ".//span[@data-component-type='s-search-results']";
            var xpathToAllResults = $".//div[contains(@cel_widget_id, 'MAIN-SEARCH_RESULTS')]";

            var result = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathToResultSpan)));
            // Get the list of Results
            // Note, this includes some (presumably) un-desirable items, such as the header "RESULTS"
            return _driver.FindElements(By.XPath(xpathToAllResults));
        }

        /// <summary>
        /// Runs a SimpleSearch test. Takes in a config file, which determines the test parameters
        /// </summary>
        /// <param name="configFilename"></param>
        private void DoSearchTest(string configFilename)
        {
            var configFilepath = GetConfigFilepath(configFilename);
            var config = TestCaseConfigReader.OpenTestCaseConfig<ExpectedItemsTestCaseConfig>(configFilepath);

            // Navigate to the website
            _driver.Url = config.URL;
            // Search for the param
            SearchAmazon(config.SearchTerm);

            // TODO: This can probably be combined with _driver.FindElements(By.Xpath(xpathTOAllResults));
            // Wait until the results are visible. This span contains all of the results, as well as other data...
            var wait = new WebDriverWait(_driver, TimeSpan.FromMinutes(1));
            var results = WaitForResults(wait);

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
        /// Gets the path to the Executing Directory. Used to get the test configs.
        /// </summary>
        /// <returns>string path to the Executing Directory</returns>
        public static string GetConfigFilepath(string configFile)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConfigFiles", configFile);
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