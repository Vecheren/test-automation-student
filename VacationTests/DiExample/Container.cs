using System;
using System.Collections.Concurrent;
using DiExample.Selenium;
using DiExample.Selenium.Page;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DiExample
{
    public static class Container
    {
        public static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IBrowser, Browser>();
            serviceCollection.AddScoped<IWebDriver, ChromeDriver>();
            serviceCollection.AddSingleton<IPageFactory, PageFactory>();
            
            ServiceProvider = serviceCollection.BuildServiceProvider();
            return ServiceProvider;
        }
        
        
        public static IServiceProvider ServiceProvider { get; set; }
    
        // Потокобезопасный словарь для хранения открытых скоупов
        // в качестве ключа может выступать id потока TestContext.CurrentContext.WorkerId
        private static ConcurrentDictionary<string, IServiceScope> scopeMap { get; } = new();
        private static string scopeKey => TestContext.CurrentContext.WorkerId ?? "debug"; 
    
        // Берем инстанс объекта из скоупа для текущего потока (теста)
        public static T GetRequiredService<T>() where T : notnull
        {
            var scope = scopeMap.GetOrAdd(scopeKey, _ => ServiceProvider.CreateScope());
            return scope.ServiceProvider.GetRequiredService<T>();
        }
        
        // Метод для очистки скоупа. После теста мы очищаем данные, и возвращаем браузер в пул
        public static void ScopeDispose()
        {
            if (!scopeMap.TryRemove(scopeKey, out var scope))
            {
                throw new Exception("Не смогли удалить скоуп из scopeMap");
            }
        
            scope.Dispose();
        }
    }
}