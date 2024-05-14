using Microsoft.Maui.Controls;
using RouteFinder.Resources.Class;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Animations;

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
            RoutePath = new Route(0, -1, -1);
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
            if (tapLocation is null) return;

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
                Node nodeInstance = new Node(id, tapLocation.Value.X, tapLocation.Value.Y, rand.Next(1, 10).ToString());
                Nodes.Add(id, nodeInstance);

                if (id > 0 && drawInstance.Nodes.ContainsKey(id - 1)) drawInstance.Nodes[id - 1].IsSelected = false;

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

                        foreach (var line in drawInstance.Lines)
                        {
                            if (lineInstance.startNodeID == line.startNodeID && lineInstance.endNodeID == line.endNodeID)
                            {
                                isExisting = true;
                            }
                        }

                        if (!isExisting) drawInstance.Lines.Add(lineInstance);
                        Canvas.Invalidate();

                        lineID++;
                        drawInstance.basePoint = new double[] { 0, 0 };
                        drawInstance.endPoint = new double[] { 0, 0 };
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
                    RoutePath.StartID = selectedNodeID;
                    break;

                case "Set Route End":

                        RoutePath.EndID = selectedNodeID;
                    
                    break;

                default: break;
            }

            drawInstance.RoutePath = RoutePath;
            Canvas.Invalidate();
        }

        private void FindShortestRoute(object sender, EventArgs e)
        {
            // Reset previous route
            RoutePath.LinePath.Clear();
            RoutePath.NodePath.Clear();

            // Check if start and end nodes are set
            if (RoutePath.StartID == -1 || RoutePath.EndID == -1)
            {
                // Start or end node not set, return
                return;
            }

            // Initialize distances and previous nodes
            Dictionary<int, double> distances = new Dictionary<int, double>();
            Dictionary<int, int> previous = new Dictionary<int, int>();
            foreach (var node in Nodes.Values)
            {
                distances[node.ID] = double.MaxValue;
                previous[node.ID] = -1;
            }
            distances[RoutePath.StartID] = 0;

            // Dijkstra's algorithm
            HashSet<int> visited = new HashSet<int>();
            while (visited.Count < Nodes.Count)
            {
                // Find the closest node not yet visited
                int closestNode = -1;
                double shortestDistance = double.MaxValue;
                foreach (var node in Nodes.Values)
                {
                    if (!visited.Contains(node.ID) && distances[node.ID] < shortestDistance)
                    {
                        closestNode = node.ID;
                        shortestDistance = distances[node.ID];
                    }
                }

                // If no reachable nodes left or reached end node
                if (closestNode == -1 || closestNode == RoutePath.EndID)
                {
                    break;
                }

                visited.Add(closestNode);

                // Update distances to neighbors
                foreach (var line in Lines.Where(l => l.startNodeID == closestNode))
                {
                    double distanceToNeighbor = distances[closestNode] + line.Length;
                    if (distanceToNeighbor < distances[line.endNodeID])
                    {
                        distances[line.endNodeID] = distanceToNeighbor;
                        previous[line.endNodeID] = closestNode;
                    }
                }
            }

            // Reconstruct shortest path
            List<int> shortestPath = new List<int>();
            int currentNode = RoutePath.EndID;
            while (currentNode != -1 && currentNode != RoutePath.StartID)
            {
                shortestPath.Insert(0, currentNode);
                currentNode = previous[currentNode];
            }
            shortestPath.Insert(0, RoutePath.StartID);

            // Add shortest path to RoutePath
            for (int i = 0; i < shortestPath.Count - 1; i++)
            {
                int startNodeID = shortestPath[i];
                int endNodeID = shortestPath[i + 1];
                foreach (var line in Lines)
                {
                    if (line.startNodeID == startNodeID && line.endNodeID == endNodeID)
                    {
                        RoutePath.LinePath.Add(line);
                        break;
                    }
                }
            }

            // Add nodes to RoutePath
            foreach (var nodeId in shortestPath)
            {
                if (Nodes.ContainsKey(nodeId))
                {
                    RoutePath.NodePath.Add(Nodes[nodeId]);
                }
            }

            // Update UI
            Canvas.Invalidate();
        }


    }
}
