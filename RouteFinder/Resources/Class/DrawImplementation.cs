using Microsoft.Maui.Graphics;
using RouteFinder.Resources.Class;

namespace RouteFinder
{
    internal class DrawImplementation : IDrawable
    {
        public int Size { get; set; }
        public double[] endPoint = { 0, 0 };
        public double[] basePoint = { 0, 0 };
        public Dictionary<int, Node> Nodes { get; } = new();
        public List<myLine> Lines { get; } = new List<myLine>(); 

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.StrokeSize = Size;

            // Draw nodes
            foreach (var node in Nodes.Values)
            {
                canvas.FillCircle((float)node.PosX, (float)node.PosY, 20);
            }

            // Draw lines
            foreach (var line in Lines)
            {
                canvas.StrokeColor = Colors.White;
                canvas.DrawLine((float)line.StartPoint[0], (float)line.StartPoint[1], (float)line.EndPoint[0], (float)line.EndPoint[1]);
            }

            // Draw dynamic line
            canvas.StrokeColor = Colors.White;
            canvas.DrawLine((float)basePoint[0], (float)basePoint[1], (float)endPoint[0], (float)endPoint[1]);
        }
    }
}