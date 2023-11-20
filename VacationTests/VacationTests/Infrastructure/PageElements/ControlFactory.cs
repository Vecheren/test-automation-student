using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Kontur.Selone.Elements;
using Kontur.Selone.Extensions;
using Kontur.Selone.Pages;
using Kontur.Selone.Selectors;
using Kontur.Selone.Selectors.Context;
using Kontur.Selone.Selectors.XPath;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using VacationTests.PageObjects;

namespace VacationTests.Infrastructure.PageElements
{
    public class ControlFactory
    {
        private readonly object[] dependencies;
        
        // UPD: В итоге понял, что нужен ифчик при создании объекта 
        
        // Изначально неправильно понял и решил всю инициализацию запускать прям отсюда
        // Рассуждал так: 
        // Задача: для определенных классов (помеченных атрибутом) автоматически инициализировать свойства страниц, контролов и коллекций контролов
        // Другими словами: эти классы не должны обращаться к ControlFactory, ControlFactory сама к ним придет и проинициализирует
        // Значит, мы должны:
        // 0. Убрать всё ручное создание контролов в классах помеченных атрибутом + сделать все нужные свойства get; private set;
        // 1. Найти все классы помеченные атрибутом (через рефлексию)
        // 2. Найти все свойства в этих классах, которые являются страницами, контролами и коллекциями контролов (через рефлексию)
        // 3. Проинициализировать эти страницы, контролы и коллекции контролов (через соответствующие методы Create... в ControlFactory)
        // 4. Вызвать всю эту штуку автоматически при запуске тестов и до поиска первого элемента (непосредственно при создании ControlFactory)
        // Как все это связано с формулировками и подсказками в задании - вообще не понимаю :) 
        // Если мы просто сделаем п. 0 и что-то поправим в CreateInstance - кто будет выполнять остальные 4 пункта?

        
        public ControlFactory(params object[] dependencies)
        {
            this.dependencies = dependencies;
            
            // var assembly = AppDomain.CurrentDomain.GetAssemblies()
            //     .Single(x => x.FullName.Split(",").First() == "VacationTests");
            // var types = assembly.DefinedTypes
            //     .Where(type => type.GetCustomAttributes()
            //         .Select(y => y.GetType())
            //         .Any(z => z.Name == "InjectControlsAttribute"));
            //
            // var webDriver = (IWebDriver)dependencies.Single(x => x.GetType().Name.ToLower().Contains("driver"));
            // foreach (var control in types)
            // {
            //     var contextBy = webDriver.Search(x => x.WithTid(nameof(control)));
            //     if (control.BaseType.Name == "PageBase")
            //     {
            //         // здесь надо передать класс control, но компилятор ругается :(
            //         CreateControl<PageBase>(contextBy);
            //     }
            //     else if (control.BaseType.Name == "ControlBase")
            //     {
            //         // CreateControl<ControlBase>(contextBy);
            //     }
            // }
        }
        
        /// <summary>Создать контрол типа TPageElement</summary>
        /// <typeparam name="TPageElement">Должен содержать конструктор, принимающий IWebDriver</typeparam>
        public TPageElement CreateControl<TPageElement>(IContextBy contextBy)
        {
            return (TPageElement)CreateInstance(typeof(TPageElement), contextBy, dependencies.Prepend(this).ToArray());
        }

        /// <summary>Создать страницу типа TPageObject</summary>
        public TPageObject CreatePage<TPageObject>(IWebDriver webDriver)
        {
            var allDependencies = dependencies.Prepend(this).Prepend(webDriver).ToArray();
            return (TPageObject)CreateInstance(typeof(TPageObject), null, allDependencies);
        }

        /// <summary>Создать коллекцию контролов типа TItem</summary>
        public ElementsCollection<TItem> CreateElementsCollection<TItem>(ISearchContext itemsSearchContext,
            ItemByLambda findItem)
        {
            return new ElementsCollection<TItem>(itemsSearchContext,
                findItem,
                (s, b, _) => CreateControl<TItem>(new ContextBy(s, b)));
        }

        private static object CreateInstance(Type controlType, IContextBy contextBy, object[] dependencies)
        {
            Console.WriteLine("В методе CreateInstanse создаем " + controlType.FullName);
            // У объекта, который хотим создать, проверяем, что конструктор есть и он один
            var constructors = controlType.GetConstructors();
            if (constructors.Length != 1)
                throw new NotSupportedException($"Контрол {controlType} должен иметь только один конструктор");
            var constructor = constructors.Single();
            // У конструктора получаеям все его входные парметры, которые ему нужны
            var parameters = constructor.GetParameters();
            var args = new List<object>();
            // Провеярем, что среди наших зависимостей есть все необходимые для создания объекта
            foreach (var parameterInfo in parameters)
            {
                var arg =
                    dependencies.Prepend(contextBy).FirstOrDefault(dep =>
                        dep != null && dep.GetType().IsAssignableTo(parameterInfo.ParameterType)) ??
                    throw new NotSupportedException(
                        $"Не поддерживаемый тип {parameterInfo.ParameterType} параметра конструктора контрола {controlType}");
                args.Add(arg);
            }

            // Вызываем конструктор и передаём ему все входные параметры
            var value = constructor.Invoke(args.ToArray());

            // Получаем контекст, по которому будем искать все контролы, входящие в состав нашего объекта
            // Здесь тест падает на SingleOrDefault
            var searchContext = contextBy?.SearchContext.SearchElement(contextBy.By) ??
                                dependencies.OfType<ISearchContext>().SingleOrDefault();
            if (searchContext == null)
                throw new NotSupportedException(
                    "Для автоматической инициализации полей контрола должен быть известен ISearchContext. " +
                    "Либо укажите IContextBy, либо передайте в зависимости WebDriver.");
            // Инициализируем контролы объекта
            
            if (controlType.GetCustomAttributes().Select(y => y.GetType())
                .Any(z => z.Name == "InjectControlsAttribute"))
            {
                Console.WriteLine("контрол с нашим атрибутом: " + controlType.FullName);
                InitializePropertiesWithControls(value, searchContext, dependencies);
            }

            // Возвращаем экземпляр объекта
            return value;
        }

        private static void InitializePropertiesWithControls(object control, ISearchContext searchContext,
            params object[] dependencies)
        {
            // У переданного объекта ищем все свойства, наследющиеся от ControlBase
            var controlProps = control.GetType().GetProperties()
                .Where(p => typeof(ControlBase).IsAssignableFrom(p.PropertyType)).ToList();

            // Для каждого найденного свойства:
            foreach (var prop in controlProps)
            {
                // проверяем, что доступен метод set;
                if (prop.SetMethod is null) continue;

                // находим атрибут BaseSearchByAttribute или его наследника ByTidAttribute
                var attribute = prop.GetCustomAttribute<BaseSearchByAttribute>(true);
                // если атрибут не найден, то берём название самого свойства,
                // а если атрибут найден, берём его значение
                var contextBy = attribute == null
                    ? searchContext.Search(x => x.WithTid(prop.Name))
                    : searchContext.Search(attribute.SearchCriteria);

                // создаём экземпляр свойства через CreateInstance,
                // чтобы иницаилизировать у сложных контролов ещё и их свойства
                var value = CreateInstance(prop.PropertyType, contextBy, dependencies);
                // присваиваем свойству объекта полученный экземпляр
                prop.SetValue(control, value);
            }
        }
    }
}