using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;


    public class SelfServices
{
    #region fields
    private WebDriverWait wait;

    #endregion




    #region Constructor
    public SelfServices(IWebDriver driver)
    {
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        PageFactory.InitElements(driver, this);
        SelfServiceUrl = driver.Url;


    }
    #endregion

    public WebDriverWait Wait => wait ?? throw new NullReferenceException();



    #region Properties

    public string SelfServiceUrl { get; private set; }


    [FindsBy(How = How.Id, Using = "loginButton")]
    public IWebElement Login { get; private set; }
    #endregion

}
