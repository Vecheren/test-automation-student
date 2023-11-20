using System.Collections.Generic;
using DiExample.Selenium;
using DiExample.Selenium.Page;
using NUnit.Framework;
using OpenQA.Selenium;

namespace DiExample.Selenium
{
    public interface IBrowser
    {
        TPage GoToPage<TPage>() where TPage : class, IPage;
        TPage GoToPage<TPage>(string path) where TPage : class, IPage;
        void GoToUrl<TPage>(string url) where TPage : class, IPage;
    }

    public class Browser : IBrowser
    {
        #region Реализация

        private static readonly List<IWebDriver> _allOpenBrowsers = new();
        private readonly IWebDriver _webDriver;
        private readonly IPageFactory _pageFactory;

        public Browser(IWebDriver webDriver, IPageFactory pageFactory)
        {
            _webDriver = webDriver;
            _pageFactory = pageFactory;
            _allOpenBrowsers.Add(webDriver);
        }

        public TPage GoToPage<TPage>() where TPage : class, IPage
            => _pageFactory.Create<TPage>(_webDriver);

        public TPage GoToPage<TPage>(string path) where TPage : class, IPage
            => _pageFactory.Create<TPage>(_webDriver, path);

        public void GoToUrl<TPage>(string url) where TPage : class, IPage
            => _webDriver.Navigate().GoToUrl(url);

        #endregion

        public static void Dispose()
        {
            foreach (var driver in _allOpenBrowsers)
            {
                driver.Dispose();
            }
        }
    }
}

[SetUpFixture]
public class BrowserDisposer
{
    [OneTimeTearDown]
    public void Dispose() => Browser.Dispose();
}