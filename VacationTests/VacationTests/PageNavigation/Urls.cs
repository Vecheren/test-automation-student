using VacationTests.PageObjects;

namespace VacationTests.PageNavigation
{
    public static class Urls
    {
        private const string Host = "https://test-automation-course.gitlab-pages.kontur.host/for-course";

        // Использовать для локального запуска сервиса Отпуска
        // private const string Host = "http://localhost:8080";
        public const string LoginPage = Host + "/#/";
        public const string AdminVacationListPage = LoginPage + "admin";

        public static string ClaimCreationPage(string employeeId)
        {
            return LoginPage + $"vacation/{employeeId}";
        }
        public static string EmployeeVacationListPage(string employeeId)
        {
            return LoginPage + $"user/{employeeId}";
        }
    }
}