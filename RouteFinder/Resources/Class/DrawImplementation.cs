using Microsoft.Maui.Graphics;
using RouteFinder.Resources.Class;
using System.Diagnostics;

namespace RouteFinder
{
    internal class DrawImplementation : IDrawable
    {
        public int Size { get; set; }
        public double[] endPoint = { 0, 0 };
        public double[] basePoint = { 0, 0 };
        public Dictionary<int, Node> Nodes { get; } = new();
        public Stack<myLine> Lines { get; } = new(); 

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.StrokeSize = Size;

            // Draw nodes
            foreach (var node in Nodes.Values)
            {
                canvas.FillCircle((float)node.PosX, (float)node.PosY, 20);
            }

            // Draw dynamic line
            canvas.StrokeColor = Colors.White;
            canvas.DrawLine((float)basePoint[0], (float)basePoint[1], (float)endPoint[0], (float)endPoint[1]);

            // Draw lines
            foreach (var line in Lines.Reverse())
            {
                Debug.WriteLine(line.ID);
                if (line.startNodeID != -1 && line.endNodeID != -1)
                {
                    canvas.DrawLine((float)Nodes[line.startNodeID].PosX, (float)Nodes[line.startNodeID].PosY, (float)Nodes[line.endNodeID].PosX, (float)Nodes[line.endNodeID].PosY);
                }
            }
        }
    }
}