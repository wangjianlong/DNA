using DNA.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class MainTool:ToolBase
    {
        public List<GYDW> List { get; set; }
        public Dictionary<string, Merge> Dict { get; set; }//地块编号->  地块信息
        public Dictionary<string, TempData> TempDict { get; set; }//企业编号->  有几块地  总共的建筑面积
        public MainTool()
        {
            Dict = new Dictionary<string, Merge>();
            StartRow = 1;
            StartCell = 0;
        }

        protected MergeBase Translate(Queue<string> queue,int ValCount,bool Flag=false)
        {
            MergeBase database = new MergeBase();
            System.Reflection.PropertyInfo[] propList = typeof(MergeBase).GetProperties();
            string temp = string.Empty;
            if (queue.Count == ValCount)
            {
                foreach (var item in propList)
                {
                    if (item.Name == "GYYD")
                    {
                        continue;
                    }
                    temp = queue.Dequeue();
                    if (!string.IsNullOrEmpty(temp))
                    {
                        if (item.PropertyType.Equals(typeof(double)))
                        {
                            double val;
                            if (double.TryParse(temp, out val))
                            {
                                item.SetValue(database, val, null);
                            }
                            else
                            {
                                Console.WriteLine(temp);
                            }
                            
                        }
                        else if (item.PropertyType.Equals(typeof(int)))
                        {
                            int m = 0;
                            if (int.TryParse(temp, out m))
                            {
                                item.SetValue(database, m, null);
                            }
                            else
                            {
                                Console.WriteLine(temp);
                            }
                            
                        }
                        else if (item.PropertyType.Equals(typeof(bool)) && !Flag)
                        {
                            switch (temp)
                            {
                                case "是":
                                    item.SetValue(database, true, null);
                                    break;
                                case "否":
                                    item.SetValue(database, false, null);
                                    break;
                            }
                        }
                    }

                }
            }
            
            return database;
        }
        public List<GYDW> Get(string SQLCommandText)
        {
            TempDict = GetDict();
            List<GYDW> List = new List<GYDW>();
            string QYBH = string.Empty;
            double JZMJ=0.0;
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLCommandText;
                    var reader = Command.ExecuteReader();
                    while (reader.Read())
                    {
                        QYBH = reader[1].ToString();
                        if (double.TryParse(reader[2].ToString(), out JZMJ))
                        {
                            var percent = .0;
                            if (TempDict.ContainsKey(QYBH))
                            {
                                var tempdata = TempDict[QYBH];
                                if (tempdata.Sum>0)
                                {
                                    percent = JZMJ / TempDict[QYBH].Sum;
                                }
                                else
                                {
                                    percent = ((double)1) / ((double)tempdata.Count);
                                }

                                if (tempdata.Count >= 2)
                                {
                                    Console.WriteLine(QYBH);
                                }
                            }
                            if (double.IsNaN(percent))
                            {
                                percent = .0;
                            }
                            List.Add(new GYDW()
                            {
                                DKBH = reader[0].ToString(),
                                QYBH = QYBH,
                                JZMJ = JZMJ,
                                Percent = percent
                            });
                        }
                    }
                }
                Connection.Close();
            }
            return List;
        }
        public TempData GetTwo(string SQLCommandText)
        {
            TempData data =null;
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLCommandText;
                    var reader = Command.ExecuteReader();
                    if (reader.Read())
                    {
                        data = new TempData()
                        {
                            Sum = double.Parse(reader[0].ToString()),
                            Count = int.Parse(reader[1].ToString())
                        };
                    }
                }
                Connection.Close();
            }
            return data;
        }
        public HY GetHY(string SQLCommandText)
        {
            HY hy = null;
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLCommandText;
                    var reader = Command.ExecuteReader();
                    if (reader.Read())
                    {
                        hy = new HY()
                        {
                            HYLB = reader[0].ToString(),
                            HYDM = reader[1].ToString()
                        };
                    }
                }
                Connection.Close();
            }
            return hy;
        }

        public Dictionary<string, TempData> GetDict()
        {
            var dict = new Dictionary<string, TempData>();
            var list=GetBase("Select QYBH from GYYD_YDDW Group By QYBH");
            foreach (var qybh in list)
            {
                dict.Add(qybh, GetTwo(string.Format("Select SUM(JZMJ),COUNT(*) from GYYD_YDDW where QYBH='{0}'", qybh)));
            }
            return dict;
        }
        public void Doing()
        {
            this.List = Get("Select DKBH,QYBH,JZMJ from GYYD_YDDW");
            //DD(this.List);
            var DKBHS = GetBase("Select DKBH from GYYD Group By DKBH");
            foreach (var dkbh in DKBHS)
            {
                var tempList = this.List.Where(e => e.DKBH == dkbh).ToList();
                var merge = new Merge()
                {
                    DKBH = dkbh,
                    CZQYSL=tempList.Count,
                    Base=new MergeBase()
                };
                foreach (var gydw in tempList)
                {
                    if (merge.Hy == null)
                    {
                        merge.Hy = GetHY(string.Format("Select HYLB, HYDM from YDDW where QYBH='{0}'", gydw.QYBH));
                    }
                    ReadData(new string[]{
                        string.Format("Select SFGXQY,CYRS,LJGDZCTZ,YDL2012,YDL2013,YDL2014,GSRKSS2012,GSRKSS2013,GSRKSS2014,DSRKSS2012,DSRKSS2013,DSRKSS2014,ZYYSR2012,ZYYSR2013,ZYYSR2014,TDZMJ,SFGSQY from YDDW where QYBH='{0}'", gydw.QYBH)
                    });
                    var mergebase = Translate(queue,17);
                    
                    if (!merge.SFGSQY && mergebase.SFGSQY)
                    {
                        merge.SFGSQY = true;
                    }
                    if (mergebase.SFGXQY)
                    {
                        merge.SFGXQY++;
                    }
                    MergeBase margin = null;
                    if (TempDict.ContainsKey(gydw.QYBH))
                    {
                        var body = TempDict[gydw.QYBH];
                        if (body.merge == null)
                        {
                            TempDict[gydw.QYBH].merge = mergebase;
                        }
                        if (body.Count == 1)
                        {
                            margin = body.merge - body.Reduce;
                        }
                        else
                        {
                            TempDict[gydw.QYBH].Count--;
                            margin = mergebase * gydw.Percent;
                            TempDict[gydw.QYBH].Reduce = body.Reduce+margin;
                        }
                    }
                    if (margin != null)
                    {
                        merge.Base = merge.Base + margin;
                       
                    }
                    //merge.Base = merge.Base + mergebase * gydw.Percent;
                    if (double.IsNaN(merge.Base.DSRKSS2012))
                    {
                        Console.WriteLine("");
                    }
                }
                Dict.Add(dkbh, merge);
            }
            foreach (var key in Dict.Keys)
            {
                var tem=Dict[key];
                var boolstr = tem.SFGSQY ? "是" : "否";
                SQLText = string.Format("UPDATE GYYD SET LJGDZCTZ={0},ZYYSR2012={1},ZYYSR2013={2},ZYYSR2014={3},DSRKSS2012={4},DSRKSS2013={5},DSRKSS2014={6},GSRKSS2012={7},GSRKSS2013={8},GSRKSS2014={9},YDL2012={10},YDL2013={11},YDL2014={12},CYRS={13},TDZMJ={14},SFGXQY={15},SYDKS=1,SFGSQY='{16}',HYLB='{17}',HYDM='{18}' where DKBH='{19}'", tem.Base.LJGDZCTZ, tem.Base.ZYYSR2012, tem.Base.ZYYSR2013, tem.Base.ZYYSR2014, tem.Base.DSRKSS2012, tem.Base.DSRKSS2013, tem.Base.DSRKSS2014, tem.Base.GSRKSS2012, tem.Base.GSRKSS2013, tem.Base.GSRKSS2014, tem.Base.YDL2012, tem.Base.YDL2013, tem.Base.YDL2014, tem.Base.CYRS,tem.Base.TDZMJ,tem.SFGXQY,boolstr,tem.Hy.HYLB,tem.Hy.HYDM, tem.DKBH);
                try
                {
                    ExecuteQuery(SQLText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }
            Console.WriteLine("成功");
        }

        public void DD(List<GYDW> List)
        {
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("CESHI");
            int Line = 1;
            foreach (var gydw in List)
            {
                IRow row = sheet.CreateRow(Line++);
                row.CreateCell(0).SetCellValue(gydw.QYBH);
                row.CreateCell(1).SetCellValue(gydw.DKBH);
                row.CreateCell(2).SetCellValue(gydw.JZMJ);
                row.CreateCell(3).SetCellValue(gydw.Percent);
            }
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "1.xls");
            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
        }

        public void Write(ref ISheet Sheet)
        {
            int Line=0;
            foreach (var key in Dict.Values)
            {
                Line=StartCell;
                IRow row = Sheet.GetRow(StartRow);
                if (row == null)
                {
                    row = Sheet.CreateRow(StartRow);
                }
                ICell cell = row.GetCell(Line);
                if (cell == null)
                {
                    cell = row.CreateCell(Line);
                }
                Line++;
                cell.SetCellValue(key.DKBH);
                row.CreateCell(Line++).SetCellValue(key.CZQYSL);
                row.CreateCell(Line++).SetCellValue(key.SFGXQY);
                WriteBase2(key.Base, Sheet, StartRow, Line++);
            }
        }

        public void WriteBase2<T>(T Data,ISheet Sheet,int Row,int Line)
        {
            System.Reflection.PropertyInfo[] propList = typeof(T).GetProperties();
            double val = 0.0;
            int Values = 0;
            IRow row = Sheet.GetRow(Row);
            if (row != null)
            {
                foreach (var item in propList)
                {
                    if (item.PropertyType.Equals(typeof(double)))
                    {
                        if (double.TryParse(item.GetValue(Data, null).ToString(), out val))
                        {
                            row.CreateCell(Line).SetCellValue(Math.Round(val, 2));
                           // row.GetCell(Line).SetCellValue(Math.Round(val, 2));
                        }
                    }
                    else if (item.PropertyType.Equals(typeof(int)))
                    {
                        if (int.TryParse(item.GetValue(Data, null).ToString(), out Values))
                        {
                            row.CreateCell(Line).SetCellValue(Values);
                            //row.GetCell(Line).SetCellValue(Values);
                        }
                    }
                    else
                    {
                        row.CreateCell(Line).SetCellValue(item.GetValue(Data, null).ToString());
                    }
                    Line++;
                }
            }
        }
        //public void Working()
        //{
        //    this.List = Get("Select DKBH,QYBH,JZMJ from GYYD_YDDW");
        //    var DKBHS = GetBase("Select DKBH from GYYD Group By DKBH");
        //    foreach (var dkbh in DKBHS)
        //    {
        //        SQLText = string.Format("Select DKBH,QYBH,JZMJ from GYYD_YDDW where DKBH={0}", dkbh);
        //        var list = Get(SQLText);
        //        if (list.Count > 0)
        //        {
        //            MergeBase mergebase = null;
        //            if (list.Count == 1)
        //            {
        //                SQLText = string.Format("Select SFGXQY,CYRS,LJGDZCTZ,YDL2012,YDL2013,YDL2014,GSRKSS2012,GSRKSS2013,GSRKSS2014,DSRKSS2012,DSRKSS2013,DSRKSS2014,ZYYSR2012,ZYYSR2013,ZYYSR2014 from YDDW where QYBH={0}", dkbh);
        //                ReadData(new string[]{
        //                    SQLText
        //                });
        //                mergebase = Translate(queue);
        //                SQLText = string.Format("Select DKBH,QYBH,JZMJ from GYYD_YDDW where QYBH={0}", list[0].QYBH);
        //                var list2 = Get(SQLText);
        //                if (list2.Count == 1)//一地一企
        //                {
                            
        //                }
        //                else//一企多地
        //                {
        //                    var areasum = double.Parse(GetOneBase(string.Format("Select SUM(JZMJ) from GYYD_YDDW where QYBH={0}", list[0].QYBH)));
        //                    if (areasum > 0)
        //                    {
        //                        areasum = list[0].JZMJ / areasum;
        //                        mergebase = mergebase * areasum;
        //                    }
        //                }
        //            }
        //            else//一地多企   求
        //            {
        //                ReadData(new string[]{
        //                    string.Format("Select COUNT(*) from YDDW inner join GYYD_YDDW on YDDW.QYBH=GYYD_YDDW.QYBH where GYYD_YYDW.DKBH={0} AND YDDW.SFGXQY=是",dkbh),
        //                    string.Format("Select SUM(YDDW.CYRS),SUM(YDDW.LJGDZCTZ),SUM(YDDW.YDL2012),SUM(YDDW.YDL2013),SUM(YDDW.YDL2014),SUM(YDDW.GSRKSS2012),SUM(YDDW.GSRKSS2013),SUM(YDDW.GSRKSS2014),SUM(YDDW.DSRKSS2012),SUM(YDDW.DSRKSS2013),SUM(YDDW.DSRKSS2014),SUM(YDDW.ZYYSR2012),SUM(YDDW.ZYYSR2013),SUM(YDDW.ZYYSR2014) from GYYD_YDDW inner join YDDW on GYYD_YDDW.QYBH=YDDW.QYBH where GYYD_YDDW.DKBH={0}",dkbh)
        //                });
        //            }
        //            if (mergebase != null)
        //            {
        //                var merge = new Merge()
        //                {
        //                    CZQYSL = 1,
        //                    SFGXQY = mergebase.SFGXQY ? 1 : 0,
        //                    Base = mergebase
        //                };
        //            }
                    

        //        }
        //    }
        //}
    }
}
