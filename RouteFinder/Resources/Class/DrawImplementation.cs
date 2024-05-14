using RouteFinder.Resources.Class;

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
        foreach (var line in Lines)
        {
            bool isInShortestPath = RoutePath != null && RoutePath.LinePath.Contains(line);
            canvas.StrokeColor = isInShortestPath ? Colors.Orange : Colors.White;

            if (line.startNodeID != -1 && line.endNodeID != -1 && Nodes.ContainsKey(line.startNodeID) && Nodes.ContainsKey(line.endNodeID))
            {
                // Calculate line length
                double startX = Nodes[line.startNodeID].PosX;
                double startY = Nodes[line.startNodeID].PosY;
                double endX = Nodes[line.endNodeID].PosX;
                double endY = Nodes[line.endNodeID].PosY;
                double length = Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2));
                line.Length = (int)length;

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
        canvas.StrokeColor = Colors.White; // Change the color to yellow for better visibility
        canvas.DrawLine((float)basePoint[0], (float)basePoint[1], (float)endPoint[0], (float)endPoint[1]);

        // Draw nodes
        foreach (var node in Nodes.Values)
        {
            bool isStartNode = RoutePath != null && node.ID == RoutePath.StartID;
            bool isEndNode = RoutePath != null && node.ID == RoutePath.EndID;
            bool isInShortestPath = RoutePath != null && RoutePath.NodePath.Any(n => n.ID == node.ID);

            if (isStartNode || isEndNode)
            {
                canvas.StrokeColor = Colors.Red;
            }
            else if (isInShortestPath)
            {
                canvas.StrokeColor = Colors.Orange;
            }
            else if (node.IsSelected)
            {
                canvas.StrokeColor = Colors.Blue;
            }
            else
            {
                canvas.StrokeColor = Colors.White;
            }

            canvas.FillCircle((float)node.PosX, (float)node.PosY, 20);
            canvas.DrawCircle((float)node.PosX, (float)node.PosY, 22);

            canvas.FontSize = 20;
            canvas.FontColor = Colors.Black;
            canvas.DrawString(node.textValue, (float)node.PosX - 10, (float)node.PosY - 10, 20, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}
