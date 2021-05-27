using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pandora.MVVM.Models
{
    public class Group
    {
        private string _id;
        private string _name;

        public Group (string Id, string Name)
        {
            _id = Id;
            _name = Name;
        }

        public Group()
        {

        }

        public string id { get { return _id; } set { _id = value; } }
        public string category { get { return _name; } set { _name = value; } }

        [JsonPropertyName("title")]
        public string category2 { set { _name = value; } }
    }
}
