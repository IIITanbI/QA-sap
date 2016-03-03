﻿namespace Test
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using SapAutomation.AemPageManager;
    using SapAutomation.AemUI.Pages.TutorialCatalogPage;
    using System;
    using QA.AutomatedMagic.MetaMagic;
    using SapAutomation.AemUI.AemComponents.InsertNewComponent;
    using System.Xml.Linq;
    using System.Linq;
    using SapAutomation.AemUI.Components.TutorialCatalog;
    using SapAutomation.AemUI.Components.ContainerFinder;
    using SapAutomation.AemUI.Components.Facets;
    using SapAutomation.AemUI.Components.FinderResults;
    using SapAutomation.AemUI.Pages.LoginPage;
    using QA.AutomatedMagic.CommandsMagic;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main(string[] args)
        {
            #region stup
            ReflectionManager.LoadAssemblies();

            ILogger log = null;
            var wdmInfo = ReflectionManager.GetCommandManagerByTypeName("WebDriverManager");
            var wdm = (WebDriverManager)wdmInfo.CreateInstance(new FirefoxWebDriverConfig() { ProfileDirectoryPath = "c:\\temp\\ffrpofiles" });

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


            var insertNewComponentConfig = new InsertNewComponentManagerConfig();
            var insertNewComponentXml = XDocument.Load("AemComponents/InsertNewComponent/InsertNewComponent.xml").Elements().First();
            var insertNewComponent = MetaType.Parse<FrameWebElement>(insertNewComponentXml);
            insertNewComponentConfig.InsertNewComponent = insertNewComponent;

            var tutorialCatalogComponent = XDocument.Load("Components/TutorialCatalog/TutorialCatalogPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig = new TutorialCatalogManagerConfig();
            config.TutorialCatalogManagerConfig.TutorialCatalogComponent = MetaType.Parse<WebElement>(tutorialCatalogComponent);

            var containerFinderComponent = XDocument.Load("Components/ContainerFinder/ContainerFinderPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig = new ContainerFinderManagerConfig();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.ContainerFinderComponent = MetaType.Parse<WebElement>(containerFinderComponent);

            var facetsComponent = XDocument.Load("Components/Facets/FacetsPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FacetsManagerConfig = new FacetsManagerConfig();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FacetsManagerConfig.FacetsComponent = MetaType.Parse<WebElement>(facetsComponent);

            var finderResultsComponent = XDocument.Load("Components/FinderResults/FinderResultsPage.xml").Elements().First();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FinderResultsManagerConfig = new FinderResultsManagerConfig();
            config.TutorialCatalogManagerConfig.ContainerFinderManagerConfig.FinderResultsManagerConfig.FinderResultsComponent = MetaType.Parse<WebElement>(finderResultsComponent);

            #endregion stup

            var loginPageManagerConfig = new LoginPageManagerConfig();

            var loginComponent = XDocument.Load("Pages/LoginPage/LoginPage.xml").Elements().First();
            loginPageManagerConfig.LoginComponent = MetaType.Parse<WebElement>(loginComponent);

            wdmInfo.ExecuteCommand(wdm, "Navigate", new List<object> { @"http://10.7.14.16:4502/cf#/content/sapdx/website/languages/en/developer/tutorial-art.html" }, log );

            var lmmanagerInfo = ReflectionManager.GetCommandManagerByTypeName("LoginPageManager");
            var loginPageManager = (LoginPageManager)lmmanagerInfo.CreateInstance(loginPageManagerConfig);
            lmmanagerInfo.ExecuteCommand(loginPageManager, "Login", new List<object> { wdm, "admin", "admin" }, log);

            var managerInfo = ReflectionManager.GetCommandManagerByTypeName("TutorialCatalogPageManager");
            var tcm = (TutorialCatalogPageManager)managerInfo.CreateInstance(config);

            var incInfo = ReflectionManager.GetCommandManagerByTypeName("InsertNewComponentManager");
            var inc = (InsertNewComponentManager)incInfo.CreateInstance(insertNewComponentConfig);

            TutorialSetupConfig setupTutorial = new TutorialSetupConfig()
            {
                TutorialCardPath = "path213",
                ExternalSource = false,
                HideFacetsWithoutResults = false
            };
            managerInfo.ExecuteCommand(tcm, "SetUpTutorialCatalog", new List<object> { wdm, setupTutorial }, log);
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
            //managerInfo.ExecuteCommand(tcm, "OpenInsertDialog", new List<object> { wdm, "DragContainerFinder" }, log);

            //incInfo.ExecuteCommand(inc, "AddComponent", new List<object> { wdm, "FacetsComponent" }, log);

            var containerSetup = new ContainerFinderConfig()
            {
                Paths = new List<string>
                {
                    "path1",
                    "path1",
                    "path1",
                    "path1",
                    "path1"
                }
            };

            managerInfo.ExecuteCommand(tcm, "SetUpContainerFinder", new List<object> { wdm, containerSetup }, log);
            wdm.Refresh(log);

            var finderSetup = new FinderResultsConfig()
            {
                Pagination = "10",
                ShowDescription = true,
                DefaultDocumentIcon = "default",
                DefaultPageIcon = "default",
                DefaultVideotIcon = "default",
                AlphabeticalSorting = AlphabeticalSorting.Both,
                SortingByDate = SortingByDate.Both

            };

            managerInfo.ExecuteCommand(tcm, "SetupFinderResults", new List<object> { wdm, finderSetup }, log);
            wdm.Refresh(log);


            var setupFacet = new FacetsSetupConfig()
            {
                HideFacets = false,
                TypeOfSelection = TypeOfSelection.RadioButtons,
                Namespaces = new List<string> {
                   "path1",
                   "path2",
                   "path3"
                }
            };
             managerInfo.ExecuteCommand(tcm, "SetUpFacets", new List<object> { wdm, setupFacet }, log);
            //        public object ExecuteCommand(object managerObject, string commandName, List<object> parObjs, ILogger log);

           // managerInfo.ExecuteCommand(tcm, "SetUpFacets", new List<object> { wdm, "test path", "test value" });

            wdm.Close(null);
        }
    }
}
