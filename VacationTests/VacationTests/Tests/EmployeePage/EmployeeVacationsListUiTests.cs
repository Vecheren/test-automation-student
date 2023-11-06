using System;
using System.Collections.Generic;
using System.Linq;
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
        [Test]
        public void CreateClaimFromUI()
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
            ClaimType claimType, (DateTime, DateTime) startAndEndDate,
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

        private string GetPeriodTextFromDates((DateTime, DateTime) dates) =>
            dates.Item1.ToString("dd.MM.yyyy") + " - " + dates.Item2.ToString("dd.MM.yyyy");
    }
}