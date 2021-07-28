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
        public static bool Auth(string login, string password)
        {
            WebClient client = new WebClient();

            client.Headers.Add("Content-Type", "application/json");

            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("username", login);
            body.Add("password", password);

            try
            {
                string response = client.UploadString(new Uri(BaseURL + "authenticate"), JsonSerializer.Serialize(body));

                new LocalServiceLocator().ApplicationConfig.AddAuth(
                    JsonSerializer.Deserialize<Dictionary<string, string>>(response)["token"],
                    JsonSerializer.Deserialize<Dictionary<string, string>>(response)["username"]
                    );

                return true;
            } catch (WebException web)
            {
                return false;
            }
        }

        public static bool Refresh()
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            try
            {
                new LocalServiceLocator().ApplicationConfig.RefreshAuth(JsonSerializer.Deserialize<Dictionary<string, string>>(client.DownloadString(new Uri(BaseURL + "refresh")))["token"]);
                return true;
            } catch (WebException webEx)
            {
                new LocalServiceLocator().UserViewModel.Token.Value = null;
                return false;
            }
        }
    }
}
