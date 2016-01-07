using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolType:ToolBase
    {
        protected List<int> Codes { get; set; }
        public ToolType()
        {
           // Init();

            //CreateView = string.Format("Create View {0} As Select * from GYYD Inner Join YDDW On GYYD.QYBH=YDDW.QYBH where GYYD.YDZMJ=GYYD.YKFTDMJ", ViewName);
            //InitView();
        }
        public override void Init(string mdbFilePath)
        {
            base.Init(mdbFilePath);
            var list = GetCodes();
            int temp = 0;
            foreach (var code in list)
            {
                temp = 0;
                if (int.TryParse(code, out temp) && temp != 0)
                {
                    temp = temp / 100;
                    if (!Codes.Contains(temp))
                    {
                        Codes.Add(temp);
                    }
                }
            }
        }
    }
}
