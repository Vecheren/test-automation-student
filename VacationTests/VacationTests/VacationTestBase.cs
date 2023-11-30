using System;
using System.Linq.Expressions;
using Kontur.Selone.WebDrivers;
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
        protected IWebDriver WebDriver => DIContainer.GetRequiredService<IPooledWebDriver>().WrappedDriver;
        protected ClaimStorage ClaimStorage => DIContainer.GetRequiredService<ClaimStorage>();
        protected LocalStorage LocalStorage => DIContainer.GetRequiredService<LocalStorage>();
        private ControlFactory ControlFactory => DIContainer.GetRequiredService<ControlFactory>();
        protected Navigation Navigation => DIContainer.GetRequiredService<Navigation>();
        private Screenshoter Screenshoter => DIContainer.GetRequiredService<Screenshoter>();
        
        [TearDown]
        public void TearDown()
        {
            ClaimStorage.ClearClaims();
            Screenshoter.SaveTestFailureScreenshot();
            DIContainer.ScopeDispose();
        }
    }
    
    
    [SetUpFixture]
    public class BrowserDisposer
    {
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DIContainer.FullDispose();
        }
    }
}