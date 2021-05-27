using Pandora.DI;
using Pandora.MVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace Pandora.Network
{
    class ItemsNetworkLogic : NetworkLogic
    {
        public static Dictionary<string, List<Item>> GetItems(string subGroupId)
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            Dictionary<string, List<Item>> items = new Dictionary<string, List<Item>>();

            try
            {
                List<ItemTag> tags = JsonSerializer.Deserialize<List<ItemTag>>(client.DownloadString(BaseURL + "api/v1/client/item/bysubgroup/" + subGroupId));

                if (tags != null && tags.Count > 0) {
                    foreach (ItemTag tag in tags)
                    {
                        if (tag.Title.Equals("Main") || tag.Title.Equals("main") || tag.Title.Equals(""))
                        {
                            items[""].AddRange(tag.Items);
                        }
                        else
                        {
                            items.Add(tag.Title, tag.Items);
                        }
                    }
                }
            } catch (WebException we)
            {
                if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthNetworkLogic.Refresh();
                }
            }

            return items;
        }

        public static Item GetItemInfo(string itemId)
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

           Item item = null;

            try
            {
                item = JsonSerializer.Deserialize<Item>(client.DownloadString(BaseURL + "api/v1/client/item/" + itemId));
            }
            catch (WebException we)
            {
                if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthNetworkLogic.Refresh();
                    item = JsonSerializer.Deserialize<Item>(client.DownloadString(BaseURL + "api/v1/client/item/" + itemId));
                }
            }

            return item;
        }

        public static byte[] GetItemSmallImage(string itemId)
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            byte[] image = null;

            try
            {
                image = client.DownloadData(BaseURL + "api/v1/client/item/" + itemId + "/photo/small");
            }
            catch (WebException we)
            {
                if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthNetworkLogic.Refresh();

                    image = client.DownloadData(BaseURL + "api/v1/client/item/" + itemId + "/photo/small");
                }
            }

            return image;
        }

        public static byte[] GetItemLargeImage(string itemId)
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            byte[] image = null;

            try
            {
                image = client.DownloadData(BaseURL + "api/v1/client/item/" + itemId + "/photo/large");
            }
            catch (WebException we)
            {
                if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthNetworkLogic.Refresh();

                    image = client.DownloadData(BaseURL + "api/v1/client/item/" + itemId + "/photo/large");
                }
            }

            return image;
        }

        public static void DownloadZIP(Item item)
        {
            WebClient client = new WebClient();

            client.Headers["Authorization"] = "Bearer " + new LocalServiceLocator().UserViewModel.Token.Value;

            string zipFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, item.Id + ".zip");

            try
            {
                client.DownloadFile(BaseURL + "api/v1/client/item/" + item.Id + "/download", zipFileName);
                new LocalServiceLocator().FileSystemMethods.UnzipModel(item, zipFileName);
            }
            catch (WebException we)
            {
                if ((we.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
                {
                    AuthNetworkLogic.Refresh();

                    client.DownloadFile(BaseURL + "api/v1/client/item/" + item.Id + "/download", zipFileName);
                    new LocalServiceLocator().FileSystemMethods.UnzipModel(item, zipFileName);
                }
            }
        }

        public Dictionary<string, List<Item>> GetMockItems(string subGroupId)
        {
            Dictionary<string, List<Item>> temp = new Dictionary<string, List<Item>>();

            temp.Add("Main", new List<Item>()
                {
                    new Item("1", "Some model", "This is sum test model for proj", "Leonis13579", 100, false),
                    new Item("2", "Some model", "This is sum test model for proj", "Leonis13579", 100, true),
                    new Item("3", "Some model", "This is sum test model for proj", "Leonis13579", 100, false),
                });
            temp.Add("Some Tag", new List<Item>()
                {
                    new Item("4", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("5", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("6", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("7", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, true),
                    new Item("8", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("9", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("10", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("11", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("12", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("14", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("15", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("16", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("17", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("18", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                    new Item("19", "SomeSome model", "This is some test model for tag grop", "Leonis13579", 1, false),
                });

            Dictionary<string, List<Item>> items = new Dictionary<string, List<Item>>();
            items.Add("", new List<Item>());

            foreach (string key in temp.Keys)
            {
                if (key.Equals("Main") || key.Equals("main") || key.Equals(""))
                {
                    items[""].AddRange(temp[key]);
                }
                else
                {
                    items.Add(key, temp[key]);
                }
            }

            return items;
        }
    }
}
