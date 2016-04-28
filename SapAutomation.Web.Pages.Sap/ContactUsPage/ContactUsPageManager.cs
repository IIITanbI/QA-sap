namespace SapAutomation.Web.Pages.Sap.ContactUsPage
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;

    [CommandManager("ContactUsPageManager")]
    public class ContactUsPageManager : BaseCommandManager
    {
        [MetaSource(nameof(ContactUsPage) + @"\ContactUsPageWebDefinition.xml")]
        public WebElement ContactUsPageWebDefinition { get; set; }

        public override void Init()
        {
            ContactUsPageWebDefinition.Init();
        }

        [Command("Command for contact us form setup")]
        public void SetUpContactUsForm(WebDriverManager webDriverManager, ContactUsPageConfig config, ILogger log)
        {
            log?.INFO("Start contact us form setup");

            try
            {
                if (config.WritingAbout != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["WritingAbout_Dropdown"], ContactUsPageWebDefinition["WritingAbout_Dropdown.Option"], config.WritingAbout, log);

                if (config.Industry != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["Industry_Dropdown"], ContactUsPageWebDefinition["Industry_Dropdown.Option"], config.Industry, log);

                if (config.SAPSolutionOfInterest != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["SAPSolutionOfInterest_Dropdown"], ContactUsPageWebDefinition["SAPSolutionOfInterest_Dropdown.Option"], config.SAPSolutionOfInterest, log);

                if (config.CompanyRevenue != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["CompanyRevenue_Dropdown"], ContactUsPageWebDefinition["CompanyRevenue_Dropdown.Option"], config.CompanyRevenue, log);

                if (config.Message != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["Message_Input"], config.Message, log);

                if (config.OfficeLocation != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["OfficeLocation_Dropdown"], ContactUsPageWebDefinition["OfficeLocation_Dropdown.Option"], config.OfficeLocation, log);

                if (config.Salutation != null)
                {
                    webDriverManager.Select(ContactUsPageWebDefinition["Salutation_Dropdown"], ContactUsPageWebDefinition["Salutation_Dropdown.Option"], config.Salutation, log);

                    webDriverManager.Click(ContactUsPageWebDefinition["Email_RadioButton.Yes"], log);
                    webDriverManager.Click(ContactUsPageWebDefinition["Telephone_RadioButton.No"], log);
                }

                if (config.ContactMethod != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["ContactMethod_Dropdown"], ContactUsPageWebDefinition["ContactMethod_Dropdown.Option"], config.ContactMethod, log);

                if (config.FirstName != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["FirstName_Input"], config.FirstName, log);

                if (config.Company != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["Company_Input"], config.Company, log);

                if (config.LastName != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["LastName_Input"], config.LastName, log);

                if (config.RelationshipToSap != null)
                    webDriverManager.Select(ContactUsPageWebDefinition["RelationshipToSap_Dropdown"], ContactUsPageWebDefinition["RelationshipToSap_Dropdown.Option"], config.RelationshipToSap, log);

                if (config.Email != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["Email_Input"], config.Email, log);

                if (config.City != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["City_Input"], config.City, log);

                if (config.Street != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["Street_Input"], config.Street, log);

                if (config.City != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["City_Input"], config.City, log);

                if (config.Phone != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["Phone_Input"], config.Phone, log);

                webDriverManager.Click(ContactUsPageWebDefinition["Submit_Button"], log);

                log?.INFO("Contact us form setup successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during contact us form setup");
                throw new DevelopmentException("Error occurred during contact us form setup", ex);
            }
        }
    }
}
