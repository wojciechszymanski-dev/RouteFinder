using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder.Resources.Class
{
    internal class Node
    {
        public int ID { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public bool IsSelected { get; set; }
        public string textValue {  get; set; }

        public Node(int id, double posX, double posY, string textValue)
        {
            this.ID = id;
            this.PosX = posX;
            this.PosY = posY;
            this.IsSelected = false;
            this.textValue = textValue;
        }
    }
}
