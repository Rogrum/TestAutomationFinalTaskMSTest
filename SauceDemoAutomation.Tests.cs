using log4net;
using log4net.Config;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SauceDemoAutomation.Pages;

namespace SauceDemoAutomation.Tests
{
    [TestClass]
    public class LoginTests
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LoginTests));

        [ThreadStatic]
        private static IWebDriver Driver;
        private LoginPage LoginPage;

        [TestInitialize]
        public void Setup()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            Logger.Info("Starting test setup");

            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            LoginPage = new LoginPage(Driver);

            Logger.Info("Browser opened and navigated to the page.");
        }

        [TestMethod]
        public void UC1_EmptyCredentials_ShowsUsernameRequired()
        {
            Logger.Info("Test UC1_EmptyCredentials_ShowsUsernameRequired started");

            LoginPage.EnterUsername("");
            LoginPage.EnterPassword("");
            LoginPage.ClickLogin();

            string errorMessage = LoginPage.GetErrorMessage();
            string usernameErrorMessage = "Epic sadface: Username is required";

            Logger.Debug($"Expected error: {usernameErrorMessage}, Actual error: {errorMessage}");

            Assert.IsTrue(errorMessage.Contains(usernameErrorMessage), $"Expected '{usernameErrorMessage}' but found '{errorMessage}'.");

            Logger.Info("Test UC1_EmptyCredentials_ShowsUsernameRequired finished");
        }

        [TestMethod]
        public void UC2_MissingPassword_ShowsPasswordRequired()
        {
            Logger.Info("Test UC2_MissingPassword_ShowsPasswordRequired started");

            LoginPage.EnterUsername("standard_user");
            LoginPage.EnterPassword("");
            LoginPage.ClickLogin();

            string errorMessage = LoginPage.GetErrorMessage();
            string passwordErrorMessage = "Epic sadface: Password is required";

            Logger.Debug($"Expected error: {passwordErrorMessage}, Actual error: {errorMessage}");

            Assert.IsTrue(errorMessage.Contains(passwordErrorMessage), $"Expected '{passwordErrorMessage}' but found '{errorMessage}'.");

            Logger.Info("Test UC2_MissingPassword_ShowsPasswordRequired finished");
        }

        [TestMethod]
        public void UC3_ValidCredentials_NavigatesToDashboard()
        {
            Logger.Info("Test UC3_ValidCredentials_NavigatesToDashboard started");

            LoginPage.EnterUsername("standard_user");
            LoginPage.EnterPassword("secret_sauce");
            LoginPage.ClickLogin();

            Assert.IsTrue(Driver.Title.Contains("Swag Labs"));

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
