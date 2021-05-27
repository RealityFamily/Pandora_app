using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pandora.MVVM.Models
{
    class ItemTag
    {
        private string _id;
        private string _title;
        private List<Item> _items;

        [JsonPropertyName("id")]
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        [JsonPropertyName("title")]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        [JsonPropertyName("itemCardShortDTOS")]
        public List<Item> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }
    }
}
