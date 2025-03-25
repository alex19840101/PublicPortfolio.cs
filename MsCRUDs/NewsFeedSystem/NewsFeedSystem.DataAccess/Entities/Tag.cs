namespace NewsFeedSystem.DataAccess.Entities
{
    public class Tag
    {
        public uint Id { get { return _id; } }

        public string Name { get { return _name; } }

        private readonly uint _id;
        private string _name = default!;

        public Tag(
            uint id,
            string name)
        {
            _id = id;
            _name = name;
        }

        public static Tag TagEntity(Core.Tag tag) =>
            new(
                id:  tag.Id,
                name: tag.Name);

        public Core.Tag GetCoreTag()
            => new(
                id: _id,
                name: _name);

        public void UpdateTagName(string newTagName) => _name = newTagName;
    }
}
