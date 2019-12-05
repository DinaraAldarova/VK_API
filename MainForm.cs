using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VK_API
{
    public partial class MainForm : Form
    {
        public ConnectAPI connect; 
        public MainForm()
        {
            InitializeComponent();
            connect = new ConnectAPI();
        }

        private void buttonAuth_Click(object sender, EventArgs e)
        {
            FormAuth formAuth = new FormAuth();
            formAuth.Owner = this;
            formAuth.ShowDialog();
        }
    }
}
