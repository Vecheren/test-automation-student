using System;
using System.Collections.Concurrent;
using Kontur.Selone.Extensions;
using Kontur.Selone.WebDrivers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Firefox;
using VacationTests.Claims;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageNavigation;

namespace VacationTests.Infrastructure
{
    public static class DIContainer
    {
        static DIContainer()
        {
            var serviceCollections = new ServiceCollection();
            serviceCollections.AddScoped<ILog, MyLog>();
            serviceCollections.AddSingleton<ControlFactory>(_ => new ControlFactory());
            serviceCollections.AddSingleton<IWebDriverFactory>(_ =>
            {
                var browser = Environment.GetEnvironmentVariable("TEST_BROWSER") ?? "chrome";
                return browser switch
                {
                    "firefox" => new FirefoxDriverFactory(),
                    "chrome" => new ChromeDriverFactory(),
                    _ => new ChromeDriverFactory()
                };
            });
            serviceCollections.AddSingleton<IWebDriverCleaner>(s =>
                new DelegateWebDriverCleaner(x => x.ResetWindows()));
            serviceCollections.AddSingleton<IWebDriverPool, WebDriverPool>();
            
            serviceCollections.AddScoped<ClaimStorage>();
            serviceCollections.AddScoped<LocalStorage>();
            serviceCollections.AddScoped<Navigation>();
            serviceCollections.AddScoped<IPooledWebDriver>(s => s.GetRequiredService<IWebDriverPool>().AcquireWrapper());
            serviceCollections.AddScoped<Screenshoter>();

            serviceProvider = serviceCollections.BuildServiceProvider();
        }

        public static T GetRequiredService<T>()
        {
            var scope = scopeMap.GetOrAdd(scopeKey, _ => serviceProvider.CreateScope());
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        public static void ScopeDispose()
        {
            scopeMap.TryRemove(scopeKey, out var scope);
            scope?.Dispose();
        }
        
        public static void FullDispose()
        {
            serviceProvider.GetService<IWebDriverPool>().Clear();
            serviceProvider.Dispose();
        }
        
        private static ConcurrentDictionary<string, IServiceScope> scopeMap { get; } = new();
        private static string scopeKey => TestContext.CurrentContext.WorkerId ?? "debug"; 
        private static ServiceProvider serviceProvider;
    }
}