using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace Geophy.Tests
{
    static class Helper
    {
        public static void ClickElem(this IWebElement element)
        {
            element.WaitUntilElementClickable();
            element.Click();
        }

        public static void EnterText(this IWebElement element, string value)
        {
            element.WaitUntilElementClickable();
            element.Clear();
            element.SendKeys(value);
        }

        public static void WaitUntilElementClickable(this IWebElement elem)
        {
            WebDriverWait wait = new WebDriverWait(SeleniumBase._browserDriver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(elem));
        }

        public static void ClickWithActions(this IWebElement element)
        {
            Actions action = new Actions(SeleniumBase._browserDriver);
            action.MoveToElement(element).Click().Build().Perform();
        }
        public static void AdvClickElem(this IWebElement element)
        {
            IJavaScriptExecutor ex = (IJavaScriptExecutor)SeleniumBase._browserDriver;
            ex.ExecuteScript("arguments[0].click();", element);
        }

        public static String SelectBrowser(string browserName)
        {
            if(browserName == "CHROME")
            {
                return "CHROME";
            }
            if(browserName == "FIREFOX")
            {
                return "CHROME";
            }
            if(browserName == "IE")
            {
                return "IE";
            }
            else{
                return "CHROME";
            }
        }

        public static void SelectDDText(this IWebElement element, string value)
        {
            element.WaitUntilElementClickable();
            new SelectElement(element).SelectByText(value); 
        }

        public static IList<string> GetAllDDOptions(this IWebElement element)
        {
            SelectElement selectElem = new SelectElement(element);
            IList<IWebElement> selectOptions = selectElem.Options;
            IList<string> optionNames = new List<string>();
            
            foreach(IWebElement elem in selectOptions)
            {
                optionNames.Add(elem.GetText());
            }
            return optionNames;  
        }

        public static string SelectRandomDDOption(this IWebElement currentTxt, IWebElement dropdown_elem, bool place_holder)
        {
            string current_option = currentTxt.GetText();
            IList<string> all_options = dropdown_elem.GetAllDDOptions();
            if(place_holder)
            {
                all_options.RemoveAt(0);
            }
            all_options.Remove(current_option);
            int new_index = all_options.Count.GenerateRandomNumber(0);
            string new_option = all_options[new_index];
            dropdown_elem.SelectDDText(new_option);
            return new_option;
        }
        
        public static String GetText(this IWebElement element)
        {
            element.WaitUntilElementClickable();
            return element.Text;
        }
        public static String GetTextByValue(this IWebElement element)
        {
            element.WaitUntilElementClickable();
            return element.GetAttribute("value");
        }

        public static String GetTextByInnerHTML(this IWebElement element)
        {
            return element.GetAttribute("innerHTML");
        }

        public static void ValPageHeaderText(this IWebElement element, string value)
        {
            element.WaitUntilElementClickable();
            string page_text = element.Text;
            bool result = page_text.Contains(value);
            if (result)
            {
                Console.WriteLine("Page Header is: " + page_text);
            }
            else
            {
                Console.WriteLine("Page Header does not contain: " + value);
                Assert.True(result);
            }
        }

        public static void ValPageTitle(this string page_title)
        {
            string _pagetitle = SeleniumBase._browserDriver.Title;
            bool result = _pagetitle.Contains(page_title);
            if (result)
            {
                Console.WriteLine("Page Title is: " + _pagetitle);
            }
            else
            {
                Console.WriteLine("Page Title does not contain: " + page_title);
                Assert.True(result);
            }
        }

        public static void WaitBeforeAction(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        public static void WaitUntilElementPresent(this IWebDriver driver, By elem)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(elem));
        }

        public static int GenerateRandomNumber(this int max_num, int min_num)
        {
            Random rnd = new Random();
            return rnd.Next(min_num, max_num);
        }

        public static bool IsAlertAccepted(this IWebDriver driver)
        {
            bool presentFlag = false;
            
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                presentFlag = true;
                alert.Accept();
            } 
            catch (NoAlertPresentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return presentFlag;
        }

        public static bool IsEqual(string str1, string str2)
        {
            return str1.Equals(str2);
        }

        public static string GetPreviousDate()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);
            return yesterday.ToString("dd/MM/yyyy");
        }

        public static void SwitchToCurrentWindow(this IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public static void SwitchToPreviousWindow(this IWebDriver driver, string page_title)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.SwitchTo().Window(driver.WindowHandles.First());
            wait.Until(_driver => driver.Title.Contains(page_title));
        }

        public static int StringToInt(this string numeric_text)
        {
            numeric_text = numeric_text.Replace(",", string.Empty).Replace("$", string.Empty);
            return int.Parse(numeric_text);
        }

        public static bool ElemEqualsText(this IWebElement element, string exp_text)
        {
            string actual_text = element.Text;
            bool result = actual_text.Equals(exp_text);
            if (!result){
                 Console.WriteLine($"{actual_text} :: is not equal to :: {exp_text}");
            }
            return result;
        }

        public static bool ElemEqualsNumber(this IWebElement element, string exp_text)
        {
            int actual_num = element.Text.StringToInt();
            int exp_num = exp_text.StringToInt();
            bool result = actual_num.Equals(exp_num);
            if (!result){
                 Console.WriteLine($"{actual_num} :: is not equal to :: {exp_num}");
            }
            return result;
        }

        public static void ElemContainsText(this IWebElement element, string exp_text)
        {
            string actual_text = element.Text;
            if (actual_text.Contains(exp_text)){
                 Console.WriteLine("Page contains: " + exp_text);
            }
            else
            {
                 Console.WriteLine(exp_text + " is not found");
            }
        }

        public static void TextContainsText(this string elem_text, string exp_text)
        {
            if (elem_text.Contains(exp_text)){
                 Console.WriteLine("Page contains: " + exp_text);
            }
            else
            {
                 Console.WriteLine(exp_text + " is not found");
            }
        }

        public static void TakeScreenShot(this IWebDriver driver)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string Runname = "image_" + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
            string image_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../Screenshots/");
            if (!Directory.Exists(image_path))
            {
                DirectoryInfo dir = Directory.CreateDirectory(image_path);
            }
            string screenshotfilename = image_path + Runname + ".png";
            ss.SaveAsFile(screenshotfilename, ScreenshotImageFormat.Png);
            Console.WriteLine($"Screenshot has been saved to: {screenshotfilename}");
        }

        private static readonly Regex sWhitespace = new Regex(@"\s+"); 
        public static string ReplaceWhitespace(this string input, string replacement) 
        { 
            return sWhitespace.Replace(input, replacement); 
        }

        public static string GetData(this string data_key)
        {
            return SeleniumBase._config[data_key];
        }

        public static string GetSectionData(this string section, string data_key)
        {
            return SeleniumBase._config.GetSection(section)[data_key];
        }
    }

}