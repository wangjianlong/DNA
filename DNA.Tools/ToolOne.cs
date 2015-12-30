 using DNA.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolOne:ToolRegion,ITool
    {
        /// <summary>
        /// 根据区域
        /// </summary>
        public Dictionary<string, DataOne> RegionsDict { get; set; }
        public Dictionary<string, DataOne> TerraceDict { get; set; }
        public string TempView2 { get; set; }
        public DataOne RegionSum { get; set; }
        public DataOne TerraceSum { get; set; }
        private int ValCount { get; set; }
        public ToolOne()
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
            StartRow2 = 22;
            StartCell = 3;
            ValCount = 25;
            TempView2 = "TEMPVIEW2";
            SheetName = "表1";
        }
        public void Working()
        {
            foreach (var region in Regions)
            {
                DataOne one = new DataOne();
                foreach (var val in SFS)
                {
                    queue.Clear();
                    //批准用地面积 总面积 未批准 建筑总面积
                    SQLText = string.Format("Select SUM(GYYD.PZYDMJ),SUM(YDDW.TDZMJ),SUM(GYYD.YDZMJ),SUM(GYYD.JZZMJ),SUM(GYYD.JZZDMJ),SUM(GYYD.WPZJZMJ),SUM(GYYD.WPZJZZDMJ),SUM(GYYD.TDDJMJ),SUM(GYYD.DYMJ),SUM(GYYD.CZQYSL) from {0} where GYYD.XZJDMC={1} AND YDDW.SFGSQY={2}", ViewName, region, val);
                    ExecuteReaderOneToQueue(SQLText);
                    SQLText = string.Format("Select COUNT(*) from YDDW where SFGXQY=是 AND XZJDMC={0} AND SFGSQY={1}", region, val);
                    ExecuteReaderOneToQueue(SQLText);
                    SQLText = string.Format("Select SUM(CYRS),SUM(LJGDZCTZ),SUM(YDL2012),SUM(YDL2013),SUM(YDL2014),SUM(GSRKSS2012),SUM(GSRKSS2013),SUM(GSRKSS2014),SUM(DSRKSS2012),SUM(DSRKSS2013),SUM(DSRKSS2014),SUM(ZYYSR2012),SUM(ZYYSR2013),SUM(ZYYSR2014) from YDDW where  XZJDMC={0} AND SFGSQY={1}", region, val);
                    ExecuteReaderOneToQueue(SQLText);
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
                    queue.Clear();
                    SQLText = string.Format("Select SUM(GYYD.PZYDMJ),SUM(YDDW.TDZMJ),SUM(GYYD.YDZMJ),SUM(GYYD.JZZMJ),SUM(GYYD.JZZDMJ),SUM(GYYD.WPZJZMJ),SUM(GYYD.WPZJZZDMJ),SUM(GYYD.TDDJMJ),SUM(GYYD.DYMJ),SUM(GYYD.CZQYSL) from {0} where GYYD.CYPTMC LIKE '%{1}%' AND YDDW.SFGSQY={2}", ViewName, terrace, val);
                    ExecuteReaderOneToQueue(SQLText);
                    SQLText = string.Format("Select COUNT(*) from {0} inner join YDDW on YDDW.QYBH={0}.QYBH where YDDW.SFGXQY=是 AND SFGSQY={1}", TempView2, val);
                    ExecuteReaderOneToQueue(SQLText);
                    SQLText = string.Format("Select SUM(YDDW.CYRS),SUM(YDDW.LJGDZCTZ),SUM(YDDW.YDL2012),SUM(YDDW.YDL2013),SUM(YDDW.YDL2014),SUM(YDDW.GSRKSS2012),SUM(YDDW.GSRKSS2013),SUM(YDDW.GSRKSS2014),SUM(YDDW.DSRKSS2012),SUM(YDDW.DSRKSS2013),SUM(YDDW.DSRKSS2014),SUM(YDDW.ZYYSR2012),SUM(YDDW.ZYYSR2013),SUM(YDDW.ZYYSR2014) from YDDW inner join {0} on YDDW.QYBH={0}.QYBH where SFGSQY={1}", TempView2, val);
                    ExecuteReaderOneToQueue(SQLText);
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
       
        public void Write(ref ISheet Sheet)
        {
            foreach (var region in RegionsDict.Keys)
            {
                Sheet.GetRow(StartRow).GetCell(1).SetCellValue(region);
                var one = RegionsDict[region];
                WriteBase(one.Up, Sheet, StartRow++, StartCell);
                WriteBase(one.Down, Sheet, StartRow++, StartCell);
            }
            WriteBase(RegionSum, Sheet, StartRow2 - 2, StartCell);
            foreach (var terrace in TerraceDict.Keys)
            {
                Sheet.GetRow(StartRow2).GetCell(1).SetCellValue(terrace);
                var one = TerraceDict[terrace];
                WriteBase(one.Up, Sheet, StartRow2++, StartCell);
                WriteBase(one.Down, Sheet, StartRow2++, StartCell);
            }
            WriteBase(TerraceSum, Sheet, 32, StartCell);
        }
        protected DataBase Translate(Queue<string> queue)
        {
            
            DataBase database = new DataBase();
            if (queue.Count == ValCount)
            {
                System.Reflection.PropertyInfo[] propList = typeof(DataBase).GetProperties();
                foreach (var item in propList)
                {
                    if (item.Name == "GYYD")
                    {
                        continue;
                    }
                    if (item.PropertyType.Equals(typeof(double)))
                    {
                        item.SetValue(database, double.Parse(queue.Dequeue()), null);
                    }
                    else if (item.PropertyType.Equals(typeof(int)))
                    {
                        item.SetValue(database, int.Parse(queue.Dequeue()), null);
                    }
                }
            }
            return database;
        }

    }
}
