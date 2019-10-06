namespace Famly.Model.Login
{
    public class AuthenticateResponse
    {
        public string accessToken { get; set; }
        public string deviceId { get; set; }
        public Links links { get; set; }

        public class Links
        {
            public string me { get; set; }
        }
    }
}
