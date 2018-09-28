using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string AppId = "WpfApplication1.exe";
        private static WindowsDriver<WindowsElement> _session;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            var path = Path.GetFullPath($"..\\..\\..\\..\\WpfApplication1\\bin\\Debug\\{AppId}");
            appCapabilities.SetCapability("app", path);
            _session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(_session);

            Thread.Sleep(2000);

            _session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);
        }

        [TestMethod]
        public void TestMethod1()
        {

            DumpXmlPageSource();
            var mainButtonElement = _session.FindElementByAccessibilityId("TheMenuButton");
            mainButtonElement.Click();

            SwitchMainWindow();
            DumpXmlPageSource();

            var itemButtonElement = _session.FindElementByAccessibilityId("ItemOne");
            itemButtonElement.Click();
        }

        public static void SwitchMainWindow()
        {
            var currentWindowHandle = _session.CurrentWindowHandle;

            // Wait for 2 minutes seconds or however long it is needed for the right window to appear 
            // and for the splash screen to be dismissed. You can replace this with a more intelligent way to
            // determine if the new main window finally appears.
            //Thread.Sleep(TimeSpan.FromMinutes(2));

            // Return all window handles associated with this process/application.
            // At this point hopefully you have one to pick from. Otherwise you can
            // simply iterate through them to identify the one you want.
            var allWindowHandles = _session.WindowHandles;

            // Assuming you only have only one window entry in allWindowHandles and it is in fact the correct one,
            // switch the session to that window as follows. You can repeat this logic with any top window with the
            // same process id (any entry of allWindowHandles)
            var newWindowHanlder = allWindowHandles[0];
            _session.SwitchTo().Window(newWindowHanlder);

            Debug.WriteLine(string.Format("Switching WindowHandler from {0} to {1} ({2})", currentWindowHandle, newWindowHanlder, string.Join(",", allWindowHandles)));

            DumpXmlPageSource();
        }

        public static void DumpXmlPageSource()
        {
            var text = _session.PageSource;
            var fn = string.Format("PageSource_Dump_{0:yyyy-MM-dd_HHmmss}.txt", DateTime.Now);

            Debug.Write(text);

            File.WriteAllText(fn, text);
        }
    }
}
