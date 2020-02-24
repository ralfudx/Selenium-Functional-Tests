using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using Xunit;

namespace Geophy.Tests
{
    class SeleniumBase: IDisposable
    {
        //auto-implemented property
        public static IWebDriver _browserDriver { get; set; }
        public static IConfiguration _config { get; set; }
        
		public SeleniumBase()
        {
            this.InitializeBrowser();
        }

        public void InitializeBrowser()
        {
            _browserDriver = new ChromeDriver("./");
            _config = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            _browserDriver.Manage().Window.Maximize();
			_browserDriver.Navigate().GoToUrl(_config["dashboardurl"]);
        }
        
        public void Dispose()
        {
            _browserDriver.TakeScreenShot();
            _browserDriver.Quit();
        }
    }
}
