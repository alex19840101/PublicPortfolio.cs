using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Warehouse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "integer")]
        public uint Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Address { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Url { get; private set; } = default!;
    }
}
