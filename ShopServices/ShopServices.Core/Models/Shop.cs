using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServices.Core.Models
{
    public class Shop
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Url { get; private set; }
    }
}
