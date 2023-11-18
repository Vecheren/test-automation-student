using System.Collections.Concurrent;
using Kontur.Selone.Extensions;
using Kontur.Selone.WebDrivers;
using NUnit.Framework;
using OpenQA.Selenium;

namespace VacationTests.Infrastructure
{
    public static class MyBrowserPool
    {
        private static IWebDriverPool pool;
        private static ConcurrentDictionary<string, IWebDriver> webDriversMap = new();
        private static string key => TestContext.CurrentContext.Test.ID;
        
        static MyBrowserPool()
        {
            var cleaner = new DelegateWebDriverCleaner(x => x.ResetWindows());
            var webDriverFactory = new ChromeDriverFactory();
            pool = new WebDriverPool(webDriverFactory, cleaner);
        }

        public static IWebDriver Get()
        {
            return webDriversMap.GetOrAdd(key, _ => pool.Acquire());
        }

        public static void Release()
        {
            if (webDriversMap.TryRemove(key, out var driver))
            {
                pool.Release(driver);
            }
        }

        public static void Dispose()
        {
            foreach (var driver in webDriversMap.Values)
            {
                driver.Dispose();   
            }
                
            webDriversMap.Clear();
            pool.Clear();
        }
    }
}

// У меня и раньше не убивались процессы хрома
// В этой части курса (еще до задания 7) ситуация усугубилась, при запуске всех тестов происходит полный ад
// Не помогло:
// 1. Добавить в исключения папку проекта + все по предложениям райдера
// 2. Запускать райдер из-под админа и наоборот не из-под админа
// 3. Убрать все TearDown, кроме одного, в VacationsTestBase
// 4. Добавлять в метод Dispose в текущем классе 
// foreach (var driver in webDriversMap.Values)
// {
//     driver.Dispose();   
// }

// Прикладываю тир дауны из VacationsTestBase
//
// [TearDown]
// public void TearDown()
// {
//     ClaimStorage.ClearClaims();
//     Screenshoter.SaveTestFailureScreenshot();
//     MyBrowserPool.Release();
// }
// [OneTimeTearDown]
// protected void OneTimeTearDown()
// {
//     MyBrowserPool.Dispose();
// }
