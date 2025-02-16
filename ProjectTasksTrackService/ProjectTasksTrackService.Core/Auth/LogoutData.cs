namespace ProjectTasksTrackService.Core.Auth
{
    public class LogoutData
    {
        /// <summary>
        /// Логин (уникальное имя аккаунта)*
        /// </summary>
        public string Login { get { return _login; } }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public int? Id { get { return _id; } }

        private readonly string _login;
        private readonly int? _id;

        public LogoutData(string login, int? id)
        {
            _login = login;
            _id = id;
        }
    }
}
