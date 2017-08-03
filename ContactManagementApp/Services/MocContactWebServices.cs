using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManagementApp.Models;

namespace ContactManagementApp.Services
{
    public class MocContactWebServices : IContactWebServices
    {
        private Mock<IContactWebServices> _mockServices = null;
        public List<Contact> _contacts = new List<Contact>();

        public event EventHandler ContactsChanged;

        public MocContactWebServices()
        {
            _mockServices = new Mock<IContactWebServices>();
            _mockServices.Setup(w => w.Create(It.IsAny<Contact>())).Callback<Contact>((contact) => { CreateCallback(contact); });
            _mockServices.Setup(w => w.Delete(It.IsAny<string>())).Callback<string>((id) => { DeleteCallback(id); });
            _mockServices.Setup(w => w.FindById(It.IsAny<string>())).Returns((string id) => _contacts.FirstOrDefault(c => c._id == id));
            _mockServices.Setup(w => w.FindByName(It.IsAny<string>())).Returns((string name) => _contacts.Where(c => c.Name != null && name != null && c.Name.ToLower().Contains(name.ToLower())));
            _mockServices.Setup(w => w.GetContacts()).Returns(() => _contacts);
            _mockServices.Setup(w => w.Update(It.IsAny<Contact>())).Callback<Contact>((contact) => { UpdateCallback(contact); });
            InitializeList();
        }

        private void InitializeList()
        {
            _contacts.Add(new Contact() { Name = "Person One", _id = "1" });
            _contacts.Add(new Contact() { Name = "Person Two", _id = "2" });
            _contacts.Add(new Contact() { Name = "Person Three", _id = "3" });
        }

        private void CreateCallback(Contact contact)
        {
            contact._id = Guid.NewGuid().ToString();
            _contacts.Add(contact);
            ContactsChanged(this, EventArgs.Empty);
        }

        private void UpdateCallback(Contact update)
        {
            var contact = _contacts.FirstOrDefault(c => c._id == update._id);
            if (contact != null)
            {
                contact.Name = update.Name;
                contact.Address = update.Address;
                contact.PhoneNumber = update.PhoneNumber;
                contact.EmailAddress = update.EmailAddress;
            }
            ContactsChanged(this, EventArgs.Empty);
        }

        private void DeleteCallback(string id)
        {
            var deleteContact = _contacts.FirstOrDefault(c => c._id == id);
            if (deleteContact != null)
                _contacts.Remove(deleteContact);
        }

        public void Create(Contact contact)
        {
            _mockServices.Object.Create(contact);
        }

        public void Delete(string id)
        {
            _mockServices.Object.Delete(id);
        }

        public Contact FindById(string id)
        {
            return _mockServices.Object.FindById(id);
        }

        public IEnumerable<Contact> FindByName(string name)
        {
            return _mockServices.Object.FindByName(name);
        }

        public IEnumerable<Contact> GetContacts()
        {
            return _mockServices.Object.GetContacts();
        }

        public void Update(Contact contact)
        {
            _mockServices.Object.Update(contact);
        }
    }
}
