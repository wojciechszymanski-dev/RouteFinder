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
        public int[] Path { get; set; }
        public Route(int iD, int startID, int endID /*,int[] path*/)
        {
            this.ID = iD;
            this.StartID = startID;
            this.EndID = endID;
            //this.Path = path;
        }
    }
}
