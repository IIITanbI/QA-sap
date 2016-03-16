namespace SapAutomation.Web.Pages.Sap.TutorialCatalogPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using Components.Sap.TutorialCatalogComponent;
    using QA.AutomatedMagic.WebDriverManager;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic;

    [CommandManager("Tutorial catalog page manager")]
    public class TutorialCatalogPageManager : BaseCommandManager
    {
        public TutorialCatalogComponentManager TutorialCatalogManager { get; set; }

        [MetaSource(nameof(TutorialCatalogPage) + @"\TutorialCatalogPageWebDefinition.xml")]
        public WebElement TutorialCatalogPageWebDefenition { get; set; }

        public TutorialCatalogPageManager()
        {
            TutorialCatalogManager = AutomatedMagicManager.CreateCommandManager<TutorialCatalogComponentManager>();
        }

        public override void Init()
        {
            TutorialCatalogPageWebDefenition.ChildWebElements.Add(TutorialCatalogManager.TutorialCatalogComponentWebDefinition);
            TutorialCatalogPageWebDefenition.Init();
        }

        [Command("Get tutorial card list on author")]
        public List<TutorialCard> GetTutorialCardsOnAuthor(WebDriverManager webDriverManager, ILogger log)
        {
            try
            {
                log?.INFO($"Get tutorial card list'");
                var list = new List<TutorialCard>();

                var cards = webDriverManager.FindElements(TutorialCatalogPageWebDefenition["Root.TutorialCard"], log);
                log?.DEBUG($"Find '{cards.Count}' cards'");

                var tutorialCard = new TutorialCard();
                foreach (var card in cards)
                {
                    try
                    {
                        log?.TRACE("Try to get card title");
                        tutorialCard.Title = webDriverManager.FindElement(card, TutorialCatalogPageWebDefenition["Root.TutorialCard.Title"], log).Text;
                        log?.TRACE($"Card title is: {tutorialCard.Title}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error occurred during parsing card Title. Card index: {cards.IndexOf(card)}", ex);
                    }
                    try
                    {
                        log?.TRACE("Try to get card URL");
                        tutorialCard.URL = webDriverManager.FindElement(card, TutorialCatalogPageWebDefenition["Root.TutorialCard.Title"], log).GetAttribute("href");
                        log?.TRACE($"Card URL is: {tutorialCard.URL}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error occurred during parsing card URL. Cart title: {tutorialCard.Title}", ex);
                    }
                    try
                    {
                        log?.TRACE("Try to get card tags");
                        var tags = webDriverManager.FindElements(TutorialCatalogPageWebDefenition["Root.TutorialCard.Tag"], log);
                        List<string> tg = new List<string>();
                        foreach (var tag in tags)
                        {
                            tg.Add(tag.Text);
                        }
                        tutorialCard.Tags = tg;
                        log?.TRACE($"Card tags is: {tutorialCard.Tags.ToString()}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error occurred during parsing card tags. Cart title: {tutorialCard.Title}", ex);
                    }
                    try
                    {
                        log?.TRACE("Try to get card status");
                        tutorialCard.Status = webDriverManager.FindElement(card, TutorialCatalogPageWebDefenition["Root.TutorialCard.Status"], log).Text;
                        log?.TRACE($"Card status is: {tutorialCard.Status}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error occurred during parsing card status. Cart title: {tutorialCard.Title}", ex);
                    }
                    list.Add(tutorialCard);
                }

                log?.INFO($"Getting tutorial card list on author completed");
                return list;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during getting tutorial cards on author");
                throw new CommandAbortException($"Error occurred during getting tutorial cards on author", ex);
            }
        }
    }
}
