using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNA.Helper;
using System.Data.OleDb;
using DNA.Models;

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
        public ToolBase()
        {
            ConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", System.Configuration.ConfigurationManager.AppSettings["DATABASE"].GetSourcesPath());
            ViewName = System.Configuration.ConfigurationManager.AppSettings["VIEWNAME"];
            DropView = string.Format("Drop View {0}", ViewName);
            queue = new Queue<string>();
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
                            if (!list.Contains(item.Trim()))
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
        protected List<string> GetRegions()
        {
            return GetBase("Select XZJDMC from YDDW Group By XZJDMC");
        }
        protected List<string> GetTerraces()
        {
            return GetBase("Select CYPTMC from GYYD Group By CYPTMC") ;
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
                    try
                    {
                        command.ExecuteNonQuery();
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
        public void Dispose()
        {
            if (!string.IsNullOrEmpty(CreateView))
            {
                ExecuteQuery(DropView);
            }
        }
        
    }
}
