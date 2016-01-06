using DNA.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolMerge:ToolBase
    {
        public List<GYDW> GYDWList { get; set; }
        public Dictionary<string, TempData> TempDict { get; set; }//  企业编号  ->  相关企业信息
        public ToolMerge()
        {
            this.GYDWList = new List<GYDW>();
            this.TempDict = new Dictionary<string, TempData>();
            Init();
        }

        public void Init()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Select QYBH from GYYD_YDDW Group By QYBH";
                    List<string> list = new List<string>();
                    string str = string.Empty;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            str = reader[0].ToString().Trim();
                            if (!string.IsNullOrEmpty(str) && !list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    foreach (string QYBH in list)
                    {
                        if (!TempDict.ContainsKey(QYBH))
                        {
                            command.CommandText = string.Format("Select SUM(JZMJ),COUNT(*) from GYYD_YDDW where QYBH='{0}'", QYBH);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    TempDict.Add(QYBH, new TempData() { Sum = double.Parse(reader[0].ToString()), Count = int.Parse(reader[1].ToString()) });
                                }
                            }
                           
                        }
                    }
                    list.Clear();
                    command.CommandText = "select DKBH from GYYD_YDDW Group By DKBH";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            str = reader[0].ToString().Trim();
                            if (!string.IsNullOrEmpty(str) && !list.Contains(str))
                            {
                                list.Add(str);
                            }
                        }
                    }
                    //DKBHList = list;
                    command.CommandText = "Select DKBH,QYBH,JZMJ from GYYD_YDDW";
                    using (var reader = command.ExecuteReader())
                    {
                        double JZMJ = 0.0;
                        string QY = string.Empty;
                        while (reader.Read())
                        {
                            QY = reader[1].ToString();
                            if (double.TryParse(reader[2].ToString(), out JZMJ))
                            {
                                var percent = .0;
                                if (TempDict.ContainsKey(QY))
                                {
                                    var tempdata = TempDict[QY];
                                    if (tempdata.Sum > 0)
                                    {
                                        percent = JZMJ / tempdata.Sum;
                                    }
                                    else
                                    {
                                        percent = ((double)1) / ((double)tempdata.Count);
                                    }
                                }
                                if (double.IsNaN(percent))
                                {
                                    percent = .0;
                                }
                                GYDWList.Add(new GYDW()
                                {
                                    DKBH = reader[0].ToString(),
                                    QYBH = QY,
                                    JZMJ = JZMJ,
                                    Percent = percent
                                });
                            }
                        }
                    }
                   
                }
                connection.Close();
            }
        }
    }
}
