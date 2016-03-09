namespace Test
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using SapAutomation.Managers.AemPageManager;
    using SapAutomation.Web.Components.Aem.InsertNewComponentForm;
    using SapAutomation.Web.Pages.Aem.LoginPage;
    using SapAutomation.Web.Components.Sap.ContainerFinderComponent;
    using SapAutomation.Web.Components.Sap.FacetsComponent;
    using SapAutomation.Web.Components.Sap.FinderResultsComponent;
    using SapAutomation.Web.Components.Sap.TutorialCatalogComponent;
    using SapAutomation.Web.Pages.Sap.TutorialCatalogPage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.IO;
    public class Program
    {
        public static void Main(string[] args)
        {
            AutomatedMagicManager.LoadAssemblies();
            AutomatedMagicManager.LoadAssemblies(Directory.GetCurrentDirectory(), SearchOption.AllDirectories);

            ILogger log = null;
            var wdmInfo = AutomatedMagicManager.GetCommandManagerByTypeName("WebDriverManager");
            var wdm = (WebDriverManager)wdmInfo.CreateInstance(new FirefoxWebDriverConfig() { ProfileDirectoryPath = "c:\\temp\\ffrpofiles" });

            var loginPageCMInfo = AutomatedMagicManager.GetCommandManagerByTypeName("LoginPageManager");
            var loginPageManager = (LoginPageManager)loginPageCMInfo.CreateInstance(null);

            var tutorialCatalogCMInfo = AutomatedMagicManager.GetCommandManagerByTypeName("TutorialCatalogPageManager");
            var tutorialCatalogPageManager = (TutorialCatalogPageManager)tutorialCatalogCMInfo.CreateInstance(null);

            var incInfo = AutomatedMagicManager.GetCommandManagerByTypeName("InsertNewComponentFormManager");
            var inc = (InsertNewComponentFormManager)incInfo.CreateInstance(null);

            wdmInfo.ExecuteCommand(wdm, "Navigate", new List<object> { @"http://10.7.14.16:4502/cf#/content/sapdx/website/languages/en/developer/mdemo.html" }, log);

            loginPageCMInfo.ExecuteCommand(loginPageManager, "Login", new List<object> { wdm, "admin", "admin" }, log);

            var setupTutorial = new TutorialCatalogComponentConfig()
            {
                TutorialCardPath = "https://github.com/tutorials/tutorials",
                ExternalSource = true,
                HideFacetsWithoutResults = false
            };

            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpTutorialCatalog", new List<object> { wdm, setupTutorial }, log);
            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "AddContainerFinderComponent", new List<object> { wdm, inc }, log);

            var containerSetup = new ContainerFinderComponentConfig()
            {
                PagePaths = new List<string>
                {
                    "/content/sapdx/website/languages/en/developer/mdemo"
                }
            };

            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpContainerFinder", new List<object> { wdm, containerSetup }, log);

            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "AddFinderResultsComponent", new List<object> { wdm, inc }, log);
            var finderSetup = new FinderResultsComponentConfig()
            {
                Pagination = "10",
                ShowDescription = true,
                DefaultDocumentIcon = "default",
                DefaultPageIcon = "default",
                DefaultVideotIcon = "default",
                AlphabeticalSorting = AlphabeticalSorting.Both,
                SortingByDate = SortingByDate.Both

            };
            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetupFinderResults", new List<object> { wdm, finderSetup }, log);

            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "AddFacetsComponent", new List<object> { wdm, inc }, log);
            var setupFacet = new FacetsComponentConfig()
            {
                HideFacets = false,
                TypeOfSelection = TypeOfSelection.RadioButtons,
                Namespaces = new List<string> {
                   "/etc/tags/tutorial/interest",
                   "/etc/tags/tutorial/product",
                   "/etc/tags/tutorial/technology"
                }
            };
            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpFacets", new List<object> { wdm, setupFacet }, log);

            wdm.Close(null);
        }
    }
}
