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
            CreateView = string.Format("Create View {0} As Select YDDW.TDZL from GYYD Inner Join YDDW On GYYD.QYBH=YDDW.QYBH", ViewName);
            InitView();
        }

    }
}
