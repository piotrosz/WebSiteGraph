using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WebSiteGraph.GraphModel
{
    [DebuggerDisplay("{Url}")]
    public class WebVertex
    {
        public string Url { get; private set; }
        
        public WebVertex(string url)
        {
            this.Url = url;
        }

        public string UrlShort
        {
            get { return Url.Replace("http://", "").Replace("www.", ""); }
        }
    }
}
