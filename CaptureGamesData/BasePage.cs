namespace CaptureGamesData
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.PageObjects;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Drawing.Imaging;
    using System.Linq;

    public abstract class BasePage
    {
        public const double DefaultScriptExecuteTimeoutSeconds = 30;
        public const double DefaultPageLoadTimeoutSeconds = 60;
        public static readonly ImageFormat DefaultImageFormat = ImageFormat.Png;

        protected IWebDriver webDriver;

        public BasePage(IWebDriver webDriver)
        {
            Contract.Assert(webDriver != null, "The web driver cannot be null.");

            this.webDriver = webDriver;

            PageFactory.InitElements(webDriver, this);
        }

        //TODO: Add return value.
        public void ClickOnLink(IWebElement LinkToclick)
        {
            if (WaitTillElementShow(LinkToclick))
            {
                IJavaScriptExecutor jsExecutor = webDriver as IJavaScriptExecutor;

                if (jsExecutor != null)
                {
                    jsExecutor.ExecuteScript("if(arguments.length > 0) { arguments[0].click(); };", LinkToclick);
                }
            }
        }

        public void ClickOnAndWaitForElementShow(IWebElement clickOn, IWebElement waitFor, int timeOut = 60)
        {
            ClickOnLink(clickOn);

            WaitTillElementShow(waitFor, timeOut);

            WaitForAjax(timeOut: timeOut);
        }

        public void ClickOnAndWaitForActionDone(IWebElement clickOn, Func<IWebDriver, bool> predicateAction)
        {
            ClickOnLink(clickOn);

            WaitTillActionDone(predicateAction);

            WaitForAjax();
        }

        public void KeyAction(IWebElement element, string key)
        {
            var action = new Actions(webDriver);

            action.MoveToElement(element).Click().Perform();

            element.SendKeys(key);
        }

        public List<string> GetDropDownListOptions(IWebElement element)
        {
            List<string> optionList = new List<string>();

            var options = element.FindElements(By.TagName("option"));

            foreach (var option in options)
            {
                optionList.Add(option.Text);
            }

            return optionList;
        }

        /// <summary>
        /// Wait until the element loads
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="element">Web element</param>
        /// <param name="timeOut">Time for timeout</param>
        public bool WaitTillElementShow(IWebElement element, int timeOut = 60)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            return wait.Until(d => element.Displayed == true);
        }

        public bool WaitTillActionDone(Func<IWebDriver, bool> predicateAction, int timeOut = 60)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            return wait.Until(predicateAction);
        }

        /// <summary>
        /// Wait until the list loads
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="element">Web element</param>
        /// <param name="timeOut">Time for timeout</param>
        public bool WaitTillListLoads(IWebElement element, int timeOut = 60)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            return wait.Until(d => GetSubListItemsUnderGivenElement(element, "list-group-item").Count > 0 ? true : false);
        }

        /// <summary>
        /// Wait until the element Hide
        /// </summary>
        /// <param name="driver">Web driver</param>
        /// <param name="element">Web element</param>
        /// <param name="timeOut">Time for timeout</param>
        public bool WaitTillElementHide(IWebElement element, int timeOut = 60)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            return wait.Until(d => element.Displayed == false);
        }

        /// <summary>
        /// WaitTillTextContains
        /// </summary>
        /// <param name="element">web element</param>
        /// <param name="text">string</param>
        /// <param name="timeOut">Time for timeout</param>
        /// <returns></returns>
        public bool WaitTillTextContains(IWebElement element, string text, int timeOut = 25)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            return wait.Until(d => element.Displayed && element.Text.Contains(text));
        }

        public void WaitForAjax(string attachedJSConditionCode = null, int timeOut = 60)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            while (true)
            {
                var isAjaxComplete = wait.Until((d) =>
                {
                    var jsExecutor = d as IJavaScriptExecutor;

                    try
                    {
                        if (jsExecutor != null)
                        {
                            bool result = true;

                            if (!string.IsNullOrWhiteSpace(attachedJSConditionCode))
                            {
                                result = (bool)jsExecutor.ExecuteScript(attachedJSConditionCode);
                            }

                            return result && (bool)jsExecutor.ExecuteScript("return jQuery && jQuery.active === 0 && actionCenter && !actionCenter.communicationInprogress();");
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }

                    return false;
                });

                if (isAjaxComplete)
                {
                    break;
                }
            }
        }

        public void WaitForDocumentReady(int timeOut = 60)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOut));

            while (true)
            {
                var isDocumentReady = wait.Until((d) =>
                {
                    var jsExecutor = d as IJavaScriptExecutor;

                    try
                    {
                        if (jsExecutor != null)
                        {
                            return jsExecutor.ExecuteScript("return document.readyState").Equals("complete");
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }

                    return false;
                });

                if (isDocumentReady)
                {
                    break;
                }
            }
        }

        public void WaitForPageReady(int timeOut = 60)
        {
            WaitForAjax(timeOut: timeOut);

            WaitForDocumentReady(timeOut);
        }

        public List<IWebElement> GetSubListItemsUnderGivenElement(IWebElement webElement, string attrName)
        {
            return webElement.FindElements(By.ClassName(attrName)).ToList();
        }

        public string TakeScreenShot(string imageFileFullPath)
        {
            return webDriver.TakeScreenShot(imageFileFullPath, DefaultImageFormat);
        }
    }
}
