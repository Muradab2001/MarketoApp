using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketo.Core.Entities
{
    public class Message:BaseEntity
    {
        public DateTime CreateTime  { get; set; }
        public string Text { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId  { get; set; }
        public string AcceptUserId { get; set; }

    }
}
