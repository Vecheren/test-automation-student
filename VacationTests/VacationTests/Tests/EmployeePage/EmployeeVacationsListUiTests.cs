using System;
using NUnit.Framework;
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
            var openEmployeeVacationListPage = Navigation.OpenEmployeeVacationListPage(); 
            // следующая строка падает на ulearn
            // openEmployeeVacationListPage.NoClaimsTextLabel.WaitPresence();
            openEmployeeVacationListPage.ClaimList.WaitAbsence();
            var claimCreationPage = openEmployeeVacationListPage.CreateButton.ClickAndOpen<ClaimCreationPage>();
            FillClaimCreationPage(claimCreationPage);
            // следующая строка падает на ulearn
            // openEmployeeVacationListPage.NoClaimsTextLabel.WaitAbsence();
            var claim = openEmployeeVacationListPage.ClaimList.Items.Wait().Single();
            claim.TitleLink.Text.Wait().EqualTo("Заявление 1");
            var date = GetPeriodLabelText();
            claim.PeriodLabel.Text.Wait().EqualTo(date);
            claim.StatusLabel.Text.Wait().EqualTo("На согласовании");
        }

        public void FillClaimCreationPage(ClaimCreationPage claimCreationPage, string claimType = "По уходу за ребенком", int daysFromNowToStartVacation = 8,
            int daysFromNowToEndVacation = 8, string directorFioLabel = "Захаров Максим Николаевич")
        {
            claimCreationPage.ClaimTypeSelect.SelectValueByText(claimType);
            if (claimType == "По уходу за ребенком")
            {
                claimCreationPage.ChildAgeInput.InputText("2");
            }
            claimCreationPage.ClaimStartDatePicker.SetValue(DateTime.Now.AddDays(daysFromNowToStartVacation).ToShortDateString());
            claimCreationPage.ClaimEndDatePicker.SetValue(DateTime.Now.AddDays(daysFromNowToEndVacation).ToShortDateString());
            claimCreationPage.DirectorFioCombobox.Click();
            claimCreationPage.DirectorFioCombobox.MenuItems.Count.Wait().EqualTo(5);
            claimCreationPage.DirectorFioCombobox.InputValue(directorFioLabel);
            claimCreationPage.DirectorFioCombobox.MenuItems.Wait().Single().Click();
            claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
            claimCreationPage.SendButton.WaitAbsence();
        }

        public string GetPeriodLabelText(int daysFromNowToStartVacation = 8, int daysFromNowToEndVacation = 8)
        {
            return DateTime.Now.AddDays(daysFromNowToStartVacation).ToShortDateString()
                   + " - "
                   + DateTime.Now.AddDays(daysFromNowToEndVacation).ToShortDateString();
        }
    }
}