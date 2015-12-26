using DNA.Helper;
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
        private string Folder { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Folder = FileHelper.OpenFolder();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Folder))
            {

            }
        }
    }
}
