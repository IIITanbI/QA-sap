using QA.AutomatedMagic;
using QA.AutomatedMagic.WebDriverManager;
using SapAutomation.AemPageManager;
using SapAutomation.AemUI.Pages.TutorialCatalogPage;
using System;
using QA.AutomatedMagic;
using QA.AutomatedMagic.MetaMagic;
using SapAutomation.AemUI.AemComponents.InsertNewComponent;
using System.Xml.Linq;
using System.Linq;
using SapAutomation.AemUI.Components.TutorialCatalog;
using SapAutomation.AemUI.Components.ContainerFinder;
using SapAutomation.AemUI.Components.Facets;
using SapAutomation.AemUI.Components.FinderResults;
using SapAutomation.AemUI.Pages.LoginPage;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ReflectionManager.LoadAssemblies();

            ILogger log = null;
            WebDriverManager wdm = new WebDriverManager(new FirefoxWebDriverConfig() { ProfileDirectoryPath = "c:\\temp\\ffrpofiles"});

            var config = new TutorialCatalogPageManagerConfig();
            config.RootFrame = new FrameWebElement()
            {
                Name = "Page Root Frame",
                Description = "Frame locator value for root frame",
                Locator = new WebLocator
                {
                    XPath = "//iframe[1]"
                }
            };


            var insertNewComponent = XDocument.Load("AemComponents/InsertNewComponent/InsertNewComponent.xml").Elements().First();
            config.AddComponentFormManagerConfig = new InsertNewComponentManagerConfig();
            config.AddComponentFormManagerConfig.InsertNewComponent = MetaType.Parse<WebElement>(insertNewComponent);

            var tutorialCatalogComponent = XDocument.Load("Components/TutorialCatalog/TutorialCatalogPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig = new TutorialCatalogManagerConfig();
            config.TutorialCatalogManagerConfig.TutorialCatalogComponent = MetaType.Parse<WebElement>(tutorialCatalogComponent);

            var containerFinderComponent = XDocument.Load("Components/ContainerFinder/ContainerFinderPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig = new ContainerFinderManagerConfig();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.ContainerFinderComponent = MetaType.Parse<WebElement>(containerFinderComponent);

            //var facetsComponent = XDocument.Load("Components/Facets/FacetsPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FacetsManagerConfig = new FacetsManagerConfig();
            //config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FacetsManagerConfig.FacetsComponent = MetaType.Parse<WebElement>(facetsComponent);

            //var finderResultsComponent = XDocument.Load("Components/Facets/FacetsPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FinderResultsManagerConfig = new FinderResultsManagerConfig();
            //config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FinderResultsManagerConfig.FinderResultsComponent = MetaType.Parse<WebElement>(finderResultsComponent);

            config.RootFrame.ChildWebElements.Add(config.AddComponentFormManagerConfig.InsertNewComponent);
            config.RootFrame.ChildWebElements.Add(config.TutorialCatalogManagerConfig.TutorialCatalogComponent);
            config.TutorialCatalogManagerConfig.TutorialCatalogComponent.ChildWebElements.Add(config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.ContainerFinderComponent);
            config.RootFrame.Init();
            wdm.Navigate(@"http://10.7.14.16:4502/cf#/content/sapdx/website/languages/en/developer/tut.html", log);

            var loginPageManagerConfig = new LoginPageManagerConfig();

            var loginComponent = XDocument.Load("Pages/LoginPage/LoginPage.xml").Elements().First();
            loginPageManagerConfig.LoginComponent = MetaType.Parse<WebElement>(loginComponent);

            var loginPageManager = new LoginPageManager(loginPageManagerConfig);
            loginPageManager.Login(wdm, "admin", "admin", log);

            var tcm = new TutorialCatalogManager(config.TutorialCatalogManagerConfig);


            var temp = wdm.Find(tcm.TutorialCatalogComponent["Root"], log);

            //tcm.OpenInsertDialog(wdm, log);
            tcm.SetUpContainerFinder(wdm, "testvalue", log);


            wdm.Close(null);
            //Console.ReadLine();
        }
    }

}
