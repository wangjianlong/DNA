using DNA.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolFive:ToolRegion,ITool
    {
        public static string CurrentName
        {
            get
            {
                return "表5工业用地（已建成）利用潜力区域汇总表.xls";
            }
        }
        public Dictionary<string, PotentialFive> PotentialDict { get; set; }//乡镇
        public Dictionary<string, PotentialFive> FPotentialDict { get; set; }
        public PotentialFive PotentialSum { get; set; }
        public PotentialFive FPotentialSum { get; set; }
        public ToolFive(string mdbFilePath)
        {
            Init(mdbFilePath);
            SheetName = "表5";
            StartRow = 3;
            StartCell = 3;
            StartRow2 = 45;
            PotentialDict = new Dictionary<string, PotentialFive>();
            FPotentialDict = new Dictionary<string, PotentialFive>();
            PotentialSum = new PotentialFive()
            {
                Up = new PotentialBase(),
                Down = new PotentialBase()
            };
            FPotentialSum = new PotentialFive()
            {
                Up = new PotentialBase(),
                Down = new PotentialBase()
            };
        }
        public void Doing()
        {
            Working();
        }
        public void Working()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand Command = connection.CreateCommand())
                {
                    double val=.0;
                    foreach (var region in Regions)
                    {
                        PotentialFive five = new PotentialFive();
                        foreach (var sf in SFS)
                        {
                            Command.CommandText = string.Format("Select SUM(JZRJQL),SUM(TZQDQL),SUM(SSCCQL),SUM(YYSSCCQL) from GYYD where XZJDMC='{0}' AND SFGSQY='{1}' AND TDSYQK='1'", region, sf);
                            using (var reader = Command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var values=new PotentialBase(){
                                                JZRJQL=double.TryParse(reader[0].ToString(),out val)?val:.0,
                                                TZQDQL=double.TryParse(reader[1].ToString(),out val)?val:.0,
                                                SSCCQL=double.TryParse(reader[2].ToString(),out val)?val:.0,
                                                YYSSCCQL=double.TryParse(reader[3].ToString(),out val)?val:.0
                                            };
                                    switch (sf)
                                    {
                                        case "是":
                                            five.Up = values;
                                            break;
                                        case "否":
                                            five.Down = values;
                                            break;
                                    }
                                }
                            }
                        }
                        PotentialDict.Add(region, five);
                        PotentialSum = PotentialSum + five;
                    }
                    string str = string.Empty;
                    foreach (var terrace in Terraces)
                    {
                        PotentialFive five = new PotentialFive();
                       
                        foreach (var sf in SFS)
                        {
                            if (terrace == "其他")
                            {
                                str = string.Format("from GYYD where SFWYCYPT='否' AND TDSYQK='1' AND SFGSQY='{0}'", sf);
                            }
                            else
                            {
                                str = string.Format("from GYYD where CYPTMC Like '%{0}%' AND TDSYQK='1' AND SFGSQY='{1}'", terrace, sf);
                            }
                            Command.CommandText = string.Format("Select SUM(JZRJQL),SUM(TZQDQL),SUM(SSCCQL),SUM(YYSSCCQL) {0}", str);
                            using (var reader = Command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var values = new PotentialBase()
                                    {
                                        JZRJQL = double.TryParse(reader[0].ToString(), out val) ? val : .0,
                                        TZQDQL = double.TryParse(reader[1].ToString(), out val) ? val : .0,
                                        SSCCQL = double.TryParse(reader[2].ToString(), out val) ? val : .0,
                                        YYSSCCQL = double.TryParse(reader[3].ToString(), out val) ? val : .0
                                    };
                                    switch (sf)
                                    {
                                        case "是":
                                            five.Up = values;
                                            break;
                                        case "否":
                                            five.Down = values;
                                            break;
                                    }
                                }
                            }
                        }
                        FPotentialDict.Add(terrace, five);
                        FPotentialSum = FPotentialSum + five;
                    }
                }
                connection.Close();
            }
        }

        public void Write(ref ISheet Sheet)
        {
            foreach (var pair in PotentialDict)
            {
                Sheet.GetRow(StartRow).GetCell(1).SetCellValue(pair.Key);
                WriteBase(pair.Value.Up, Sheet, StartRow++, StartCell);
                WriteBase(pair.Value.Down, Sheet, StartRow++, StartCell);
            }
            WriteBase(PotentialSum.Up, Sheet, StartRow2 - 2, StartCell);
            WriteBase(PotentialSum.Down, Sheet, StartRow2 - 1, StartCell);
            foreach (var pair in FPotentialDict)
            {
                Sheet.GetRow(StartRow2).GetCell(1).SetCellValue(pair.Key);
                WriteBase(pair.Value.Up, Sheet, StartRow2++, StartCell);
                WriteBase(pair.Value.Down, Sheet, StartRow2++, StartCell);
            }
            WriteBase(PotentialSum.Up, Sheet, 85, StartCell);
            WriteBase(PotentialSum.Down, Sheet, 86, StartCell);
        }
        public string GetCurrentName()
        {
            return CurrentName;
        }
    }
}
