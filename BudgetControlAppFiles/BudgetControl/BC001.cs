using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace BudgetControl
{
    class BC001
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Budget")]
        public Double Budget { get; set; }

        [JsonProperty(PropertyName = "Remain")]
        public Double Remain { get; set; }
    }
}
