using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace VK_API
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
            InitBrowser();
        }
        public ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("https://oauth.vk.com/authorize?client_id=6620613&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=friends,video,photos,offline&response_type=token&v=5.52");
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.
        }
        private void FormAuth_Load(object sender, EventArgs e)
        {
            
        }
    }
}
