using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsFeedSystem.DataAccess.Entities
{
    public class AuthUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{ get { return _id; } }
        public string Login { get { return _login; } }
        public string UserName { get { return _userName; } }
        public string Email { get { return _email; } }
        public string PasswordHash { get { return _passwordHash; } }
        public string Nick { get { return _nick; } }
        public string Phone { get { return _phone; } }
        public string Role { get { return _role; } }
        public int? GranterId{ get { return _granterId; } }
        public DateTime CreatedDt { get { return _createdDt; } }
        public DateTime? LastUpdateDt { get { return _lastUpdateDt; } }

        private readonly int _id;
        private string _login;
        private string _userName;
        private string _email;
        private string _passwordHash;
        private string _nick;
        private string _phone;
        private string _role;
        private int? _granterId;
        private readonly DateTime _createdDt;
        private DateTime? _lastUpdateDt;

        public AuthUser(
            int id,
            string login,
            string userName,
            string email,
            string passwordHash,
            string nick,
            string phone,
            string role,
            int? granterId,
            DateTime createdDt,
            DateTime? lastUpdateDt)
        {
            _id = id;
            _login = login;
            _userName = userName;
            _email = email;
            _passwordHash = passwordHash;
            _nick = nick;
            _phone = phone;
            _role = role;
            _granterId = granterId;
            _createdDt = createdDt;
            _lastUpdateDt = lastUpdateDt;
        }

        public void UpdateLogin(string newLogin) => _login = newLogin;
        public void UpdateName(string newUserName) => _userName = newUserName;
        public void UpdateEmail(string newEmail) => _email = newEmail;
        public void UpdatePasswordHash(string newPasswordHash) => _passwordHash = newPasswordHash;
        public void UpdateNick(string newNick) => _nick = newNick;
        public void UpdatePhone(string newPhone) => _phone = newPhone;
        public void UpdateRole(string newRole) => _role = newRole;
        public void UpdateGranterId(int granterId) => _granterId = granterId;
        public void UpdateLastUpdateDt(DateTime? lastUpdateDt) => _lastUpdateDt = lastUpdateDt;

        public override bool Equals(object obj)
        {
            var comparedAuthUser = (AuthUser)obj;
            if (comparedAuthUser.Id != _id ||
                !string.Equals(comparedAuthUser.Login, _login) ||
                !string.Equals(comparedAuthUser.UserName, _userName) ||
                !string.Equals(comparedAuthUser.Email, _email) ||
                !string.Equals(comparedAuthUser.PasswordHash, _passwordHash) ||
                !string.Equals(comparedAuthUser.Nick, _nick) ||
                !string.Equals(comparedAuthUser.Phone, _phone) ||
                !string.Equals(comparedAuthUser.Role, _phone) ||
                comparedAuthUser.CreatedDt != _createdDt ||
                comparedAuthUser.LastUpdateDt != _lastUpdateDt)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"{_role} {_login} {_userName} ";
        }
    }
}
