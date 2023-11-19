using System;
using System.Linq.Expressions;
using NUnit.Framework;
using OpenQA.Selenium;
using VacationTests.Claims;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageNavigation;

// Классы с тестами запускаются параллельно.
// Тесты внутри одного класса проходят последовательно.
[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(4)]

namespace VacationTests
{
    public abstract class VacationTestBase
    {
        protected IWebDriver WebDriver => MyBrowserPool.Get();
        protected ClaimStorage ClaimStorage => new(LocalStorage);
        protected LocalStorage LocalStorage => new(WebDriver);
        private ControlFactory ControlFactory => new(LocalStorage, ClaimStorage);
        protected Navigation Navigation => new(WebDriver, ControlFactory);
        private Screenshoter Screenshoter => new(WebDriver); 
        
        [TearDown]
        public void TearDown()
        {
            try
            {
                ClaimStorage.ClearClaims();
                Screenshoter.SaveTestFailureScreenshot();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                MyBrowserPool.Release();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            try
            {
                MyBrowserPool.Dispose();
            }
            catch(Exception e)
            {
                Console.WriteLine("Не удалось выполнить MyBrowserPool.Dispose()");
                Console.WriteLine(e);
            }
        }
    }
}