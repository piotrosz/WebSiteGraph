﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using WebSiteGraph.GraphModel;

namespace WebSiteGraph.ViewModel
{
    public class WebGraphLayout : GraphLayout<WebVertex, WebEdge, WebGraph> { }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string layoutAlgorithmType;
        private WebGraph graph;
        private string url;
        private List<string> layoutAlgorithmTypes;

        public void ReLayoutGraph()
        {
            graph = WebPage.GetGraph(new Uri(Url));
            
            NotifyPropertyChanged("Graph");
        }

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public WebGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }

        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                NotifyPropertyChanged("Url");
            }
        }

        public MainWindowViewModel()
        {
            //Graph = GraphCreator.Create(Directory);

            layoutAlgorithmTypes = new List<string>()
            {
                "BoundedFR",
                "Circular",
                "CompoundFDP",
                "EfficientSugiyama",
                "FR",
                "ISOM",
                "KK",
                "LinLog",
                "Tree"
            };

            // default
            LayoutAlgorithmType = "Tree";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
