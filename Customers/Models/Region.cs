using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Models
{
    public enum RegionCodes
    {
        North,
        East,
        South,
        West,
        Central,
    };

    public class Region
    {
        public string AccountRepresentative { get; set; }
        public RegionCodes Code { get; set; }
        public List<Customer> CustomerList { get; set; }

        public int CustomerCount
        {
            get { if (CustomerList == null) return 0;  return CustomerList.Count;  }
        }
    }
}
