using ContactManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagementApp.Services
{
    public interface IContactWebServices
    {
        event EventHandler ContactsChanged;

        void Create(Contact contact);

        void Update(Contact contact);

        void Delete(string id);

        Contact FindById(string id);

        IEnumerable<Contact> FindByName(string name);

        IEnumerable<Contact> GetContacts();
    }
}
