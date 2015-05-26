using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace CMongoDB
{
    public class User
    {
        /// <summary>
        /// For MongoDB Id 
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年齡
        /// </summary>
        public int Age { get; set; }
    }
    public partial class index : System.Web.UI.Page
    { 
        string tempage;
        protected void Page_Load(object sender, EventArgs e)
        {      
               string connectstring = "mongodb://127.0.0.1:27017";
               MongoClient client = new MongoClient(connectstring);
               MongoServer server = client.GetServer();
                server.Connect();
                //已連線到資料庫
                MongoDatabase database = server.GetDatabase("test");
                //取得資料表
                MongoCollection<User> collection = database.GetCollection<User>("collection1");
                //取得colletion
                var query = Query.Matches("Name", new BsonRegularExpression(""));
                var all_user = collection.Find(query);
                foreach (var user in all_user)
                {
                    Response.Write("Name:" + user.Name + " , " + "Age:" + user.Age + " , " + "Id:" + user.Id + "<br />");
                }
                //取得collection1
                /*BsonDocument inserData = new BsonDocument{
                {"name","Joe"},{"age",20}
            };
                collection.Insert(inserData);*/ 
                if (model.Text == "Delete")
                {
                    Label2.Visible = false;
                    age.Visible = false;
                    DropDownList1.Visible = false;
                    DropDownList2.Visible = false;
                    age.Text = "1";
                    DropDownList1.Items.Clear();
                    DropDownList2.Items.Clear();  
                }
                else if(model.Text == "Insert")
                {
                    Label2.Visible = true;
                    age.Visible = true;
                    DropDownList1.Visible = false;
                    DropDownList2.Visible = false;
                    DropDownList1.Items.Clear();
                    DropDownList2.Items.Clear();  
                }
                else if (model.Text == "Update" && DropDownList1.Text == "")
                {
                    Label2.Visible = true;
                    age.Visible = true;
                    DropDownList1.Visible = true;
                    DropDownList2.Visible = true;
                    foreach (var user in all_user)
                    {
                        DropDownList1.Items.Remove(new ListItem(user.Name, user.Name));
                        DropDownList1.Items.Add(new ListItem(user.Name, user.Name));
                    }
                    var query2 = Query.EQ("Name", DropDownList1.Text);
                    var all_user2 = collection.Find(query2);
                    foreach (var user in all_user2)
                    {
                        DropDownList2.Items.Add(new ListItem(user.Age.ToString(), user.Age.ToString()));
                    }
                }
                else if (model.Text == "Update" && DropDownList1.Text != "")
                {
                    Label2.Visible = true;
                    age.Visible = true;
                    DropDownList1.Visible = true;
                    DropDownList2.Visible = true;
                    DropDownList2.Items.Clear();
                    var query2 = Query.EQ("Name", DropDownList1.Text);
                    var all_user2 = collection.Find(query2);
                    foreach (var user in all_user2)
                    {
                        DropDownList2.Items.Add(new ListItem(user.Age.ToString(), user.Age.ToString()));
                    }
                }
                else if (model.Text == "Search")
                {
                    Label2.Visible = false;
                    age.Visible = false;
                    DropDownList1.Visible = false;
                    DropDownList2.Visible = false;
                    age.Text = "1";
                }
            server.Disconnect();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            try
            {
                server.Connect();
                //已連線到資料庫
                MongoDatabase database = server.GetDatabase("test");
                //取得test資料表
                MongoCollection<User> collection = database.GetCollection<User>("collection1");
                //取得collection1
                string nametext = name.Text;
                string agetext = age.Text;
                
                if (nametext != "" && agetext != "")
                {
                    int n;
                    if (int.TryParse(agetext, out n))
                    {
                        int agetext2 = Convert.ToInt32(agetext);
                        if (model.Text == "Insert")
                        {
                            /*var query = Query.EQ("Name", new BsonRegularExpression(nametext));
                            var user = collection.FindOne(query);
                            var save = user.Name;*/
                            
                            User somebody = new User { Name = nametext, Age = agetext2 };
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
                                        Response.Write("<script>alert('輸入的Name已有人使用過,請重新輸入')</script>");
                                    }
                                }
                            }
                            else
                            {
                                Response.Redirect(Request.FilePath);
                            }
                        }
                        else if(model.Text == "Delete")
                        {
                            var deletequery = Query.EQ("Name",nametext);
                            collection.Remove(deletequery); 
                            Response.Redirect(Request.FilePath);
                        }
                        else if (model.Text == "Update")
                        {                            
                            var namequery = Query.EQ("Name", DropDownList1.Text);
                            var updatenamequery = Update.Set("Name", nametext);
                            var agequery = Query.EQ("Age", Convert.ToInt32(DropDownList2.Text));
                            var updateagequery = Update.Set("Age", agetext2);                            
                            collection.Update(namequery,updatenamequery);
                            collection.Update(agequery, updateagequery);
                            Response.Redirect(Request.FilePath);
                        }
                        else if (model.Text == "Search")
                        {
                            var query = Query.EQ("Name", nametext);
                            var all_user = collection.Find(query);
                            if (all_user.Count() >= 1)
                            {
                                foreach (var user in all_user)
                                {
                                    Response.Write("已找到->Name:" + user.Name + ", Age:" + user.Age);
                                }
                            }
                            else
                            {
                                Response.Write("<script>alert('找不到此用戶')</script>");
                            }
                        }
                        name.Text = "";
                        age.Text = "";
                    }

                    else
                    {
                        Response.Write("<script>alert('輸入的age不為數字!')</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('表格中有未填項目!')</script>");
                }
                server.Disconnect();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
    }
}
