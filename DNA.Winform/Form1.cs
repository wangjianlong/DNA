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
            
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                Manager manager = new Manager(this.FilePath);
                manager.Analyze2();
            }
        }
    }
}
