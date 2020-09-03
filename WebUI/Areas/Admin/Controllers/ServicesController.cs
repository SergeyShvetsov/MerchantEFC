using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RebuildIndex()
        {
            //string directoryPath = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\LuceneIndexes";
            //directory = FSDirectory.Open(directoryPath);
            //analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            //writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            return View();
        }
    }
}
