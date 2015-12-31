using DNA.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class MainTool:ToolBase
    {
        public List<GYDW> List { get; set; }
        public Dictionary<string, Merge> Dict { get; set; }
        public MainTool()
        {
            Dict = new Dictionary<string, Merge>();
        }

        protected MergeBase Translate(Queue<string> queue)
        {
            MergeBase database = new MergeBase();
            if (queue.Count == ValCount)
            {
                System.Reflection.PropertyInfo[] propList = typeof(MergeBase).GetProperties();
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
                    else if (item.PropertyType.Equals(typeof(bool)))
                    {
                        switch (queue.Dequeue())
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
            return database;
        }
        public List<GYDW> Get(string SQLCommandText)
        {
            List<GYDW> List = new List<GYDW>();
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLText;
                    var reader = Command.ExecuteReader();
                    while (reader.Read())
                    {
                        List.Add(new GYDW()
                        {
                            DKBH = reader[0].ToString(),
                            QYBH = reader[1].ToString(),
                            JZMJ = double.Parse(reader[2].ToString())
                        });
                    }
                }
                Connection.Close();
            }
            return List;
        }
        public void Working()
        {
            var DKBHS = GetBase("Select DKBH from GYYD_YDDW Group By DKBH");
            foreach (var dkbh in DKBHS)
            {
                SQLText = string.Format("Select DKBH,QYBH,JZMJ from GYYD_YDDW where DKBH={0}", dkbh);
                var list = Get(SQLText);
                

                if (list.Count > 0)
                {
                    SQLText = string.Format("Select SFGXQY,CYRS,LJGDZCTZ,YDL2012,YDL2013,YDL2014,GSRKSS2012,GSRKSS2013,GSRKSS2014,DSRKSS2012,DSRKSS2013,DSRKSS2014,ZYYSR2012,ZYYSR2013,ZYYSR2014 from YDDW where QYBH={0}", dkbh);
                    ReadData(new string[]{
                        SQLText
                    });
                    var mergebase = Translate(queue);
                    var merge = new Merge()
                    {
                        CZQYSL = 1,
                        SFGXQY=mergebase.SFGXQY?1:0,
                        Base=mergebase
                    };
                    if (list.Count == 1)
                    {
                        SQLText = string.Format("Select DKBH,QYBH,JZMJ from GYYD_YDDW where QYBH={0}", list[0].QYBH);
                        var list2 = Get(SQLText);
                        if (list2.Count == 1)//一地一企
                        {
                            
                        }
                        else//一企多地
                        {

                        }
                    }
                    else//一地多企
                    {

                    }
                }
            }
        }
    }
}
