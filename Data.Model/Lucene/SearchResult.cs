using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.Lucene
{
    public class SearchResults
    {
        public SearchResults() => Hits = new List<Hit>();
        public string Time { get; set; }
        public int TotalHits { get; set; }
        public IList<Hit> Hits { get; set; }
    }

    public class Hit
    {
        public float Score { get; set; }
        public int? ProductId { get; set; }
        public int? ModelId { get; set; }
        public int? StoreId { get; set; }
        public int? CityId { get; set; }
        public string[] Categories { get; set; }
        public string Name { get; set; }
        public double? Rating { get; set; }
        public double? Price { get; set; }

    }
}
