using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using RouteFinder.Resources.Class;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace RouteFinder
{
    internal class DrawImplementation : IDrawable
    {
        public int Size { get; set; }
        public double[] endPoint = { 0, 0 };
        public double[] basePoint = { 0, 0 };
        public Dictionary<int, Node> Nodes { get; } = new();
        public List<MyLine> Lines { get; } = new(); 
        public Route RoutePath;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.StrokeSize = 5;

            // Draw lines and labels
            canvas.StrokeColor = Colors.White;
            foreach (var line in Lines)
            {
                if (line.startNodeID != -1 && line.endNodeID != -1 && Nodes.ContainsKey(line.startNodeID) && Nodes.ContainsKey(line.endNodeID))
                {
                    // Calculate line length
                    double startX = Nodes[line.startNodeID].PosX;
                    double startY = Nodes[line.startNodeID].PosY;
                    double endX = Nodes[line.endNodeID].PosX;
                    double endY = Nodes[line.endNodeID].PosY;
                    double length = Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2));

                    canvas.DrawLine((float)startX, (float)startY, (float)endX, (float)endY);

                    double textX = (startX + endX) / 2;
                    double textY = (startY + endY) / 2;
                    string text = $"{length:0}";

                    // Draw length on the lines
                    canvas.FontSize = 20;
                    canvas.FontColor = Colors.Black;
                    canvas.DrawString(text, (float)textX, (float)textY, HorizontalAlignment.Center);
                }
            }

            // Draw dynamic line
            canvas.StrokeColor = Colors.White;
            canvas.DrawLine((float)basePoint[0], (float)basePoint[1], (float)endPoint[0], (float)endPoint[1]);

            // Draw nodes
            foreach (var node in Nodes.Values)
            {
                if (RoutePath != null && (node.ID == RoutePath.StartID || node.ID == RoutePath.EndID))
                {
                    canvas.StrokeColor = Colors.Red;
                    canvas.DrawCircle((float)node.PosX, (float)node.PosY, 22);
                }
                if (node.IsSelected)
                {
                    canvas.StrokeColor = Colors.Blue;
                    canvas.DrawCircle((float)node.PosX, (float)node.PosY, 22);
                }
                else
                {
                    canvas.StrokeColor = Colors.White;
                }

                canvas.FillCircle((float)node.PosX, (float)node.PosY, 20);

                canvas.FontSize = 20;
                canvas.FontColor = Colors.Black;
                canvas.DrawString(node.textValue, (float)node.PosX - 10, (float)node.PosY - 10, 20, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }
    }
}