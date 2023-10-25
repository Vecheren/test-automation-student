using FluentAssertions;
using NUnit.Framework;
using VacationTests.Infrastructure;
using VacationTests.Infrastructure.PageElements;

namespace VacationTests.Tests.Navigation
{
    // Задание 3.2: нужно поднять этот тест
    public class NavigationExercise2Tests : VacationTestBase
    {
        [Test]
        public void LoginPage_TitleTest()
        {
            var enterPage = Navigation.OpenLoginPage();
            enterPage.TitleLabel.WaitPresence();
            enterPage.TitleLabel.Text.Wait().EqualTo(("Вход в сервис"));
        }
    }
}