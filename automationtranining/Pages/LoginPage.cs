using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;

namespace Malafi.Tests.Pages
{
    // فئة LoginPage تمثل صفحة تسجيل الدخول وتحتوي على العناصر والوظائف المتعلقة بتسجيل الدخول
    public class LoginPage
    {
        #region Fields
        private IWebDriver driver;  // المتصفح الذي سيتم استخدامه للتحكم في الصفحة.
        private WebDriverWait wait;  // أداة الانتظار التي ستنتظر العناصر حتى تظهر أو تصبح قابلة للتفاعل.
        private Dictionary<string, IWebElement> loginPageLinks;  // قاموس يحتوي على روابط الصفحة.

        #endregion

        #region Constructor
        // المُنشئ الذي يتم تمرير WebDriver لإنشاء الصفحة وتحميل العناصر.
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;  // حفظ الـ WebDriver في متغير.
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));  // تحديد وقت الانتظار (10 ثواني).

            // تهيئة عناصر الصفحة باستخدام PageFactory.
            PageFactory.InitElements(driver, this);

            // تهيئة قاموس الروابط في الصفحة.
            LoginPageLinks = new Dictionary<string, IWebElement>();
            // إضافة الروابط المختلفة للصفحة في القاموس.
            LoginPageLinks.Add("Forgot your password?", ForgetMyCredentialLinks);
            LoginPageLinks.Add("Self Services", SlefServicesLinks);
        }
        #endregion

        #region Properties
        // خاصية للانتظار (WebDriverWait) التي يمكن الوصول إليها من خارج الفئة.
        public WebDriverWait Wait => wait;

        // تعريف العناصر في صفحة تسجيل الدخول باستخدام الـ FindBy.
        [FindsBy(How = How.Id, Using = "b1-LanguageButton")]
        public IWebElement Language { get; private set; }  // عنصر تغيير اللغة.

        [FindsBy(How = How.Id, Using = "b2-UsernameInput")]
        public IWebElement UserName { get; private set; }  // حقل إدخال اسم المستخدم.

        [FindsBy(How = How.Id, Using = "b2-PasswordInput")]
        public IWebElement PasswordTextBox { get; private set; }  // حقل إدخال كلمة المرور.

        [FindsBy(How = How.Id, Using = "b2-PasswordInput")]
        public IWebElement PasswordValue { get; private set; }  // قيمة كلمة المرور (تكرار لحقل كلمة المرور).

        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        public IWebElement LoginButton { get; private set; }  // زر تسجيل الدخول.

        [FindsBy(How = How.ClassName, Using = "feedback-message-error")]
        public IWebElement Errormessage { get; private set; }  // رسالة الخطأ في حالة حدوث مشكلة في تسجيل الدخول.

        [FindsBy(How = How.LinkText, Using = "Forgot your password?")]
        public IWebElement ForgetMyCredentialLinks { get; private set; }  // رابط "نسيت كلمة المرور".

        [FindsBy(How = How.LinkText, Using = "Self Services")]
        public IWebElement SlefServicesLinks { get; private set; }  // رابط "الخدمات الذاتية".

        // خاصية الوصول إلى قاموس الروابط.
        public Dictionary<string, IWebElement> LoginPageLinks { get => loginPageLinks; private set => loginPageLinks = value; }
        #endregion

        #region Methods - Page Navigation

        /// <summary>
        /// يقوم بتسجيل الدخول باستخدام اسم المستخدم وكلمة المرور المحددين.
        /// </summary>
        /// <param name="userName">اسم المستخدم</param>
        /// <param name="password">كلمة المرور</param>
        /// <returns>يوجه المستخدم إلى الصفحة الرئيسية بعد تسجيل الدخول بنجاح</returns>
        public HomePage Login(string userName, string password)
        {
            // إدخال اسم المستخدم في الحقل المخصص.
            this.UserName.SendKeys(userName);

            // إدخال كلمة المرور في حقل كلمة المرور.
            this.PasswordTextBox.SendKeys(password);

            // إضافة تأخير لتقليد الوقت الذي قد يستغرقه المستخدم في إدخال البيانات (يتم قراءته من خصائص النظام).
         ///   Thread.Sleep(int.Parse(Properties.Resources.LoginSleepTime) * 1000);

            // النقر على زر "تسجيل الدخول".
            this.LoginButton.Click();

            // بعد تسجيل الدخول، يتم توجيه المستخدم إلى الصفحة الرئيسية.
            return new HomePage(driver);
        }

        /// <summary>
        /// ينقر على رابط معين في الصفحة.
        /// </summary>
        /// <param name="LinkText">نص الرابط</param>
        /// <returns>يوجه المستخدم إلى صفحة الخدمات الذاتية بعد النقر على الرابط.</returns>
        public SelfServices ClickOnLinkText(string LinkText)
        {
            // النقر على الرابط المطلوب من خلال القاموس الذي يحتوي على روابط الصفحة.
            this.LoginPageLinks[LinkText].Click();

            // بعد النقر على الرابط، يتم توجيه المستخدم إلى صفحة "الخدمات الذاتية".
            return new SelfServices(driver);
        }

        /// <summary>
        /// يقوم بتغيير اللغة بين الإنجليزية والعربية.
        /// </summary>
        /// <param name="language">اللغة التي سيتم تحديدها (إما "en" أو "ar")</param>
        public void ChangeLanguage(string language = "en")
        {
            while (true)
            {
                // إذا كانت اللغة المطلوبة هي الإنجليزية ولكن النص الحالي هو "عربي"، نكسر الحلقة.
                if (language == "en" && Language.Text == "عربي")
                    break;

                // إذا كانت اللغة المطلوبة هي العربية ولكن النص الحالي هو "English"، نكسر الحلقة.
                if (language == "ar" && Language.Text == "English")
                    break;

                // إذا كانت النصوص غير متوافقة، نضغط على زر تغيير اللغة.
                Language.Click();
            }
        }
        #endregion
    }
}
