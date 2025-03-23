namespace NewsFeedSystem.Core
{
    public class Tag
    {
        public int Id { get { return _id; } }

        public string Name { get { return _name; } }

        private readonly int _id;
        private readonly string _name = default!;

        public Tag(
            int id,
            string name)
        {
            _id = id;
            _name = name;
        }
    }
}
