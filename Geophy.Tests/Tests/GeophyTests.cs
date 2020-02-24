using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;
using System.Resources;
using Xunit;

namespace Geophy.Tests
{
    public class GeophyTests : IClassFixture<SeleniumBase>
    {
        [Fact]
        [Trait("Category", "SmokeTest")]
        public void Verify_RunValuation_Details()
        {
            //---- Pre-condition -> user must be logged in -----//
            EntryPO entry = new EntryPO();
            SearchPO search = entry.Login(EntryValidationField.Complete, "email".GetData(), "password".GetData());
            Assert.Contains("validation".GetSectionData("successfulLogin"), search.welcomeText.GetText());
            //---- Perform task -----//
            search.RunValuation("address".GetData(), "netOperatingIncome".GetData(), "numberOfUnits".GetData(), "yearOfConstruction".GetData());
            search.CheckHeaderTexts();
            Assert.True(search.ValidateValuationDetails("netOperatingIncome".GetData(), "numberOfUnits".GetData(), "yearOfConstruction".GetData()));
            
            //clean up - for other tests
            search.Logout();
            Assert.Contains("validation".GetSectionData("loginFormHeader"), entry.entryHeaderText.GetText());
        }


        [Fact]
        [Trait("Category", "SmokeTest")]
        public void Verify_RunValuation_ModifyInputDetails()
        {
            //---- Pre-condition -> user must be logged in -----//
            EntryPO entry = new EntryPO();
            SearchPO search = entry.Login(EntryValidationField.Complete, "email".GetData(), "password".GetData());
            Assert.Contains("validation".GetSectionData("successfulLogin"), search.welcomeText.GetText());
            //---- Pre-condition -> user must runValuation first -----//
            search.RunValuation("address".GetData(), "netOperatingIncome".GetData(), "numberOfUnits".GetData(), "yearOfConstruction".GetData());
            search.CheckHeaderTexts();
            Assert.True(search.ValidateValuationDetails("netOperatingIncome".GetData(), "numberOfUnits".GetData(), "yearOfConstruction".GetData()));
            //---- Perform task -----//
            search.ClickEditLink();
            search.RunValuation("address".GetData(), "netOperatingIncome2".GetData(), "numberOfUnits2".GetData(), "yearOfConstruction2".GetData());
            search.CheckHeaderTexts();
            Assert.True(search.ValidateValuationDetails("netOperatingIncome2".GetData(), "numberOfUnits2".GetData(), "yearOfConstruction2".GetData()));
            
            //---- clean up -> for other tests ----//
            search.Logout();
            Assert.Contains("validation".GetSectionData("loginFormHeader"), entry.entryHeaderText.GetText());
        }

        [Fact]
        [Trait("Category", "SmokeTest")]
        public void Verify_SignUp()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.Complete, entry.BuildEmail(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            Assert.Contains("validation".GetSectionData("successfulSignupHeader"), entry.entryHeaderText.GetText());
        }

        [Fact]
        [Trait("Category", "SmokeTest")]
        public void Verify_Login()
        {
            EntryPO entry = new EntryPO();
            SearchPO search = entry.Login(EntryValidationField.Complete, "email".GetData(), "password".GetData());
            Assert.Contains("validation".GetSectionData("successfulLogin"), search.welcomeText.GetText());

            //---- clean up -> for other tests ----//
            search.Logout();
            Assert.Contains("validation".GetSectionData("loginFormHeader"), entry.entryHeaderText.GetText());
        }

        [Fact]
        [Trait("Category", "SmokeTest")]
        public void Verify_Logout()
        {
            //---- Pre-condition -> user must be logged in -----//
            EntryPO entry = new EntryPO();
            SearchPO search = entry.Login(EntryValidationField.Complete, "email".GetData(), "password".GetData());
            //---- Perform task -----//
            search.Logout();
            Assert.Contains("validation".GetSectionData("loginFormHeader"), entry.entryHeaderText.GetText());
        }


        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_Login_WithoutEmail()
        {
            EntryPO entry = new EntryPO();
            entry.Login(EntryValidationField.Email, "email".GetData(), "password".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.Email);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_Login_WithoutPassword()
        {
            EntryPO entry = new EntryPO();
            entry.Login(EntryValidationField.Password, "email".GetData(), "password".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.Password);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_Login_WithUnverifiedEmail()
        {
            EntryPO entry = new EntryPO();
            entry.Login(EntryValidationField.Complete, "unverifiedEmail".GetData(), "password".GetData());
            Assert.Contains("validation".GetSectionData("unverifiedEmailLogin"), entry.unverifiedEmailMsg.GetText());
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_Login_WithIncorrectDetails()
        {
            EntryPO entry = new EntryPO();
            entry.Login(EntryValidationField.Complete, "email".GetData(), "unverifiedEmail".GetData());
            Assert.Contains("validation".GetSectionData("incorrectLoginDetails"), entry.unverifiedEmailMsg.GetText());
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_SignUp_WithoutEmail()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.Email, "email".GetData(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.Email);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_SignUp_WithoutPassword()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.Password, "email".GetData(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.Password);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_SignUp_WithoutFirstName()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.FirstName, "email".GetData(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.FirstName);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_SignUp_WithoutLastName()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.LastName, "email".GetData(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.LastName);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_SignUp_WithoutCheckBoxTandC()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.CheckboxTandC, "email".GetData(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            string val_text = entry.GetValidationMessage(EntryValidationField.CheckboxTandC);
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }

        [Fact]
        [Trait("Category", "RegressionTests")]
        public void Verify_SignUp_WithRegisteredEmail()
        {
            EntryPO entry = new EntryPO();
            entry.SignUp(EntryValidationField.Complete,"unverifiedEmail".GetData(), "password".GetData(), "firstName".GetData(), "lastName".GetData());
            string val_text = "validation".GetSectionData("registeredEmailSignup");
            Assert.Contains(val_text, entry.CheckListText(entry.loginErrorMsgs, val_text));
        }



    }
}
