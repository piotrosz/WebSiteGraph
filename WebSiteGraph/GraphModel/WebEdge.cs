using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using QuickGraph;

namespace WebSiteGraph.GraphModel
{
    public class WebEdge : Edge<WebVertex>
    {
        public WebEdge(WebVertex source, WebVertex target)
            : base(source, target)
        {
        }
    }
}
