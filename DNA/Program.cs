using DNA.Helper;
using DNA.Tools;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DNA
{
    class Program
    {
        static void Main(string[] args)
        {
           
            try
            {
                string SaveFilePath = args[0];
                //string SaveFilePath = @"C:\Users\loowootech\Desktop\1.xls";
                string MdbFilePath = args[1];
                //string MdbFilePath = @"C:\Users\loowootech\Desktop\mdb2.mdb";
                string modelPath = args[2];
                //string modelPath = @"E:\Github\DNA\DNA.Winform\bin\Debug\Excels.xls";
                Console.WriteLine(string.Format("成功读取Access数据库路径：{0}", SaveFilePath));
                Console.WriteLine(string.Format("成功读取Excel文件输出路径：{0}", MdbFilePath));
                Console.WriteLine(string.Format("成功读取Excel模型文件路径:{0}", modelPath));
                if (!string.IsNullOrEmpty(SaveFilePath) && !string.IsNullOrEmpty(MdbFilePath))
                {
                    Console.WriteLine("开始准备分析数据.........");
                    var Manager = new Manager(SaveFilePath, MdbFilePath, modelPath);
                    Console.WriteLine("程序开始分析..........");
                    Manager.Analyze();
                    Console.WriteLine("完成数据分析和Excel生成");
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
        }
    }
}
