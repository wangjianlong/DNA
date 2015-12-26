using System;
using System.Collections.Generic;
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
        public static string GetSourcesPath(this string FileName)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathBase, FileName);
        }
    }
}
