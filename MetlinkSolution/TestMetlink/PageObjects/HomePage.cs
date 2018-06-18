using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace TestMetlink.PageObjects
{
    public class HomePage
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private const string Url = "https://www.metlink.org.nz/";

		// web elements
        [FindsBy(How = How.CssSelector, Using = "div.navbar-right-holder li>a[title='Sign In']")]
        private IWebElement signInLink;

		[FindsBy(How = How.CssSelector, Using = "div.navbar-right-holder li>a[title='Log out']")]
		private IWebElement logOutLink;

		[FindsBy(How = How.Id, Using = "CustomMemberLoginForm_LoginForm_Email")]
		private IWebElement emailInputField;

		[FindsBy(How = How.Id, Using = "CustomMemberLoginForm_LoginForm_Password")]
		private IWebElement passwordInputField;

		[FindsBy(How = How.Id, Using = "CustomMemberLoginForm_LoginForm_action_dologin")]
		private IWebElement signInButton;

		[FindsBy(How = How.Id, Using = "JourneyPlannerForm_JourneyPlannerForm_From")]
		private IWebElement fromLocationInputField;

		[FindsBy(How = How.Id, Using = "JourneyPlannerForm_JourneyPlannerForm_To")]
		private IWebElement toLocationInputField;

		[FindsBy(How = How.Id, Using = "JourneyPlannerForm_JourneyPlannerForm_action_doForm")]
		private IWebElement showMyJourneyButton;

		[FindsBy(How = How.Id, Using = "clearJourneyPlan")]
		private IWebElement newJourneyButton;

		[FindsBy(How = How.CssSelector, Using = "div.fares__list-wrapper")]
		private IWebElement faresList;

		[FindsBy(How = How.CssSelector, Using = "div.fares__right>span.fares__price")]
		private IList<IWebElement> fares;

		// constructor
		public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            PageFactory.InitElements(driver, this);
			driver.Navigate().GoToUrl(Url);
		}

		// web events
		public void Flush()
		{
			logOutLink.Click();
		}

		public void EnterCredentials(string email, string password)
		{
			signInLink.Click();
			emailInputField.SendKeys(email);
			passwordInputField.SendKeys(password);
			signInButton.Click();
		}

		public void CheckPrice(string fromLocation, string toLocation)
		{
			fromLocationInputField.SendKeys(fromLocation);
			toLocationInputField.SendKeys(toLocation);
			// TODO: add assertions here
			// expect(from_location_input_field.text).to eq from_location
			// expect(to_location_input_field.text).to eq to_location
			showMyJourneyButton.Click();
		}

		public void TellPrice(string fromLocation, string toLocation)
		{
			wait.Until(ExpectedConditions.ElementToBeClickable(faresList)); // tips: wait until the inquiry result is displayed
			Reports.ReportLog.Log("\nThe bus price from location A (" + fromLocation + ") to location B (" + toLocation + ") is:\nAdult Cash: " + fares.ElementAtOrDefault(0).GetAttribute("innerHTML").Trim() + "\nChild Cash: " + fares.ElementAtOrDefault(1).GetAttribute("innerHTML").Trim()); // tips: use "innerHTML" to get the text from a web element
		}

		public void ClearJourneyPlan()
		{
			newJourneyButton.Click();
		}
	}
}