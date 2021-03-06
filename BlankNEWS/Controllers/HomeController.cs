﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleFeedReader;
using HtmlAgilityPack;
using System.Net.Http;
using BlankNEWS.Models;
using System.Net;
using System.Xml.Linq;

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
            if (!String.IsNullOrEmpty(Keyword) && !String.IsNullOrEmpty(Algorithm))
            {
                var feed = new FeedReader();
                var news = feed.RetrieveFeed("http://www.antaranews.com/rss/terkini");
                ArrayList result = new ArrayList();
                HtmlDocument htmlDoc = new HtmlDocument();
                using (var article = new HttpClient())
                {
                    foreach (var i in news)
                    {
                        var response = article.GetAsync((i as FeedItem).Uri).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var content = response.Content;
                            htmlDoc.LoadHtml(content.ReadAsStringAsync().Result);
                            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@id='content_news']");
                            if (nodes != null)
                            {
                                var node = nodes.First();
                                StringMatch testSearch = new StringMatch(node.InnerText, Keyword);
                                if (Algorithm.Equals("KMP"))
                                {
                                    //search with KMP
                                    //if found, replace i.Content to string that contain keyword, then add to result
                                    String searchResult = testSearch.KMPsearch();
                                    if (!String.IsNullOrEmpty(searchResult))
                                    {
                                        i.Content = searchResult;
                                        result.Add(i as FeedItem);
                                    }
                                }
                                else if (Algorithm.Equals("Booyer-Moore"))
                                {
                                    //search with Booyer-Moore
                                    //if found, replace i.summary to string that contain keyword, then add to result
                                    String searchResult = testSearch.BoyerMooreSearch();
                                    if (!String.IsNullOrEmpty(searchResult))
                                    {
                                        i.Content = searchResult;
                                        result.Add(i as FeedItem);
                                    }
                                }
                                else if (Algorithm.Equals("Regex"))
                                {
                                    //search with Regex
                                    //if found, replace i.summary to string that contain keyword, then add to result
                                    String searchResult = testSearch.RegexSearch();
                                    if (!String.IsNullOrEmpty(searchResult))
                                    {
                                        i.Content = searchResult;
                                        result.Add(i as FeedItem);
                                    }
                                }
                            }
                        }
                    }
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
