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
        protected List<string> Terraces { get; set; }

        public ToolRegion()
        {
            this.Regions = GetRegions();
            this.Terraces = GetTerraces();
            this.Terraces.Add("其他");
            //CreateView = string.Format("Create View {0} As Select * from GYYD Inner Join YDDW On GYYD.QYBH=YDDW.QYBH where GYYD.YDZMJ=GYYD.YKFTDMJ", ViewName);
            //InitView();
        }

    }
}
