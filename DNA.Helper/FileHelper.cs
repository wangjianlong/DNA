using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DNA.Helper
{
    public static class FileHelper
    {
        private static string PathBase = "../../Sources/";
        public static string OpenFolder()
        {
            string Folder = string.Empty;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Folder = folderBrowserDialog.SelectedPath;
            }
            return Folder;
        }
        public static string SaveFile()
        {
            string FilePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "生成文件路径";
            saveFileDialog.Filter = "2003.xls|.xls|2007.xlsx|.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = saveFileDialog.FileName;
            }
            return FilePath;
        }

        public static string OperFile(string Title, string Filter)
        {
            string filePath = string.Empty;
            OpenFileDialog openfileDialog = new OpenFileDialog();
            openfileDialog.Title = Title;
            openfileDialog.Filter = Filter;
            if (openfileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openfileDialog.FileName;
            }
            return filePath;
        }
        public static string GetSourcesPath(this string FileName)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathBase, FileName);
        }

        public static IWorkbook OperWorkbook(this string FilePath)
        {
            IWorkbook workbook = null;
            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = WorkbookFactory.Create(fs);
            }
            return workbook;
        }
    }
}
