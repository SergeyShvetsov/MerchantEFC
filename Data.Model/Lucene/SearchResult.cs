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
        public int? Id { get; set; }
    }
}
