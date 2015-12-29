﻿using System;
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
        }
        public void Analyze()
        {
            ToolOne tool = new ToolOne();
            ISheet sheet = WorkBook.GetSheet(tool.SheetName);
            if (sheet != null)
            {
                tool.Working();
                tool.Write(ref sheet);
            }
            Save();
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