using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Selone.Pages;
using Kontur.Selone.Properties;
using NUnit.Framework;
using VacationTests.Claims;
using VacationTests.Helpers;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;
using VacationTests.PageElements;
using VacationTests.PageObjects;

namespace VacationTests.Tests.AdminPage
{
    public class AdminPageTests : VacationTestBase
    {
        [Test]
        public void CreateClaims_FromUI_ShouldAddClaimsToAdminPage()
        {
            var claimBuilder = new ClaimBuilder();
            var vacationListPage = Navigation.OpenEmployeeVacationListPage();
            Helper.CreateClaimFromUI(vacationListPage, claimBuilder.Build());
            vacationListPage = Navigation.OpenEmployeeVacationListPage("2");
            Helper.CreateClaimFromUI(vacationListPage, claimBuilder.Build());
            var adminVacationListPage = Navigation.OpenAdminVacationListPage();
            adminVacationListPage.ClaimList.ClaimItems.Count.Wait().EqualTo(2);
        }

        [Test]
        public void CreateClaims_FromLocalStorage_ShouldAddClaimsToAdminPage()
        {
            var adminVacationListPage = Navigation.OpenAdminVacationListPage();
            ClaimStorage.Add(new[] { Claim.CreateChildType(), Claim.CreateChildType() });
            adminVacationListPage.Refresh();
            adminVacationListPage.ClaimList.ClaimItems.Count.Wait().EqualTo(2);
        }

        [Test]
        public void ClaimsButtons_CheckWithoutOrder_ShouldSuccess()
        {
            var adminVacationListPage = Navigation.OpenAdminVacationListPage();
            var claims = new[]
            {
                Claim.CreateDefault() with { Status = ClaimStatus.Accepted },
                Claim.CreateDefault() with { Status = ClaimStatus.Rejected },
                Claim.CreateDefault() with { Status = ClaimStatus.NonHandled }
            };
            ClaimStorage.Add(claims);
            adminVacationListPage.Refresh();
            adminVacationListPage.ClaimList.ClaimItems
                .Select(x => Props.Create(x.TitleLink.Text, x.AcceptButton.Visible, x.RejectButton.Visible))
                .Wait()
                .EquivalentTo(new[]
                {
                    ("Заявление " + claims[0].Id, false, false),
                    ("Заявление " + claims[1].Id, false, false),
                    ("Заявление " + claims[2].Id, true, true)
                });
        }


        [Test]
        public void ClaimContent_OnClaimList_ShouldBeCorrect()
        {
            var adminVacationPage = Navigation.OpenAdminVacationListPage();
            var claim = new ClaimBuilder().Build();
            ClaimStorage.Add(new[] { claim });
            adminVacationPage.Refresh();
            var claimItem = adminVacationPage.ClaimList.ClaimItems.Single();
            new[]
            {
                Props.Create(claimItem.TitleLink.Text,
                    claimItem.PeriodLabel.Text,
                    claimItem.StatusLabel.Text,
                    claimItem.AcceptButton.Visible,
                    claimItem.RejectButton.Visible)
            }.Wait().EqualTo(new[]
            {
                (
                    "Заявление " + claim.Id,
                    (claim.StartDate, claim.EndDate).ToString(" - "),
                    claim.Status.GetDescription(),
                    claim.Status == ClaimStatus.NonHandled,
                    claim.Status == ClaimStatus.NonHandled)
            });
        }

        [Test]
        public void ClaimContent_OnClaimLightBox_ShouldBeCorrect()
        {
            var adminVacationPage = Navigation.OpenAdminVacationListPage();
            var claim = new ClaimBuilder().Build();
            ClaimStorage.Add(new[] { claim });
            adminVacationPage.Refresh();
            var claimItem = adminVacationPage.ClaimList.ClaimItems.Single();
            var claimLightbox = claimItem.TitleLink.ClickAndOpen<ClaimLightbox>();
            new[]
            {
                Props.Create(claimLightbox.StatusLabel.Text,
                    claimLightbox.ClaimTypeLabel.Text,
                    claimLightbox.PeriodLabel.Text,
                    claimLightbox.DirectorFioLabel.Text)
            }.Wait().EqualTo(new[]
            {
                (claim.Status.GetDescription(),
                    claim.Type.GetDescription(),
                    (claim.StartDate, claim.EndDate).ToString(" - "),
                    claim.Director.Name)
            });
        }

        [TestCaseSource(nameof(ClaimLightBoxCases))]
        public void ChangeClaimStatus_FromLightbox_ShouldSuccess(Func<ClaimLightbox, Button> getButton, string status)
        {
            var adminVacationPage = Navigation.OpenAdminVacationListPage();
            var claim = new ClaimBuilder().Build();
            ClaimStorage.Add(new[] { claim });
            adminVacationPage.Refresh();
            var claimLightbox = adminVacationPage.ClaimList.ClaimItems.Single().TitleLink.ClickAndOpen<ClaimLightbox>();
            getButton(claimLightbox).ClickAndOpen<AdminVacationListPage>();
            adminVacationPage.ClaimList.ClaimItems.Single().StatusLabel.Text.Wait().EqualTo(status);
        }

        [TestCaseSource(nameof(ClaimListCases))]
        public void ChangeClaimStatus_FromList_ShouldSuccess(Func<AdminClaimItem, Button> getButton, string status)
        {
            var adminVacationPage = Navigation.OpenAdminVacationListPage();
            var claim = new ClaimBuilder().Build();
            ClaimStorage.Add(new[] { claim });
            adminVacationPage.Refresh();

            getButton(adminVacationPage.ClaimList.ClaimItems.Single()).Click();
            adminVacationPage.ClaimList.ClaimItems.Single().StatusLabel.Text.Wait().EqualTo(status);
        }


        [Test]
        public void CreateClaims_CheckWithOrder_ShouldSuccess()
        {
            var adminVacationPage = Navigation.OpenAdminVacationListPage();
            var claimBuilder = new ClaimBuilder();
            var claim = claimBuilder.WithPeriod(DateTime.Now.Date.AddDays(4), DateTime.Now.Date.AddDays(7)).Build();
            var claim2 = claimBuilder.WithPeriod(DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(111))
                .Build();
            ClaimStorage.Add(new[] { claim, claim2 });
            adminVacationPage.Refresh();

            adminVacationPage.ClaimList.ClaimItems.Select(x => x.TitleLink.Text)
                .Wait()
                .EqualTo(new[]
                {
                    "Заявление " + claim.Id,
                    "Заявление " + claim2.Id
                });
        }

        private static IEnumerable<TestCaseData> ClaimLightBoxCases()
        {
            yield return new TestCaseData(new Func<ClaimLightbox, Button>(x => x.Footer.AcceptButton),
                ClaimStatus.Accepted.GetDescription());
            yield return new TestCaseData(new Func<ClaimLightbox, Button>(x => x.Footer.RejectButton),
                ClaimStatus.Rejected.GetDescription());
        }

        private static IEnumerable<TestCaseData> ClaimListCases()
        {
            yield return new TestCaseData(new Func<AdminClaimItem, Button>(x => x.AcceptButton),
                ClaimStatus.Accepted.GetDescription());
            yield return new TestCaseData(new Func<AdminClaimItem, Button>(x => x.RejectButton),
                ClaimStatus.Rejected.GetDescription());
        }
    }
}