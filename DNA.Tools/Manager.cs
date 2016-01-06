using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNA.Helper;
using NPOI.SS.UserModel;
using System.IO;

namespace DNA.Tools
{
    public class Manager
    {
        private string ModelExcelPath { get; set; }
        public IWorkbook WorkBook { get; set; }
        public string SaveFilePath { get; set; }
        public Manager(string SavePath)
        {
            ModelExcelPath = System.Configuration.ConfigurationManager.AppSettings["EXCELS"].GetSourcesPath();
            WorkBook = ModelExcelPath.OperWorkbook();
            SaveFilePath = SavePath;
        }
        public void Analyze()
        {
            ToolOne tool = new ToolOne();
            ISheet sheet = WorkBook.GetSheet(tool.SheetName);
            if (sheet != null)
            {
                tool.Doing();
                //tool.Working();
                tool.Write(ref sheet);
            }
            Save();
        }
        public void Analyze2()
        {
            MainTool mainTool = new MainTool();
            ISheet sheet = WorkBook.CreateSheet("ceshi");
            if (sheet != null)
            {
                mainTool.Doing();
               // mainTool.Write(ref sheet);
            }
        }

        public void Save()
        {
            using (var fs = new FileStream(SaveFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                WorkBook.Write(fs);
            }
        }
    }
}
