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
                RefreshTable();
                RefreshInfo();
            }
        }

        public void HideItems()
        {
            this.Enabled = false;
        }

        public void ShowItems()
        {
            this.Enabled = true;
        }

        public void RefreshInfo()
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(connect.GetListPosts());

            comboBox.SelectedIndex = 0;
            string id = comboBox.SelectedItem.ToString();

            richTextBox.Text = connect.GetPost(id);
            groupBox.Text = "Редактирование поста №" + id;

            int countPosts = dataGridView.Rows.Count;
            DateTime selectedDate = monthCalendar.SelectionStart;
            int countPostsBeforeDate = 0;
            for (int i = 0; i < countPosts; i++)
            {
                if ((DateTime)dataGridView["date", i].Value < selectedDate)
                {
                    countPostsBeforeDate++;
                }
            }
            labelAll.Text = "Всего постов: " + countPosts;
            labelBefore.Text = "Из них до " + selectedDate.ToString("dd.MM.yyyy") + ": " + countPostsBeforeDate;
            numericUpDownPosts.Maximum = countPostsBeforeDate;
            progressBar.Visible = false;
            buttonDeletePosts.Visible = true;
        }

        public void RefreshInfo(string id)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(connect.GetListPosts());

            comboBox.SelectedItem = id;

            richTextBox.Text = connect.GetPost(id);
            groupBox.Text = "Редактирование поста №" + id;

            int countPosts = dataGridView.Rows.Count;
            DateTime selectedDate = monthCalendar.SelectionStart;
            int countPostsBeforeDate = 0;
            for (int i = 0; i < countPosts; i++)
            {
                if ((DateTime)dataGridView["date", i].Value < selectedDate)
                {
                    countPostsBeforeDate++;
                }
            }
            labelAll.Text = "Всего постов: " + countPosts;
            labelBefore.Text = "Из них до " + selectedDate.ToString("dd.MM.yyyy") + ": " + countPostsBeforeDate;
            numericUpDownPosts.Maximum = countPostsBeforeDate;
            progressBar.Visible = false;
            buttonDeletePosts.Visible = true;
        }

        public void RefreshTable()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            DataGridViewColumn columnId = new DataGridViewColumn();
            columnId.HeaderText = "id";
            columnId.Name = "id";
            columnId.CellTemplate = new DataGridViewTextBoxCell();

            DataGridViewColumn columnDate = new DataGridViewColumn();
            columnDate.HeaderText = "Дата публикации";
            columnDate.Name = "date";
            columnDate.CellTemplate = new DataGridViewTextBoxCell();

            DataGridViewColumn columnText = new DataGridViewColumn();
            columnText.HeaderText = "Текст поста";
            columnText.Name = "text";
            columnText.Width = 1500;
            columnText.CellTemplate = new DataGridViewTextBoxCell();

            dataGridView.Columns.Add(columnId);
            dataGridView.Columns.Add(columnDate);
            dataGridView.Columns.Add(columnText);

            PostInfo[] info = connect.GetListPostsDate();
            foreach (PostInfo row in info)
            {
                dataGridView.Rows.Add(row.id, row.dateTime, row.text.Substring(0, row.text.Length % 300));
            }
            dataGridView.Sort(dataGridView.Columns["date"], ListSortDirection.Ascending);
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
                RefreshInfo(id);
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

            RefreshInfo();
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

            RefreshTable();
            RefreshInfo();

            ShowItems();
        }

        private void buttonDeletePosts_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(numericUpDownPosts.Value);
            DateTime selectedDate = monthCalendar.SelectionStart;

            progressBar.Value = 0;
            progressBar.Maximum = count;
            progressBar.Visible = true;

            for (int i = 0; i < count; i++)
            {
                if ((DateTime)dataGridView["date", i].Value < selectedDate)
                {
                    connect.DeletePost(dataGridView["id", i].Value.ToString());

                    progressBar.Value++;
                }
            }

            progressBar.Visible = false;
            this.Enabled = false;
            RefreshTable();
            RefreshInfo();
            this.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            int countPosts = dataGridView.Rows.Count;
            DateTime selectedDate = monthCalendar.SelectionStart;
            int countPostsBeforeDate = 0;
            for (int i = 0; i < countPosts; i++)
            {
                if ((DateTime)dataGridView["date", i].Value < selectedDate)
                {
                    countPostsBeforeDate++;
                }
            }
            labelAll.Text = "Всего постов: " + countPosts;
            labelBefore.Text = "Из них до " + selectedDate.ToString("dd.MM.yyyy") + ": " + countPostsBeforeDate;
            numericUpDownPosts.Maximum = countPostsBeforeDate;
        }
    }
}