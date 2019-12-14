using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using CefSharp;
using CefSharp.WinForms;

namespace VK_API
{
    public partial class FormAuth : Form
    {
        MainForm main;
        public FormAuth()
        {
            InitializeComponent();
            InitBrowser();
        }

        public ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            browser = new ChromiumWebBrowser("https://oauth.vk.com/authorize?client_id=6620613&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=friends,wall,offline&response_type=token&v=5.52");
            browser.Dock = DockStyle.Fill;
            browser.AddressChanged += Check_Token;

            this.Controls.Add(browser);
        }

        private void Check_Token(object sender, AddressChangedEventArgs e)
        {
            if (browser.Address.ToString().IndexOf("access_token=") != -1)
            {
                GetToken();
            }
        }

        private void GetToken()
        {
            char[] separators = { '=', '&' };
            string[] url = browser.Address.ToString().Split(separators);
            /*
            [0]: "https://oauth.vk.com/blank.html#access_token"
            [1]: "6665c62906f0a1c7558235ff9dd7c27f9b5a29d1624329442642c9d5eafc4ce43bcbb015c6aef3814d63a"
            [2]: "expires_in"
            [3]: "0"
            [4]: "user_id"
            [5]: "67234060"
            */

            main = this.Owner as MainForm;
            main.connect = new ConnectAPI();
            main.connect.access_token = url[1];
            main.connect.user_id = url[5];
            main.connect.SaveToken();
        }

        private void FormAuth_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
