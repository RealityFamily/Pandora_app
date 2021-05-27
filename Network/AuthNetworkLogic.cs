using Pandora.DI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pandora.Network
{
    class AuthNetworkLogic : NetworkLogic
    {
        public static void Auth(string login, string password)
        {
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", "application/json");

            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("username", login);
            body.Add("password", password);

            string response = client.UploadString(new Uri(BaseURL + "authenticate"), JsonSerializer.Serialize(body));

            new LocalServiceLocator().ApplicationConfig.AddAuth(
                JsonSerializer.Deserialize<Dictionary<string, string>>(response)["token"],
                JsonSerializer.Deserialize<Dictionary<string, string>>(response)["username"]
                );
        }

        public static void Refresh()
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            new LocalServiceLocator().ApplicationConfig.RefreshAuth(JsonSerializer.Deserialize<Dictionary<string, string>>(client.DownloadString(new Uri(BaseURL + "refresh")))["token"]);
        }
    }
}
