using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder.Resources.Class
{
    internal class Route
    {
        public int ID { get; set; }
        public int StartID { get; set; }
        public int EndID { get; set; }
        public List<Node> NodePath { get; set; }
        public List<MyLine> LinePath { get; set; }
        public Route(int iD, int startID, int endID)
        {
            this.ID = iD;
            this.StartID = startID;
            this.EndID = endID;
            LinePath = new List<MyLine>();
            NodePath = new List<Node>();
        }
    }
}
