using DNA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolOne:ToolRegion,ITool
    {
        public Dictionary<string, DataOne> Dict { get; set; }
        public ToolOne()
        {
            Dict = new Dictionary<string, DataOne>();
        }

        public void Working()
        {
            foreach (var region in Regions)
            {
                string SQL = string.Format("Select * from GYYD where XZJDMC={0}")
            }
        }

        public void GetLandUse(string SQLCommand)
        {
            
        }

    }
}
