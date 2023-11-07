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

        // Получение количества элементов в хранилище
        public long Length => (long) 42; // todo для курсанта: написать код

        // Очистка всего хранилища
        public void Clear()
        {
            webDriver.JavaScriptExecutor().ExecuteScript($"localStorage.clear();");
        }

        // Получение данных по ключу keyName
        public string GetItem(string keyName)
        {
            // todo для курсанта: написать код
            return null;
        }

        // Получение ключа на заданной позиции
        public string Key(int keyNumber)
        {
            // todo для курсанта: написать код
            return null;
        }

        // Удаление данных с ключом keyName
        public void RemoveItem(string keyName)
        {
            // todo для курсанта: написать код
        }

        // Сохранение пары ключ/значение
        public void SetItem(string keyName, string value)
        {
            webDriver.JavaScriptExecutor().ExecuteScript($"localStorage.setItem(\"{keyName}\", '{value}');");
        }
    }
}