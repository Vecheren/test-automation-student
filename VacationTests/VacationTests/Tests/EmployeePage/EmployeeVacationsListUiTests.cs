using System;
using System.Linq;
using Kontur.Selone.Extensions;
using Kontur.Selone.Pages;
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
        private EmployeeVacationListPage Init()
        {
            var page = Navigation.OpenEmployeeVacationListPage();
            ClaimStorage.ClearClaims();
            page.Refresh();
            page.ClaimList.Items.Count.Wait().EqualTo(0);
            return page;
        }

        [Test]
        public void CreateVacations_ShouldAddItemsToClaimsList()
        {
            var employeeVacationListPage = Init();
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(50), DateTime.Now.Date.AddDays(54)),
            };

            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 4);
            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 5);

            employeeVacationListPage.ClaimList.Items.Count.Wait().EqualTo(2);
        }

        [Test]
        public void ClaimsList_ShouldDisplayRightTitles_InRightOrder()
        {
            var employeeVacationListPage = Init();
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(50), DateTime.Now.Date.AddDays(55)),
                (DateTime.Now.Date.AddDays(28), DateTime.Now.Date.AddDays(33))
            };

            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 6);
            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 7);
            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[2], 8);

            employeeVacationListPage
                .ClaimList.Items
                .Select(claim => claim.TitleLink.Text)
                .Wait()
                .EqualTo(new[] { "Заявление 1", "Заявление 2", "Заявление 3" });
        }

        [Test]
        public void ClaimsList_ShouldDisplayRightTitleAndStatus_IgnoringOrder()
        {
            var employeeVacationListPage = Init();
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(50), DateTime.Now.Date.AddDays(55))
            };

            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 1);
            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 9);

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
            var employeeVacationListPage = Init();
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(117), DateTime.Now.Date.AddDays(120))
            };

            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[0], 2);
            CreateClaimFromUI(employeeVacationListPage, ClaimType.Child, startAndEndDates[1], 3);

            employeeVacationListPage.ClaimList.Items
                .Wait()
                .Single(x => x.TitleLink.Text, Is.EqualTo("Заявление 2"))
                .PeriodLabel.Text
                .Wait()
                .EqualTo(startAndEndDates[1].ToString(" - "));
        }

        private EmployeeVacationListPage CreateClaimFromUI(EmployeeVacationListPage employeeVacationListPage,
            ClaimType claimType,
            (DateTime, DateTime) startAndEndDate,
            int? childAge = null,
            string directorFio = "Захаров Максим Николаевич")
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
            claimCreationPage.DirectorFioCombobox.SelectValue(directorFio);
            employeeVacationListPage = claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
            employeeVacationListPage.CreateButton.WaitPresence();
            return employeeVacationListPage;
        }
    }

    /*public static class DateTimeTupleExtensions
    {
        public static string ToString(this (DateTime, DateTime) startAndEndDate, string divider)
        {
            return string.Join(divider, new[] { startAndEndDate.Item1, startAndEndDate.Item2 }
                    .Select(x => x.ToString("dd.MM.yyyy")));
        }
    }*/
}