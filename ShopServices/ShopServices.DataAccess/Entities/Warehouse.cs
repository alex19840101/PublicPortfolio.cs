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
        public uint Id { get; private set; }
    }
}
