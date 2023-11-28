using VacationTests.Claims;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageObjects;

namespace VacationTests.Helpers
{
    public static class Helper
    {
        public static void CreateClaimFromUI(EmployeeVacationListPage employeeVacationListPage, Claim2 claim)
        {
            employeeVacationListPage.CreateButton.WaitPresence();
            var claimCreationPage = employeeVacationListPage.CreateButton.ClickAndOpen<ClaimCreationPage>();
            claimCreationPage.ClaimTypeSelect.SelectValueByText(claim.Type.GetDescription());
            if (claim.Type == ClaimType.Child && claim.ChildAgeInMonths != null)
            {
                claimCreationPage.ChildAgeInput.InputText($"{claim.ChildAgeInMonths}");
            }

            claimCreationPage.ClaimStartDatePicker.SetValue(claim.StartDate.ToString("dd.MM.yyyy"));
            claimCreationPage.ClaimEndDatePicker.SetValue(claim.EndDate.ToString("dd.MM.yyyy"));
            claimCreationPage.DirectorFioCombobox.SelectValue(claim.Director.Name);
            employeeVacationListPage = claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
            employeeVacationListPage.CreateButton.WaitPresence();
        }
    }
}