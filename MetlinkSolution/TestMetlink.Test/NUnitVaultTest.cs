using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestMetlink.PageObjects;
using TestMetlink.Reports;

namespace TestMetlink.Test
{
	[TestFixture]
	public class NUnitMetlinkTest
	{
		IWebDriver driver;

		/// <summary>
		/// Initialise the ChromeDriver.
		/// </summary>
		[SetUp]
		public void Init()
		{
			ChromeOptions options = new ChromeOptions();
			options.AddArguments("--start-maximized");
			options.AddArguments("disable-infobars");
			driver = new ChromeDriver(options);
		}

        /// <summary>
        /// This is an end-to-end test case with fixed parameter values.
        /// </summary>
	    [Test]
        public void CheckBusFareTestCase1()
		{
			// Set up the test in ReportLog wrapper class
			ReportLog.CreateTest("CheckBusFares", "This is an end-to-end test case regarding the bus fares inquiries");
            try
			{
				HomePage homePage = new HomePage(driver);
				string email = "newzealand1126@gmail.com";
				string password = "Scsgdtcy3";
				string fromLocation = "65 Victoria St, Wellington, 6011";
				string toLocation = "104 Moorefield Road, Johnsonville, Wellington, New Zealand";

				// define the test steps
				void TestSteps(string from_Location, string to_Location, IWebDriver driver)
				{
					// Log in
					homePage.EnterCredentials(email, password);

					// Input the journey
					homePage.CheckPrice(from_Location, to_Location);

					// Check the fares
					homePage.TellPrice(from_Location, to_Location);

					// Reset the input fields
					homePage.ClearJourneyPlan();

					driver.Quit();
				}

				// run test steps
				TestSteps(fromLocation, toLocation, driver);
			}
			catch (Exception e)
			{
				// Test failed due to unforeseen exception.
				ReportLog.Fail(e.Message + "\n" + e.StackTrace);
                ReportLog.Fail("UnforeseenException", ReportLog.TakeScreenShot("UnforeseenException", driver));
				throw e;
			}
		}

		/// <summary>
		/// This is a parameterized Test.
		/// </summary>
		/// <param name="from_Location"> Set your starting point.</param>
		/// <param name="to_Location"> Set your end point.</param>
		[TestCase("65 Victoria St, Wellington, 6011", "104 Moorefield Road, Johnsonville, Wellington, New Zealand")]
		public void CheckBusFareTestCase2(string from_Location, string to_Location)
		{
			// Set up the test in ReportLog wrapper class.
			ReportLog.CreateTest("CheckBusFares", "This is an end-to-end test case regarding the bus fares inquiries");
			try
			{
				HomePage homePage = new HomePage(driver);
				string email = "newzealand1126@gmail.com";
				string password = "Scsgdtcy3";

				void TestSteps(IWebDriver driver)
				{
					// Log in
					homePage.EnterCredentials(email, password);

					// Input the journey
					homePage.CheckPrice(from_Location, to_Location);

					// Check the fares
					homePage.TellPrice(from_Location, to_Location);

					// Reset the input fields
					homePage.ClearJourneyPlan();

					driver.Quit();
				}

				TestSteps(driver);
			}
			catch (Exception e)
			{
				// Test failed due to unforeseen exception.
				ReportLog.Fail(e.Message + "\n" + e.StackTrace);
				ReportLog.Fail("UnforeseenException", ReportLog.TakeScreenShot("UnforeseenException", driver));
				throw e;
			}
		}

		/// <summary>
		/// Quit the ChromeDriver and Flush the Reporter.
		/// </summary>
		[TearDown]
		public void CleanUp()
		{
            ReportLog.Flush();
		}
	}
}