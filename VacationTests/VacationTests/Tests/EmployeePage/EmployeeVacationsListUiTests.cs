using System;
using System.Linq;
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
            openEmployeeVacationListPage.NoClaimsTextLabel.WaitPresence();
            openEmployeeVacationListPage.ClaimList.WaitAbsence();

            var claimCreationPage = openEmployeeVacationListPage.CreateButton.ClickAndOpen<ClaimCreationPage>();
            var vacationStartDate = DateTime.Now.AddDays(6).ToShortDateString();
            var vacationEndDate = DateTime.Now.AddDays(8).ToShortDateString();
            claimCreationPage.ClaimStartDatePicker.SetValue(vacationStartDate);
            claimCreationPage.ClaimEndDatePicker.SetValue(vacationEndDate);
            claimCreationPage.DirectorFioCombobox.Click();
            claimCreationPage.DirectorFioCombobox.MenuItems.Count.Wait().EqualTo(5);
            claimCreationPage.DirectorFioCombobox.MenuItems.Single(x => x.IdLabel.Text.Get() == "22410").Click();
            claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();

            openEmployeeVacationListPage.NoClaimsTextLabel.WaitAbsence();
            var claim = openEmployeeVacationListPage.ClaimList.Items.Wait().Single();
            claim.TitleLink.Text.Wait().EqualTo("Заявление 1");
            claim.PeriodLabel.Text.Wait().EqualTo(vacationStartDate + " - " + vacationEndDate);
            claim.StatusLabel.Text.Wait().EqualTo("На согласовании");
        }
    }
}