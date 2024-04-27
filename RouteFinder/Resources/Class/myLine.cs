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
        public int startNodeID { get; set; }
        public int endNodeID { get; set; }

        public myLine(int id, int startNodeID, int endNodeID)
        {
            this.ID = id;
            this.startNodeID = startNodeID;
            this.endNodeID = endNodeID;
        }
    }
}
