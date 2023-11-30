// using System;
// using System.Collections.Concurrent;
// using Kontur.Selone.Extensions;
// using Kontur.Selone.WebDrivers;
// using NUnit.Framework;
// using OpenQA.Selenium;
//
// namespace VacationTests.Infrastructure
// {
//     public static class MyBrowserPool
//     {
//         private static IWebDriverPool pool;
//         private static ConcurrentDictionary<string, IWebDriver> webDriversMap = new();
//         private static string key => TestContext.CurrentContext.Test.ID;
//         
//         static MyBrowserPool()
//         {
//             var browser = Environment.GetEnvironmentVariable("TEST_BROWSER") ?? "chrome";
//             IWebDriverFactory factory;
//             switch (browser)
//             {
//                 case "chrome":
//                     factory = new ChromeDriverFactory();
//                     break;
//                 case "firefox":
//                     factory = new FirefoxDriverFactory();
//                     break;
//                 default:
//                     factory = new ChromeDriverFactory();
//                     break;
//             }
//             
//             var cleaner = new DelegateWebDriverCleaner(x => x.ResetWindows());
//             pool = new WebDriverPool(factory, cleaner);
//         }
//
//         public static IWebDriver Get()
//         {
//             return webDriversMap.GetOrAdd(key, _ => pool.Acquire());
//         }
//
//         public static void Release()
//         {
//             if (webDriversMap.TryRemove(key, out var driver))
//             {
//                 pool.Release(driver);
//             }
//         }
//
//         public static void Dispose()
//         {
//             foreach (var driver in webDriversMap.Values)
//             {
//                 driver.Close();
//                 driver.Quit();
//             }
//             pool.Clear();
//         }
//     }
// }
