using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManagementApp.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ContactManagementApp.Services
{
    public class NodeJsContactWebServices : IContactWebServices
    {
        private ILogger _logger = new EventViewerLogger();

        public event EventHandler ContactsChanged;

        public void Create(Contact contact)
        {
            using (var client = CreateClient())
            {
                using (HttpResponseMessage response = client.PostAsJsonAsync("contacts", contact).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        LogInformation("Create(Contact)", response.StatusCode.ToString(), response.ReasonPhrase);
                        ContactsChanged(this, EventArgs.Empty);
                    }
                    else
                    {
                        var message = LogError("Create(Contact)", response.StatusCode.ToString(), response.ReasonPhrase);
                        throw new Exception(message);
                    }
                }
            }
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Contact FindById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> FindByName(string name)
        {
            IEnumerable<Contact> contacts = null;
            using (var client = CreateClient())
            {
                using (HttpResponseMessage response = client.GetAsync(string.Format("contactsbyname/{0}", name)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        LogInformation("FindByName(string)", response.StatusCode.ToString(), response.ReasonPhrase);
                        contacts = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result;
                    }
                    else
                    {
                        var message = LogError("FindByName(string)", response.StatusCode.ToString(), response.ReasonPhrase);
                        throw new Exception(message);
                    }
                    return contacts;
                }
            }
        }

        public IEnumerable<Contact> GetContacts()
        {
            IEnumerable<Contact> contacts = null;
            using (var client = CreateClient())
            {
                using (HttpResponseMessage response = client.GetAsync("contacts").Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        LogInformation("GetContacts()", response.StatusCode.ToString(), response.ReasonPhrase);
                        contacts = response.Content.ReadAsAsync<IEnumerable<Contact>>().Result;
                    }
                    else
                    {
                        var message = LogError("GetContacts()", response.StatusCode.ToString(), response.ReasonPhrase);
                        throw new Exception(message);
                    }
                    return contacts;
                }
            }
        }

        public void Update(Contact contact)
        {
            using (var client = CreateClient())
            {
                using (HttpResponseMessage response = client.PutAsJsonAsync("contacts", contact).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        LogInformation("Update(Contact)", response.StatusCode.ToString(), response.ReasonPhrase);
                        ContactsChanged(this, EventArgs.Empty);
                    }
                    else
                    {
                        var message = LogError("Update(Contact)", response.StatusCode.ToString(), response.ReasonPhrase);
                        throw new Exception(message);
                    }
                }
            }
        }

        public HttpClient CreateClient()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("Error creating web services client. {0}", e.Message));
                throw e;
            }
        }

        private string LogInformation(string source, string statusCode, string reasonPhrase)
        {
            var message = string.Format("Webservices - {0} {1} ({2})", source, statusCode, reasonPhrase);
            _logger.LogInformation(message);
            return message;
        }

        private string LogError(string source, string statusCode, string reasonPhrase)
        {
            var message = string.Format("Error: Webservices - {0} {1} ({2})", source, statusCode, reasonPhrase);
            _logger.LogError(message);
            return message;
        }
    }
}
