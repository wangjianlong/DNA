using DNA.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolRegion:ToolBase
    {
        protected List<string> Regions { get; set; }

        public ToolRegion()
        {
            this.Regions = MDBHelper.GetAllDistrict();
        }
    }
}
