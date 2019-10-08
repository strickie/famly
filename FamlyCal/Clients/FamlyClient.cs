using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Famly.Model;
using Famly.Model.Calendar;
using Famly.Model.Login;

namespace FamlyCal.Clients
{
    public class FamlyClient : IFamlyClient
    {
        private HttpClient Client { get; }

        public string AccessToken
        {
            set
            {
                Client.DefaultRequestHeaders.Add("x-famly-accesstoken", value);
            }
        }

        public FamlyClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://app.famly.co/api/");
            Client = client;
        }

        public async Task Login(string email, string password)
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

                AccessToken = res.accessToken;
            }
            else
            {
                throw new LoginException($"{response.StatusCode} - {response.ReasonPhrase}");
            }            
        }

        public enum CalendarUnit
        {
            DAY,
            WEEK,
            MONTH
        };

        public async Task<CalendarResponse> GetCalendar(DateTime from, CalendarUnit unit, int numberOfUnit)
        {
            var response = await Client.GetAsync($"v2/calendar?type={unit}&day={ToFamlyDate(from)}&limit={numberOfUnit}");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsAsync<ICollection<Period>>();

                return new CalendarResponse { Periodes = res };
            }

            return null;
        }


        private string ToFamlyDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
