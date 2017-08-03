using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagementApp.Models
{
    public class Contact
    {
        public static event EventHandler ContactChanged;

        public Contact()
        {
            Address = new Address();
        }

        public string _id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}
