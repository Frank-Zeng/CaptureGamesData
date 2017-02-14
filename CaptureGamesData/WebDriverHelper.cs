namespace CaptureGamesData
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.PhantomJS;
    using System;

    public static class WebDriverHelper
    {
        public static IWebDriver CreateWebDriverByTargetBrowser(BrowserType targetType)
        {
            IWebDriver driver = null;
            
            switch (targetType)
            {
                case BrowserType.Chrome:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                    
                    ChromeDriver chromeDriver = new ChromeDriver(@"\Drivers", chromeOptions);

                    if (chromeDriver.HasWebStorage)
                    {
                        chromeDriver.WebStorage.LocalStorage.Clear();
                        chromeDriver.WebStorage.SessionStorage.Clear();
                    }

                    driver = chromeDriver;

                    break;
                case BrowserType.Firefox:
                    FirefoxProfile fireFoxProfile = new FirefoxProfile(Environment.CurrentDirectory + @"\..\..\Drivers");
                    fireFoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream,text/csv");

                    FirefoxOptions fireFoxOptions = new FirefoxOptions();
                    fireFoxOptions.Profile = fireFoxProfile;

                    FirefoxDriver fireFoxDriver = new FirefoxDriver(fireFoxOptions);

                    if (fireFoxDriver.HasWebStorage)
                    {
                        fireFoxDriver.WebStorage.LocalStorage.Clear();
                        fireFoxDriver.WebStorage.SessionStorage.Clear();
                    }

                    driver = fireFoxDriver;

                    break;
                case BrowserType.IE:
                    InternetExplorerOptions ieOptions = new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        EnableNativeEvents = true,
                        EnsureCleanSession = true,
                        EnableFullPageScreenshot = true,
                        IgnoreZoomLevel = true,
                        RequireWindowFocus = true,
                        PageLoadStrategy = InternetExplorerPageLoadStrategy.Eager
                    };

                    InternetExplorerDriver ieDriver = new InternetExplorerDriver(Environment.CurrentDirectory +  @"\..\..\Drivers", ieOptions);

                    if (ieDriver.HasWebStorage)
                    {
                        ieDriver.WebStorage.LocalStorage.Clear();
                        ieDriver.WebStorage.SessionStorage.Clear();
                    }

                    driver = ieDriver;

                    break;
                case BrowserType.Edge:
                    EdgeOptions edgeOptions = new EdgeOptions()
                    {
                        PageLoadStrategy = EdgePageLoadStrategy.Eager
                    };

                    EdgeDriver edgeDriver = new EdgeDriver(edgeOptions);

                    if (edgeDriver.HasWebStorage)
                    {
                        edgeDriver.WebStorage.LocalStorage.Clear();
                        edgeDriver.WebStorage.SessionStorage.Clear();
                    }

                    driver = edgeDriver;

                    break;
                case BrowserType.PhantomJSDriver:
                    //   PhantomJSOptions options = new PhantomJSOptions();
                    var service = PhantomJSDriverService.CreateDefaultService(Environment.CurrentDirectory + @"\..\..\Drivers");
                    service.WebSecurity = false;
                    PhantomJSDriver ghostDriver = new PhantomJSDriver(service);

                    driver = ghostDriver;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(targetType.ToString(), "Unexpected Target type passed");
            }

            return driver;
        }
    }
}
