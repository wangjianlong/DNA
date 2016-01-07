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
        private string MdbFilePath { get; set; }
        public Manager(string SavePath,string MdbFilePath)
        {
            ModelExcelPath = System.Configuration.ConfigurationManager.AppSettings["EXCELS"].GetSourcesPath();
            WorkBook = ModelExcelPath.OperWorkbook();
            SaveFilePath = SavePath;
            this.MdbFilePath = MdbFilePath;
        }
        public void Analyze()
        {
            MainTool maintool = new MainTool(MdbFilePath);
            maintool.Doing();
            MergeTool mergetool = new MergeTool(MdbFilePath);
            mergetool.Working();
            ToolOne tool = new ToolOne(MdbFilePath);
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
            //MainTool mainTool = new MainTool();
            //ISheet sheet = WorkBook.CreateSheet("ceshi");
            //if (sheet != null)
            //{
            //    mainTool.Doing();
            //   // mainTool.Write(ref sheet);
            //}
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
