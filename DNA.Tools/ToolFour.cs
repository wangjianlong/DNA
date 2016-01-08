using DNA.Models;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data.OleDb;

namespace DNA.Tools
{
    public class ToolFour:ToolPurpose,ITool
    {
        public List<TDSJYTJG> List { get; set; }
        public Dictionary<string, ChangePurpose> Dict { get; set; }
        public ChangePurpose Sum { get; set; }
        public ToolFour(string mdbFilePath)
        {
            Init(mdbFilePath);
            Dict = new Dictionary<string, ChangePurpose>();
            List = GetData();
            Sum = new ChangePurpose();
            StartRow = 3;
            StartRow2 = 27;
            StartCell = 1;
            SheetName = "表4";
        }
        public void Doing()
        {
            Working();
        }

        public void Working()
        {
            if (List != null)
            {
                foreach (var item in List)
                {
                    var XZQ = GetOneBase(string.Format("Select XZJDMC from GYYD where DKBH='{0}'", item.DKBH));
                    if (!string.IsNullOrEmpty(XZQ))
                    {
                        var val = new ChangePurpose()
                        {
                            Number = 1,
                            SumArea = item.Area
                        };
                        switch (item.SJYT)
                        {
                            case "05":
                                val.Area05 = item.Area;
                                break;
                            case "07":
                                val.Area07 = item.Area;
                                break;
                            case "08":
                                val.Area08 = item.Area;
                                break;
                            default:
                                val.AreaOther = item.Area;
                                break;
                        }
                        val = val / 1000;
                        if (Dict.ContainsKey(XZQ))
                        {
                            Dict[XZQ] = Dict[XZQ] + val;
                        }
                        else
                        {
                            Dict.Add(XZQ, val);
                        }
                    }
                }
            }
        }

        public List<TDSJYTJG> GetData()
        {
            var list = new List<TDSJYTJG>();
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand Command = connection.CreateCommand())
                {
                    Command.CommandText = "Select DKBH,SJYT,YDMJ from TDSJYTJG";
                    var reader = Command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new TDSJYTJG()
                        {
                            DKBH = reader[0].ToString(),
                            SJYT = reader[1].ToString(),
                            Area = double.Parse(reader[2].ToString())
                        });
                    }
                }
                connection.Close();
            }
            return list;
        }
        public void Write(ref ISheet Sheet)
        {
            foreach (var key in Dict.Keys)
            {
                Sheet.GetRow(StartRow).GetCell(0).SetCellValue(key);
                var val = Dict[key];
                WriteBase(val, Sheet, StartRow++, StartCell);
                Sum = Sum + val;
            }
            WriteBase(Sum, Sheet, StartRow2, StartCell);
        }
    }
}
