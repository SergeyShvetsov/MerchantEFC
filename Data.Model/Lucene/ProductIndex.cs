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
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly string indexPath;

        public readonly bool Locked = true;
        public ProductIndex(string rootPath)
        {
            indexPath = Path.Combine(rootPath, "LuceneIndex");
            analyzer = SetupAnalyzer();
            queryParser = SetupQueryParser(analyzer);
            var attempts = 100;
            while (Locked && attempts > 0)
            {
                try
                {
                    writer = new IndexWriter(FSDirectory.Open(indexPath), new IndexWriterConfig(MATCH_LUCENE_VERSION, analyzer));
                    searchManager = new SearcherManager(writer, true, null);
                    Locked = false;
                }
                catch (LockObtainFailedException)
                {
                    attempts--;
                }
            }
        }

        private Analyzer SetupAnalyzer() => new StandardAnalyzer(MATCH_LUCENE_VERSION);

        private QueryParser SetupQueryParser(Analyzer analyzer) => new QueryParser(MATCH_LUCENE_VERSION, "tags", analyzer);

        public int Build(IQueryable<CatalogItem> items)
        {
            if (items == null) throw new ArgumentNullException();

            writer.DeleteAll();
            var i = 0;
            foreach (var item in items)
            {
                i++;
                writer.AddDocument(BuildDocument(item));
            }

            writer.Flush(true, true);
            writer.Commit();
            return i;
        }
        public void Add(CatalogItem item)
        {
            if (item == null) throw new ArgumentNullException();

            writer.AddDocument(BuildDocument(item));
            writer.Flush(true, true);
            writer.Commit();
        }
        public void Update(CatalogItem item)
        {
            if (item == null) throw new ArgumentNullException();

            writer.UpdateDocument(new Term("modelid", item.ModelId.ToString()), BuildDocument(item));
            writer.Flush(true, true);
            writer.Commit();
        }
        public void Delete(int itemId)
        {
            writer.DeleteDocuments(new Term("modelid", itemId.ToString()));
            writer.Flush(true, true);
            writer.Commit();
        }
        public void DeleteProduct(int productId)
        {
            writer.DeleteDocuments(new Term("productid", productId.ToString()));
            writer.Flush(true, true);
            writer.Commit();
        }

        public SearchResults Search(string serchString, CatalogFilters filters)
        {
            Query query = queryParser.Parse(serchString);

            searchManager.MaybeRefreshBlocking();
            IndexSearcher searcher = searchManager.Acquire();
            try
            {
                TopDocs topdDocs = searcher.Search(query, Int32.MaxValue);
                return CompileResults(searcher, topdDocs, filters);
            }
            finally
            {
                searchManager.Release(searcher);
                searcher = null;
            }
        }
        private SearchResults CompileResults(IndexSearcher searcher, TopDocs topdDocs, CatalogFilters filters)
        {
            SearchResults searchResults = new SearchResults() { TotalHits = topdDocs.TotalHits };
            foreach (var result in topdDocs.ScoreDocs)
            {
                Document document = searcher.Doc(result.Doc);
                var searchResult = new Hit
                {
                    ProductId = document.GetField("productid")?.GetInt32Value(),
                    ModelId = document.GetField("modelid")?.GetInt32Value(),
                    StoreId = document.GetField("storeid")?.GetInt32Value(),
                    CityId = document.GetField("cityid")?.GetInt32Value(),
                    Categories = document.GetField("categories")?.GetStringValue().Split(";"),
                    Name = document.GetField("name")?.GetStringValue(),
                    Rating = document.GetField("rating")?.GetDoubleValue(),
                    Price = document.GetField("price")?.GetDoubleValue(),
                    Score = result.Score,
                };

                if (searchResult.ProductId == null) continue;
                if (filters.StoreId != null && filters.StoreId != searchResult.StoreId) continue;
                if (filters.CityId != null && filters.CityId != searchResult.CityId) continue;
                if (!string.IsNullOrEmpty(filters.Category) && !searchResult.Categories.Any(a => a.StartsWith(filters.Category))) continue;

                searchResults.Hits.Add(searchResult);

                // Order and Pagination
            }

            return searchResults;
        }
        private Document BuildDocument(CatalogItem item)
        {
            Document doc = new Document
            {
                new Int32Field("productid", item.ProductId, Field.Store.YES),
                new Int32Field("modelid", item.ModelId, Field.Store.YES),
                new Int32Field("storeid", item.StoreId, Field.Store.YES),
                new Int32Field("cityid", item.CityId, Field.Store.YES),
                new StringField("categories",item.Categories, Field.Store.YES),
                new StringField("name",item.Name, Field.Store.YES),
                new DoubleField("rating",item.Rating,Field.Store.YES),
                new DoubleField("price",item.SalesPrice ?? item.Price,Field.Store.YES),
                new TextField("tags", item.Name + ";" + item.Tags, Field.Store.NO)
            };

            return doc;
        }
        public void Dispose()
        {
            searchManager?.Dispose();
            analyzer?.Dispose();
            writer?.Dispose();
        }
    }
}
