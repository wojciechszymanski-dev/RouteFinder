using Microsoft.Maui.Controls;
using RouteFinder.Resources.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace RouteFinder
{
    public partial class MainPage : ContentPage
    {
        readonly DrawImplementation drawInstance;
        private Route route;
        internal Dictionary<int, Node> Nodes { get; } = new();
        internal List<MyLine> Lines { get; } = new();
        internal Route RoutePath;

        Random rand = new Random();
        int id = 0;
        int routeID = 0;
        int lineID = 0;

        public MainPage()
        {
            InitializeComponent();
            drawInstance = new DrawImplementation();
            Canvas.Drawable = drawInstance;
        }

        public Point? GetTapLocation(TappedEventArgs e)
        {
            var tap = (TappedEventArgs)e;
            var location = tap.GetPosition(Canvas);
            return location;
        }

        public void CanvasTapped(object sender, TappedEventArgs e)
        {
            var tapLocation = GetTapLocation(e);
            if(tapLocation is null) return;

                drawInstance.basePoint = new double[] { tapLocation.Value.X, tapLocation.Value.Y };
                drawInstance.endPoint = new double[] { tapLocation.Value.X, tapLocation.Value.Y };
                bool intersects = false;
                int clickedNodeID = -1;
                foreach (var node in Nodes.Values)
                {
                    double distance = CalculateDistance(tapLocation.Value.X, tapLocation.Value.Y, node.PosX, node.PosY);
                    if (distance <= NodeRadius)
                    {
                        intersects = true;
                        clickedNodeID = node.ID;
                        Canvas.Invalidate();
                        break;
                    }   
                    node.IsSelected = false;
                }

                foreach (var node in Nodes.Values)
                {
                    node.IsSelected = false;
                }

                if (intersects)
                {
                    drawInstance.Nodes[clickedNodeID].IsSelected = true;
                    Canvas.Invalidate();
                }
                else
                {
                    Node nodeInstance = new Node(id, tapLocation.Value.X, tapLocation.Value.Y, rand.Next(1,10).ToString());
                    Nodes.Add(id, nodeInstance);

                    
                    if (id > 0 && drawInstance.Nodes.ContainsKey(id-1)) drawInstance.Nodes[id - 1].IsSelected = false;

                    drawInstance.Size = 2;
                    drawInstance.Nodes[id] = nodeInstance;
                    drawInstance.Nodes[id].IsSelected = true;

                    Canvas.Invalidate();
                    id++;
                }
        }

            private double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private const double NodeRadius = 40;

        private void PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            int startNodeID = -1;
            int endNodeID = -1;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    startNodeID = FindNearest(false);
                    if (startNodeID != -1)
                    {
                        drawInstance.Nodes[startNodeID].IsSelected = true;
                        drawInstance.basePoint = new double[] { drawInstance.Nodes[startNodeID].PosX, drawInstance.Nodes[startNodeID].PosY };
                        drawInstance.endPoint = drawInstance.basePoint;
                        Canvas.Invalidate();
                    }
                    break; 

                case GestureStatus.Running:
                    int id2 = FindNearest(false);
                    if (id2 != -1)
                    {
                        drawInstance.endPoint = new double[] { drawInstance.basePoint[0] + e.TotalX, drawInstance.basePoint[1] + e.TotalY };
                        Canvas.Invalidate();
                    }
                    break;

                case GestureStatus.Completed:
                    startNodeID = FindNearest(false);
                    endNodeID = FindNearest(true);
                    if (startNodeID != -1 && endNodeID != -1)
                    {
                       
                        drawInstance.Nodes[startNodeID].IsSelected = false;
                        MyLine lineInstance = new MyLine(lineID, startNodeID, endNodeID);
                        bool isExisting = false;

                        foreach(var line in drawInstance.Lines)
                        {
                            if(lineInstance.startNodeID == line.startNodeID && lineInstance.endNodeID == line.endNodeID)
                            {
                                isExisting = true;
                            }
                        } 
                            
                        if(!isExisting) drawInstance.Lines.Add(lineInstance);
                        Canvas.Invalidate();

                        lineID++;
                        drawInstance.basePoint = new double[] {0,0};
                        drawInstance.endPoint = new double[] {0, 0};
                    }
                    break;
            }
        }


        private int FindNearest(bool isEnd)
        {
            double distance = double.MaxValue;
            double tmp;
            int selectedNodeID = -1;

            switch (isEnd)
            {
                case true:
                    foreach (var node in drawInstance.Nodes.Values)
                    {
                        tmp = CalculateDistance(node.PosX, node.PosY, drawInstance.endPoint[0], drawInstance.endPoint[1]);
                        if (tmp < distance)
                        {
                            distance = tmp;
                            selectedNodeID = node.ID;
                        }
                    }
                    break;
                case false:
                    foreach (var node in drawInstance.Nodes.Values)
                    {
                        tmp = CalculateDistance(node.PosX, node.PosY, drawInstance.basePoint[0], drawInstance.basePoint[1]);
                        if (tmp < distance)
                        {
                            distance = tmp;
                            selectedNodeID = node.ID;
                        }
                    }
                    break;
            }

            return selectedNodeID;
        }

        private void DeleteNode(object sender, EventArgs e)
        {
            var selectedNodes = drawInstance.Nodes.Values.Where(node => node.IsSelected).ToList();

            foreach (var node in selectedNodes)
            {
                foreach (var line in Lines.Where(line => line.startNodeID == node.ID || line.endNodeID == node.ID).ToList())
                {
                    Lines.Remove(line);
                    drawInstance.Lines.Remove(line);
                }

                Nodes.Remove(node.ID);
                drawInstance.Nodes.Remove(node.ID);
            }

            Canvas.Invalidate();
        }

        private void SetRoute(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int selectedNodeID = 0;

            var selectedNodes = drawInstance.Nodes.Values.Where(node => node.IsSelected).ToList();
            if (selectedNodes.Count == 1)
            {
                selectedNodeID = selectedNodes.First().ID;
            }
            else return;

            switch (button.Text)
            {
                case "Set Route Start":
                    RoutePath = new Route(0, selectedNodeID, -1);
                    break;

                case "Set Route End":
                    if (RoutePath.StartID != -1)
                    {
                        RoutePath.EndID = selectedNodeID;
                    }
                    break;

                default: break;
            }

            drawInstance.RoutePath = RoutePath;
            Canvas.Invalidate();
        }

    }
}