using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopServices.DataAccess.Entities
{
    public class Trade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }
    }
}
