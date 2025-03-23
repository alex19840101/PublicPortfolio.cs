namespace NewsFeedSystem.Core
{
    public class Topic
    {
        public int Id { get { return _id; } }

        public string Name { get { return _name; } }

        private readonly int _id;
        private readonly string _name = default!;

        public Topic(
            int id,
            string name)
        {
            _id = id;
            _name = name;
        }
    }
}
