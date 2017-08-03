using ContactManagementApp.Models;
using ContactManagementApp.Services;
using ContactManagementApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactManagementApp.ViewModel
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Contact> _contacts = null;
        private IContactWebServices _webService = null;
        private ILogger _logger = new EventViewerLogger();
        private string _searchText = null;

        private ContactView _contactViewDialog = null;

        private ICommand _editContactCommand;
        private ICommand _saveContactCommand;
        private ICommand _cancelSaveCommand;
        private ICommand _searchCommand;
        private ICommand _clearSearchCommand;
        private ICommand _createNewCommand;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ContactsViewModel() : this(IocProvider.Resolve<IContactWebServices>()) { }

        public ContactsViewModel(IContactWebServices webService)
        {
            _webService = webService;
            GetContacts();
            _webService.ContactsChanged += ContactsChanged;
        }

        #region Commands
        public ICommand EditContactCommand
        {
            get
            {
                return _editContactCommand ?? (_editContactCommand = new CommandHandler(() => EditContact()));
            }
        }
        public ICommand SaveContactCommand
        {
            get
            {
                return _saveContactCommand ?? (_saveContactCommand = new CommandHandler(() => SaveContact()));
            }
        }
        public ICommand CancelSaveCommand
        {
            get
            {
                return _cancelSaveCommand ?? (_cancelSaveCommand = new CommandHandler(() => CancelSave()));
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new CommandHandler(() => Search()));
            }
        }
        public ICommand ClearSearchCommand
        {
            get
            {
                return _clearSearchCommand ?? (_clearSearchCommand = new CommandHandler(() => ClearSearch()));
            }
        }
        public ICommand CreateNewCommand
        {
            get
            {
                return _createNewCommand ?? (_createNewCommand = new CommandHandler(() => CreateNew()));
            }

        }
        #endregion

        #region Properties
        public IEnumerable<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                NotifyPropertyChanged("Contacts");
            }
        }

        public Contact SelectedContact { get; set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                NotifyPropertyChanged("SearchText");
            }
        }
        
        public Contact EditingContact { get; set; }

        private void ContactsChanged(object sender, EventArgs e)
        {
            GetContacts();
        }
        #endregion

        public void GetContacts()
        {
            Contacts = _webService.GetContacts();
        }

        public void SaveContact()
        {
            try
            {
                if (!string.IsNullOrEmpty(EditingContact._id))
                {
                    _webService.Update(EditingContact);
                }
                else
                {
                    _webService.Create(EditingContact);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(string.Format("Error saving contact. {0}", e.Message));
            }

            if (_contactViewDialog != null)
            {
                _contactViewDialog.Close();
                _contactViewDialog = null;
            }
        }

        private void CancelSave()
        {
            _logger.LogInformation("Canceling create/edit contact");
            if (_contactViewDialog != null)
            {
                _contactViewDialog.Close();
                _contactViewDialog = null;
            }
        }

        private void EditContact()
        {
            EditingContact = SelectedContact;
            OpenContactView();
        }

        private void CreateNew()
        {
            EditingContact = new Contact();
            OpenContactView();
        }

        private void OpenContactView()
        {
            try
            {
                _contactViewDialog = new ContactView(this);
                _contactViewDialog.ShowDialog();
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("Error on contact view. {0}", e.Message));
            }
        }

        public void Search()
        {
            Contacts = _webService.FindByName(SearchText);
            _logger.LogInformation(string.Format("Searching for {0}.  Found {1} contacts.", SearchText, Contacts == null ? 0 : Contacts.Count()));
        }

        private void ClearSearch()
        {
            GetContacts();
            SearchText = null;
        }

        private void NotifyPropertyChanged(String propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
