using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureGamesData
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 10;
            while(number-- > 0)
            {
                string siteUrl = "https://www.baidu.com";
                IWebDriver driver = WebDriverHelper.CreateWebDriverByTargetBrowser(BrowserType.PhantomJSDriver);
                try
                {
                    driver.AdjustWindowSizeAndLocation()
                          .ClearCookies()
                          .SetupTimeout(30, 30);
                    driver.Navigate().GoToUrl(siteUrl);
                }
                catch (Exception ex)

                {
                    Trace.WriteLine($"Some exception happened at InitializeEnvironment: { ex.Message }");
                }
            }

        }
    }
}
