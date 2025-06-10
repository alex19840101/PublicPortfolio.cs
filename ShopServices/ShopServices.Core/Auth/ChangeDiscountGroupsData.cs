using System.Collections.Generic;

namespace ShopServices.Core.Auth
{
    public class ChangeDiscountGroupsData
    {
        /// <summary>
        /// Id покупателя
        /// </summary>
        public uint BuyerId { get { return _buyerId; } }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get { return _login; } }
        public uint GranterId { get { return _granterId; } }
        public string GranterLogin { get { return _granterLogin; } }
        public List<uint> DiscountGroups { get { return _discountGroups; } }

        private readonly uint _buyerId;
        private readonly string _login;
        private readonly uint _granterId;
        private readonly string _granterLogin;
        private readonly List<uint> _discountGroups;

        public ChangeDiscountGroupsData(
            uint buyerId,
            string login,
            List<uint> discountGroups,
            uint granterId,
            string granterLogin)
        {
            _buyerId = buyerId;
            _login = login;
            _discountGroups = discountGroups;
            _granterId = granterId;
            _granterLogin = granterLogin;
        }
    }
}