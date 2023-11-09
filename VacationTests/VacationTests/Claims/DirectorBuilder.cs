namespace VacationTests.Claims
{
    public class DirectorBuilder
    {
        private int id = 14;
        private string name = "Бублик Владимир Кузьмич";
        private string position = "Директор департамента";

        public DirectorBuilder WithId(int newId)
        {
            id = newId;
            return this;
        }

        public DirectorBuilder WithName(string newName)
        {
            name = newName;
            return this;
        }

        public DirectorBuilder WithPosition(string newPosition)
        {
            position = newPosition;
            return this;
        }

        public Director Build() => new(id, name, position);
    }
}