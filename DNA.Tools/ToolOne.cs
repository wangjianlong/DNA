 using DNA.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolOne:ToolRegion,ITool
    {
        public Dictionary<string, DataOne> Dict { get; set; }
        private int ValCount { get; set; }
        public ToolOne()
        {
            Dict = new Dictionary<string, DataOne>();
            ValCount = 25;
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
                Dict.Add(region, one);
            }
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
