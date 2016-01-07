using DNA.Helper;
using DNA.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DNA.Winform
{
    public partial class Form1 : Form
    {
        private string FilePath { get; set; }
        private string MdbPath { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FilePath = FileHelper.SaveFile();
            this.textBox1.Text = FilePath;
            //Folder = FileHelper.OpenFolder();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(this.FilePath)&&!string.IsNullOrEmpty(MdbPath))
            {
                Manager manager = new Manager(this.FilePath,MdbPath);
                manager.Analyze();
                MessageBox.Show("生成成功！");
            }
            else
            {
                MessageBox.Show("请指定Mdb文件路径以及结果表格输出路径");
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
    }
}
