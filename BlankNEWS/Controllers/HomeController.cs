using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleFeedReader;

namespace BlankNEWS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewData["Message"] = "This is a news aggregator application. Search all kinds of latest news here.";

            return View();
        }

        public ActionResult Search(string Algorithm, string Keyword)
        {
            var feed = new FeedReader();
            var items = feed.RetrieveFeed("http://rss.vivanews.com/get/all");
            if (!String.IsNullOrEmpty(Keyword) && !String.IsNullOrEmpty(Algorithm))
            {
                ArrayList result = new ArrayList();
                foreach (var i in items)
                {
                    if (Algorithm.Equals("KMP"))
                    {
                        //search with KMP
                        //if found, replace i.summary to string that contain keyword, then add to result
                    }
                    else if (Algorithm.Equals("Booyer-Moore"))
                    {
                        //search with Booyer-Moore
                        //if found, replace i.summary to string that contain keyword, then add to result
                    }
                    else if (Algorithm.Equals("Regex"))
                    {
                        //search with Regex
                        //if found, replace i.summary to string that contain keyword, then add to result
                    }
                    result.Add(i as FeedItem);  //delete this later
                }
                ViewBag.items = result;
            }
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
