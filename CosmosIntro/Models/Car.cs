using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosIntro.Models
{
    public class Car
    {
        [JsonProperty(PropertyName = "id")]
        public string  Id { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        public bool Equals(Car obj)
        {
            if (obj == null)
            {
                return false;
            }
            return string.Equals(this.Id, obj.Id) &&
                   string.Equals(this.Color, obj.Color);
        }
    }
}
