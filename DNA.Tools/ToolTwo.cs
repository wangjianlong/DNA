using DNA.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolTwo:ToolType,ITool
    {

        public Dictionary<int, DataOne> Dict { get; set; }
        public DataOne Sum { get; set; }
        public ToolTwo(string mdbFilePath)
        {
            Dict = new Dictionary<int, DataOne>();
            Sum = new DataOne()
            {
                Up = new DataBase(),
                Down = new DataBase()
            };
            StartRow = 4;
            StartCell = 3;
            StartRow2 = 66;
            SheetName = "表2";
            Init(mdbFilePath);
        }
        public void Doing()
        {
            Working();
        }
        public void Working()
        {
            foreach (var type in Codes)
            {
                DataOne one = new DataOne();
                foreach (var val in SFS)
                {
                    ReadData(new string[]{
                        string.Format("Select SUM(PZYDMJ),SUM(YDZMJ),SUM(WJPZYDMJ),SUM(JZZMJ),SUM(JZZDMJ),SUM(WPZJZMJ),SUM(WPZJZZDMJ),SUM(TDDJMJ),SUM(DYMJ),SUM(CZQYSL) from GYYD_YDDW where HYDM={0} AND SFGSQY='{1}' AND TDSYQK='1'",type, val),
                        string.Format("Select COUNT(*) from GYYD_YDDW where SFGXQY='是' AND HYDM={0} AND SFGSQY='{1}' AND TDSYQK='1'", type, val),
                        string.Format("Select SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from GYYD_YDDW where  HYDM={0} AND SFGSQY='{1}' AND TDSYQK='1'", type, val)
                    });
                    DataBase database = Translate(queue)/10000;
                    switch (val)
                    {
                        case "是":
                            one.Up = database;
                            break;
                        case "否":
                            one.Down = database;
                            break;
                    }
                }
                Dict.Add(type, one);
                Sum = Sum + one;

            }
        }
        public void Write(ref ISheet Sheet)
        {
            int TempCode = 0;
            for (var i = StartRow; i < StartRow2; i = i + 2)
            {
                if (int.TryParse(Sheet.GetRow(i).GetCell(0).ToString(), out TempCode))
                {
                    if (Dict.ContainsKey(TempCode))
                    {
                        var one = Dict[TempCode];
                        WriteBase(one.Up, Sheet, i, StartCell);
                        WriteBase(one.Down, Sheet, i + 1, StartCell);
                    }
                }
            }
            WriteBase(Sum.Up, Sheet, StartRow2, StartCell);
            WriteBase(Sum.Down, Sheet, StartRow2 + 1, StartCell);
        }
    }
}
