using NUnit.Framework;

namespace VacationTests.Tests.Navigation
{
    public class NavigationTests : VacationTestBase
    {
        [Test]
        public void LoginPage_NavigationTest()
        {
            var enterPage = Navigation.OpenLoginPage();
            enterPage.WaitLoaded();
            var adminVacationListPage = enterPage.LoginAsAdmin();
            Assert.IsTrue(adminVacationListPage.IsAdminPage);
        }
    }
}