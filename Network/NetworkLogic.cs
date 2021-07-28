using Pandora.DI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Pandora.Network
{
    public class NetworkLogic
    {
        protected static string BaseURL = "https://pandora.dev.realityfamily.ru/";

        public static T DownloadString<T> (string url)
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            try
            {
                T response = JsonSerializer.Deserialize<T>(client.DownloadString(BaseURL + url));

                return response;
            }
            catch (WebException we)
            {
                if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthNetworkLogic.Refresh();
                }
                return default(T);
            }
        }
    }
}
