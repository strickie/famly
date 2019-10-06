using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Famly.Model;

namespace FamlyCal.Clients
{
    public class FamlyClient : IFamlyClient
    {
        public HttpClient Client { get; }

        public FamlyClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://app.famly.co/api/");
            Client = client;
        }

        public async Task<string> Login(string email, string password)
        {
            AuthenticateRequest req = new AuthenticateRequest
            {
                email = email,
                password = password,
            };

            var response = await Client.PostAsJsonAsync("login/login/authenticate", req);

            if (response.IsSuccessStatusCode)
            {
                AuthenticateResponse res = await response.Content.ReadAsAsync<AuthenticateResponse>();
                Console.WriteLine(res.accessToken);

                return res.accessToken;
            }

            return null;
        }
    }
}
