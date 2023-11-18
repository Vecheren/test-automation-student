using System;
using System.Collections.Concurrent;
using Kontur.Selone.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace VacationTests.Infrastructure
{
    public static class MyBrowserPool
    {
        private static ConcurrentDictionary<string, IWebDriver> pool = new();
        private static string key => TestContext.CurrentContext.WorkerId ?? "debug";

        public static void Release() => Get().ResetWindows();

        public static void Dispose()
        {
            foreach (var browser in pool.Values)
            {
                browser.Dispose();
            }
        }

        public static IWebDriver Get()
        {
            var browser = pool.GetOrAdd(key, _ => new ChromeDriverFactory().Create());
            return browser;
        }
    }
}