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
        public bool isSelected { get; set; }

        public Node(int id, double posX, double posY)
        {
            this.ID = id;
            this.PosX = posX;
            this.PosY = posY;
            this.isSelected = false;
        }
    }
}
