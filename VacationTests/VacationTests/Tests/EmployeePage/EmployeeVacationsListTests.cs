using System;
using System.Linq;
using Kontur.Selone.Pages;
using Kontur.Selone.Properties;
using NUnit.Framework;
using VacationTests.Claims;
using VacationTests.Helpers;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageObjects;

namespace VacationTests.Tests.EmployeePage
{
    public class EmployeeVacationsListTests : VacationTestBase
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
            ClaimStorage.Add(new[]{Claim.CreateChildType()});
            var claim = Claim.CreateChildType();
            Helper.CreateClaimFromUI(employeeVacationListPage, claim.Type, (claim.StartDate, claim.EndDate), claim.ChildAgeInMonths);
            employeeVacationListPage.Refresh();
            
            employeeVacationListPage.ClaimList.Items.Count.Wait().EqualTo(2); 
        }

        [Test]
        public void ClaimsList_ShouldDisplayRightTitles_InRightOrder()
        {
            var employeeVacationListPage = Init();
            ClaimStorage.Add(new[]{Claim.CreateChildType(), Claim.CreateChildType(), Claim.CreateChildType()});
            employeeVacationListPage.Refresh();
            
            employeeVacationListPage
                .ClaimList.Items
                .Select(claim => claim.TitleLink.Text)
                .Wait()
                .EqualTo(new[]{"Заявление 1", "Заявление 2", "Заявление 3"});
        }
        
        [Test]
        public void ClaimsList_ShouldDisplayRightTitleAndStatus_IgnoringOrder()
        {
            var employeeVacationListPage = Init();
            var claim1 = Claim.CreateChildType() with
            {
                StartDate = DateTime.Now.Date.AddDays(117),
                EndDate = DateTime.Now.Date.AddDays(128)
            };
            var claim2 = Claim.CreateChildType() with
            {
                StartDate = DateTime.Now.Date.AddDays(20),
                EndDate = DateTime.Now.Date.AddDays(26)
            };
            ClaimStorage.Add(new[]{claim1, claim2});
            employeeVacationListPage.Refresh();
            
            employeeVacationListPage
                .ClaimList.Items
                .Select(claim => Props.Create(claim.TitleLink.Text, claim.PeriodLabel.Text, claim.StatusLabel.Text))
                .Wait()
                .EquivalentTo(new[]
                {
                    ("Заявление 1", (claim1.StartDate, claim1.EndDate).ToString(" - "), ClaimStatus.NonHandled.GetDescription()),
                    ("Заявление 2", (claim2.StartDate, claim2.EndDate).ToString(" - "), ClaimStatus.NonHandled.GetDescription())
                });
        }

        [Test]
        public void ClaimsList_ShouldDisplayRightPeriodForItem()
        {
            var employeeVacationListPage = Init();
            var claim1 = Claim.CreateChildType() with
            {
                StartDate = DateTime.Now.Date.AddDays(117),
                EndDate = DateTime.Now.Date.AddDays(128)
            };
            var claim2 = Claim.CreateChildType() with
            {
                StartDate = DateTime.Now.Date.AddDays(20),
                EndDate = DateTime.Now.Date.AddDays(26)
            };
            ClaimStorage.Add(new[]{claim1, claim2});
            employeeVacationListPage.Refresh();

            employeeVacationListPage.ClaimList.Items
                .Wait()
                .Single(x => x.TitleLink.Text, Is.EqualTo("Заявление 2"))
                .PeriodLabel.Text
                .Wait()
                .EqualTo((claim2.StartDate, claim2.EndDate).ToString(" - "));
        }
    }
}