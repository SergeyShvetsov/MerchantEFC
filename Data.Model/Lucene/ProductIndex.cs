using Data.Model.Models;
using Data.Tools.Extensions;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Lucene
{
    public class ProductIndex : IDisposable
    {
        private const LuceneVersion MATCH_LUCENE_VERSION = LuceneVersion.LUCENE_48;
        private readonly IndexWriter writer;
        private readonly Analyzer analyzer;
        private readonly QueryParser queryParser;
        private readonly SearcherManager searchManager;

        public ProductIndex(string indexPath)
        {

            analyzer = SetupAnalyzer();
            queryParser = SetupQueryParser(analyzer);
            writer = new IndexWriter(FSDirectory.Open(indexPath), new IndexWriterConfig(MATCH_LUCENE_VERSION, analyzer));
            searchManager = new SearcherManager(writer, true, null);
        }

        private Analyzer SetupAnalyzer() => new StandardAnalyzer(MATCH_LUCENE_VERSION);

        private QueryParser SetupQueryParser(Analyzer analyzer) => new QueryParser(MATCH_LUCENE_VERSION, "title", analyzer);

        public void Build(IQueryable<Product> products)
        {
            if (products == null) throw new ArgumentNullException();

            foreach (var product in products)
                writer.AddDocuments(BuildDocuments(product));

            writer.Flush(true, true);
            writer.Commit();
        }
        public void Add(Product product)
        {
            if (product == null) throw new ArgumentNullException();

            writer.AddDocuments(BuildDocuments(product));
            writer.Flush(true, true);
            writer.Commit();
        }
        public void Update(Product product)
        {
            if (product == null) throw new ArgumentNullException();

            writer.DeleteDocuments(new Term("Id", product.Id.ToString()));
            writer.AddDocuments( BuildDocuments(product));
            writer.Flush(true, true);
            writer.Commit();
        }
        public void Delete(Product product)
        {
            if (product == null) throw new ArgumentNullException();

            writer.DeleteDocuments(new Term("Id", product.Id.ToString()));
            writer.Flush(true, true);
            writer.Commit();
        }

        public SearchResults Search(string serchString)
        {
            Query query = queryParser.Parse(serchString);
            searchManager.MaybeRefreshBlocking();
            IndexSearcher searcher = searchManager.Acquire();

            try
            {
                TopDocs topdDocs = searcher.Search(query, Int32.MaxValue);
                return CompileResults(searcher, topdDocs);
            }
            finally
            {
                searchManager.Release(searcher);
                searcher = null;
            }
        }
        private SearchResults CompileResults(IndexSearcher searcher, TopDocs topdDocs)
        {
            SearchResults searchResults = new SearchResults() { TotalHits = topdDocs.TotalHits };
            foreach (var result in topdDocs.ScoreDocs)
            {
                Document document = searcher.Doc(result.Doc);
                var searchResult = new Hit
                {
                    Id = document.GetField("Id")?.GetInt32Value(),
                    Score = result.Score,
                };
                searchResults.Hits.Add(searchResult);
            }

            return searchResults;
        }
        private Document BuildDocument(Product product, string category)
        {
            Document doc = new Document
            {
                new Int32Field("Id", product.Id, Field.Store.YES),
                new Int32Field("StoreId", product.StoreId, Field.Store.YES),
                new Int32Field("CityId", product.Store.CityId, Field.Store.YES),
                new StringField("Category",category, Field.Store.YES),
                new TextField("Tags", product.Name +" " + product.Tags, Field.Store.NO)
            };

            return doc;
        }
        private IEnumerable<Document> BuildDocuments(Product product)
        {
            var result = new List<Document>();
            foreach(var cat in product.Categories)
            {
                result.Add(BuildDocument(product, cat.Category));
            }
            return result;
        }
        public void Dispose()
        {
            searchManager?.Dispose();
            analyzer?.Dispose();
            writer?.Dispose();
        }
    }
}
