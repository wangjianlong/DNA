﻿using DNA.Helper;
using DNA.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DNA.Winform
{
    public delegate void UpdateProgressDelegate(string message);
    public partial class Form1 : Form
    {
        private string FilePath { get; set; }
        private string MdbPath { get; set; }
        private string ModelExcelPath { get; set; }
        private string ExcelFolder { get; set; }
        private Thread _thread { get; set; }
        public Generted g { get; set; }
        public Form1()
        {
            InitializeComponent();
            ModelExcelPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["EXCELS"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FilePath = FileHelper.SaveFile();
            this.textBox1.Text = FilePath;
            //Folder = FileHelper.OpenFolder();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ExcelFolder = FileHelper.SaveFolder();
            this.textBox3.Text = ExcelFolder;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            _thread = new Thread(Start);
            _thread.IsBackground = true;
            _thread.Start();
            //if (!string.IsNullOrEmpty(MdbPath))
            //{
                
            //    if (!string.IsNullOrEmpty(this.ExcelFolder))
            //    {
            //        Manager manager = new Manager(MdbPath);
            //        manager.Analyze2(this.ExcelFolder);
            //        MessageBox.Show("生成成功");
            //    }
            //    else if (!string.IsNullOrEmpty(this.FilePath))
            //    {
            //        Manager manager = new Manager(this.FilePath, MdbPath);
            //        manager.Analyze();
            //        MessageBox.Show("生成成功！");
            //    }
            //    else
            //    {
            //        MessageBox.Show("未指定输出文件路径");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("未指定Mdb文件路径");
            //}
            


            //if (!string.IsNullOrEmpty(this.FilePath)&&!string.IsNullOrEmpty(MdbPath))
            //{
            //    Manager manager = new Manager(this.FilePath, MdbPath);
            //    manager.Analyze();
            //    MessageBox.Show("生成成功！");
            //    //var thread = new Thread(RunAsync);
            //    //thread.Start();
            //}
            //else
            //{
            //    MessageBox.Show("请指定Mdb文件路径以及结果表格输出路径");
            //}
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(MdbPath))
            {

                if (!string.IsNullOrEmpty(this.ExcelFolder))
                {
                    Manager manager = new Manager(MdbPath);
                    manager.Analyze2(this.ExcelFolder);
                    MessageBox.Show("生成成功");
                }
                else if (!string.IsNullOrEmpty(this.FilePath))
                {
                    Manager manager = new Manager(this.FilePath, MdbPath);
                    manager.Analyze();
                    MessageBox.Show("生成成功！");
                }
                else
                {
                    MessageBox.Show("未指定输出文件路径");
                }
            }
            else
            {
                MessageBox.Show("未指定Mdb文件路径");
            }
        }
        private void RunAsync()
        {
            g = new Generted(FilePath, MdbPath,ModelExcelPath);
            try
            {
                g.Run(UpdateProgress);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void UpdateProgress(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            if (listBox1.InvokeRequired)
            {
                var d = new UpdateProgressDelegate(UpdateProgress);
                listBox1.Invoke(d, message);
            }
            else
            {
                listBox1.Items.Add(string.Format("[{0:HH:mm:ss}]  {1}", DateTime.Now, message));
                listBox1.TopIndex = listBox1.Items.Count - listBox1.ClientSize.Height / listBox1.ItemHeight;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MdbPath = FileHelper.OperFile("请指定Access数据库文件路径", "Access 数据库(*.mdb)|*.mdb");
            if (!string.IsNullOrEmpty(MdbPath))
            {
                this.textBox2.Text = MdbPath;
            }
        }
        private void WClose()
        {
            if (_thread != null)
            {
                if (_thread.IsAlive)
                {
                    _thread.Abort();
                }
                else
                {
                    _thread.Join();
                }
            }
           
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.WClose();
            this.Close();
        }

        
    }
}
