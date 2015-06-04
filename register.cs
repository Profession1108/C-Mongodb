using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mario
{
    public partial class Form3 : Form
    {
        string MessageText;
        public Form3()
        {
            InitializeComponent();
            password.UseSystemPasswordChar = true;
            confirmpassword.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            try
            {
                if (password.Text != confirmpassword.Text)
                {
                    MessageText = "密碼和驗證碼輸入不同";
                    MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    server.Connect();
                    //已連線到資料庫
                    MongoDatabase database = server.GetDatabase("Mario");
                    //取得test資料表
                    MongoCollection<Mario.Rock1.User> collection = database.GetCollection<Mario.Rock1.User>("Alldata");
                    //取得collection1
                    string nametext = name.Text;
                    string agetext = age.Text;
                    string passwordtext = password.Text;
                    string emailtext = email.Text;
                    string dropdownlisttext = comboBox1.Text;

                    if (dropdownlisttext != "")
                    {
                        if (IsValidEmailAddress(emailtext) == true)
                        {
                            if (nametext != "" && agetext != "")
                            {
                                int n;
                                if (int.TryParse(agetext, out n))
                                {
                                    int agetext2 = Convert.ToInt32(agetext);
                                    /*var query = Query.EQ("Name", new BsonRegularExpression(nametext));
                                    var user = collection.FindOne(query);
                                    var save = user.Name;*/
                                    Mario.Rock1.Pass1 userpass = new Mario.Rock1.Pass1 { Pass = "1-1", Time = 300 };
                                    Mario.Rock1.Mario1 usermario = new Mario.Rock1.Mario1 { Life = 5, Money = 0 , Point = 0};
                                    Mario.Rock1.User somebody = new Mario.Rock1.User { Name = nametext, Age = agetext2, Password = passwordtext, Email = emailtext, Country = dropdownlisttext, mario = usermario , pass = userpass };
                                    collection.Insert(somebody);
                                    var query = Query.EQ("Name", nametext);
                                    var all_user = collection.Find(query);
                                    if (all_user.Count() > 1)
                                    {
                                        foreach (var user in all_user)
                                        {
                                            if (user.Name == nametext && user.Age == agetext2 && user.Id == somebody.Id)
                                            {
                                                var deletequery = Query.EQ("_id", somebody.Id);
                                                collection.Remove(deletequery);
                                                MessageText = "輸入的Name已有人使用過,請重新輸入!";
                                                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageText = "註冊成功!";
                                        MessageBox.Show(MessageText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        server.Disconnect();
                                        Form2 frm = new Form2();
                                        this.Visible = false;
                                        frm.ShowDialog();
                                        this.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageText = "E-mail輸入錯誤!";
                            MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageText = "請選擇一個城市!";
                        MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                server.Disconnect();
            }
            catch (Exception ex)
            {
                MessageText = "" + ex;
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            this.Visible = false;
            frm.ShowDialog();
            this.Close();
        }

        public bool IsValidEmailAddress(string email)
        {
            try
            {
                MailAddress ma = new MailAddress(email);                
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            name.Text = "";
            password.Text = "";
            confirmpassword.Text = "";
            age.Value = 0;
            email.Text = "";
            comboBox1.Text = "";
        }
    }
}
