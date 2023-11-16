using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using VacationTests.Claims;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageNavigation;

// Классы с тестами запускаются параллельно.
// Тесты внутри одного класса проходят последовательно.
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace VacationTests
{
    public abstract class VacationTestBase
    {
        protected IWebDriver WebDriver = new ChromeDriverFactory().Create();
        protected ClaimStorage ClaimStorage => new(LocalStorage);
        protected LocalStorage LocalStorage => new(WebDriver);
        private ControlFactory ControlFactory => new(LocalStorage, ClaimStorage);
        protected Navigation Navigation => new(WebDriver, ControlFactory);
        private Screenshoter Screenshoter => new(WebDriver); 

        [OneTimeTearDown]
        protected void OneTimeTearDown() => WebDriver.Dispose();
        
        [TearDown]
        public void TearDown() => Screenshoter.SaveTestFailureScreenshot();
    }
    public static class DateTimeTupleExtensions
    {
        public static string ToString(this (DateTime, DateTime) startAndEndDate, string divider)
        {
            return string.Join(divider, new[] { startAndEndDate.Item1, startAndEndDate.Item2 }
                .Select(x => x.ToString("dd.MM.yyyy")));
        }
    }
}