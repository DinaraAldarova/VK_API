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
using CefSharp;
using CefSharp.WinForms;

namespace VK_API
{
    public partial class MainForm : Form
    {
        public ConnectAPI connect;

        public MainForm()
        {
            InitializeComponent();
            HideItems();
            connect = new ConnectAPI();
            connect.ReadToken();
            if (!connect.available)
            {
                FormAuth formAuth = new FormAuth();
                formAuth.Owner = this;
                formAuth.ShowDialog();
            }
            if (!connect.available)
            {
                MessageBox.Show("Не удалось авторизоваться", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(connect.GetListPosts());

                comboBox.SelectedIndex = 0;
                string id = comboBox.SelectedItem.ToString();
                richTextBox.Text = connect.GetPost(id);
                groupBox.Text = "Редактирование поста №" + id;
            }
        }

        public void HideItems()
        {
            //label.Enabled = false;
            //comboBox.Enabled = false;
            //buttonCreate.Enabled = false;
            //groupBox.Enabled = false;
            ////richTextBox.Enabled = false;
            ////buttonUpdate.Enabled = false;
            ////buttonDelete.Enabled = false;
            //buttonRefresh.Enabled = false;
            this.Enabled = false;
        }

        public void ShowItems()
        {
            //label.Enabled = true;
            //comboBox.Enabled = true;
            //buttonCreate.Enabled = true;
            //groupBox.Enabled = true;
            ////richTextBox.Enabled = true;
            ////buttonUpdate.Enabled = true;
            ////buttonDelete.Enabled = true;
            //buttonRefresh.Enabled = true;
            this.Enabled = true;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            HideItems();
            string id = connect.CreatePost("New post");
            if (id == "")
            {
                MessageBox.Show("Не удалось создать пост", "Ошибка создания", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(connect.GetListPosts());
                comboBox.SelectedItem = id;
                richTextBox.Text = connect.GetPost(id);
                groupBox.Text = "Редактирование поста №" + id;
            }
            ShowItems();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            HideItems();
            bool success = connect.UpdatePost(comboBox.SelectedItem.ToString(), richTextBox.Text);
            if (!success)
            {
                MessageBox.Show("Не удалось изменить пост", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            ShowItems();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            HideItems();
            int index = comboBox.SelectedIndex;
            bool success = connect.DeletePost(comboBox.SelectedItem.ToString());
            if (!success)
            {
                MessageBox.Show("Не удалось удалить пост", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            comboBox.Items.Clear();
            comboBox.Items.AddRange(connect.GetListPosts());

            comboBox.SelectedIndex = index;
            string id = comboBox.SelectedItem.ToString();
            richTextBox.Text = connect.GetPost(id);
            groupBox.Text = "Редактирование поста №" + id;
            ShowItems();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideItems();
            string id = comboBox.SelectedItem.ToString();
            richTextBox.Text = connect.GetPost(id);
            groupBox.Text = "Редактирование поста №" + id;
            ShowItems();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            HideItems();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(connect.GetListPosts());

            comboBox.SelectedIndex = 0;
            string id = comboBox.SelectedItem.ToString();
            richTextBox.Text = connect.GetPost(id);
            groupBox.Text = "Редактирование поста №" + id;
            ShowItems();
        }
    }
}