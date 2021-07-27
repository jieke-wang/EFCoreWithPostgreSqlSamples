using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Account
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime? RegisterDate { get; set; }
        public bool RegisteredUser { get; set; }
        public decimal Balance { get; set; }
    }
}
