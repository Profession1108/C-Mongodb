using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mario
{
    public partial class Form2 : Form
    {
        string MessageText;
        bool exit = true;
        public Form2()
        {
            InitializeComponent();
            password.UseSystemPasswordChar = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            this.Visible = false;
            frm.ShowDialog();
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<Mario.Rock1.User> collection = database.GetCollection<Mario.Rock1.User>("Alldata");
            string nametext = name.Text;
            string passwordtext = password.Text;
            var query = Query.EQ("Name", nametext);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    if (user.Name == nametext)
                    {
                        if (user.Password == passwordtext)
                        {
                            Rock1 frm = new Rock1();
                            frm.Name = nametext;
                            this.Visible = false;
                            frm.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageText = "帳號或密碼輸入錯誤";
                            MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageText = "帳號或密碼輸入錯誤";
                        MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageText = "帳號或密碼輸入錯誤";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reset1_Click(object sender, EventArgs e)
        {
            name.Text = "";
            password.Text = ""; 
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            exit = false;
            this.Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (exit) this.Close();
        }
    }
}
