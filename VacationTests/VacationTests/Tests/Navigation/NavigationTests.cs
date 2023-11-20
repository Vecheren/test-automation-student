using System;
using System.Linq;
using System.Reflection;
using Kontur.Selone.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace VacationTests.Tests.Navigation
{
    public class NavigationTests : VacationTestBase
    {
        // [Test]
        // public void Test()
        // {
        //     var assembly = AppDomain.CurrentDomain.GetAssemblies()
        //         .Single(x => x.FullName.Split(",").First() == "VacationTests");
        //     var types = assembly.DefinedTypes
        //         .Where(type => type.GetCustomAttributes()
        //             .Select(y => y.GetType())
        //             .Any(z => z.Name == "InjectControlsAttribute"));
        //
        //     // var webDriver = (IWebDriver)dependencies.Single(x => x.GetType().Name.ToLower().Contains("driver"));
        //     foreach (var control in types)
        //     {
        //         // var contextBy = webDriver.Search(x => x.WithTid(nameof(control)));
        //         if (control.BaseType.Name == "PageBase")
        //         {
        //             // Нельзя так взять и вставить тип в качестве generic
        //             // CreatePage<control>(webDriver);
        //         }
        //         else if (control.BaseType.Name == "ControlBase")
        //         {
        //             // CreateControl<control>(contextBy);
        //
        //             // if (nameof(control).Contains("ElementsCollection"))
        //             // {
        //             //     CreateElementsCollection<IElementsCollection<ControlBase>>(contextBy.SearchContext, )
        //             // }
        //             // else
        //             // {
        //             //     CreateControl<ControlBase>(contextBy);
        //             // }
        //         }
        //     }
        // }
        
        [Test]
        public void LoginPage_LoginAsAdmin_ShouldOpenAdminVacationPage()
        {
            var enterPage = Navigation.OpenLoginPage();
            enterPage.WaitLoaded();
            var adminVacationListPage = enterPage.LoginAsAdmin();
            Assert.IsTrue(adminVacationListPage.IsAdminPage);
        }
    }
}