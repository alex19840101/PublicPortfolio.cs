using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsFeedSystem.DataAccess.Entities
{
    public class Topic
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get { return _id; } }

        public string Name { get { return _name; } }

        // Многие ко многим (жесткие проверки и связи в БД избыточны в данном пет-проекте):
        // /// <summary> Навигационное свойство -> новости </summary>
        // public ICollection<News> News { get; set; } = [];

        private readonly uint _id;
        private string _name = default!;

        public Topic(
            uint id,
            string name)
        {
            _id = id;
            _name = name;
        }

        public static Topic TopicEntity(Core.Topic topic) =>
            new(
                id: topic.Id,
                name: topic.Name);

        public Core.Topic GetCoreTopic()
            => new(
                id: _id,
                name: _name);

        public void UpdateTopicName(string newTopicName) => _name = newTopicName;
        public override string ToString()
        {
            return $"{_id} {_name}";
        }
    }
}
