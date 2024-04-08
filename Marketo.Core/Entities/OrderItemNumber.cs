using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketo.Core.Entities
{
    public class OrderItemNumber:BaseEntity
    {
        public int Number { get; set; }
        public List<Slider> slider { get; set; }
  
    }
}
