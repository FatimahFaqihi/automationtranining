using DocumentFormat.OpenXml.Bibliography;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Xml.Linq;


namespace Malafi.Tests.Pages
{
    public class MalafiHome
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public MalafiHome(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = ".ThemeGrid_MarginGutter > .OSFillParent")]
        public IWebElement DocumentTypeLink { get; private set; }

        [FindsBy(How = How.XPath, Using = "//div[@id='b2-PageLinks']/div[2]/a[5]/i")]
        public IWebElement EmployeesSearchLink { get; private set; }

        [FindsBy(How = How.LinkText, Using = "Documents Group")]
        public IWebElement DocumentGroupLink { get; private set; }

        [FindsBy(How = How.XPath, Using = "//span[contains(.,'Registered Employees')]")]
        public IWebElement RegisteredEmployeesLink { get; private set; }


        [FindsBy(How = How.LinkText, Using = "Dashboard")]
        public IWebElement DashboardLink { get; private set; }


        [FindsBy(How = How.LinkText, Using = "My Files")]
        public IWebElement MyFilesLink { get; private set; }


        [FindsBy(How = How.LinkText, Using = "Inbox")]
        public IWebElement InboxLink { get; private set; }

        public WebDriverWait Wait => wait;

      

    }






}