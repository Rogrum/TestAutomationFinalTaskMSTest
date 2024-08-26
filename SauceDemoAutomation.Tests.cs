using log4net;
using log4net.Config;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SauceDemoAutomation.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace SauceDemoAutomation.Tests
{
    [TestClass]
    public class LoginTests
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LoginTests));

        [ThreadStatic]
        private static IWebDriver? Driver;
        private LoginPage? LoginPage;

        private const string PageUrl = "https://www.saucedemo.com/";

        [TestInitialize]
        public void Setup()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            Logger.Info("Starting test setup");

            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl(PageUrl);
            LoginPage = new LoginPage(Driver);

            Logger.Info("Browser opened and navigated to the page.");
        }

        [DataTestMethod]
        [DataRow("", "", "Epic sadface: Username is required")]
        public void UC1_EmptyCredentials_ShowsUsernameRequired(string username, string password, string expectedError)
        {
            Logger.Info("Test UC1_EmptyCredentials_ShowsUsernameRequired started");

            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();

            string errorMessage = LoginPage.GetErrorMessage();
            Logger.Debug($"Expected error: {expectedError}, Actual error: {errorMessage}");

            Assert.IsTrue(errorMessage.Contains(expectedError), $"Expected '{expectedError}' but found '{errorMessage}'.");

            Logger.Info("Test UC1_EmptyCredentials_ShowsUsernameRequired finished");
        }

        [DataTestMethod]
        [DataRow("standard_user", "", "Epic sadface: Password is required")]
        public void UC2_MissingPassword_ShowsPasswordRequired(string username, string password, string expectedError)
        {
            Logger.Info("Test UC2_MissingPassword_ShowsPasswordRequired started");

            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();

            string errorMessage = LoginPage.GetErrorMessage();
            Logger.Debug($"Expected error: {expectedError}, Actual error: {errorMessage}");

            Assert.IsTrue(errorMessage.Contains(expectedError), $"Expected '{expectedError}' but found '{errorMessage}'.");

            Logger.Info("Test UC2_MissingPassword_ShowsPasswordRequired finished");
        }

        [DataTestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void UC3_ValidCredentials_NavigatesToDashboard(string username, string password)
        {
            Logger.Info("Test UC3_ValidCredentials_NavigatesToDashboard started");

            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();

            Assert.IsTrue(Driver!.Title.Contains("Swag Labs"));

            Logger.Info("Test UC3_ValidCredentials_NavigatesToDashboard finished");
        }

        [TestCleanup]
        public void Teardown()
        {
            Logger.Info("Test teardown started");
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
                Logger.Info("Browser closed and disposed.");
            }
        }
    }
}

