using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestMetlink;

namespace TestMetlink.Reports
{
    /// <summary>
    /// This a wrapper class that contains an ExtentReporter and a Log4Net logger. Methods in the class send information to both.
    /// </summary>
    public class ReportLog
    {
        private static ExtentReports extent;
        private static ExtentTest test;
        private static ExtentHtmlReporter htmlReporter;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Log information to the HTML Report and the Log file.
        /// </summary>
        /// <param name="info">The information being stored.</param>
        public static void Log(string info)
        {
            test.Info(info);
            log.Info(info);
        }

        /// <summary>
        /// Test passed. Log pass information to the HTML Report and the Log file.
        /// </summary>
        /// <param name="info">The test result information.</param>
        public static void Pass(string info)
        {
            test.Pass(info);
            log.Info("Test passed " + info);
        }

        /// <summary>
        /// Test failed. Log fail information to the HTML Report and the log file.
        /// </summary>
        /// <param name="info">The test result information.</param>
        public static void Fail(string info)
        {
            test.Fail(info);
            log.Error("Test failed " + info);
        }

        /// <summary>
        /// Test failed. Log fail information to the HTML Report and the log file.
        /// </summary>
        /// <param name="info">The test result information.</param>
        /// <param name="screenshotPath">Path to the screenshot.</param>
        public static void Fail(string info, string screenshotPath)
        {
            test.Fail(info, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
            log.Error(info);
            log.Error("Screen shot at " + screenshotPath);
        }

		/// <summary>
		/// The input assertion method
		/// </summary>
		public static void InputAssert(string expectedInput, IWebElement actualInputFromWebElement, IWebDriver driver, string testCase)
		{
			Task.Delay(200).Wait();
			try
			{
				Assert.AreEqual(expectedInput, actualInputFromWebElement.GetAttribute("value"));
			    Pass("The input '" + expectedInput + "' has been verified.");
			}
			catch (AssertionException a)
			{
				// Test failed due to assertion error.
				Fail(a.Message, TakeScreenShot(testCase, driver));
                throw a;
            }

		}

		/// <summary>
		/// The selection assertion method
		/// </summary>
		public static void SelectAssert(string expectedSelectedText, SelectElement actualSelectionFromWebElement, IWebDriver driver, string testCase)
		{
			Task.Delay(200).Wait();
			try
			{
				int index;
				for (index = 0; index < actualSelectionFromWebElement.Options.Count; index++)
				{
					if (expectedSelectedText == actualSelectionFromWebElement.Options[index].Text)
					{
						break;
					}
				}
				Assert.True(actualSelectionFromWebElement.Options[index].Selected);
			}
			catch (AssertionException a)
			{
				// Test failed due to assertion error.
				Fail(a.Message, TakeScreenShot(testCase, driver));
                throw a;
            }
			Pass("The input '" + expectedSelectedText + "' has been selected.");
		}
		/// <summary>
		/// Take a screenshot using the driver. Save it by appending the current date/time to the supplied filename. Return it's path.
		/// </summary>
		/// <param name="filename">The name of the file</param>
		/// <returns>The path to the screenshot</returns>
		public static string TakeScreenShot(string filename, IWebDriver driver)
        {
            ITakesScreenshot takeScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takeScreenshot.GetScreenshot();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string finalpath = path + "..\\..\\..\\..\\MetlinkSolution\\TestResults\\ErrorScreenshots\\" + filename + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".png";
            screenshot.SaveAsFile(finalpath);
            return finalpath;
        }


        /// <summary>
        /// Get the Extent Reporter. If one does not exist, create and return one.
        /// </summary>
        /// <returns>The Extent Reporter</returns>
        public static ExtentReports GetExtent()
        {
            if (extent != null)
            {
                return extent;
            }
            extent = new ExtentReports();
            extent.AttachReporter(GetHtmlReporter());
            extent.AddSystemInfo("Tester", "Charles");
            extent.AddSystemInfo("OS", "Windows 10");
            extent.AddSystemInfo("Browser", "Google Chrome");
            extent.AddSystemInfo("Date/Time", DateTime.Now.ToString());
            return extent;
        }

        /// <summary>
        /// Create the HTML reporter to be attached to the Extent Reporter.
        /// </summary>
        /// <returns>New HTML Reporter</returns>
        private static ExtentHtmlReporter GetHtmlReporter()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\MetlinkSolution\\TestResults/"; /* Tips! */
            var fileName = "ReportByExtentReports.html";
            htmlReporter = new ExtentHtmlReporter(dir + fileName);
            htmlReporter.Configuration().ChartVisibilityOnOpen = true;
            htmlReporter.Configuration().ChartLocation = ChartLocation.Top;
            htmlReporter.Configuration().DocumentTitle = "Automation Report";
            htmlReporter.Configuration().Theme = Theme.Dark;
            htmlReporter.Configuration().ReportName = "Metlink Event Tests";

            return htmlReporter;
        }

        /// <summary>
        /// Creates a test in the Extent Reporter.
        /// </summary>
        /// <param name="name">Name of the test</param>
        /// <param name="description">Description of the test</param>
        public static ExtentTest CreateTest(String name, String description)
        {
            test = GetExtent().CreateTest(name, description);
            return test;
        }

        /// <summary>
        /// Flush the ExtentReporter and append Test Completed to log file.
        /// </summary>
        public static void Flush()
        {
            Log("Test case completed.");
            GetExtent().Flush();
        }

    }
}