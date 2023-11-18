using System.Collections.Concurrent;
using Kontur.Selone.Extensions;
using Kontur.Selone.WebDrivers;
using NUnit.Framework;
using OpenQA.Selenium;

namespace VacationTests.Infrastructure
{
    public static class MyBrowserPool
    {
        // У меня была проблема с убийством процессов хрома, и она осталась
        // Только теперь она усугубилась, при запуске всех тестов происходит полный ад
        // 
        
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
            webDriversMap.Clear();
            pool.Clear();
        }
    }
}