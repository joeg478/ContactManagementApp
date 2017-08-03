using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagementApp.Models
{
    public class Address
    {
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public override string ToString()
        {
            var address = new StringBuilder();
            address.AppendLine(Line1);
            if (!string.IsNullOrEmpty(Line2))
            address.AppendLine(Line2);
            if (!string.IsNullOrEmpty(City))
                address.AppendFormat("{0}, {1} {2}", City, State, ZipCode);
            else
                address.AppendFormat("{0} {1}", State, ZipCode);
            return address.ToString();
        }
    }
}
