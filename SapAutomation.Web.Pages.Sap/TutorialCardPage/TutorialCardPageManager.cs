namespace SapAutomation.Web.Pages.Sap.TutorialCardPage
{
    using OpenQA.Selenium;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TutorialCatalogPage;

    [CommandManager("TutorialCardPageManager")]
    public class TutorialCardPageManager : BaseCommandManager
    {
        [MetaSource(nameof(TutorialCardPage) + @"\TutorialCardPageWebDefenition.xml")]
        public WebElement TutorialCardPageWebDefinition { get; set; }

        [MetaSource(nameof(TutorialCardPage) + @"\TutorialCardWebDefinition.xml")]
        public WebElement TutorialCardWebDefinition { get; set; }

        public WebElement TutorialCardPublishPageWebDefinition { get; set; }

        public override void Init()
        {
            TutorialCardPageWebDefinition.ChildWebElements.Add(MetaType.CopyObjectWithCast(TutorialCardWebDefinition));
            TutorialCardPageWebDefinition.Init();

            TutorialCardPublishPageWebDefinition = MetaType.CopyObjectWithCast(TutorialCardWebDefinition);
            TutorialCardPublishPageWebDefinition.Init();
        }

        [Command("Command for change tutorial card title")]
        public void ChangeTutorialCardTitle(WebDriverManager webDriverManager, string title, ILogger log)
        {
            var body = TutorialCardPageWebDefinition["TutorialCardPage_Body"];
            var editor = body["TutorialCardEditor_Form"];

            webDriverManager.Click(body["Edit_Button"], log);

            webDriverManager.SendKeys(editor["Selected_Tab.Text_Area.TutorialCardTitle_Row"], title, log);

            webDriverManager.Click(editor["OK_Button"], log);
        }

        [Command("GetTutorialCardOnAuthor")]
        public TutorialCard GetTutorialCardOnAuthor(WebDriverManager webDriverManager, ILogger log)
        {
            //var tutorialCard = TutorialCardPageWebDefinition["TutorialCard"];
            //return GetTutorialCard(tutorialCard, webDriverManager, log);

            return GetTutorialCard(TutorialCardPublishPageWebDefinition, webDriverManager, false, log);
        }

        [Command("GetTutorialCardOnPublish")]
        public TutorialCard GetTutorialCardOnPublish(WebDriverManager webDriverManager, ILogger log)
        {
            return GetTutorialCard(TutorialCardPublishPageWebDefinition, webDriverManager, true, log);
        }

        private TutorialCard GetTutorialCard(WebElement tutorialCardElement, WebDriverManager webDriverManager, bool isPublish, ILogger log)
        {
            try
            {
                log?.INFO($"Get tutorial card'");

                var card = webDriverManager.FindElement(tutorialCardElement, log);

                var tutorialCard = new TutorialCard();
                tutorialCard.Location = card.Location;

                IWebElement container = null;
                var containerWebElement = tutorialCardElement["DeveloperText1_Container"];
                try
                {
                    log?.DEBUG("Try to find container for Title and Description");
                    container = webDriverManager.FindElement(card, containerWebElement, log);
                    log?.TRACE("Container found");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during searching container for Title and Description", ex);
                }
                try
                {
                    log?.DEBUG("Try to get card title");
                    tutorialCard.Title = webDriverManager.FindElement(container, containerWebElement["Title"], log).Text;
                    log?.TRACE($"Card title is: {tutorialCard.Title}");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during parsing card Title", ex);
                }
                try
                {
                    log?.DEBUG("Try to get card description");
                    tutorialCard.Description = webDriverManager.FindElement(container, containerWebElement["Description"], log).Text;
                    log?.TRACE($"Card description is: {tutorialCard.Description}");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during parsing card URL. Cart title: {tutorialCard.Title}", ex);
                }
                try
                {
                    log?.DEBUG("Try to get card tags");
                    var tags = webDriverManager.FindElements(card, tutorialCardElement["Tag"], log);
                    List<string> tg = new List<string>();
                    foreach (var tag in tags)
                    {
                        log?.TRACE($"Tag: {tag.Text}");
                        tg.Add(tag.Text);
                        if (isPublish)
                        {
                            try
                            {
                                var link = webDriverManager.FindElement(tag, tutorialCardElement["Tag.TagLink"], log).GetAttribute("href");
                                log?.TRACE($"Tag: {tag.Text} link is: {link}");
                                if (tutorialCard.TagLinks == null) tutorialCard.TagLinks = new Dictionary<string, string>();
                                tutorialCard.TagLinks.Add(tag.Text, link);
                            }
                            catch (Exception ex)
                            {
                                log?.WARN($"Error occurred during parsing tag url. Tag: {tag.Text}", ex);
                                if (tutorialCard.TagLinks == null) tutorialCard.TagLinks = new Dictionary<string, string>();
                                tutorialCard.TagLinks.Add(tag.Text, null);
                            }
                        }
                    }
                    tutorialCard.Tags = tg;
                    log?.TRACE($"Found tags count: {tutorialCard.Tags.Count}");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during parsing card tags. Cart title: {tutorialCard.Title}", ex);
                }
                try
                {
                    log?.DEBUG("Try to get card status");
                    tutorialCard.Status = webDriverManager.FindElement(container, containerWebElement["Status"], log).Text;
                    log?.TRACE($"Card status is: {tutorialCard.Status}");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during parsing card status. Cart title: {tutorialCard.Title}", ex);
                }

                containerWebElement = tutorialCardElement["DeveloperText2_Container"];
                try
                {
                    log?.DEBUG("Try to find container for Content");
                    container = webDriverManager.FindElement(card, containerWebElement, log);
                    log?.TRACE("Container found");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during searching container for Content", ex);
                }
                try
                {
                    log?.TRACE("Try to get card content");
                    tutorialCard.Content = webDriverManager.FindElement(container, containerWebElement["Content"], log).Text;
                    log?.TRACE($"Card content is: {tutorialCard.Content}");
                }
                catch (Exception ex)
                {
                    log?.WARN($"Error occurred during parsing card content. Cart title: {tutorialCard.Title}", ex);
                }

                log?.DEBUG($"Getting tutorial card completed");

                return tutorialCard;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during getting tutorial card");
                throw new CommandAbortException($"Error occurred during getting tutorial card", ex);
            }
        }
    }
}
