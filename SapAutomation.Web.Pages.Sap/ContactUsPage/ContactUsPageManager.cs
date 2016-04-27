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

        [Command("Command for contact us form setup")]
        public void SetUpContactUsForm(WebDriverManager webDriverManager, ContactUsPageManagerConfig config, ILogger log)
        {
            log?.INFO("Start contact us form setup");

            try
            {
                if (config.WritingAbout != WritingAbouts.NONE)
                    webDriverManager.Select(ContactUsPageWebDefinition["WritingAboutDropdown"], ContactUsPageWebDefinition["WritingAboutDropdown.WritingAboutDropdownOption"], config.WritingAbout.ToString(), log);

                switch (config.WritingAbout)
                {
                    case WritingAbouts.General:
                        break;
                    case WritingAbouts.Industry:
                        {
                            if (config.Industry != Industries.NONE)
                                webDriverManager.Select(ContactUsPageWebDefinition["IndustryDropdown"], ContactUsPageWebDefinition["IndustryDropdown.IndustryDropdownOption"], config.Industry.ToString(), log);

                            if (config.SAPSolutionOfInterest != SAPSolutionsOfInterest.NONE)
                                webDriverManager.Select(ContactUsPageWebDefinition["SAPSolutionOfInterestDropdown"], ContactUsPageWebDefinition["SAPSolutionOfInterestDropdown.SAPSolutionOfInterestDropdownOption"], config.SAPSolutionOfInterest.ToString(), log);

                            if (config.CompanyRevenue != CompanyRevenues.NONE)
                                webDriverManager.Select(ContactUsPageWebDefinition["CompanyRevenueDropdown"], ContactUsPageWebDefinition["CompanyRevenueDropdown.CompanyRevenueDropdownOption"], config.CompanyRevenue.ToString(), log);
                            break;
                        }
                    default:
                        throw new NotImplementedException($"There are no workflow implementation for {config.WritingAbout} writing option");
                }

                if (config.Message != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["MessageField"], config.Message, log);

                if (config.OfficeLocation != OfficeLocations.NONE)
                    webDriverManager.Select(ContactUsPageWebDefinition["OfficeLocationDropdown"], ContactUsPageWebDefinition["OfficeLocationDropdown.OfficeLocationDropdownOption"], config.OfficeLocation.ToString(), log);

                switch (config.OfficeLocation)
                {
                    case OfficeLocations.GB:
                        {
                            if (config.Salutation != Salutations.NONE)
                            {
                                var value = config.Salutation.ToString().Equals("Mr") ? "0002" : "0001";
                                webDriverManager.Select(ContactUsPageWebDefinition["SalutationDropdown"], ContactUsPageWebDefinition["SalutationDropdown.SalutationDropdownOption"], value, log);
                            }

                            log?.USEFULL($"Turn on additional info by email");
                            webDriverManager.Click(ContactUsPageWebDefinition["EmailRadioButton.EmailRadioButtonYes"], log);

                            log?.USEFULL($"Turn off additional info by telephone");
                            webDriverManager.Click(ContactUsPageWebDefinition["TelephoneRadioButton.TelephoneRadioButtonNo"], log);

                            break;
                        }
                    case OfficeLocations.US:
                        break;
                    default:
                        throw new NotImplementedException($"There are no workflow implementation for {config.OfficeLocation} office location");
                }

                if (config.ContactMethod != ContactMethods.NONE)
                    webDriverManager.Select(ContactUsPageWebDefinition["ContactMethodDropdown"], ContactUsPageWebDefinition["ContactMethodDropdown.ContactMethodDropdownOption"], config.ContactMethod.ToString(), log);

                if (config.FirstName != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["FirstNameField"], config.FirstName, log);

                if (config.Company != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["CompanyField"], config.Company, log);

                if (config.LastName != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["LastNameField"], config.LastName, log);

                if (config.RelationshipToSap != RelationshipsToSap.NONE)
                    webDriverManager.Select(ContactUsPageWebDefinition["RelationshipToSapDropdown"], ContactUsPageWebDefinition["RelationshipToSapDropdown.RelationshipToSapOption"], config.RelationshipToSap.ToString(), log);

                if (config.Email != null)
                    webDriverManager.SendKeys(ContactUsPageWebDefinition["EmailField"], config.Email, log);

                switch (config.ContactMethod)
                {
                    case ContactMethods.email:
                        {
                            if (config.City != null)
                                webDriverManager.SendKeys(ContactUsPageWebDefinition["CityField"], config.City, log);
                            break;
                        }
                    case ContactMethods.mail:
                        {
                            if (config.Street != null)
                                webDriverManager.SendKeys(ContactUsPageWebDefinition["StreetField"], config.Street, log);

                            if (config.City != null)
                                webDriverManager.SendKeys(ContactUsPageWebDefinition["CityField"], config.City, log);

                            if (config.Phone != null)
                                webDriverManager.SendKeys(ContactUsPageWebDefinition["PhoneField"], config.Phone, log);
                            break;
                        }
                    case ContactMethods.phone:
                        {
                            if (config.Phone != null)
                                webDriverManager.SendKeys(ContactUsPageWebDefinition["PhoneField"], config.Phone, log);
                            break;
                        }
                    default:
                        throw new NotImplementedException($"There are no workflow implementation for {config.ContactMethod} preferred contact method");
                }
                webDriverManager.Click(ContactUsPageWebDefinition["SubmitButton"], log);

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
