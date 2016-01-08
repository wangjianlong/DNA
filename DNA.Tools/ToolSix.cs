using DNA.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolSix:ToolType,ITool
    {
        public Dictionary<int, PotentialFive> TypeDict { get; set; }
        public PotentialFive SSum { get; set; }
        public ToolSix(string mdbFilePath)
        {
            Init(mdbFilePath);
            SheetName = "表6";
            StartRow = 4;
            StartCell = 3;
            StartRow2 = 66;
            TypeDict = new Dictionary<int, PotentialFive>();
            SSum = new PotentialFive()
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
                    double val = .0;
                    foreach (var code in Codes)
                    {
                        var five = new PotentialFive();
                        foreach (var sf in SFS)
                        {
                            Command.CommandText = string.Format("Select SUM(JZRJQL),SUM(TZQDQL),SUM(SSCCQL),SUM(YYSSCCQL) from GYYD where HYDM='{0}' AND SFGSQY='{1}'AND TDSYQK='1'", code,sf);
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
                        TypeDict.Add(code, five);
                        SSum = SSum + five;
                        
                    }
                }
                connection.Close();
            }
        }
        public void Write(ref ISheet Sheet)
        {
            int code=0;
            for (var i = StartRow; i < StartRow2; i = i + 2)
            {
                if (int.TryParse(Sheet.GetRow(i).GetCell(0).ToString(), out code))
                {
                    if (TypeDict.ContainsKey(code))
                    {
                        var five = TypeDict[code];
                        WriteBase(five.Up, Sheet, i, StartCell);
                        WriteBase(five.Down, Sheet, i + 1, StartCell);
                    }
                }
            }
            WriteBase(SSum.Up, Sheet, StartRow2, StartCell);
            WriteBase(SSum.Down, Sheet, StartRow2 + 1, StartCell);

        }
    }
}
