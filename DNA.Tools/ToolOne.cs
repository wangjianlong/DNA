 using DNA.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolOne : ToolRegion, ITool
    {
        public static string CurrentName
        {
            get
            {
                return "表1工业用地（已建成）区域汇总表.xls";
            }
        }
        /// <summary>
        /// 根据区域
        /// </summary>
        public Dictionary<string, DataOne> RegionsDict { get; set; }
        public Dictionary<string, DataOne> TerraceDict { get; set; }
        public string TempView2 { get; set; }
        public DataOne RegionSum { get; set; }
        public DataOne TerraceSum { get; set; }
        public ToolOne(string mdbFilePath)
        {
            RegionsDict = new Dictionary<string, DataOne>();
            TerraceDict = new Dictionary<string, DataOne>();
            RegionSum = new DataOne()
            {
                Up = new DataBase(),
                Down = new DataBase()
            };
            TerraceSum = new DataOne()
            {
                Up = new DataBase(),
                Down = new DataBase()
            };
            StartRow = 4;
            StartRow2 = 46;
            StartCell = 3;
            ValCount = 25;
            TempView2 = "TEMPVIEW2";
            SheetName = "表1";
            Init(mdbFilePath);
        }

        public void Doing()
        {
            #region  获取 乡镇
            foreach (var region in Regions)
            {
                DataOne one = new DataOne();
                foreach (var val in SFS)
                {
                    //SQLText = string.Format("Select SUM(PZYDMJ),SUM(YDZMJ),SUM(WJPZYDMJ),SUM(JZZMJ),SUM(JZZDMJ),SUM(WPZJZMJ),SUM(WPZJZZDMJ),SUM(TDDJMJ),SUM(DYMJ),SUM(CZQYSL),SUM(SFGXQY),SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from GYYD where XZJDMC='{0}' AND TDSYQK='1' AND SFGSQY='{1}'", region, val);
                    //ReadData(new string[] { SQLText });

                    ReadData(new string[]{
                        string.Format("Select SUM(PZYDMJ),SUM(YDZMJ),SUM(WJPZYDMJ),SUM(JZZMJ),SUM(JZZDMJ),SUM(WPZJZMJ),SUM(WPZJZZDMJ),SUM(TDDJMJ),SUM(DYMJ),SUM(CZQYSL) from GYYD_YDDW where XZQMC='{0}' AND TDSYQK='1' AND SFGSQY='{1}'", region, val),
                        string.Format("Select Count(*) from GYYD_YDDW where XZQMC='{0}' AND TDSYQK='1' AND SFGSQY='{1}' AND SFGXQY='是'", region, val),
                        string.Format("Select SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from GYYD_YDDW where XZQMC='{0}' AND TDSYQK='1' AND SFGSQY='{1}'",region,val)
                    });
                    var database = Translate(queue)/10000;
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
                RegionsDict.Add(region, one);
                RegionSum = RegionSum + one;
            }
            #endregion

            #region  获取  工业园
            foreach (var terrace in Terraces)
            {
                DataOne one = new DataOne();
                foreach (var val in SFS)
                {
                    string str = string.Empty;
                    if (terrace == "其他")
                    {
                        str = string.Format("from GYYD_YDDW where SFWYCYPT='否' AND TDSYQK='1' AND SFGSQY='{0}'", val);
                        //SQLText = string.Format("Select SUM(PZYDMJ),SUM(YDZMJ),SUM(WJPZYDMJ),SUM(JZZMJ),SUM(JZZDMJ),SUM(WPZJZMJ),SUM(WPZJZZDMJ),SUM(TDDJMJ),SUM(DYMJ),SUM(CZQYSL),SUM(SFGXQY),SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from GYYD where SFWYCYPT='否' AND TDSYQK='1' AND SFGSQY='{0}'", val);
                    }
                    else
                    {
                        str = string.Format("from GYYD_YDDW where CYPTMC Like '%{0}%' AND TDSYQK='1' AND SFGSQY='{1}'",terrace, val);
                        //SQLText = string.Format("Select SUM(PZYDMJ),SUM(YDZMJ),SUM(WJPZYDMJ),SUM(JZZMJ),SUM(JZZDMJ),SUM(WPZJZMJ),SUM(WPZJZZDMJ),SUM(TDDJMJ),SUM(DYMJ),SUM(CZQYSL),SUM(SFGXQY),SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from GYYD where CYPTMC Like '%{0}%' AND TDSYQK='1' AND SFGSQY='{1}'", terrace, val);
                    }
                   
                    //ReadData(new string[] { SQLText });
                    ReadData(new string[]{
                        string.Format("Select SUM(PZYDMJ),SUM(YDZMJ),SUM(WJPZYDMJ),SUM(JZZMJ),SUM(JZZDMJ),SUM(WPZJZMJ),SUM(WPZJZZDMJ),SUM(TDDJMJ),SUM(DYMJ),SUM(CZQYSL) {0}",str),
                        string.Format("Select Count(*) {0} AND SFGXQY='是'",str),
                        string.Format("Select SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) {0}",str)
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
                TerraceDict.Add(terrace, one);
                TerraceSum = TerraceSum + one;
            }
            #endregion
        }
        public void Working()
        {
            foreach (var region in Regions)
            {
                DataOne one = new DataOne();
                foreach (var val in SFS)
                {
                    ReadData(new string[]{
                        string.Format("Select SUM(GYYD.PZYDMJ),SUM(YDDW.TDZMJ),SUM(GYYD.YDZMJ),SUM(GYYD.JZZMJ),SUM(GYYD.JZZDMJ),SUM(GYYD.WPZJZMJ),SUM(GYYD.WPZJZZDMJ),SUM(GYYD.TDDJMJ),SUM(GYYD.DYMJ),SUM(GYYD.CZQYSL) from {0} where GYYD.XZJDMC={1} AND YDDW.SFGSQY={2}", ViewName, region, val),
                        string.Format("Select * from YDDW inner join GYYD_YDDW on "),
                        string.Format("Select COUNT(*) from YDDW where SFGXQY=是 AND XZJDMC={0} AND SFGSQY={1}", region, val),
                        string.Format("Select SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from YDDW where  XZJDMC={0} AND SFGSQY={1}", region, val)
                    });
                    DataBase databse = Translate(queue);
                    switch (val)
                    {
                        case "是":
                            one.Up = databse;
                            break;
                        case "否":
                            one.Down = databse;
                            break;
                    }
                }
                RegionsDict.Add(region, one);
                RegionSum = RegionSum + one;
            }

            foreach (var terrace in Terraces)
            {
                DataOne one = new DataOne();
                var list = GetBase(string.Format("Select DKBH from GYYD where CYPTMC Like '%{0}%'", terrace));
                SQLText = string.Format("Create View {0} As Select GYYD_YDDW.QYBH As QYBH from GYYD inner join GYYD_YDDW on GYYD.DKBH=GYYD_YDDW.DKBH where GYYD.CYPTMC Like '%{1}%' Group By GYYD_YDDW.QYBH", TempView2, terrace);
                ExecuteQuery(SQLText);
                foreach (var val in SFS)
                {
                    ReadData(new string[]{
                        string.Format("Select SUM(GYYD.PZYDMJ),SUM(YDDW.TDZMJ),SUM(GYYD.YDZMJ),SUM(GYYD.JZZMJ),SUM(GYYD.JZZDMJ),SUM(GYYD.WPZJZMJ),SUM(GYYD.WPZJZZDMJ),SUM(GYYD.TDDJMJ),SUM(GYYD.DYMJ),SUM(GYYD.CZQYSL) from {0} where GYYD.CYPTMC LIKE '%{1}%' AND YDDW.SFGSQY={2}", ViewName, terrace, val),
                        string.Format("Select COUNT(*) from {0} inner join YDDW on YDDW.QYBH={0}.QYBH where YDDW.SFGXQY=是 AND SFGSQY={1}", TempView2, val),
                        string.Format("Select SUM(YDDW.CYRS),SUM(YDDW.LJGDZCTZ),SUM(YDDW.YDL2012),SUM(YDDW.YDL2013),SUM(YDDW.YDL2014),SUM(YDDW.GSRKSS2012),SUM(YDDW.GSRKSS2013),SUM(YDDW.GSRKSS2014),SUM(YDDW.DSRKSS2012),SUM(YDDW.DSRKSS2013),SUM(YDDW.DSRKSS2014),SUM(YDDW.ZYYSR2012),SUM(YDDW.ZYYSR2013),SUM(YDDW.ZYYSR2014) from YDDW inner join {0} on YDDW.QYBH={0}.QYBH where SFGSQY={1}", TempView2, val)
                    });
                    DataBase database = Translate(queue);
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
                SQLText = string.Format("Drop View {0}", TempView2);
                ExecuteQuery(SQLText);
                TerraceDict.Add(terrace, one);
                TerraceSum = TerraceSum + one;
            }
        }

        public void execute(string SQLCommandText)
        {
            var list = GetBase(SQLCommandText);
            foreach (var qybh in list)
            {

            }
        }


        public void Write(ref ISheet Sheet)
        {
            foreach (var region in RegionsDict.Keys)
            {
                Sheet.GetRow(StartRow).GetCell(1).SetCellValue(region);
                var one = RegionsDict[region];
                WriteBase(one.Up, Sheet, StartRow++, StartCell);
                WriteBase(one.Down, Sheet, StartRow++, StartCell);
            }
            WriteBase(RegionSum.Up, Sheet, StartRow2 - 2, StartCell);
            WriteBase(RegionSum.Down, Sheet, StartRow2 - 1, StartCell);
            foreach (var terrace in TerraceDict.Keys)
            {
                Sheet.GetRow(StartRow2).GetCell(1).SetCellValue(terrace);
                var one = TerraceDict[terrace];
                WriteBase(one.Up, Sheet, StartRow2++, StartCell);
                WriteBase(one.Down, Sheet, StartRow2++, StartCell);
            }
            WriteBase(TerraceSum.Up, Sheet, 86, StartCell);
            WriteBase(TerraceSum.Down, Sheet, 87, StartCell);
        }
        public string GetCurrentName()
        {
            return CurrentName;
        }


    }
}
