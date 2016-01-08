using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DNA.Winform
{
    public class Generted
    {
        private string SaveFilePath { get; set; }
        public string MdbFilePath { get; set; }
        public string ModelExcelPath { get; set; }

        public Generted(string saveFilePath, string mdbFilePath,string ModelExcelPath)
        {
            this.SaveFilePath = saveFilePath;
            this.MdbFilePath = mdbFilePath;
            this.ModelExcelPath = ModelExcelPath;
        }

        private System.Diagnostics.Process process { get; set; }
        private UpdateProgressDelegate progressDelegate;
        public void Run(UpdateProgressDelegate progressDelegate)
        {
            this.progressDelegate = progressDelegate;
            process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "PP/DNA.exe";
            process.StartInfo.Arguments = string.Format("{0} {1} {2}", SaveFilePath,MdbFilePath,ModelExcelPath);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += WorkerProcess_OutputDataReceived;
            process.Start();
            process.BeginOutputReadLine();
        }

        public void WorkerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (process == sender)
            {
                progressDelegate(string.Format("{0}", e.Data));
            }
        }
    }
}
