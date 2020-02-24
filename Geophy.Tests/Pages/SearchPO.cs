using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Geophy.Tests
{
    class SearchPO
    {
        public static String _SelectedAddress { get; set; }
        public SearchPO()
        {
            PageFactory.InitElements(SeleniumBase._browserDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "span[class^='hidden lg']")]
        public IWebElement welcomeText { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Logout']")]
        public IWebElement logoutLinkText { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Contact Us']")]
        public IWebElement contactUsLinkText { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Account']")]
        public IWebElement accountLinkText { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='History']")]
        public IWebElement historyLinkText { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Search']")]
        public IWebElement searchLinkText { get; set; }

        [FindsBy(How = How.Id, Using = "address_input")]
        public IWebElement addressTextField { get; set; } 

        [FindsBy(How = How.CssSelector, Using = "div.pac-container div")]
        public IWebElement googleDropdownText { get; set; }

        [FindsBy(How = How.Id, Using = "noi")]
        public IWebElement incomeTextField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[name=number_of_units]")]
        public IWebElement unitsTextField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[name=year_built]")]
        public IWebElement yearBuiltTextField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[name=year_renovated]")]
        public IWebElement yearRenovatedTextField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "input[name=occupancy]")]
        public IWebElement occupancyTextField { get; set; }

        [FindsBy(How = How.Id, Using = "introjsRunValuationButton")]
        public IWebElement runValidationButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div.flex-row div h1")]
        public IWebElement propertyHeaderText { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "div#value-drivers div h1")]
        public IWebElement valueDriversHeaderText { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "div#neighborhood div h1")]
        public IWebElement neighborhoodHeaderText { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[text()='Multifamily']")]
        public IWebElement searchHeaderText { get; set; }

        [FindsBy(How = How.CssSelector, Using = "a[class='underline lowercase']")]
        public IWebElement editLinkText { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "div[class='sm:w-1/2 sm:pl-4'] h4")]
        public IWebElement addressText { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[text()='Number of units']/parent::*/td[2]")]
        public IWebElement unitsText { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[text()='Year of construction']/parent::*/td[2]")]
        public IWebElement yearOfConstructionText { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[text()='Year of renovation']/parent::*/td[2]")]
        public IWebElement yearOfRenovationText { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[text()='NOI']/parent::*/td[2]")]
        public IWebElement incomeText { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[text()='NOI per unit']/parent::*/td[2]")]
        public IWebElement incomePerUnitText { get; set; }

        [FindsBy(How = How.XPath, Using = "//td[text()='Occupancy']/parent::*/td[2]")]
        public IWebElement occupancyText { get; set; }



        public EntryPO Logout()
        {
            logoutLinkText.ClickElem();
            return new EntryPO();
        }

        public void RunValuation(string address, string income, string units, string year)
        {
            addressTextField.EnterText(address);
            googleDropdownText.ClickElem();
            incomeTextField.EnterText(income);
            unitsTextField.EnterText(units);
            yearBuiltTextField.EnterText(year);
            occupancyTextField.EnterText("occupancy".GetData());
            yearRenovatedTextField.EnterText("yearOfRenovation".GetData());
            _SelectedAddress = addressTextField.GetTextByValue();
            Console.WriteLine($"Inputed address is : {_SelectedAddress}");
            runValidationButton.ClickElem();
        }

        public void EnterSearchDetails(Enum enum_item, string address, string income, string units, string year)
        {
            switch(enum_item)
            {
                case SearchValidationField.Address:
                    address = string.Empty;
                    break;
                case SearchValidationField.Income:
                    income = string.Empty;
                    break;
                case SearchValidationField.Units:
                    units = string.Empty;
                    break;
                case SearchValidationField.Year:
                    year = string.Empty;
                    break;
            }
            addressTextField.EnterText(address);
            incomeTextField.EnterText(income);
            unitsTextField.EnterText(units);
            yearBuiltTextField.EnterText(year);
        }

        public void CheckHeaderTexts()
        {
            propertyHeaderText.ValPageHeaderText("validation".GetSectionData("propertyHeader"));
            valueDriversHeaderText.ValPageHeaderText("validation".GetSectionData("valueDriversHeader"));
            neighborhoodHeaderText.ValPageHeaderText("validation".GetSectionData("neighborhoodHeader"));
        }

        public bool ValidateValuationDetails(string income, string units, string year)
        {
            IList<bool> results = new List<bool>();
            int income_per_unit = income.StringToInt()/units.StringToInt();

            results.Add(addressText.GetText().Contains(_SelectedAddress.ToUpper()));
            results.Add(unitsText.ElemEqualsNumber(units));
            results.Add(yearOfConstructionText.ElemEqualsText(year));
            results.Add(incomeText.ElemEqualsNumber(income));
            results.Add(incomePerUnitText.ElemEqualsNumber(income_per_unit.ToString()));
            results.Add(yearOfRenovationText.ElemEqualsText("yearOfRenovation".GetData()));
            results.Add(occupancyText.ElemEqualsText("occupancy".GetData() + "%"));
            Console.WriteLine($"Validation details obtained are : {results[0]} : {results[1]} : {results[2]} : {results[3]}");
            
            if(results.Contains(false))
            {
                return false;
            }else
            {
                return true;
            }
        }

        public void ClickEditLink()
        {
            editLinkText.ClickElem();
            searchHeaderText.ValPageHeaderText("validation".GetSectionData("searchFormHeader"));
        }


    }
}