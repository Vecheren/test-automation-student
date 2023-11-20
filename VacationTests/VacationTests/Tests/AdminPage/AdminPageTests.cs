using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Schema;
using FluentAssertions;
using Kontur.RetryableAssertions.Extensions;
using Kontur.Selone.Elements;
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
            var dates = new[]
            {
                (DateTime.Now.Date.AddDays(8), DateTime.Now.Date.AddDays(15)),
                (DateTime.Now.Date.AddDays(10), DateTime.Now.Date.AddDays(20))
            };
            var vacationListPage = Navigation.OpenEmployeeVacationListPage();
            Helper.CreateClaimFromUI(vacationListPage, ClaimType.Child, dates[0], 7);
            vacationListPage = Navigation.OpenEmployeeVacationListPage("2");
            Helper.CreateClaimFromUI(vacationListPage, ClaimType.Child, dates[1], 6);
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
                .Select(x => Props.Create(x.AcceptButton.Visible, x.RejectButton.Visible))
                .Wait()
                .EquivalentTo(new[] { (true, true), (false, false), (false, false) });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ClaimContent_OnClaimList_ShouldBeCorrect(bool checkOnList)
        {
            var adminVacationPage = Navigation.OpenAdminVacationListPage();
            var claim = new ClaimBuilder().Build();
            ClaimStorage.Add(new[] { claim });
            adminVacationPage.Refresh();
            var claimItem = adminVacationPage.ClaimList.ClaimItems.Single();

            if (checkOnList)
            {
                new[]
                {
                    Props.Create(claimItem.TitleLink.Text,
                        claimItem.PeriodLabel.Text,
                        claimItem.StatusLabel.Text,
                        claimItem.AcceptButton.Visible,
                        claimItem.RejectButton.Visible)
                }.Wait().EqualTo(new[]
                {
                    ("Заявление " + claim.Id,
                        (claim.StartDate, claim.EndDate).ToString(" - "),
                        claim.Status.GetDescription(),
                        claim.Status == ClaimStatus.NonHandled,
                        claim.Status == ClaimStatus.NonHandled)
                });
            }
            else
            {
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
            var claim2 = claimBuilder.WithPeriod(DateTime.Now.Date.AddDays(100), DateTime.Now.Date.AddDays(111)).Build();
            ClaimStorage.Add(new[] { claim, claim2 });
            adminVacationPage.Refresh();

            adminVacationPage.ClaimList.ClaimItems.Select(claimItem => Props.Create(
                    claimItem.TitleLink.Text,
                    claimItem.PeriodLabel.Text,
                    claimItem.StatusLabel.Text,
                    claimItem.AcceptButton.Visible,
                    claimItem.RejectButton.Visible))
                .Wait().EqualTo(new[]
                {
                    ("Заявление " + claim.Id,
                        (claim.StartDate, claim.EndDate).ToString(" - "),
                        claim.Status.GetDescription(),
                        claim.Status == ClaimStatus.NonHandled,
                        claim.Status == ClaimStatus.NonHandled),
                    ("Заявление " + claim2.Id,
                        (claim2.StartDate, claim2.EndDate).ToString(" - "),
                        claim2.Status.GetDescription(),
                        claim2.Status == ClaimStatus.NonHandled,
                        claim2.Status == ClaimStatus.NonHandled)
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


        // Работает, но переусложнено и подходит только для одного кейса :)
        // private static IEnumerable<TestCaseData> ClaimContentCases()
        // {
        //     yield return new TestCaseData(new Func<AdminClaimItem, Props<string, string, string, bool, bool>[]>(
        //             claimItem =>
        //             {
        //                 return new[]
        //                 {
        //                     Props.Create(claimItem.TitleLink.Text,
        //                         claimItem.PeriodLabel.Text,
        //                         claimItem.StatusLabel.Text,
        //                         claimItem.AcceptButton.Visible,
        //                         claimItem.RejectButton.Visible)
        //                 };
        //             }),
        //         new Func<Claim2, IEnumerable<(string, string, string, bool, bool)>>(claim =>
        //         {
        //             return new[]
        //             {
        //                 ("Заявление " + claim.Id,
        //                     (claim.StartDate, claim.EndDate).ToString(" - "),
        //                     claim.Status.GetDescription(),
        //                     claim.Status == ClaimStatus.NonHandled,
        //                     claim.Status == ClaimStatus.NonHandled)
        //             };
        //         }));
        // }
    }
}