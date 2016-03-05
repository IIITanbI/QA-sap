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
                TutorialCardPath = "path213",
                ExternalSource = false,
                HideFacetsWithoutResults = false
            };
            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpTutorialCatalog", new List<object> { wdm, setupTutorial }, log);
            wdm.Refresh(log);
            //managerInfo.ExecuteCommand(tcm, "SetUpTutorialCatalog", new List<object> { wdm, "testvalue" });

            //wdmInfo.ExecuteCommand(wdm, "Refresh", new List<object> { }, log);
            //managerInfo.ExecuteCommand(tcm, "AddComponent", new List<object> { wdm, "ContainerFinderComponent" }, log);
            //managerInfo.ExecuteCommand(tcm, "AddContainerFinder", new List<object> { wdm }, log);

            //managerInfo.ExecuteCommand(tcm, "OpenInsertDialog", new List<object> { wdm, "DragContainerFinder" }, log);
            //managerInfo.ExecuteCommand(tcm, "OpenInsertDialog", new List<object> { wdm, "DragContainerFinder" }, log);
            //incInfo.ExecuteCommand(inc, "AddComponent", new List<object> { wdm, "ContainerFinderComponent" }, log);
            //managerInfo.ExecuteCommand(tcm, "OpenInsertDialog", new List<object> { wdm, "DragFinderResult" }, log);
            // incInfo.ExecuteCommand(inc, "AddComponent", new List<object> { wdm, "FinderResultComponent" }, log);

            //managerInfo.ExecuteCommand(tcm, "OpenInsertDialog", new List<object> { wdm, "DragFacets" }, log);
            //incInfo.ExecuteCommand(inc, "AddComponent", new List<object> { wdm, "FacetsComponent" }, log);

            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "OpenInsertDialog", new List<object> { wdm, "DragContainerFinder" }, log);
            incInfo.ExecuteCommand(inc, "AddComponent", new List<object> { wdm, "ContainerFinderComponent" }, log);
            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpContainerFinder", new List<object> { wdm, "val" }, log);

            var containerSetup = new ContainerFinderComponentConfig()
            {
                PagePaths = new List<string>
                {
                    "path1",
                    "path1",
                    "path1",
                    "path1",
                    "path1"
                }
            };

            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpContainerFinder", new List<object> { wdm, containerSetup }, log);
            wdm.Refresh(log);

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
            wdm.Refresh(log);


            var setupFacet = new FacetsComponentConfig()
            {
                HideFacets = false,
                TypeOfSelection = TypeOfSelection.RadioButtons,
                Namespaces = new List<string> {
                   "path1",
                   "path2",
                   "path3"
                }
            };
            tutorialCatalogCMInfo.ExecuteCommand(tutorialCatalogPageManager, "SetUpFacets", new List<object> { wdm, setupFacet }, log);
            //        public object ExecuteCommand(object managerObject, string commandName, List<object> parObjs, ILogger log);

            // managerInfo.ExecuteCommand(tcm, "SetUpFacets", new List<object> { wdm, "test path", "test value" });

            wdm.Close(null);
        }
    }
}
