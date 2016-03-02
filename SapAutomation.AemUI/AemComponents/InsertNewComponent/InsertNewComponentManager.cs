namespace SapAutomation.AemUI.AemComponents.InsertNewComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager(typeof(InsertNewComponentManagerConfig), "Insert new component manager")]
    public class InsertNewComponentManager : ICommandManager
    {
        public WebElement InsertNewComponent;

        public InsertNewComponentManager(InsertNewComponentManagerConfig config)
        {
            InsertNewComponent = config.InsertNewComponent;
        }
    }
}
