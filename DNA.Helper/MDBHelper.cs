using DNA.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Helper
{
    public static class MDBHelper
    {
        private static string[] SFGSQYS = { "是", "否" };
        private static string ConnectionString { get; set; }

        static MDBHelper()
        {
            ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}",System.Configuration.ConfigurationManager.AppSettings["DATABASE"].GetSourcesPath());
        }

        public static void ReadBase(string SQLCommand)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand Command = connection.CreateCommand())
                {
                    Command.CommandText = SQLCommand;
                    var reader = Command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(string.Format("{0}{1}{2}{3}", reader[0], reader[1], reader[2], reader[3]));
                    }
                }
                connection.Close();
            }
        }
        public static void ReadOne(string SQLCommand)
        {
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLCommand;
                    var reader = Command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                    }
                }
                Connection.Close();
            }
        }
        public static List<string> GetAllDistrict()
        {
            var list = new List<string>();
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = "Select XZJDMC from YDDW GROUP BY XZJDMC";
                    var reader = Command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader[0].ToString());
                    }
                }
                Connection.Close();
            }
            return list;
        }
        //public static Dictionary<string, DataOne> GetExcelOneData(this List<string> List)
        //{
        //    var dict = new Dictionary<string, DataOne>();
        //    using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
        //    {
        //        Connection.Open();  
        //        using (OleDbCommand Command = Connection.CreateCommand())
        //        {
        //            foreach (var key1 in List)
        //            {
        //                var Values = new DataOne();
        //                foreach (var key2 in SFGSQYS)
        //                {
        //                    Command.CommandText = string.Format("Select SUM(WPZJZMJ),SUM(CYRS) from YDDW where TDZMJ=YKFTDMJ AND SFGSQY='{0}' AND XZJDMC='{1}'", key2, key1);
        //                    var reader = Command.ExecuteReader();
        //                    if (reader.Read()) 
        //                    {
        //                        var val = new DataBase()
        //                        {
        //                            WPZJZMJ = double.Parse(reader[0].ToString()),
        //                            CYRS = int.Parse(reader[1].ToString()),
        //                            LJGDZCTZ = double.Parse(reader[2].ToString()),
        //                            Electricity = new double[]{
        //                        double.Parse(reader[3].ToString()),
        //                        double.Parse(reader[4].ToString()),
        //                        double.Parse(reader[5].ToString())
        //                        },
        //                            CentralTax = new double[]{
        //                        double.Parse(reader[6].ToString()),
        //                        double.Parse(reader[7].ToString()),
        //                        double.Parse(reader[8].ToString())
        //                        },
        //                            FarmTax = new double[]{
        //                        double.Parse(reader[9].ToString()),
        //                        double.Parse(reader[10].ToString()),
        //                        double.Parse(reader[11].ToString())
        //                        },
        //                            MainBusiness = new double[]{
        //                        double.Parse(reader[12].ToString()),
        //                        double.Parse(reader[13].ToString()),
        //                        double.Parse(reader[14].ToString())
        //                        }
        //                        };
        //                        switch (key2)
        //                        {
        //                            case "是":
        //                                Values.Up = val;
        //                                break;
        //                            case "否":
        //                                Values.Down = val;
        //                                break;
        //                        }
        //                    }
        //                }
        //                dict.Add(key1, Values);
        //             }
                    
        //        }
        //        Connection.Close();
        //    }
        //    return dict;
        //}
    }
}
