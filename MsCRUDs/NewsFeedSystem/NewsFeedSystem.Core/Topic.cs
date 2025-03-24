namespace NewsFeedSystem.Core
{
    public class Topic
    {
        public uint Id { get { return _id; } }

        public string Name { get { return _name; } }

        private readonly uint _id;
        private readonly string _name = default!;

        public Topic(
            uint id,
            string name)
        {
            _id = id;
            _name = name;
        }
    }
}
