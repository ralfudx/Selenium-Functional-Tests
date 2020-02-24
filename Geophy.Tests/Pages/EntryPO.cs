using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Geophy.Tests
{
    class EntryPO
    {
        public EntryPO()
        {
            PageFactory.InitElements(SeleniumBase._browserDriver, this);
            BackToBaseURL(SeleniumBase._browserDriver);
        }

        [FindsBy(How = How.Id, Using = "email")]
        public IWebElement emailTextField { get; set; }

        [FindsBy(How = How.Id, Using = "password")]
        public IWebElement passwordTextField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span.checkmark")]
        public IWebElement checkBox { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "button[type='submit']")]
        public IWebElement submitButton { get; set; } 

        [FindsBy(How = How.Id, Using = "first-name")]
        public IWebElement firstNameTextField { get; set; }

        [FindsBy(How = How.Id, Using = "last-name")]
        public IWebElement lastNameTextField { get; set; }

        [FindsBy(How = How.Id, Using = "company")]
        public IWebElement companyTextField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "li.mt-2")]
        public IWebElement loginErrorMsg { get; set; }

        [FindsBy(How = How.CssSelector, Using = "ul.list-disc li")]
        public IList<IWebElement> loginErrorMsgs { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div.my-8 a")]
        public IWebElement signUpLinkText { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='flex items-center mb-2']/h2[2]")]
        public IWebElement entryHeaderText { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div.alert--warning label")]
        public IWebElement unverifiedEmailMsg { get; set; }


        public SearchPO Login(Enum enum_item, string email, string password)
        {
            if(enum_item.Equals(EntryValidationField.Complete))
            {
                emailTextField.EnterText(email);
                passwordTextField.EnterText(password);
                checkBox.ClickElem();
            }
            else if(enum_item.Equals(EntryValidationField.Email))
            {
                passwordTextField.EnterText(password);
            }
            else if(enum_item.Equals(EntryValidationField.Password))
            {
                emailTextField.EnterText(email);
            }
            
            submitButton.ClickElem();
            return new SearchPO();
        }

        public void SignUp(Enum enum_item, string email, string password, string first_name, string last_name)
        {
            signUpLinkText.ClickElem();

            SignUpFillFormFields(enum_item, email, password, first_name, last_name);
            
            companyTextField.EnterText("company".GetData());
            submitButton.ClickElem();
        }

        public string CheckListText(IList<IWebElement> elem_list, string text)
        {
            string saved_text = String.Empty;
            foreach (IWebElement elem in elem_list)
            {
                if(elem.Text.Contains(text))
                {
                    saved_text = elem.Text;
                }
            }
            Console.WriteLine($"Matched message is: {saved_text}");
            return saved_text;
        }

        public string GetValidationMessage(Enum enum_item)
        {
            string val_text = string.Empty;
            switch(enum_item)
            {
                case EntryValidationField.Email:
                    val_text = "validation".GetSectionData("emailRequiredField");
                    break;
                case EntryValidationField.Password:
                    val_text = "validation".GetSectionData("passwordRequiredField");
                    break;
                case EntryValidationField.FirstName:
                    val_text = "validation".GetSectionData("firstNameRequiredField");
                    break;
                case EntryValidationField.LastName:
                    val_text = "validation".GetSectionData("lastNameRequiredField");
                    break;
                case EntryValidationField.CheckboxTandC:
                    val_text = "validation".GetSectionData("uncheckedTandC");
                    break;
            }
            return val_text;
        }

        public void SignUpFillFormFields(Enum enum_item, string email, string password, string first_name, string last_name)
        {
            switch(enum_item)
            {
                case EntryValidationField.Email:
                    email = string.Empty;
                    break;
                case EntryValidationField.Password:
                    password = string.Empty;
                    break;
                case EntryValidationField.FirstName:
                    first_name = string.Empty;
                    break;
                case EntryValidationField.LastName:
                    last_name = string.Empty;
                    break;
                case EntryValidationField.CheckboxTandC: 
                    checkBox.ClickElem();
                    break;
            }
            firstNameTextField.EnterText(first_name);
            lastNameTextField.EnterText(last_name);
            emailTextField.EnterText(email);
            passwordTextField.EnterText(password);
            checkBox.ClickElem();
        }
        public void BackToBaseURL(IWebDriver driver)
        {
            if(driver.Url != "dashboardurl".GetData())
            {
                driver.Navigate().GoToUrl("dashboardurl".GetData());
            }
            else
            {
                driver.Navigate().Refresh();
            }
        }

        public string BuildEmail()
        {
            string email = string.Empty;
            string first_part = 500.GenerateRandomNumber(100).ToString();
            string second_part = 999.GenerateRandomNumber(501).ToString();
            email = $"test{first_part}{second_part}@gmail.com";
            Console.WriteLine($"New user email is: {email}");
            return email;
        }
    }
}