using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace WebSiteGraph.GraphModel
{
    public class WebGraph : BidirectionalGraph<WebVertex, WebEdge>
    {
        public WebGraph() { }

        public WebGraph(bool allowParallelEdges) 
            : base(allowParallelEdges) { }

         public WebGraph(bool allowParallelEdges, int vertexCapacity) 
             : base(allowParallelEdges, vertexCapacity) { }
    }
}
