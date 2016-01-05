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
            IWorkbook workbook2 = new HSSFWorkbook();//.xls
            IWorkbook workbook3 = new XSSFWorkbook();//.xlsx
            IWorkbook workbook = null;
            try
            {
                using (FileStream fs = new FileStream("", FileMode.Open, FileAccess.Read))
                {
                    workbook = WorkbookFactory.Create(fs);
                }

            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            ISheet sheet = workbook.GetSheet("sheet1");
            sheet = workbook.GetSheetAt(0);
            IRow row = sheet.GetRow(0);
            if (row == null)
            {
                row = sheet.CreateRow(0);
            }
            ICell cell = row.GetCell(0);
            if (cell == null)
            {
                cell = row.CreateCell(0);
            }
            cell.SetCellValue("wangjianlong ");

            List<string> List = new List<string>();
            List<int> list1 = new List<int>();
            List<double> List2 = new List<double>();
            
            Dictionary<int, string> Dict = new Dictionary<int, string>();

            int Count = sheet.LastRowNum;
            int Number = row.LastCellNum;
            
            string[] array=new string[Count];

            for (var i = 0; i < Count; i++)
            {
                row = sheet.GetRow(i);
                for (var j = 0; j < Number; j++)
                {
                    cell = row.GetCell(j);
                    string str = cell.ToString();//g公式
                    switch (cell.CellType)
                    {
                        case CellType.Formula:
                            try
                            {
                                str= cell.NumericCellValue.ToString();
                                
                            }
                            catch (Exception ex)
                            {

                            }
                            break;
                    }
                    if (!List.Contains(str))
                    {
                        List.Add(str);
                    }
                    if (list1.Contains(int.Parse(str)))
                    {
                        int a=0;
                        if (int.TryParse(str, out a))
                        {

                        }
                    }

                }
            }
            using (var fs = new FileStream(@"C:\", FileMode.OpenOrCreate, FileAccess.Write))
            {
                workbook.Write(fs);
            }



            

            string x = "3.65";
            string y = "3.65";
            double a = double.Parse(x);
            double b = double.Parse(y);
            Console.WriteLine(string.Format("a:{0}  b:{1}  a/b={2}", a, b, a / b));
            int temp = 1314;
            Console.WriteLine(temp / 100);
            Console.ReadLine();
        }
    }
}
