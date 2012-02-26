using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using WebSiteGraph.GraphModel;

namespace WebSiteGraph
{
    public class WebPage
    {
        public Uri Url { get; set; }

        public static WebGraph GetGraph(Uri urlRoot)
        {
            var graph = new WebGraph();
            var queue = new Queue<Uri>();
            var allSiteUrls = new HashSet<Uri>();

            queue.Enqueue(urlRoot);
            allSiteUrls.Add(urlRoot);

            while (queue.Count > 0)
            {
                Uri url = queue.Dequeue();

                HttpWebRequest oReq = (HttpWebRequest)WebRequest.Create(url);
                oReq.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5";

                var httpResponse = (HttpWebResponse)oReq.GetResponse();

                WebPage result;

                if (httpResponse.ContentType.StartsWith("text/html", StringComparison.InvariantCultureIgnoreCase))
                {
                    var htmlDocument = new HtmlDocument();
                    try
                    {
                        var resultStream = httpResponse.GetResponseStream();
                        htmlDocument.Load(resultStream); // The HtmlAgilityPack
                        result = new Internal() { Url = url, HtmlDocument = htmlDocument };
                    }
                    catch (WebException ex)
                    {
                        result = new WebPage.Error() { Url = url, Exception = ex };
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Url", url);
                        throw;
                    }

                    string vertexUrl = url.ToString().Replace(urlRoot.ToString(), "/").Replace("//", "/");
                    WebVertex v1 = graph.Vertices.SingleOrDefault(v => v.Url == vertexUrl);
                    if (v1 == null)
                    {
                        v1 = new WebVertex(vertexUrl);
                        graph.AddVertex(v1);
                    }

                    var links = htmlDocument.DocumentNode.SelectNodes(@"//a[@href]");
                    if (links != null)
                    {
                        foreach (HtmlNode link in links)
                        {
                            HtmlAttribute att = link.Attributes["href"];
                            if (att == null) continue;
                            string href = att.Value;
                            if (href.StartsWith("javascript", StringComparison.InvariantCultureIgnoreCase)) continue; // ignore javascript on buttons using a tags

                            Uri urlNext = new Uri(href, UriKind.RelativeOrAbsolute);
                            Uri urlNext2 = new Uri(href, UriKind.RelativeOrAbsolute);

                            // Make it absolute if it's relative
                            if (!urlNext.IsAbsoluteUri)
                            {
                                urlNext = new Uri(Path.Combine(urlRoot.AbsoluteUri, urlNext.ToString()));
                            }

                            if (!allSiteUrls.Contains(urlNext))
                            {
                                allSiteUrls.Add(urlNext); // keep track of every page we've handed off

                                if (urlRoot.IsBaseOf(urlNext))
                                {
                                    queue.Enqueue(urlNext);
                                }

                                string vertexUrl2 = urlNext2.ToString();
                                if (!vertexUrl2.StartsWith("http://") && !vertexUrl2.StartsWith("/"))
                                {
                                    vertexUrl2 = "/" + vertexUrl2;
                                }

                                WebVertex v2 = graph.Vertices.SingleOrDefault(v => v.Url == vertexUrl2);
                                if (v2 == null)
                                {
                                    v2 = new WebVertex(vertexUrl2);
                                    graph.AddVertex(v2);
                                }
                                graph.AddEdge(new WebEdge(v1, v2));
                            }
                        }
                    }
                }
            }

            return graph;
        }

        public class Error : WebPage
        {
            public int HttpResult { get; set; }
            public Exception Exception { get; set; }
        }

        public class External : WebPage {}

        public class Internal : WebPage
        {
            public virtual HtmlDocument HtmlDocument { get; internal set; }
        }
    }
}
