using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

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

        public void ClearUsername()
        {
            var usernameText = Driver.FindElement(usernameInput);

            Actions actions = new Actions(Driver);

            actions.Click(usernameText)
                   .Click(usernameText)
                   .Click(usernameText)
                   .Perform();

            usernameText.SendKeys(Keys.Backspace);
        }

        public void EnterPassword(string password)
        {
            Driver.FindElement(passwordInput).SendKeys(password);
        }

        public void ClearPassword()
        {
            var passwordText = Driver.FindElement(passwordInput);

            Actions actions = new Actions(Driver);

            actions.Click(passwordText)
                   .Click(passwordText)
                   .Click(passwordText)
                   .Perform();

            passwordText.SendKeys(Keys.Backspace);
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

