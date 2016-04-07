namespace SapAutomation.Web.Pages.Sap.TutorialCatalogPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using Components.Sap.TutorialCatalogComponent;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic;
    using OpenQA.Selenium;
    using QA.AutomatedMagic.MetaMagic;

    [CommandManager("Tutorial catalog page manager")]
    public class TutorialCatalogPageManager : BaseCommandManager
    {
        public TutorialCatalogComponentManager TutorialCatalogManager { get; set; }

        [MetaSource(nameof(TutorialCatalogPage) + @"\TutorialCatalogAuthorPageWebDefenition.xml")]
        public WebElement TutorialCatalogAuthorPageWebDefenition { get; set; }

        [MetaSource(nameof(TutorialCatalogPage) + @"\TutorialCardWebDefinition.xml")]
        public WebElement TutorialCardWebDefinition { get; set; }

        public WebElement TutorialCatalogPublishPageWebDefenition { get; set; }

        public TutorialCatalogPageManager()
        {
            TutorialCatalogManager = AutomatedMagicManager.CreateCommandManager<TutorialCatalogComponentManager>();
        }

        public override void Init()
        {
            TutorialCatalogAuthorPageWebDefenition.ChildWebElements.Add(TutorialCatalogManager.TutorialCatalogComponentWebDefinition);
            TutorialCatalogAuthorPageWebDefenition.ChildWebElements.Add(MetaType.CopyObjectWithCast(TutorialCardWebDefinition));
            TutorialCatalogAuthorPageWebDefenition.Init();

            TutorialCatalogPublishPageWebDefenition = MetaType.CopyObjectWithCast(TutorialCardWebDefinition);
            TutorialCatalogPublishPageWebDefenition.Init();
        }

        [Command("Get tutorial card list on publish")]
        public List<TutorialCard> GetTutorialCardsOnPublish(WebDriverManager webDriverManager, ILogger log)
        {
            return GetTutorialCards(TutorialCatalogPublishPageWebDefenition, webDriverManager, true, log);
        }

        [Command("Get tutorial card list on author")]
        public List<TutorialCard> GetTutorialCardsOnAuthor(WebDriverManager webDriverManager, ILogger log)
        {
            var tutorialCard = TutorialCatalogAuthorPageWebDefenition["TutorialCard"];
            return GetTutorialCards(tutorialCard, webDriverManager, false, log);
        }

        private List<TutorialCard> GetTutorialCards(WebElement tutorialCardElement, WebDriverManager webDriverManager, bool isPublish, ILogger log)
        {
            try
            {
                log?.INFO($"Get tutorial card list'");
                var list = new List<TutorialCard>();

                var cards = webDriverManager.FindElements(tutorialCardElement, log);
                log?.DEBUG($"Find '{cards.Count}' cards'");

                foreach (var card in cards)
                {
                    var tutorialCard = new TutorialCard();
                    tutorialCard.Location = card.Location;
                    try
                    {
                        log?.TRACE("Try to get card title");
                        tutorialCard.Title = webDriverManager.FindElement(card, tutorialCardElement["Title"], log).Text;
                        log?.TRACE($"Card title is: {tutorialCard.Title}");
                    }
                    catch (Exception ex)
                    {
                        log?.WARN($"Error occurred during parsing card Title. Card index: {cards.IndexOf(card)}", ex);
                        tutorialCard.Title = null;
                    }
                    try
                    {
                        log?.TRACE("Try to get card URL");
                        tutorialCard.URL = webDriverManager.FindElement(card, tutorialCardElement["Url"], log).GetAttribute("href");
                        log?.TRACE($"Card URL is: {tutorialCard.URL}");

                        var name = tutorialCard.URL.Substring(tutorialCard.URL.LastIndexOf("/"));
                        name = name.Substring(1, name.LastIndexOf(".") - 1).ToLower();
                        tutorialCard.Name = name;
                    }
                    catch (Exception ex)
                    {
                        log?.WARN($"Error occurred during parsing card URL. Cart title: {tutorialCard?.Title}", ex);
                        tutorialCard.URL = null;
                    }
                    try
                    {
                        log?.TRACE("Try to get card description");
                        tutorialCard.Description = webDriverManager.FindElement(card, tutorialCardElement["Description"], log).Text;
                        log?.TRACE($"Card description is: {tutorialCard.Description}");
                    }
                    catch (Exception ex)
                    {
                        log?.WARN($"Error occurred during parsing card URL. Cart title: {tutorialCard?.Title}", ex);
                        tutorialCard.Description = null;
                    }
                    try
                    {
                        log?.TRACE("Try to get card tags");
                        var tags = webDriverManager.FindElements(card, tutorialCardElement["Tag"], log);
                        List<string> tg = new List<string>();
                        Dictionary<string, string> tgl = new Dictionary<string, string>();
                        foreach (var tag in tags)
                        {
                            tg.Add(tag.Text);
                            if (isPublish)
                            {
                                try
                                {
                                    var link = webDriverManager.FindElement(tag, tutorialCardElement["Tag.TagLink"], log).GetAttribute("href");
                                    log?.TRACE($"Tag: {tag.Text} link is: {link}");
                                    tgl.Add(tag.Text, link);
                                }
                                catch(Exception ex)
                                {
                                    log?.WARN($"Error occurred during parsing tag url. Tag: {tag.Text}", ex);
                                    tgl = null;
                                }
                            }
                        }
                        tutorialCard.Tags = tg;
                        if (isPublish)
                        {
                            tutorialCard.TagLinks = tgl;
                        }
                        log?.TRACE($"Card tags is: {tutorialCard.Tags.ToString()}");
                    }
                    catch (Exception ex)
                    {
                        log?.WARN($"Error occurred during parsing card tags. Cart title: {tutorialCard?.Title}", ex);
                        tutorialCard.Tags = null;
                    }
                    try
                    {
                        log?.TRACE("Try to get card status");
                        tutorialCard.Status = webDriverManager.FindElement(card, tutorialCardElement["Status"], log).Text;
                        log?.TRACE($"Card status is: {tutorialCard.Status}");
                    }
                    catch (Exception ex)
                    {
                        log?.WARN($"Error occurred during parsing card status. Cart title: {tutorialCard?.Title}", ex);
                        tutorialCard.Status = null;
                    }
                    list.Add(tutorialCard);
                }

                log?.DEBUG($"Getting tutorial card list completed");
                log?.INFO($"Found cards count: {list.Count}");
                return list;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during getting tutorial cards");
                throw new CommandAbortException($"Error occurred during getting tutorial cards", ex);
            }
        }
    }
}
