using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContactManagementApp.ViewModel;
using ContactManagementApp.Services;
using ContactManagementApp.Models;
using System.Linq;

namespace ContactManagement.UnitTests
{
    [TestClass]
    public class ContactsVmUnitTest
    {
        private static ContactsViewModel _viewModel = null;
        private static MocContactWebServices _webServices = null;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _webServices = new MocContactWebServices();
            _viewModel = new ContactsViewModel(_webServices);
        }

        [TestMethod]
        public void GetContacts()
        {
            _viewModel.GetContacts();
            Assert.IsTrue(_viewModel.Contacts != null && _viewModel.Contacts.Count() == _webServices._contacts.Count);
        }

        [TestMethod]
        public void FindContactByName()
        {
            var searchText = _webServices._contacts[0].Name;
            _viewModel.SearchText = searchText;
            _viewModel.Search();
            Contact contact = null;
            if (_viewModel.Contacts != null && _viewModel.Contacts.Count() == 1)
                contact = _viewModel.Contacts.FirstOrDefault();
            Assert.IsTrue(contact != null && contact.Name == searchText);
        }

        [TestMethod]
        public void AddContact()
        {
            var initCount = _webServices._contacts.Count;
            _viewModel.EditingContact = new Contact() { Name = "New Contact" };
            _viewModel.SaveContact();
            _viewModel.GetContacts();
            var newCount = _viewModel.Contacts != null ? _viewModel.Contacts.Count() : 0;
            Assert.IsTrue(initCount + 1 == newCount);
        }


        [TestMethod]
        public void EditContact()
        {
            var initCount = _webServices._contacts.Count;
            var contact = _webServices._contacts[0];
            contact._id = "new id";
            contact.Name = contact.Name + "NEW";

            _viewModel.EditingContact = contact;
            _viewModel.SaveContact();
            _viewModel.GetContacts();

            var newCount = _viewModel.Contacts != null ? _viewModel.Contacts.Count() : 0;
            var updatedContact = _webServices._contacts.FirstOrDefault(c => c.Name == contact.Name);
            Assert.IsTrue(initCount == newCount && updatedContact != null);
        }
    }
}
