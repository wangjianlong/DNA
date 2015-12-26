using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNA.Helper;

namespace DNA.Tools
{
    public class ToolBase
    {
        protected string ConnectionString { get; set; }
        protected string OutPutPath { get; set; }
        protected string ModelFilePath { get; set; }
        public ToolBase()
        {
            ConnectionString=string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}",System.Configuration.ConfigurationManager.AppSettings["DATABASE"].GetSourcesPath())
        }
    }
}
