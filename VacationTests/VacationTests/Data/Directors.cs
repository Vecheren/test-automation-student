using VacationTests.Claims;

namespace VacationTests.Data
{
    public static class Directors
    {
        public static Director Default => New.Build();

        public static Director SuperDirector => New
            .WithId(24320)
            .WithName("Кирпичников Алексей Николаевич")
            .WithPosition("Руководитель управления")
            .Build();
        
        public static DirectorBuilder New => new();
    }
}