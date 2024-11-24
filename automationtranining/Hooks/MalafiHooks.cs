using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System.Configuration;
using TechTalk.SpecFlow;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow.Tracing;
using Malafi.Tests.Pages;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using System.IO;
using System.Linq;
using System.Threading;
using automationtranining.Properties;
using DocumentFormat.OpenXml.CustomProperties;

namespace automationtranining.Tests.Hooks
{
    // يتم استخدام Binding لتحديد فئة MalafiHooks كـ "hook" في SpecFlow
    [Binding]
    public sealed class MalafiHooks
    {
        // تعريف المتغيرات لتخزين معلومات السيناريو والخاصية
        private ScenarioContext scenarioContext;
        private FeatureContext featureContext;
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;

        // يتم تمرير FeatureContext و ScenarioContext في المُنشئ لتمكين الوصول إلى معلومات السيناريو والخاصية
        public MalafiHooks(FeatureContext fContext, ScenarioContext context)
        {
            scenarioContext = context;
            featureContext = fContext;
        }

        // يتم تنفيذ هذا الhook بعد كل خطوة في السيناريو
        [AfterStep]
        public void InsertReportingSteps(ScenarioContext sc)
        {
            var stepType = sc.StepContext.StepInfo.StepDefinitionType.ToString();
            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
            object TestResult = getter.Invoke(sc, null);

            if (sc.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(sc.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<When>(sc.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(sc.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    scenario.CreateNode<And>(sc.StepContext.StepInfo.Text);
            }

            if (sc.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(sc.StepContext.StepInfo.Text).Fail(sc.TestError);
                if (stepType == "When")
                    scenario.CreateNode<When>(sc.StepContext.StepInfo.Text).Fail(sc.TestError);
                if (stepType == "Then")
                    scenario.CreateNode<Then>(sc.StepContext.StepInfo.Text).Fail(sc.TestError);
                if (stepType == "And")
                    scenario.CreateNode<And>(sc.StepContext.StepInfo.Text).Fail(sc.TestError);
            }
        }

        // يتم تنفيذ هذا الhook قبل بدء اختبار (Feature)
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featurecontext)
        {
            featureName = extent.CreateTest(featurecontext.FeatureInfo.Title);
        }

        // يتم تنفيذ هذا الhook قبل بدء السيناريو
        [BeforeScenario]
        public void BeforeScenario()
        {
            var scenarioTitle = scenarioContext.ScenarioInfo.Title
                + string.Join("_", scenarioContext.ScenarioInfo.Arguments.Values.OfType<string>().ToList());

            // تحديد المتصفح المناسب بناءً على القيمة الموجودة في الملف Properties.Resources.Browser
            IWebDriver driver;
            switch (Properties.Resources.Browser) // استخدام Browser من Resources
            {
                case "Chrome":
                    driver = new ChromeDriver(); // استخدام Chrome
                    break;
                case "Edge":
                    driver = new EdgeDriver(); // استخدام Edge
                    break;
                case "Firefox":
                    driver = new FirefoxDriver(); // استخدام Firefox
                    break;
                default:
                    driver = new ChromeDriver(); // في حال لم يتم تحديد المتصفح، سيتم استخدام Chrome
                    break;
            }

            // التوجه إلى عنوان URL المحدد في Properties.Resources.StartURL
            driver.Navigate()
                .GoToUrl(Properties.Resources.StartURL); // استخدام StartURL من Resources
            driver.Manage().Window.Maximize(); // تكبير نافذة المتصفح

            Thread.Sleep(2000); // الانتظار لثوانٍ بعد فتح المتصفح

            // حفظ WebDriver في الـ ScenarioContext لاستخدامه في الخطوات التالية
            scenarioContext["WebDriver"] = driver;
            scenarioContext["LoginPage"] = new LoginPage(driver); // تهيئة صفحة تسجيل الدخول
        }
        // هذا الhook يتم تنفيذه فقط إذا كانت السيناريو يحتوي على الوسم (Tag) "@tag1"
        [BeforeScenario("@tag1")]
        public void BeforeScenarioWithTag()
        {
            // تنفيذ بعض المنطق الخاص قبل السيناريو إذا كانت العلامة "@tag1" موجودة
        }

        // مثال على تنفيذ hook مع ترتيب معين
        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            // يمكن تحديد ترتيب تنفيذ الـ hooks باستخدام خاصية Order
        }

        // هذا الhook يتم تنفيذه بعد انتهاء السيناريو
        [AfterScenario]
        public void AfterScenario()
        {
            var driver = scenarioContext["WebDriver"] as IWebDriver ?? throw new NullReferenceException("Web Driver");

            if (scenarioContext.TestError != null)
            {
                TakeScreenshot(driver);
            }

            if (driver != null)
            {
                driver.Close();
                driver.Dispose();
            }
        }

        // يتم تنفيذ هذا الhook قبل جميع السيناريوهات لإنشاء تقرير
        [BeforeTestRun]
        public static void InitializeReport()
        {
            var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "testresults");
            if (!Directory.Exists(artifactDirectory))
                Directory.CreateDirectory(artifactDirectory);

            string fileNameBase = Path.Combine(artifactDirectory, string.Format("report_{0}.html", DateTime.Now.ToString("yyyyMMdd_HHmmss")));

            var htmlReporter = new ExtentSparkReporter(fileNameBase);

            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        // يتم تنفيذ هذا الhook بعد انتهاء جميع السيناريوهات لتفريغ التقرير النهائي
        [AfterTestRun]
        public static void TearDownReport()
        {
            extent.Flush(); // تفريغ التقرير النهائي وحفظه
        }

        // وظيفة لالتقاط صورة للشاشة في حال حدوث خطأ
        private void TakeScreenshot(IWebDriver driver)
        {
            try
            {
                string fileNameBase = "error", artifactDirectory;
                PrepareFile(ref fileNameBase, out artifactDirectory);

                string pageSource = driver.PageSource;
                string sourceFilePath = Path.Combine(artifactDirectory, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);

                ITakesScreenshot takesScreenshot = (driver as ITakesScreenshot) ?? throw new NullReferenceException("Driver as screenshot should not be null.");
                var screenshot = takesScreenshot.GetScreenshot();

                string screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");
                screenshot.SaveAsFile(screenshotFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);
            }
        }

        // تجهيز اسم الملف والمجلد لحفظ الملفات الخاصة بالأخطاء
        private void PrepareFile(ref string fileNameBase, out string artifactDirectory)
        {
            fileNameBase = string.Format(fileNameBase + "_{0}_{1}_{2}",
                                                                featureContext.FeatureInfo.Title.ToIdentifier(),
                                                                scenarioContext.ScenarioInfo.Title.ToIdentifier(),
                                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "testresults");
            if (!Directory.Exists(artifactDirectory))
                Directory.CreateDirectory(artifactDirectory);
        }
    }
}
