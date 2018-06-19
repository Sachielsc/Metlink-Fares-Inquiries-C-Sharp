using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Objectivity.Test.Automation.Tests.NUnit.DataDriven;
using System.Configuration;

namespace TestMetlink.Test.DataDriven
{
	/// <summary>
	/// DataDriven methods for NUnit test framework
	/// </summary>
	public static class TestData
	{
		public static IEnumerable DataDrivenTest1
		{
			get
			{
				// call the method ReadXlsxDataDriveFile() from its definitions:
				// public static IEnumerable<TestCaseData> ReadXlsxDataDriveFile(string path, string sheetName, [Optional] string[] diffParam, [Optional] string testName)
				return DataDrivenHelper.ReadXlsxDataDriveFile(ConfigurationManager.AppSettings["DataDrivenFileXlsx"], "Journey1", new[] { "fromLocation", "toLocation" }, "Journey1Test");
			}
		}
	}
}