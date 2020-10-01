using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeShell.Models
{
    public class Command
    {
        public int CommandId { get; set; }
        public string CommandReq { get; set; }
        public string Exec { get; set; }
        public string Args { get; set; }
        public DateTime Time { get; set; }
    }
}
