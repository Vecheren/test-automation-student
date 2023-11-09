using Kontur.Selone.Extensions;
using OpenQA.Selenium;

namespace VacationTests.Infrastructure
{
    public class LocalStorage
    {
        private readonly IWebDriver webDriver;

        public LocalStorage(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public long Length => (long)webDriver.JavaScriptExecutor().ExecuteScript("return localStorage.length;");


        public void Clear()
        {
            webDriver.JavaScriptExecutor().ExecuteScript("localStorage.clear();");
        }

        public string GetItem(string keyName)
        {
            return webDriver.JavaScriptExecutor().ExecuteScript($"return localStorage.getItem(\'{keyName}\');")
                ?.ToString();
        }

        public string Key(int keyNumber)
        {
            return webDriver.JavaScriptExecutor().ExecuteScript($"return localStorage.key({keyNumber});")?.ToString();
        }

        public void RemoveItem(string keyName)
        {
            webDriver.JavaScriptExecutor().ExecuteScript($"localStorage.removeItem(\'{keyName}\');");
        }

        public void SetItem(string keyName, string value)
        {
            webDriver.JavaScriptExecutor().ExecuteScript($"localStorage.setItem(\"{keyName}\", '{value}');");
        }
    }
}