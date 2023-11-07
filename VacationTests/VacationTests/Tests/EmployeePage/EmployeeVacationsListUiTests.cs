using System;
using System.ComponentModel;
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
        public void TearDown()
        {
            // должно быть в LocalStorage.Clean, но там ulearn не увидит
            WebDriver.JavaScriptExecutor().ExecuteScript($"localStorage.clear();");
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

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0]);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1]);
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

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0]);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1]);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[2]);
            var expected = new[]
            {
                ("Заявление 1", GetPeriodTextFromDates(startAndEndDates[0]), ClaimStatus.NonHandled.GetDescription()),
                ("Заявление 2", GetPeriodTextFromDates(startAndEndDates[1]), ClaimStatus.NonHandled.GetDescription()),
                ("Заявление 3", GetPeriodTextFromDates(startAndEndDates[2]), ClaimStatus.NonHandled.GetDescription())
            };
            CheckClaimOnVacationListPage(employeeVacationListPage, false, expected);
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

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0]);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1]);
            var expected = new[]
            {
                ("Заявление 1", GetPeriodTextFromDates(startAndEndDates[0]), ClaimStatus.NonHandled.GetDescription()),
                ("Заявление 2", GetPeriodTextFromDates(startAndEndDates[1]), ClaimStatus.NonHandled.GetDescription())
            };
            CheckClaimOnVacationListPage(employeeVacationListPage, true, expected);
        }
        
        [Test]
        public void ClaimsList_ShouldDisplayRightPeriodForItem()
        {
            var startAndEndDates = new[]
            {
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(110)),
                (DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(120))
            };
            var employeeVacationListPage = Navigation.OpenEmployeeVacationListPage();

            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[0]);
            FillClaimCreationPage(employeeVacationListPage, ClaimType.Child, startAndEndDates[1]);

            employeeVacationListPage.ClaimList.Items
                .Single(x => x.TitleLink.Text.Wait().GetValue() == "Заявление 2")
                .PeriodLabel.Text
                .Wait()
                .EqualTo(GetPeriodTextFromDates(startAndEndDates[1]));
        }

        private void CheckClaimOnVacationListPage(EmployeeVacationListPage page, bool ignoreClaimsOrder,
            params (string, string, string)[] expected)
        {
            var claims = page.ClaimList.Items
                .Select(claim => Props.Create(claim.TitleLink.Text, claim.PeriodLabel.Text, claim.StatusLabel.Text))
                .Wait();
            if (ignoreClaimsOrder)
            {
                claims.EquivalentTo(expected);
            }
            else
            {
                claims.EqualTo(expected);
            }
        }

        private EmployeeVacationListPage FillClaimCreationPage(EmployeeVacationListPage employeeVacationListPage,
            ClaimType claimType, 
            (DateTime, DateTime) startAndEndDate,
            string directorFioLabel = "Захаров Максим Николаевич")
        {
            var claimCreationPage = employeeVacationListPage.CreateButton.ClickAndOpen<ClaimCreationPage>();
            claimCreationPage.ClaimTypeSelect.SelectValueByText(claimType.GetDescription());
            if (claimType == ClaimType.Child)
            {
                claimCreationPage.ChildAgeInput.InputText("2");
            }
            claimCreationPage.ClaimStartDatePicker.SetValue(startAndEndDate.Item1.ToString("dd.MM.yyyy"));
            claimCreationPage.ClaimEndDatePicker.SetValue(startAndEndDate.Item2.ToString("dd.MM.yyyy"));
            claimCreationPage.DirectorFioCombobox.SelectValue(directorFioLabel);
            return claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
        }

        private string GetPeriodTextFromDates((DateTime, DateTime) startAndEndDate) =>
            startAndEndDate.Item1.ToString("dd.MM.yyyy") + " - " + startAndEndDate.Item2.ToString("dd.MM.yyyy");
    }
}