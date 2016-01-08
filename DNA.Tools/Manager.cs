using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNA.Helper;
using NPOI.SS.UserModel;
using System.IO;
using DNA.Models;

namespace DNA.Tools
{
    public class Manager
    {
        private string ModelExcelPath { get; set; }
        public IWorkbook WorkBook { get; set; }
        public string SaveFilePath { get; set; }
        private string MdbFilePath { get; set; }
        public Manager(string SavePath,string MdbFilePath,string ModelPath=null)
        {
            if (!string.IsNullOrEmpty(ModelPath))
            {
                ModelExcelPath = ModelPath;
            }
            else
            {
                ModelExcelPath = System.Configuration.ConfigurationManager.AppSettings["EXCELS"].GetSourcesPath();
            }
            
            WorkBook = ModelExcelPath.OperWorkbook();
            SaveFilePath = SavePath;
            this.MdbFilePath = MdbFilePath;
        }
        public void Analyze()
        {
            
            MainTool maintool = new MainTool(MdbFilePath);
            maintool.Doing();
            Console.WriteLine("完成GYYD表数据合并生成....................");
            MergeTool mergetool = new MergeTool(MdbFilePath);
            mergetool.Working();
            Console.WriteLine("完成GYYD_YDDW表数据合并生成...............");
            ITool tool = null;
            foreach (SheetEnum sheet in Enum.GetValues(typeof(SheetEnum)))
            {
                switch (sheet)
                {
                    case SheetEnum.one:
                        tool = new ToolOne(MdbFilePath);
                        break;
                    case SheetEnum.two:
                        tool = new ToolTwo(MdbFilePath);
                        break;
                    case SheetEnum.three:
                        tool = new ToolThree(MdbFilePath);
                        break;
                    case SheetEnum.four:
                        tool = new ToolFour(MdbFilePath);
                        break;
                    case SheetEnum.five:
                        tool = new ToolFive(MdbFilePath);
                        break;
                    case SheetEnum.six:
                        tool = new ToolSix(MdbFilePath);
                        break;
                }
                Console.WriteLine(string.Format("开始对{0}数据生成工作",tool.GetSheetName()));
                ISheet Bsheet = WorkBook.GetSheet(tool.GetSheetName());
                if (Bsheet != null)
                {
                    tool.Doing();
                    Console.WriteLine(string.Format("完成对{0}数据的采集",tool.GetSheetName()));
                    tool.Write(ref Bsheet);
                    Console.WriteLine(string.Format("成功保存{0}的数据到Sheet中",tool.GetSheetName()));
                }
            }
            //ToolOne tool = new ToolOne(MdbFilePath);
            //ISheet sheet = WorkBook.GetSheet(tool.SheetName);
            //if (sheet != null)
            //{
            //    tool.Doing();
            //    tool.Write(ref sheet);
            //}
            Save();
            Console.WriteLine("完成结果表格的生成");
            
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
