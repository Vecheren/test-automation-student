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
            FillClaimCreationPage(claimCreationPage, 6, 8);
            openEmployeeVacationListPage.NoClaimsTextLabel.WaitAbsence();
            var claim = openEmployeeVacationListPage.ClaimList.Items.Wait().Single();
            claim.TitleLink.Text.Wait().EqualTo("Заявление 1");
            var date = GetPeriodLabelText(6, 8);
            claim.PeriodLabel.Text.Wait().EqualTo(date);
            claim.StatusLabel.Text.Wait().EqualTo("На согласовании");
        }

        private void FillClaimCreationPage(ClaimCreationPage claimCreationPage, int daysFromNowToStartVacation = 6,
            int daysFromNowToEndVacation = 8, string directorIdLabel = "22410")
        {
            claimCreationPage.ClaimStartDatePicker.SetValue(DateTime.Now.AddDays(daysFromNowToStartVacation).ToShortDateString());
            claimCreationPage.ClaimEndDatePicker.SetValue(DateTime.Now.AddDays(daysFromNowToEndVacation).ToShortDateString());
            claimCreationPage.DirectorFioCombobox.Click();
            claimCreationPage.DirectorFioCombobox.MenuItems.Count.Wait().EqualTo(5);
            claimCreationPage.DirectorFioCombobox.MenuItems.Single(x => x.IdLabel.Text.Get() == directorIdLabel).Click();
            claimCreationPage.SendButton.ClickAndOpen<EmployeeVacationListPage>();
        }

        private string GetPeriodLabelText(int daysFromNowToStartVacation = 6, int daysFromNowToEndVacation = 8)
        {
            return DateTime.Now.AddDays(daysFromNowToStartVacation).ToShortDateString()
                   + " - "
                   + DateTime.Now.AddDays(daysFromNowToEndVacation).ToShortDateString();
        }
    }
}