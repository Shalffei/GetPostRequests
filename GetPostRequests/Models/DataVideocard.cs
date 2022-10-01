using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace GetPostRequests.Models
{
    public class Alternate
    {
        [JsonPropertyName("lang")]
        public string Lang { get; set; }

        [JsonPropertyName("hreflang")]
        public string Hreflang { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("subdomain")]
        public string Subdomain { get; set; }
    }

    public class DataVideocard
    {
        [JsonPropertyName("ids")]
        public List<int> Ids { get; set; }

        [JsonPropertyName("ids_count")]
        public int IdsCount { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("show_next")]
        public int ShowNext { get; set; }

        [JsonPropertyName("goods_with_filter")]
        public int GoodsWithFilter { get; set; }

        [JsonPropertyName("goods_in_category")]
        public int GoodsInCategory { get; set; }

        [JsonPropertyName("goods_limit")]
        public int GoodsLimit { get; set; }

        [JsonPropertyName("active_pages")]
        public List<int> ActivePages { get; set; }

        [JsonPropertyName("shown_page")]
        public int ShownPage { get; set; }

        [JsonPropertyName("alternate")]
        public List<Alternate> Alternate { get; set; }
    }

    public class RootIds
    {
        [JsonPropertyName("data")]
        public DataVideocard Data { get; set; }
    }


}
