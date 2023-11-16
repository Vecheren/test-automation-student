using System;
using VacationTests.Claims;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageObjects;

namespace VacationTests.Helpers
{
    public static class Helper
    {
        public static void CreateClaimFromUI(EmployeeVacationListPage employeeVacationListPage,
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
        }
    }
}