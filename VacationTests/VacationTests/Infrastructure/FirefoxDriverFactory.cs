using System;
using Kontur.Selone.Extensions;
using Kontur.Selone.WebDrivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace VacationTests.Infrastructure
{
    public class FirefoxDriverFactory : IWebDriverFactory
    {
        public IWebDriver Create()
        {
            var firefoxDriverService = FirefoxDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            var options = new FirefoxOptions();
            options.AddArguments("--start-maximized", "--disable-extensions");
            var firefoxDriver = new FirefoxDriver(firefoxDriverService, options, TimeSpan.FromSeconds(180));
            firefoxDriver.Manage().Window.SetSize(1280, 960);
            return firefoxDriver;
        }
    }
}