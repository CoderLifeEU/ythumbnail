using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YThumbnail.Models;
using System.Net;
using System.IO;
using AngleSharp;
using YThumbnail.Extension;
using System.IO.Compression;

namespace YThumbnail.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Thumbnail");
            //return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Thumbnail()
        {
            ThumbnailRequest model = new ThumbnailRequest();
            model.Link = "https://www.youtube.com/watch?v=-saw_cLQYeM&index=2&list=UUURLZDHYWZryGIHU0ksA_Gg";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Thumbnail(ThumbnailRequest model)
        {
            if(!ModelState.IsValid)
            return View(model);
            else
            {
                var absolutePath = AppDomain.CurrentDomain.BaseDirectory;
                var path = absolutePath + "\\" + model.Prefix;
                bool folderExists = Directory.Exists(path);
                if (!folderExists)
                    Directory.CreateDirectory(path);

                // Setup the configuration to support document loading
                var config = Configuration.Default.WithDefaultLoader();

                var address = model.Link;
                // Asynchronously get the document in a new context using the configuration
                var document = await BrowsingContext.New(config).OpenAsync(address);
                // This CSS selector gets the desired content
                var cellSelector = "a";
                // Perform the query to get all cells with the content
                var cells = document.QuerySelectorAll(cellSelector);
                // We are only interested in the text - select it with LINQ
                var titles = cells.Select(m => m.TextContent);
                var links = cells.Select(m => m.GetAttribute("href")).ToList();
                var sortedLinks = links.Where(x => x.Contains("/watch?v")).ToList();
                sortedLinks = sortedLinks.Select(x => SubstringExtensions.BeforeIfContains(SubstringExtensions.BeforeIfContains(SubstringExtensions.After(x, "/watch?v="), "&index="), "&list=")).ToList();

                var tasks = sortedLinks.Select(link => Task.Factory.StartNew(() =>
                {
                    using (WebClient webClient = new WebClient())
                    {
                        string thumbnailLink = string.Format("https://img.youtube.com/vi/{0}/maxresdefault.jpg", link);
                        try
                        {
                            webClient.DownloadFile(thumbnailLink, string.Format(path + "\\{0}.jpg", link));
                            webClient.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                    }

                })).ToArray();
                Task.WaitAll(tasks);




                return RedirectToAction("Results", new { prefix = model.Prefix });
            }
        }

        public IActionResult Results(string prefix)
        {
            return View("Results", prefix);
        }

        public IActionResult Download(string prefix)
        {
            var absolutePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = absolutePath + "\\" + prefix;
            var filename = prefix + ".zip";
            string zipPath = path +filename;

            ZipFile.CreateFromDirectory(path, zipPath);

            byte[] fileBytes = System.IO.File.ReadAllBytes(zipPath);
            return File(fileBytes, "application/x-msdownload", filename);
        }

       
    }
}
