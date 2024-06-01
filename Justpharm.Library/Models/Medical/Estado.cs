using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Justpharm.Library.Models.Medical
{
    public class Estado
    {
        [JsonPropertyName("aut")]
        public long Aut { get; set; }
    }
}
