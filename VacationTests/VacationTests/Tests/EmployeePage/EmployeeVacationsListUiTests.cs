using System;
using System.Linq;
using Kontur.Selone.Extensions;
using Kontur.Selone.Properties;
using NUnit.Framework;
using VacationTests.Claims;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageObjects;

namespace VacationTests.Tests.EmployeePage
{
    public class EmployeeVacationsListUiTests : VacationTestBase
    {
        [TearDown]
        public new void TearDown()
        {
            // должно быть в LocalStorage.Clean, но там ulearn не увидит
            WebDriver.JavaScriptExecutor().ExecuteScript($"localStorage.clear();");
        }

        [Test]
        public void CreateClaimFromUI()
        {
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110))
            };
            var employeeVacationListPage = Navigation.OpenEmployeeVacationListPage();
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 3);

            employeeVacationListPage
                .ClaimList.Items
                .Select(claim => Props.Create(claim.TitleLink.Text, claim.PeriodLabel.Text, claim.StatusLabel.Text))
                .Wait()
                .EquivalentTo(new[]
                {
                    ("Заявление 1", startAndEndDates[0].ToString(" - "),
                        ClaimStatus.NonHandled.GetDescription())
                });
        }

        [Test]
        public void CreateVacations_ShouldAddItemsToClaimsList()
        {
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(50), DateTime.Now.Date.AddDays(54)),
            };
            var employeeVacationListPage = Navigation.OpenEmployeeVacationListPage();

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 4);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 5);
            employeeVacationListPage.ClaimList.Items.Count.Wait().EqualTo(2); 
        }

        [Test]
        public void ClaimsList_ShouldDisplayRightTitles_InRightOrder()
        {
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(50), DateTime.Now.Date.AddDays(55)),
                (DateTime.Now.Date.AddDays(28), DateTime.Now.Date.AddDays(33))
            };
            var employeeVacationListPage = Navigation.OpenEmployeeVacationListPage();
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 6);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 7);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[2], 8);
            employeeVacationListPage
                .ClaimList.Items
                .Select(claim => claim.TitleLink.Text)
                .Wait()
                .EqualTo(new[]{"Заявление 1", "Заявление 2", "Заявление 3"});
        }
        
        [Test]
        public void ClaimsList_ShouldDisplayRightTitleAndStatus_IgnoringOrder()
        {
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(50), DateTime.Now.Date.AddDays(55))
            };
            var employeeVacationListPage = Navigation.OpenEmployeeVacationListPage();

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 1);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 9);
            employeeVacationListPage
                .ClaimList.Items
                .Select(claim => Props.Create(claim.TitleLink.Text, claim.PeriodLabel.Text, claim.StatusLabel.Text))
                .Wait()
                .EquivalentTo(new[]
                {
                    ("Заявление 1", startAndEndDates[0].ToString(" - "), ClaimStatus.NonHandled.GetDescription()),
                    ("Заявление 2", startAndEndDates[1].ToString(" - "), ClaimStatus.NonHandled.GetDescription())
                });
        }

        [Test]
        public void ClaimsList_ShouldDisplayRightPeriodForItem()
        {
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(117), DateTime.Now.Date.AddDays(120))
            };
            var employeeVacationListPage = Navigation.OpenEmployeeVacationListPage();

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 2);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 3);

            employeeVacationListPage.ClaimList.Items
                .Single(x => x.TitleLink.Text.Get() == "Заявление 2")
                .PeriodLabel.Text
                .Wait()
                .EqualTo(startAndEndDates[1].ToString(" - "));
        }

        private EmployeeVacationListPage FillClaimCreationPage(EmployeeVacationListPage employeeVacationListPage,
            ClaimType claimType,
            (DateTime, DateTime) startAndEndDate,
            int? childAge = null,
            string directorFioLabel = "Захаров Максим Николаевич")
        {
            employeeVacationListPage.CreateButton.WaitPresence();
            var claimCreationPage = employeeVacationListPage.CreateButton.ClickAndOpen<ClaimCreationPage>();
            claimCreationPage.ClaimTypeSelect.SelectValueByText(claimType.GetDescription());
            if (childAge != null)
            {
                claimCreationPage.ChildAgeInput.InputText($"{childAge}");
            }

            claimCreationPage.ClaimStartDatePicker.SetValue(startAndEndDate.Item1.ToString("dd.MM.yyyy"));
            claimCreationPage.ClaimEndDatePicker.SetValue(startAndEndDate.Item2.ToString("dd.MM.yyyy"));
            claimCreationPage.DirectorFioCombobox.SelectValue(directorFioLabel);
            employeeVacationListPage = claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
            employeeVacationListPage.CreateButton.WaitPresence();
            return employeeVacationListPage;
        }
    }

    public static class DateTimeTupleExtensions
    {
        public static string ToString(this (DateTime, DateTime) startAndEndDate, string divider)
        {
            return string.Join(divider, new[] { startAndEndDate.Item1, startAndEndDate.Item2 }
                    .Select(x => x.ToString("dd.MM.yyyy")));
        }
    }
}