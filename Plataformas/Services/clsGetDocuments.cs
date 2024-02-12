using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetLabel_LP.Services
{
        // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
        public class OrderDocument
        {
            [JsonPropertyName("date_uploaded")]
            public DateTime date_uploaded { get; set; }

            [JsonPropertyName("file_name")]
            public string file_name { get; set; }

            [JsonPropertyName("file_size")]
            public int file_size { get; set; }

            [JsonPropertyName("id")]
            public int id { get; set; }

            [JsonPropertyName("order_id")]
            public string order_id { get; set; }

            [JsonPropertyName("type")]
            public string type { get; set; }
        }

        public class clsGetDocuments
    {
            [JsonPropertyName("order_documents")]
            public List<OrderDocument> order_documents { get; set; }

            [JsonPropertyName("total_count")]
            public int total_count { get; set; }
        }
}
