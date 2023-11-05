using System;
using NUnit.Framework;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageElements;
using VacationTests.PageObjects;

namespace VacationTests.Tests.EmployeePage
{
    public class EmployeeVacationsListUiTests : VacationTestBase
    {
        [Test]
        public void CreateClaimFromUI()
        {
            var claimCreationPage = Navigation.OpenEmployeeVacationListPage().CreateButton.ClickAndOpen<ClaimCreationPage>();
            // вместо прошлой строки можно сделать переход по URL заявления Urls.LoginPage + "vacation/1"
            // но на юлерне 'Navigation.OpenPage<TPageObject>(string)' is inaccessible due to its protection level 
            var employeeVacationListPage = FillClaimCreationPage(claimCreationPage);
            var claim = employeeVacationListPage.ClaimList.Items.Wait().Single();
            CheckClaimOnVacationListPage(claim);
        }

        private void CheckClaimOnVacationListPage(EmployeeClaimItem claim, int claimNumber = 1, int daysToStartVacation = 8, int daysToEndVacation = 8, string status = "На согласовании")
        {
            claim.TitleLink.Text.Wait().EqualTo($"Заявление {claimNumber}");
            claim.PeriodLabel.Text.Wait().EqualTo(GetPeriodLabelText(daysToStartVacation, daysToEndVacation));
            claim.StatusLabel.Text.Wait().EqualTo(status);
        }

        private EmployeeVacationListPage FillClaimCreationPage(ClaimCreationPage claimCreationPage, string claimType = "По уходу за ребенком", int daysToStartVacation = 8,
            int daysToEndVacation = 8, string directorFioLabel = "Захаров Максим Николаевич")
        {
            claimCreationPage.ClaimTypeSelect.SelectValueByText(claimType);
            if (claimType == "По уходу за ребенком")
            {
                claimCreationPage.ChildAgeInput.InputText("2");
            }
            claimCreationPage.ClaimStartDatePicker.SetValue(DateTime.Now.AddDays(daysToStartVacation).ToString("dd.MM.yyyy"));
            claimCreationPage.ClaimEndDatePicker.SetValue(DateTime.Now.AddDays(daysToEndVacation).ToString("dd.MM.yyyy"));
            claimCreationPage.DirectorFioCombobox.SelectValue(directorFioLabel);
            return claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
        }

        private string GetPeriodLabelText(int daysFromStartVacation = 8, int daysToEndVacation = 8)
        {
            return DateTime.Now.AddDays(daysFromStartVacation).ToString("dd.MM.yyyy")
                   + " - "
                   + DateTime.Now.AddDays(daysToEndVacation).ToString("dd.MM.yyyy");
        }
    }
}