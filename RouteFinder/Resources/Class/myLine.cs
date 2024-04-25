using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteFinder.Resources.Class
{
    internal class myLine
    {
        // change from start/end points to node id's 
        public int ID { get; set; }
        public double[] StartPoint { get; set; }
        public double[] EndPoint { get; set; }

        public myLine(int id, double[] startPoint, double[] endPoint)
        {
            this.ID = id;
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
    }
}
