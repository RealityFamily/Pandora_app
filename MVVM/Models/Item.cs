using Pandora.DI;
using Pandora.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace Pandora.MVVM.Models
{
    public class Item
    {
        public enum ItemType
        {
            Model,
            Material,
            Asset,
            Unknown,
            Category,
        }

        private string _id;
        private string _name;
        private string _description;
        private string _author;
        private int _size;
        private bool _premium;

        private ItemType _type;

        private byte[] _smallImageCache;
        private byte[] _largeImageCache;


        public Item(string Id, string Name, string Description, string Author, int Size, bool Premium)
        {
            _id = Id;
            _name = Name;
            _description = Description;
            _author = Author;
            _size = Size;
            _premium = Premium;
            _type = ItemType.Model;
        }

        public Item(Item item)
        {
            _id = item.Id;
            _name = item.Name;
            _description = item.Description;
            _author = item.Author;
            _size = item.Size;
            _premium = item.Premium;
            _type = item._type;
        }

        public Item()
        {

        }

        [JsonPropertyName("id")]
        public string Id { get { return _id; } set { _id = value; } }
        [JsonPropertyName("name")]
        public string Name { get { return _name; } set { _name = value; } }
        [JsonPropertyName("description")]
        public string Description { get { return _description; } set { _description = value; } }
        [JsonPropertyName("authorNick")]
        public string Author { get { return _author; } set { _author = value; } }
        [JsonPropertyName("modelSize")]
        public int Size { get { return _size; } set { _size = value; } }
        public bool Premium { get { return _premium; } set { _premium = value; } }

        [JsonIgnore]
        public bool Contains
        {
            get
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    return new LocalServiceLocator().FileSystemMethods.GetLocalId().Any(id => id.Equals(_id));
                }
                else
                {
                    return false;
                }
            }
        }
        [JsonIgnore]
        public BitmapImage SmallImage
        {
            get
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    if (!Contains)
                    {
                        _smallImageCache = ItemsNetworkLogic.GetItemSmallImage(Id);
                    } else {
                        _smallImageCache = new LocalServiceLocator().FileSystemMethods.GetLocalItemInfo(_id)._smallImageCache;
                    }

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(_smallImageCache);
                    image.EndInit();
                    return image;
                }

                return null;
            }
        }
        [JsonIgnore]
        public BitmapImage LargeImage
        {
            get
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    if (!Contains)
                    {
                        _largeImageCache = ItemsNetworkLogic.GetItemLargeImage(Id);
                    }
                    else
                    {
                        _largeImageCache = new LocalServiceLocator().FileSystemMethods.GetLocalItemInfo(_id)._largeImageCache;
                    }

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(_largeImageCache);
                    image.EndInit();
                    return image;
                }

                return null;
            }
        }

        public byte[] SmallImageCache
        {
            get
            {
                return _smallImageCache;
            }
            set
            {
                _smallImageCache = value;
            }
        }
        public byte[] LargeImageCache
        {
            get
            {
                return _largeImageCache;
            }
            set
            {
                _largeImageCache = value;
            }
        }

        [JsonPropertyName("type")]
        public ItemType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public static Item GetItemInfo(Item item)
        {
            Item info;
            if (item.Contains)
            {
                info = new LocalServiceLocator().FileSystemMethods.GetLocalItemInfo(item.Id);
            } else
            {
                info = ItemsNetworkLogic.GetItemInfo(item.Id);
            }

            info._smallImageCache = item._smallImageCache;
            return info;
        }
    }
}
