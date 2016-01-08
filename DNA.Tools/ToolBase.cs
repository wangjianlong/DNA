using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNA.Helper;
using System.Data.OleDb;
using DNA.Models;
using NPOI.SS.UserModel;

namespace DNA.Tools
{
    public class ToolBase:IDisposable
    {
        protected string[] SFS = { "是", "否" };
        protected string ConnectionString { get; set; }
        protected string OutPutPath { get; set; }
        protected string ModelFilePath { get; set; }
        protected string ViewName { get; set; }
        protected string CreateView { get; set; }
        protected string DropView { get; set; }
        protected string SQLText { get; set; }
        protected Queue<string> queue { get; set; }
        public string SheetName { get; set; }
        protected int StartRow { get; set; }
        protected int StartRow2 { get; set; }
        protected int StartCell { get; set; }
        protected int ValCount { get; set; }
        public ToolBase()
        {
            ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", System.Configuration.ConfigurationManager.AppSettings["DATABASE"].GetSourcesPath());
            ViewName = System.Configuration.ConfigurationManager.AppSettings["VIEWNAME"];
            DropView = string.Format("Drop View {0}", ViewName);
            queue = new Queue<string>();
        }
        public  virtual void Init(string mdbFilePath)
        {
            ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", mdbFilePath);
        }

        protected List<string> GetBase(string SQLCommandText)
        {
            var list = new List<string>();
            string str = string.Empty;
            
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = SQLCommandText;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        str = reader[0].ToString();
                        string[] array = str.Split('|');
                        foreach (var item in array)
                        {
                            if (!list.Contains(item.Trim())&&!string.IsNullOrEmpty(item.Trim()))
                            {
                                list.Add(item.Trim());
                            }
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }
        protected string GetOneBase(string SQLCommandText)
        {
            string str = string.Empty;
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand Command = connection.CreateCommand())
                {
                    Command.CommandText = SQLCommandText;
                    var reader = Command.ExecuteReader();
                    if (reader.Read())
                    {
                        str = reader[0].ToString();
                    }
                }
                connection.Close();
            }
            return str;
        }
        protected List<string> GetRegions()
        {
            return GetBase("Select XZJDMC from GYYD Group By XZJDMC");
        }
        protected List<string> GetTerraces()
        {
            return GetBase("Select CYPTMC from GYYD Group By CYPTMC") ;
        }
        protected List<string> GetCodes()
        {
            return GetBase("Select HYDM from YDDW Group By HYDM");
        }
        protected void ExecuteReaderOneToQueue(string SQLCommandText)
        {
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLCommandText;
                    var reader = Command.ExecuteReader();
                    if (reader.Read())
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            queue.Enqueue(reader[i].ToString());
                        }
                    }
                }
                Connection.Close();
            }
        }

        protected void ExecuteQuery(string SQlCommandText)
        {
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand command = Connection.CreateCommand())
                {
                    command.CommandText = SQlCommandText;
                    command.ExecuteNonQuery();
                    try
                    {
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        
                    }
                    
                }
                Connection.Close();
            }
        }

        protected virtual void InitView()
        {
            if (!string.IsNullOrEmpty(CreateView))
            {
                ExecuteQuery(CreateView);
            }
        }
        protected void WriteBase<T>(T Data, ISheet Sheet, int Row, int Line)
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
                            row.GetCell(Line).SetCellValue(Math.Round(val, 2));
                        }
                    }
                    else if (item.PropertyType.Equals(typeof(int)))
                    {
                        if (int.TryParse(item.GetValue(Data, null).ToString(), out Values))
                        {
                            row.GetCell(Line).SetCellValue(Values);
                        }
                    }
                    Line++;
                }
            }
        }
        protected void ReadData(string[] SQLCommandTexts)
        {
            queue.Clear();
            if (SQLCommandTexts != null)
            {
                foreach (var str in SQLCommandTexts)
                {
                    ExecuteReaderOneToQueue(str);
                }
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
                    var str = queue.Dequeue();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (item.PropertyType.Equals(typeof(double)))
                        {
                            double val = 0.0;
                            if (double.TryParse(str, out val))
                            {
                                item.SetValue(database, val, null);
                            }
                            else
                            {
                                Console.WriteLine(str);
                            }
                            
                        }
                        else if (item.PropertyType.Equals(typeof(int)))
                        {
                            int m = 0;
                            if (int.TryParse(str, out m))
                            {
                                item.SetValue(database, m, null);
                            }
                            else
                            {
                                Console.WriteLine(str);
                            }
                            
                        }
                    }
                    
                }
            }
            return database;
        }
        public string GetSheetName()
        {
            return SheetName;
        }
        public void Dispose()
        {
            if (!string.IsNullOrEmpty(CreateView))
            {
                ExecuteQuery(DropView);
            }
        }
        
    }
}
