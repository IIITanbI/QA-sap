﻿namespace SapAutomation.AemUI.Components.TutorialCatalog
{
    using ContainerFinder;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class TutorialCatalogManager : ICommandManager
    {
        public ContainerFinderManager ContainerFinderManager { get; set; }

        public WebElement TutorialCatalogComponent;

        public TutorialCatalogManager(TutorialCatalogManagerConfig config)
        {
            TutorialCatalogComponent = config.TutorialCatalogComponent;
            ContainerFinderManager = new ContainerFinderManager(config.ContainerFinderManagerConfig);
            TutorialCatalogComponent.ChildWebElements.Add(config.ContainerFinderManagerConfig.ContainerFinderComponent);
        }
    }
}
