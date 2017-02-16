namespace CaptureGamesData
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using System.Diagnostics.Contracts;

    public class DetailPanelPage : BasePage
    {
        [FindsBy(How = How.Id, Using = "spanHomePageTitle")]
        public IWebElement PageTitle;

        [FindsBy(How = How.Id, Using = "detailPage")]
        public IWebElement DetailPage;

        [FindsBy(How = How.Id, Using = "productList")]
        public IWebElement ProductList;

        [FindsBy(How = How.Id, Using = "divOnlineOrderProductList")]
        public IWebElement OnlineRunnerProductList;

        [FindsBy(How = How.Id, Using = "divConfirm")]
        public IWebElement ConfirmBtn;

        [FindsBy(How = How.Id, Using = "txtHoldLocationNotes")]
        public IWebElement HoldLocationNote;

        [FindsBy(How = How.Id, Using = "print-Template")]
        public IWebElement PrintTemplate;

        [FindsBy(How = How.Id, Using = "divAlertDangerErrorMsg")]
        public IWebElement AlertDangerErrorMsg;

        [FindsBy(How = How.Id, Using = "btnCloseOnlineProductConfirmErrorBanner")]
        public IWebElement ConfirmErrorBanner;

        [FindsBy(How = How.Id, Using = "ownerName")]
        public IWebElement OwnerName;

        [FindsBy(How = How.Id, Using = "associateName")]
        public IWebElement AssociateName;

        public DetailPanelPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public string GetAssignToOwnerName()
        {
            Contract.Assert(WaitTillElementShow(DetailPage), "Failed to load detail page");

            return DetailPage.FindElement(By.XPath("//p[contains(text(), 'Assigned to:')]")).Text;
        }

        public string GetDetailItem(string item)
        {
            Contract.Assert(WaitTillElementShow(DetailPage), "Failed to load detail page");

            return DetailPage.FindElement(By.XPath("//p[@data-bind='text:selectedRequest()." + item + "']")).Text;
        }

        public void ClickAllProductRadioButtonsByName(string optionName)
        {
            var productList = GetSubListItemsUnderGivenElement(OnlineRunnerProductList, "list-group-item");

            Contract.Assert(WaitTillElementShow(OnlineRunnerProductList), "The product list is not shown as expected.");

            foreach (IWebElement product in productList)
            {
                //TODO: Improve perf
                IWebElement radioBtn = product.FindElement(By.XPath("//span[contains(text(), '" + optionName + "')]"))
                    .FindElement(By.XPath("..")).FindElement(By.XPath("input[@type='radio']"));

                radioBtn.Click();

                if(!radioBtn.Selected)
                {
                    radioBtn.SendKeys(Keys.Space);
                }
            }
        }

        public void InputTextHoldLocationNote()
        {
            WaitTillElementShow(HoldLocationNote);

            HoldLocationNote.SendKeys("5");
        }
    }
}
