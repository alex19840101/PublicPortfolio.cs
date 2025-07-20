using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsFeedSystem.DataAccess.Entities
{
    public class Tag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get { return _id; } }

        public string Name { get { return _name; } }

        private readonly uint _id;
        private string _name = default!;

        // Многие ко многим (жесткие проверки и связи в БД избыточны в данном пет-проекте):
        // /// <summary> Навигационное свойство -> новости </summary>
        // public ICollection<News> News { get; set; } = [];
        

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
        public override string ToString()
        {
            return $"{_id} {_name}";
        }
    }
}
