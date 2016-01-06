using DNA.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class MergeTool:ToolMerge
    {
        public Dictionary<string, Union> AreaDict { get; set; }//地块编号  批准用地面积 土地登记面积  抵押土地面积  总面积等等
        public Dictionary<string, string> WayDict { get; set; }//地块编号   实际用途  
        public Dictionary<string,MergeBase> FacDict { get; set; }
        public MergeTool()
        {
            AreaDict = new Dictionary<string, Union>();
            WayDict = new Dictionary<string, string>();
            FacDict = new Dictionary<string, MergeBase>();
        }

        public void InitM()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand Command = connection.CreateCommand())
                {
                    Command.CommandText = "Select DKBH,PZYDMJ,TDDJMJ,DYMJ,YDZMJ,WJPZYDMJ,JZZMJ,JZZDMJ,WPZJZMJ,WPZJZZDMJ from GYYD";
                    using (var reader = Command.ExecuteReader())
                    {
                        string DKBH = string.Empty;
                        while (reader.Read())
                        {
                            DKBH = reader[0].ToString().Trim();
                            if (!string.IsNullOrEmpty(DKBH) && !AreaDict.ContainsKey(DKBH))
                            {
                                AreaDict.Add(DKBH, new Union()
                                {
                                    AreaBase = new AreaBase()
                                    {
                                        PZYDMJ = double.Parse(reader[1].ToString()),
                                        TDDJMJ = double.Parse(reader[2].ToString()),
                                        DYMJ = double.Parse(reader[3].ToString())
                                    },
                                    AreaOne = new AreaOne()
                                    {
                                        YDZMJ = double.Parse(reader[4].ToString()),
                                        WJPZYDMJ = double.Parse(reader[5].ToString()),
                                        JZZMJ = double.Parse(reader[6].ToString()),
                                        JZZDMJ = double.Parse(reader[7].ToString()),
                                        WPZJZMJ = double.Parse(reader[8].ToString()),
                                        WPZJZZDMJ = double.Parse(reader[9].ToString())
                                    }
                                });
                            }
                        }
                    }
                    Command.CommandText = "Select DKBH,SJYT from TDSJYTJG";
                    using (var reader = Command.ExecuteReader())
                    {
                        string DKBH = string.Empty;
                        string SJYT = string.Empty;
                        while (reader.Read())
                        {
                            DKBH = reader[0].ToString();
                            SJYT = reader[1].ToString();
                            if (!string.IsNullOrEmpty(DKBH) && !string.IsNullOrEmpty(SJYT))
                            {
                                if (!WayDict.ContainsKey(DKBH))
                                {
                                    WayDict.Add(DKBH, SJYT);
                                }
                                else
                                {
                                    WayDict[DKBH] = WayDict[DKBH] + "," + SJYT;
                                }
                            }
                        }
                    }
                    Command.CommandText = "Select QYBH,SFGXQY,HYDM,XZJDMC,SFGSQY,CYRS,LJGDZCTZ,YDL2012,YDL2013,YDL2014,GSRKSS2012,GSRKSS2013,GSRKSS2014,DSRKSS2012,DSRKSS2013,DSRKSS2014,ZYYSR2012,ZYYSR2013,ZYYSR2014 from YDDW";
                    using (var reader = Command.ExecuteReader())
                    {
                        string QYBH = string.Empty;
                        while (reader.Read())
                        {
                            QYBH = reader[0].ToString();
                            if (!string.IsNullOrEmpty(QYBH)&&!FacDict.ContainsKey(QYBH))
                            {
                                try
                                {
                                    double val = 0.0;
                                    FacDict.Add(QYBH, new MergeBase()
                                    {
                                        SFGXQY = reader[1].ToString() == "是" ? true : false,
                                        HYDM = reader[2].ToString(),
                                        XZJDMC = reader[3].ToString(),
                                        SFGSQY = reader[4].ToString() == "是" ? true : false,
                                        CYRS = int.Parse(reader[5].ToString()),
                                        LJGDZCTZ = double.TryParse(reader[6].ToString(),out val)?val:.0,
                                        YDL2012 = double.TryParse(reader[7].ToString(),out val)?val:.0,
                                        YDL2013 = double.TryParse(reader[8].ToString(),out val)?val:.0,
                                        YDL2014 = double.TryParse(reader[9].ToString(),out val)?val:.0,
                                        GSRKSS2012 = double.TryParse(reader[10].ToString(),out val)?val:.0,
                                        GSRKSS2013 = double.TryParse(reader[11].ToString(),out val)?val:.0,
                                        GSRKSS2014 = double.TryParse(reader[12].ToString(),out val)?val:.0,
                                        DSRKSS2012 = double.TryParse(reader[13].ToString(),out val)?val:.0,
                                        DSRKSS2013 = double.TryParse(reader[14].ToString(),out val)?val:.0,
                                        DSRKSS2014 = double.TryParse(reader[15].ToString(),out val)?val:.0,
                                        ZYYSR2012 = double.TryParse(reader[16].ToString(),out val)?val:.0,
                                        ZYYSR2013 = double.TryParse(reader[17].ToString(),out val)?val:.0,
                                        ZYYSR2014 = double.TryParse(reader[18].ToString(),out val)?val:.0
                                    });
                                    Console.WriteLine("成功");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                                
                            }
                        }
                    }
                   
                   
                }
                connection.Close();
            }
        }
        public void Working()
        {
            InitM();
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    #region 更新批准用地面积 土地登记面积  抵押面积

                    Command.CommandText = "UPDATE GYYD_YDDW SET PZYDMJ=NULL,TDDJMJ=NULL,DYMJ=NULL where 地块所有人 IS NULL";
                    try
                    {
                        Command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    foreach (string DKBH in AreaDict.Keys)
                    {
                        var values = AreaDict[DKBH].AreaBase;
                        if (values != null)
                        {
                            Command.CommandText = string.Format("UPDATE GYYD_YDDW SET PZYDMJ={0},TDDJMJ={1},DYMJ={2} Where 地块所有人 IS NOT NULL AND DKBH='{3}'", values.PZYDMJ, values.TDDJMJ, values.DYMJ, DKBH);
                            try
                            {
                                Command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        
                    }
                    #endregion




                    string commandText = string.Empty;
                    foreach (var item in GYDWList)
                    {
                        commandText = "UPDATE GYYD_YDDW SET";
                        if(WayDict.ContainsKey(item.DKBH))
                        {
                            commandText += " SJYT='" + WayDict[item.DKBH]+"',";
                        }
                        if (AreaDict.ContainsKey(item.DKBH))
                        {
                            var one = AreaDict[item.DKBH].AreaOne;
                            one = one * (item.JZMJ / one.JZZMJ);
                            commandText += string.Format(" YDZMJ={0},WJPZYDMJ={1},JZZMJ={2},JZZDMJ={3},WPZJZMJ={4},WPZJZZDMJ={5},CZQYSL=1,", one.YDZMJ, one.WJPZYDMJ, one.JZZMJ, one.JZZDMJ, one.WPZJZMJ, one.WPZJZZDMJ);
                        }
                        if (FacDict.ContainsKey(item.QYBH))
                        {
                            var fac = FacDict[item.QYBH];
                            fac = fac * item.Percent;
                            commandText += string.Format(" SFGXQY='{0}',HYDM='{1}',XZQMC='{2}',SFGSQY='{3}',CYRS={4},LJGDZCTZ={5},YDL2012={6},YDL2013={7},YDL2014={8},GSRKSS2012={9},GSRKSS2013={10},GSRKSS2014={11},DSRKSS2012={12},DSRKSS2013={13},DSRKSS2014={14},ZYYSR2012={15},ZYYSR2013={16},ZYYSR2014={17}",
                                fac.SFGXQY ? "是" : "否", fac.HYDM, fac.XZJDMC, fac.SFGSQY ? "是" : "否", fac.CYRS, fac.LJGDZCTZ, fac.YDL2012, fac.YDL2013, fac.YDL2014, fac.GSRKSS2012, fac.GSRKSS2013, fac.GSRKSS2014, fac.DSRKSS2012, fac.DSRKSS2013, fac.DSRKSS2014, fac.ZYYSR2012, fac.ZYYSR2013, fac.ZYYSR2014
                                );
                        }
                        commandText += string.Format(" where DKBH='{0}' AND QYBH='{1}'", item.DKBH, item.QYBH);
                        try
                        {
                            Command.CommandText = commandText;
                            Command.ExecuteNonQuery();
                            Console.WriteLine("成功");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                    }
                }
                Connection.Close();
            }
        }
    }
}
