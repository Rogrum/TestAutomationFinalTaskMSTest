using OpenQA.Selenium;

namespace SauceDemoAutomation.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver Driver;
        private readonly By usernameInput = By.Id("user-name");
        private readonly By passwordInput = By.Id("password");
        private readonly By loginButton = By.Id("login-button");
        private readonly By errorMessage = By.CssSelector("[data-test='error']");

        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void EnterUsername(string username)
        {
            Driver.FindElement(usernameInput).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            Driver.FindElement(passwordInput).SendKeys(password);
        }

        public void ClickLogin()
        {
            Driver.FindElement(loginButton).Click();
        }

        public string GetErrorMessage()
        {
            return Driver.FindElement(errorMessage).Text;
        }
    }
}
