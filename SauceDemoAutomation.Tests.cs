using Serilog;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SauceDemoAutomation.Pages;

namespace SauceDemoAutomation.Tests
{
    [TestClass]
    public class LoginTests
    {
        private static ILogger Logger;

        [ThreadStatic]
        private static IWebDriver? Driver;
        private LoginPage? LoginPage;

        private const string PageUrl = "https://www.saucedemo.com/";

        [ClassInitialize]
        public static void GlobalSetup(TestContext context)
        {
            Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/test.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Logger.Information("Serilog configured successfully.");
        }

        [TestInitialize]
        public void Setup()
        {
            Logger.Information("Starting test setup");

            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl(PageUrl);
            LoginPage = new LoginPage(Driver);

            Logger.Information("Browser opened and navigated to the page.");
        }

        [DataTestMethod]
        [DataRow("", "", "Epic sadface: Username is required")]
        public void UC1_EmptyCredentials_ShowsUsernameRequired(string username, string password, string expectedError)
        {
            Logger.Information("Test UC1_EmptyCredentials_ShowsUsernameRequired started");

            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();

            string errorMessage = LoginPage.GetErrorMessage();
            Logger.Debug($"Expected error: {expectedError}, Actual error: {errorMessage}");

            Assert.IsTrue(errorMessage.Contains(expectedError), $"Expected '{expectedError}' but found '{errorMessage}'.");

            Logger.Information("Test UC1_EmptyCredentials_ShowsUsernameRequired finished");
        }

        [DataTestMethod]
        [DataRow("standard_user", "", "Epic sadface: Password is required")]
        public void UC2_MissingPassword_ShowsPasswordRequired(string username, string password, string expectedError)
        {
            Logger.Information("Test UC2_MissingPassword_ShowsPasswordRequired started");

            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();

            string errorMessage = LoginPage.GetErrorMessage();
            Logger.Debug($"Expected error: {expectedError}, Actual error: {errorMessage}");

            Assert.IsTrue(errorMessage.Contains(expectedError), $"Expected '{expectedError}' but found '{errorMessage}'.");

            Logger.Information("Test UC2_MissingPassword_ShowsPasswordRequired finished");
        }

        [DataTestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void UC3_ValidCredentials_NavigatesToDashboard(string username, string password)
        {
            Logger.Information("Test UC3_ValidCredentials_NavigatesToDashboard started");

            LoginPage!.EnterUsername(username);
            LoginPage.EnterPassword(password);
            LoginPage.ClickLogin();

            Assert.IsTrue(Driver!.Title.Contains("Swag Labs"));

            Logger.Information("Test UC3_ValidCredentials_NavigatesToDashboard finished");
        }

        [TestCleanup]
        public void Teardown()
        {
            Logger.Information("Test teardown started");
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
                Logger.Information("Browser closed and disposed.");
            }
        }

        [ClassCleanup]
        public static void GlobalTeardown()
        {
            Logger.Information("Test class teardown completed.");
            Log.CloseAndFlush();
        }
    }
}


