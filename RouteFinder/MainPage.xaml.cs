using Microsoft.Maui.Controls;
using RouteFinder.Resources.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RouteFinder
{
    public partial class MainPage : ContentPage
    {
        readonly DrawImplementation drawInstance;
        internal Dictionary<int, Node> Nodes { get; } = new();
        internal Stack<myLine> Lines { get; } = new();
        int id = 0;
        int lineID = 0;
        double baseX;
        double baseY;

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

            if (tapLocation != null)
            {
                drawInstance.basePoint = new double[] { tapLocation.Value.X, tapLocation.Value.Y };
                drawInstance.endPoint = new double[] { tapLocation.Value.X, tapLocation.Value.Y };
                bool intersects = false;
                foreach (var node in Nodes.Values)
                {
                    double distance = CalculateDistance(tapLocation.Value.X, tapLocation.Value.Y, node.PosX, node.PosY);
                    if (distance <= NodeRadius)
                    {
                        intersects = true;
                        break;
                    }
                }

                if (!intersects)
                {
                    Node nodeInstance = new Node(id, tapLocation.Value.X, tapLocation.Value.Y);
                    Nodes.Add(id, nodeInstance);

                    drawInstance.Size = 2;
                    drawInstance.Nodes[id] = nodeInstance;

                    Canvas.Invalidate();

                    id++;
                }
            }
        }

        private double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private const double NodeRadius = 40;

        private void PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var tappedEventArgs = new TappedEventArgs(e);
            int startNodeID = -1;
            int endNodeID = -1;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    startNodeID = FindNearest(false);
                    if (startNodeID != -1)
                    {
                        drawInstance.basePoint = new double[] { drawInstance.Nodes[startNodeID].PosX, drawInstance.Nodes[startNodeID].PosY };
                        drawInstance.endPoint = drawInstance.basePoint;
                        Canvas.Invalidate();
                    }
                    break;

                case GestureStatus.Running:
                    int id2 = FindNearest(false);
                    if(id2 != -1)
                    {
                        drawInstance.endPoint = new double[] { drawInstance.basePoint[0] + e.TotalX, drawInstance.basePoint[1] + e.TotalY };
                        Canvas.Invalidate();
                    }
                    
                    break;

                case GestureStatus.Completed:
                    // TODO: Fix disappearing lines
                    endNodeID = FindNearest(true);
                    if(endNodeID != -1)
                    {
                        drawInstance.endPoint = new double[] { drawInstance.Nodes[endNodeID].PosX, drawInstance.Nodes[endNodeID].PosY };
                        myLine lineInstance = new myLine(lineID, startNodeID, endNodeID);
                        drawInstance.Lines.Push(lineInstance);
                        Canvas.Invalidate();
                        lineID++;
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

            Debug.WriteLine(selectedNodeID);
            return selectedNodeID;
        }

    }
}