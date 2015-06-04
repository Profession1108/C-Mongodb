using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
    public partial class Rock1 : Form
    {
        bool right, left;
        bool jump;
        bool standright, standleft;
        string judge;
        int map = 0,map2 = 0,mapchange = 0,jumpchange = 0,gameover = 0,growup = 0,grow = 1;
        int i = 0;
        int temp = 0;
        int[] aftercheck = new int[] { 0, 0, 0, 0, 0, 0, 8, 0 };
        int replay = 0,replay2 = 0,replay3 = 0;
        int jumpheight = 15;
        int state = 0; //  0為初始狀態 1為吃香菇後的狀態
        string MessageText;
        PictureBox none = new PictureBox();
        public class Pass1
        {
            //關卡
            public string Pass { get; set; }
            //每關固定關卡時間
            public int Time { get; set; }
        }
        public class Mario1
        {
            //生命
            public int Life { get; set; }
            //金錢
            public int Money { get; set; }
            //分數
            public int Point { get; set; }
        }
        public class User
        {
            // For MongoDB Id 
            [BsonId]
            public ObjectId Id { get; set; }

            // 使用者姓名
            public string Name { get; set; }
            // 年齡
            public int Age { get; set; }
            // 密碼
            public string Password { get; set; }
            // 電子郵件　
            public string Email { get; set; }
            // 城市
            public string Country { get; set; }
            // mario
            public Mario1 mario { get; set; }
            // 關卡資訊
            public Pass1 pass { get; set; }
        }

        public Rock1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<User> collection = database.GetCollection<User>("Alldata");
            gameplayer.Text = "玩家: " + Name;
            var query = Query.EQ("Name", Name);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    life.Text = "生命: " + user.mario.Life;
                    money.Text = string.Format("{0:D3}",user.mario.Money);
                    point.Text = string.Format("{0:D6}", user.mario.Point);
                    time.Text = "剩餘時間:\n    " + string.Format("{0:D3}", user.pass.Time);
                    pass.Text = "目前關卡: " + user.pass.Pass;
                }

            }
            else
            {
                MessageText = "找不到此用戶";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            map = 0;
            map2 = 0;
            jumpchange = 0;
            gameover = 0;
            temp = 0;
            replay = 0;
            replay2 = 0;
            replay3 = 0;
            jumpheight = 15;
            state = 0;
            for(i = 0; i < aftercheck.Length; i++)
            {
                if (i == 6)
                {
                    aftercheck[i] = 8;
                }
                else
                {
                    aftercheck[i] = 0;
                }
            }
            this.DoubleBuffered = true;
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            judge = "SmallSmallstand_r.png";
            //背景
            this.screen.Image = Image.FromFile("background_1.png");
            this.screen.Left = 0;
            this.screen.Width = 1230;
            this.pictureBox2.Image = Image.FromFile("background_2.png");
            this.pictureBox2.Left = 1220;
            this.pictureBox2.Width = 60;
            this.pictureBox1.Image = Image.FromFile("background_1.png");
            this.pictureBox1.Left = 1220;
            this.pictureBox1.Width = 60;
            this.pictureBox1.Visible = false;
            //玩家
            this.gameplayer.BackColor = Color.Transparent;
            this.gameplayer.Parent = this.screen;
            this.life.BackColor = Color.Transparent;
            this.life.Parent = this.screen;
            this.money.BackColor = Color.Transparent;
            this.money.Parent = this.screen;
            this.point.BackColor = Color.Transparent;
            this.point.Parent = this.screen;
            this.label1.BackColor = Color.Transparent;
            this.label1.Parent = this.screen;
            this.label3.BackColor = Color.Transparent;
            this.label3.Parent = this.screen;
            this.moneyimage.Image = Image.FromFile("Coin.png");
            this.moneyimage.BackColor = Color.Transparent;
            this.moneyimage.Parent = this.screen;
            this.pass.BackColor = Color.Transparent;
            this.pass.Parent = this.screen;
            this.time.BackColor = Color.Transparent;
            this.time.Parent = this.screen;
            this.gameplayer.Left = 56;
            this.life.Left = 790;
            this.money.Left = 523;
            this.point.Left = 198;
            this.label1.Left = 495;
            this.label3.Left = 198;
            this.moneyimage.Left = 469;
            this.pass.Left = 539;
            this.time.Left = 1010;
            //map1
            map01on();
            this.player.BackColor = Color.Transparent;
            this.player.Parent = this.screen;
            this.player.Left= 142;
            this.player.Top = screen.Height - 127;
            player.Width = 30;
            player.Height = 30;
            this.Coin.Image = Image.FromFile("Coin.png");
            this.Coin.BackColor = Color.Transparent;
            this.Coin.Parent = this.screen;
            this.Box1.Image = Image.FromFile("Box.png");
            this.Box1.BackColor = Color.Transparent;
            this.Box1.Parent = this.screen;
            this.Coin2.Image = Image.FromFile("Coin.png");
            this.Coin2.BackColor = Color.Transparent;
            this.Coin2.Parent = this.screen;
            this.Box2.Image = Image.FromFile("Box.png");
            this.Box2.BackColor = Color.Transparent;
            this.Box2.Parent = this.screen;
            this.Coin3.Image = Image.FromFile("Coin.png");
            this.Coin3.BackColor = Color.Transparent;
            this.Coin3.Parent = this.screen;
            this.Box3.Image = Image.FromFile("Box.png");
            this.Box3.BackColor = Color.Transparent;
            this.Box3.Parent = this.screen;
            this.Coin4.Image = Image.FromFile("Coin.png");
            this.Coin4.BackColor = Color.Transparent;
            this.Coin4.Parent = this.screen;
            this.Box4.Image = Image.FromFile("Box.png");
            this.Box4.BackColor = Color.Transparent;
            this.Box4.Parent = this.screen;
            this.Rock.Image = Image.FromFile("Rock.png");
            this.Rock.BackColor = Color.Transparent;
            this.Rock.Parent = this.screen;
            this.Rock2.Image = Image.FromFile("Rock.png");
            this.Rock2.BackColor = Color.Transparent;
            this.Rock2.Parent = this.screen;
            this.Rock3.Image = Image.FromFile("Rock.png");
            this.Rock3.BackColor = Color.Transparent;
            this.Rock3.Parent = this.screen;
            //map2
            this.Box5.Image = Image.FromFile("Box.png");
            this.Box5.BackColor = Color.Transparent;
            this.Box6.Image = Image.FromFile("Box.png");
            this.Box6.BackColor = Color.Transparent;
            this.Box7.Image = Image.FromFile("Box.png");
            this.Box7.BackColor = Color.Transparent;
            this.Rock4.Image = Image.FromFile("Rock.png");
            this.Rock4.BackColor = Color.Transparent;
            this.Rock5.Image = Image.FromFile("Rock.png");
            this.Rock5.BackColor = Color.Transparent;
            this.Coin5.Image = Image.FromFile("Coin.png");
            this.Coin5.BackColor = Color.Transparent;
            this.Coin6.Image = Image.FromFile("Coin.png");
            this.Coin6.BackColor = Color.Transparent;
            this.mushroom.Image = Image.FromFile("mushroom.gif");
            this.mushroom.BackColor = Color.Transparent;
            this.Box5.Parent = this.pictureBox2;
            this.Box6.Parent = this.pictureBox2;
            this.Box7.Parent = this.pictureBox2;
            this.Rock4.Parent = this.pictureBox2;
            this.Rock5.Parent = this.pictureBox2;
            this.Coin5.Parent = this.pictureBox2;
            this.Coin6.Parent = this.pictureBox2;
            this.mushroom.Parent = this.pictureBox2;
            this.mushroom.Height = 28;
            this.mushroom.Width = 28;
            this.mushroom.Left = 460;
            this.mushroom.Top = 340;
            this.mushroom.Visible = false;
            axWindowsMediaPlayer1.URL = "music.mp3";
            axWindowsMediaPlayer2.URL = "Coin_Sound.mp3";
            axWindowsMediaPlayer2.settings.volume = 100;
            axWindowsMediaPlayer2.Ctlcontrols.stop();
            axWindowsMediaPlayer3.URL = "Jump_Sound.mp3";
            axWindowsMediaPlayer3.settings.volume = 10;
            axWindowsMediaPlayer3.Ctlcontrols.stop();
            axWindowsMediaPlayer4.URL = "Die_Sound.mp3";
            axWindowsMediaPlayer4.settings.volume = 100;
            axWindowsMediaPlayer4.settings.rate = 1;
            axWindowsMediaPlayer4.Ctlcontrols.stop();
            server.Disconnect();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameover == 1 || growup == 1)
            {
            }
            else
            {
                if (e.KeyCode == Keys.Right)
                {
                    right = true;
                    standright = true;
                    timer2.Enabled = true;
                }
                if (e.KeyCode == Keys.Left)
                {
                    left = true;
                    standleft = true;
                    timer2.Enabled = true;
                }
                if (e.KeyCode == Keys.Escape) { this.Close(); } //Esc -> exit

                if (jump != true)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        if (jumpchange == 2)
                        {
                        }
                        else
                        {
                            jump = true;
                            jumpheight = 22;
                            axWindowsMediaPlayer3.Ctlcontrols.play();
                            //初始狀態的往右邊跳
                            //吃香菇後的往右邊跳
                            if (judge == "Smallstand_r.png")
                            {
                                judge = "Smalljump_r.png";
                                standright = true;
                                timer5.Enabled = true;
                            }
                            else if (judge == "Bigstand_r.png")
                            {
                                judge = "Bigjump_r.png";
                                standright = true;
                                timer5.Enabled = true;
                            }
                            //初始狀態的往左邊跳
                            //吃香菇後的往左邊跳
                            if (judge == "Smallstand_l.png")
                            {
                                judge = "Smalljump_l.png";
                                standleft = true;
                                timer5.Enabled = true;
                            }
                            else if (judge == "Bigstand_l.png")
                            {
                                judge = "Bigjump_l.png";
                                standleft = true;
                                timer5.Enabled = true;
                            }
                        }
                    }
                }
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (gameover == 1 || growup == 1)
            {
            }
            else
            {
                if (e.KeyCode == Keys.Right)
                {
                    right = false;
                    if (state == 0)
                    {
                        judge = "Smallstand_r.png";
                    }
                    else if (state == 1)
                    {
                        judge = "Bigstand_r.png";
                    }
                    timer3.Enabled = true;
                }
                if (e.KeyCode == Keys.Left)
                {
                    left = false;
                    if (state == 0)
                    {
                        judge = "Smallstand_l.png";
                    }
                    else if (state == 1)
                    {
                        judge = "Bigstand_l.png";
                    }
                    timer3.Enabled = true;
                }
            }
        }
        private void rlcollision(PictureBox player,PictureBox block)
        {
            //Right Side Collision
            if (block.Left - 4 <= player.Right && block.Right >= player.Right && block.Top <= player.Bottom - 4 && block.Bottom >= player.Top)
            {
                right = false;
            }
            //Left Side Collision
            if (block.Right + 6 >= player.Left && block.Left <= player.Left && block.Top <= player.Bottom - 4 && block.Bottom >= player.Top)
            {
                left = false;
            }
        }
        private void tbcollision(PictureBox player, PictureBox block,int coin,bool check,int state)
        {
            // Top Side Collision
            if (player.Left + player.Width >= block.Left && player.Left + player.Width <= block.Left + block.Width + player.Width && player.Top + player.Height >= block.Top && player.Top <= block.Top)
            {
                if (state == 1)
                {
                    player.Top = block.Location.Y - player.Height;
                }
                else
                {
                    jump = false;
                    jumpheight = 0;
                    player.Top = block.Location.Y - player.Height;
                }
            }
            //Bottom Side Collision
            if (block.Bottom >= player.Top - 4 && block.Right >= player.Right - player.Size.Width && block.Left <= player.Left + player.Size.Width && block.Top <= player.Top + 4)
            {
                if (state == 1)
                {
                }
                else
                {
                    jumpheight = -1;
                    switch (coin)
                    {
                        case 1:
                            if (check == true && aftercheck[1] != 1)
                            {
                                Coin.Visible = true;
                                Box1.Image = Image.FromFile("Box_check.png");
                                AddCoin();
                                aftercheck[1] = 1;
                                timer7.Enabled = true;
                            }
                            break;
                        case 2:
                            if (check == true && aftercheck[2] != 1)
                            {
                                Coin2.Visible = true;
                                Box2.Image = Image.FromFile("Box_check.png");
                                AddCoin();
                                aftercheck[2] = 1;
                                timer7.Enabled = true;
                            }
                            break;
                        case 3:
                            if (check == true && aftercheck[3] != 1)
                            {
                                Coin3.Visible = true;
                                Box3.Image = Image.FromFile("Box_check.png");
                                AddCoin();
                                aftercheck[3] = 1;
                                timer7.Enabled = true;
                            }
                            break;
                        case 4:
                            if (check == true && aftercheck[4] != 1)
                            {
                                Coin4.Visible = true;
                                Box4.Image = Image.FromFile("Box_check.png");
                                AddCoin();
                                aftercheck[4] = 1;
                                timer7.Enabled = true;
                            }
                            break;
                        case 5:
                            if (check == true && aftercheck[5] != 1)
                            {
                                Coin5.Visible = true;
                                Box5.Image = Image.FromFile("Box_check.png");
                                AddCoin();
                                aftercheck[5] = 1;
                                timer7.Enabled = true;
                            }
                            break;
                        case 6:
                            if (check == true && aftercheck[6] != 1)
                            {
                                Coin6.Visible = true;
                                aftercheck[6] -= 1;
                                AddCoin();
                                if (aftercheck[6] == 2)
                                {
                                    Box6.Image = Image.FromFile("Box_check.png");
                                    aftercheck[6] = 1;
                                }
                                timer7.Enabled = true;
                            }
                            break;
                        case 7:
                            if (check == true && aftercheck[7] != 1)
                            {
                                Box7.Image = Image.FromFile("Box_check.png");
                                mushroom.Visible = true;
                                timer11.Enabled = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Allrlcollision(PictureBox something)
        {
            //map1
            rlcollision(something, Box1);
            rlcollision(something, Box2);
            rlcollision(something, Box3);
            rlcollision(something, Box4);
            rlcollision(something, Rock);
            rlcollision(something, Rock2);
            rlcollision(something, Rock3);
            //map2
            rlcollision(something, Box5);
            rlcollision(something, Box6);
            rlcollision(something, Box7);
            rlcollision(something, Rock4);
            rlcollision(something, Rock5);
        }

        private void Alltbcollision(PictureBox something, int state)
        {
            //map1
            tbcollision(something, Box1, 1, true, state);
            tbcollision(something, Box2, 2, true, state);
            tbcollision(something, Box3, 3, true, state);
            tbcollision(something, Box4, 4, true, state);
            tbcollision(something, Rock, 1, false, state);
            tbcollision(something, Rock2, 2, false, state);
            tbcollision(something, Rock3, 3, false, state);
            //map2
            tbcollision(something, Box5, 5, true, state);
            tbcollision(something, Box6, 6, true, state);
            tbcollision(something, Box7, 7, true, state);
            tbcollision(something, Rock4, 4, false, state);
            tbcollision(something, Rock5, 5, false, state);
        }

        private void dropposition(PictureBox player, PictureBox block)
        {
            if (player.Top + player.Height >= background_1.Top)
            {
                if (mapchange == 2)
                {
                    if (player.Left >= 826 && player.Right - player.Width <= 931 )
                    {
                        jump = false;
                        timer9.Enabled = true;
                    }
                    else
                    {
                        player.Top = background_1.Top - player.Height;
                        jump = false;
                    }
                }
                else
                {
                    if (mapchange == 2)
                    {
                    }
                    else
                    {
                        player.Top = background_1.Top - player.Height;
                        jump = false;
                    }
                }
            }
            else if (player.Top + player.Height <= block.Top)
            {
                if (player.Right < block.Left || player.Left > block.Right)
                {
                    if (mapchange == 2)
                    {
                    }
                    else
                    {
                        player.Top += 5;
                    }
                }
            }
            else
            {
                player.Top += 5;
            }
        }

        private void dropholeRight(PictureBox player)
        {
            if (mapchange == 2)
            {
                if (player.Left >= 826 && player.Right - player.Width <= 931 && player.Bottom >= pictureBox2.Height - background_1.Height)
                {
                    jumpchange = 2;
                    timer9.Enabled = true;
                    if (player.Right - player.Width >= 931)
                    {
                        player.Left = player.Left - 7;
                    }
                }
            }
        }

        private void dropholeLeft(PictureBox player)
        {
            if (mapchange == 2)
            {
                if (player.Left >= 826 && player.Right - player.Width <= 931 && player.Bottom >= pictureBox2.Height - background_1.Height)
                {
                    jumpchange = 2;
                    timer9.Enabled = true;
                    if (player.Left <= 826)
                    {
                        player.Left = player.Left + 7;
                    }
                }
            }
        }

        private void changemap(PictureBox before, PictureBox screen, PictureBox pictureBox2, PictureBox pictureBox1)
        {
            if (right == true)
            {
                if (temp == map - 1)
                {
                    before.Left -= 3;
                    screen.Left -= 3;
                    pictureBox2.Left -= 3;
                    pictureBox2.Width += 3;
                    if (pictureBox2.Location.X <= 0)
                    {
                        pictureBox2.Left = 0;
                        pictureBox2.Width = 1230;

                        pictureBox1.Left = 1230;
                        pictureBox1.Width = 50;
                        temp = map;
                        map++;
                    }
                    if (player.Left + 15 >= 1230)
                    {
                        if (mapchange == 2)
                        {
                            map02off();
                            //map03on();
                        }
                        else
                        {
                            map01off();
                            map02on();
                        }
                        this.player.Parent = pictureBox2;
                        fixedposition1230(pictureBox2);
                        player.Left = 0;
                        map2++;
                    }
                }
                else
                {
                    screen.Left -= 3;
                    pictureBox2.Left -= 3;
                    pictureBox2.Width += 3;
                    if (pictureBox2.Location.X <= 0)
                    {
                        pictureBox2.Left = 0;
                        pictureBox2.Width = 1230;

                        pictureBox1.Left = 1230;
                        pictureBox1.Width = 50;
                        temp = map;
                        map++;
                    }
                    if (player.Left + 15 >= 1230)
                    {
                        if (mapchange == 2)
                        {
                            map02off();
                        }
                        else
                        {
                            map01off();
                            map02on();
                        }
                        this.player.Parent = pictureBox2;
                        fixedposition1230(pictureBox2);
                        player.Left = 0;
                        map2++;
                    }
                }
            }
            if (left == true)
            {
                if (map2 == 0)
                {
                    this.player.BackColor = Color.Transparent;
                    this.player.Parent = screen;
                    if (screen.Left > 0)
                    {
                        screen.Left -= 3;
                        pictureBox2.Left -= 3;
                        pictureBox2.Width += 3;
                    }
                    else
                    {
                        screen.Left += 3;
                        pictureBox2.Left += 3;
                    }
                    if (player.Left - 15 < 0)
                    {
                        map01on();
                        this.player.Parent = screen;
                        fixedposition0(screen);
                        player.Left = 0;
                    }
                }
                else if (map2 == 1)
                {
                    if (temp == map - 1)
                    {
                        if (before.Left < 0)
                        {
                            before.Left += 3;
                            screen.Left += 3;
                            pictureBox2.Left += 3;
                        }
                        else if (before.Left >= 0)
                        {
                        }
                        if (player.Left - 15 < 0)
                        {
                            map01on();
                            this.player.Parent = before;
                            fixedposition0(before);
                            player.Left = 1230;
                            map--;
                            map2--;
                        }
                    }
                    else
                    {
                        if (screen.Left > 0)
                        {
                        }
                        else
                        {
                            screen.Left += 3;
                            pictureBox2.Left += 3;
                            pictureBox2.Width -= 3;
                        }
                        if (player.Left - 15 < 0)
                        {
                            map01on();
                            this.player.Parent = screen;
                            fixedposition0(screen);
                            player.Left = 1230;
                            map2--;
                        }
                    }
                }
                else if (map2 == 2)
                {
                    if (screen.Left > 0)
                    {
                    }
                    else
                    {
                        before.Left += 3;
                        screen.Left += 3;
                        pictureBox2.Left += 3;
                        pictureBox2.Width -= 3;
                    }
                    if (player.Left - 15 < 0)
                    {
                        map02on();
                        this.player.Parent = screen;
                        fixedposition0(screen);
                        player.Left = 1230;
                        map2--;
                    }
                }
            }
        }

        private void changemap2(PictureBox before, PictureBox screen, PictureBox pictureBox2, PictureBox pictureBox1)
        {
            if (right == true)
            {
                if (screen.Left > 200)
                {
                    before.Left -= 3;
                    screen.Left -= 3;
                    pictureBox2.Left -= 3;
                }
                if (before.Left <= 40)
                {
                    screen.Width = 1270;
                }
                if (player.Left + 15 >= 1230)
                {
                    if (mapchange == 2)
                    {
                        map02off();
                    }
                    else if (mapchange == 3)
                    {
                    }
                    else
                    {
                        map01off();
                    }
                    this.player.Parent = screen;
                    fixedposition1230(screen);
                    player.Left = 1010;
                }

            }
            if (left == true)
            {
                if (before.Left < 0)
                {
                    before.Left += 3;
                    screen.Left += 3;
                    pictureBox2.Left += 3;
                }
                else if (before.Left >= 0)
                {
                }
                if (player.Left - 15 < 0)
                {
                    map02on();
                    this.player.Parent = before;
                    fixedposition0(before);
                    player.Left = 1230;
                    temp = map - 2;
                    map--;
                    map2--;
                }
            }
        }

        private void map01on()
        {
            //map1
            Box1.Width = 33;
            Box1.Height = 33;
            Box1.Left = 321;
            Box1.Top = 496;
            Box2.Width = 33;
            Box2.Height = 33;
            Box2.Left = 480;
            Box2.Top = 496;
            Box3.Width = 33;
            Box3.Height = 33;
            Box3.Left = 544;
            Box3.Top = 496;
            Box4.Width = 33;
            Box4.Height = 33;
            Box4.Left = 511;
            Box4.Top = 368;
            Coin.Width = 21;
            Coin.Height = 30;
            Coin.Left = 327;
            Coin.Top = 467;
            Coin2.Width = 21;
            Coin2.Height = 30;
            Coin2.Left = 486;
            Coin2.Top = 466;
            Coin3.Width = 21;
            Coin3.Height = 30;
            Coin3.Left = 550;
            Coin3.Top = 467;
            Coin4.Width = 21;
            Coin4.Height = 30;
            Coin4.Left = 517;
            Coin4.Top = 338;
            Rock.Width = 33;
            Rock.Height = 33;
            Rock.Left = 447;
            Rock.Top = 496;
            Rock2.Width = 33;
            Rock2.Height = 33;
            Rock2.Left = 511;
            Rock2.Top = 496;
            Rock3.Width = 33;
            Rock3.Height = 33;
            Rock3.Left = 577;
            Rock3.Top = 496;
            map02off();
            mapchange = 0;
            //map2
            /*Box1.Enabled = true;Box2.Enabled = true;Box3.Enabled = true;Box4.Enabled = true;Coin.Enabled = true;Coin2.Enabled = true;Coin3.Enabled = true;Coin4.Enabled = true;Rock.Enabled = true;Rock2.Enabled = true;Rock3.Enabled = true;Box1.Visible = true;Box2.Visible = true;Box3.Visible = true;Box4.Visible = true;Coin.Visible = true;Coin2.Visible = true;Coin3.Visible = true;Coin4.Visible = true;Rock.Visible = true;Rock2.Visible = true;Rock3.Visible = true;*/
        }

        private void map02on()
        {
            Box5.Width = 33;
            Box5.Height = 33;
            Box5.Left = 423;
            Box5.Top = 463;
            Box6.Width = 33;
            Box6.Height = 33;
            Box6.Left = 496;
            Box6.Top = 463;
            Box7.Width = 33;
            Box7.Height = 33;
            Box7.Left = 457;
            Box7.Top = 368;
            Coin5.Width = 21;
            Coin5.Height = 30;
            Coin5.Left = 428;
            Coin5.Top = 433;
            Coin6.Width = 21;
            Coin6.Height = 30;
            Coin6.Left = 502;
            Coin6.Top = 433;
            Rock4.Width = 33;
            Rock4.Height = 33;
            Rock4.Left = 390;
            Rock4.Top = 496;
            Rock5.Width = 33;
            Rock5.Height = 33;
            Rock5.Left = 528;
            Rock5.Top = 497;
            mapchange = 2;
        }
        private void map01off()
        {
            Box1.Width = 0;
            Box1.Height = 0;
            Box1.Left = 0;
            Box1.Top = 0;
            Box2.Width = 0;
            Box2.Height = 0;
            Box2.Left = 0;
            Box2.Top = 0;
            Box3.Width = 0;
            Box3.Height = 0;
            Box3.Left = 0;
            Box3.Top = 0;
            Box4.Width = 0;
            Box4.Height = 0;
            Box4.Left = 0;
            Box4.Top = 0;
            Coin.Width = 0;
            Coin.Height = 0;
            Coin.Left = 0;
            Coin.Top = 0;
            Coin2.Width = 0;
            Coin2.Height = 0;
            Coin2.Left = 0;
            Coin2.Top = 0;
            Coin3.Width = 0;
            Coin3.Height = 0;
            Coin3.Left = 0;
            Coin3.Top = 0;
            Coin4.Width = 0;
            Coin4.Height = 0;
            Coin4.Left = 0;
            Coin4.Top = 0;
            Rock.Width = 0;
            Rock.Height = 0;
            Rock.Left = 0;
            Rock.Top = 0;
            Rock2.Width = 0;
            Rock2.Height = 0;
            Rock2.Left = 0;
            Rock2.Top = 0;
            Rock3.Width = 0;
            Rock3.Height = 0;
            Rock3.Left = 0;
            Rock3.Top = 0;
            mapchange = 2;
        }

        private void map02off()
        {
            Box5.Width = 0;
            Box5.Height = 0;
            Box5.Left = 0;
            Box5.Top = 0;
            Box6.Width = 0;
            Box6.Height = 0;
            Box6.Left = 0;
            Box6.Top = 0;
            Box7.Width = 0;
            Box7.Height = 0;
            Box7.Left = 0;
            Box7.Top = 0;
            Coin5.Width = 0;
            Coin5.Height = 0;
            Coin5.Left = 0;
            Coin5.Top = 0;
            Coin6.Width = 0;
            Coin6.Height = 0;
            Coin6.Left = 0;
            Coin6.Top = 0;
            Rock4.Width = 0;
            Rock4.Height = 0;
            Rock4.Left = 0;
            Rock4.Top = 0;
            Rock5.Width = 0;
            Rock5.Height = 0;
            Rock5.Left = 0;
            Rock5.Top = 0;
            mapchange = 3;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Allrlcollision(player);

            //只按右鍵
            if (right == true)
            {
                if (mapchange == 2)
                {
                    dropholeRight(player);
                }
                this.player.Left += 7;

            }
            //只按左鍵
            if (left == true) 
            {
                if (mapchange == 2)
                {
                    dropholeLeft(player);
                }
                this.player.Left -= 7;
            } 
            
            if (jump == true)
            {
                player.Top -= jumpheight;
                jumpheight -= 1;
            }
            if (map > 1)
            {
                if (player.Right - player.Width*2 >= pictureBox1.Right)
                {
                    player.Left = pictureBox1.Right - player.Width*2;
                }
            }
            
            if (mapchange == 2)
            {
                dropposition(player, Box1);
            }
            else
            {
                dropposition(player, Box1);
            }

            Alltbcollision(player, 0);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (right == true && timer4.Enabled == false)
            {
                if (state == 0)
                {
                    player.Image = Image.FromFile("Smallwalk_r.gif");
                    judge = "Smallstand_r.png";
                }
                else if (state == 1)
                {
                    player.Image = Image.FromFile("Bigwalk_r.gif");
                    judge = "Bigstand_r.png";
                }
                timer2.Dispose();
                timer4.Enabled = true;                
            }
            if (left == true && timer4.Enabled == false)
            {
                if (state == 0)
                {
                    player.Image = Image.FromFile("Smallwalk_l.gif");
                    judge = "Smallstand_l.png";
                }
                else if (state == 1)
                {
                    player.Image = Image.FromFile("Bigwalk_l.gif");
                    judge = "Bigstand_l.png";
                }
                timer2.Dispose();
                timer4.Enabled = true;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (right == false && standright == true)
            {
                if (judge == "Smallstand_r.png")
                {
                    player.Image = Image.FromFile("Smallstand_r.png");
                    judge = "Smallstand_r.png";
                }
                else if (judge == "Bigstand_r.png")
                {
                    player.Image = Image.FromFile("Bigstand_r.png");
                    judge = "Bigstand_r.png";
                }
                standright = false;
                timer3.Dispose();
            }
            if (left == false && standleft == true)
            {
                if (judge == "Smallstand_l.png")
                {
                    player.Image = Image.FromFile("Smallstand_l.png");
                    judge = "Smallstand_l.png";
                }
                else if (judge == "Bigstand_l.png")
                {
                    player.Image = Image.FromFile("Bigstand_l.png");
                    judge = "Bigstand_l.png";
                }
                standleft = false;
                timer3.Dispose();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            replay++;
            if (right == true && replay % 10 == 0)
            {
                replay = 0;
                if (judge == "Smallstand_r.png")
                {
                    player.Image = Image.FromFile("Smallwalk_r.gif");
                }
                else if (judge == "Bigstand_r.png")
                {
                    player.Image = Image.FromFile("Bigwalk_r.gif");
                }
                timer4.Dispose();
            }
            if (left == true && replay % 10 == 0)
            {
                replay = 0;
                if (judge == "Smallstand_l.png")
                {
                    player.Image = Image.FromFile("Smallwalk_l.gif");
                }
                else if (judge == "Bigstand_l.png")
                {
                    player.Image = Image.FromFile("Bigwalk_l.gif");
                }
                timer4.Dispose();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            if (standright == true)
            {
                //切換初始角色往右跳躍圖
                if (judge == "Smalljump_r.png")
                {
                    player.Image = Image.FromFile("Smalljump_r.png");
                    timer5.Dispose();
                    timer6.Enabled = true;
                }
                //切換吃香菇後角色往右跳躍圖
                else if (judge == "Bigjump_r.png")
                {
                    player.Image = Image.FromFile("Bigjump_r.png");
                    timer5.Dispose();
                    timer6.Enabled = true;
                }
            }
            if (standleft == true)
            {
                //切換初始角色往左跳躍圖
                if (judge == "Smalljump_l.png")
                {
                    player.Image = Image.FromFile("Smalljump_l.png");
                    timer5.Dispose();
                    timer6.Enabled = true;
                }
                //切換吃香菇後角色往左跳躍圖
                else if (judge == "Bigjump_l.png")
                {
                    player.Image = Image.FromFile("Bigjump_l.png");
                    timer5.Dispose();
                    timer6.Enabled = true;
                }
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            //切換初始角色往右跳躍完後向右站立圖
            if (judge == "Smalljump_r.png")
            {
                player.Image = Image.FromFile("Smallstand_r.png");
                standright = true;
                judge = "Smallstand_r.png";          
                timer6.Dispose();
                //為了讓跳躍完還能切換往右跑的圖
                timer4.Enabled = true;
            }
            //切換吃香菇後角色往右跳躍完後向右站立圖
            else if (judge == "Bigjump_r.png")
            {
                player.Image = Image.FromFile("Bigstand_r.png");
                standright = true;
                judge = "Bigstand_r.png";
                timer6.Dispose();
                //為了讓跳躍完還能切換往右跑的圖
                timer4.Enabled = true;
            }
            //切換初始角色往左跳躍完後向左站立圖
            if (judge == "Smalljump_l.png")
            {
                player.Image = Image.FromFile("Smallstand_l.png");
                standleft = true;
                judge = "Smallstand_l.png";
                timer6.Dispose();
                //為了讓跳躍完還能切換往左跑的圖
                timer4.Enabled = true;
            }
            //切換吃香菇後角色往左跳躍完後向左站立圖
            else if (judge == "Bigjump_l.png")
            {
                player.Image = Image.FromFile("Bigstand_l.png");
                standleft = true;
                judge = "Bigstand_l.png";
                timer6.Dispose();
                //為了讓跳躍完還能切換往左跑的圖
                timer4.Enabled = true;
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            Coin.Visible = false;
            Coin2.Visible = false;
            Coin3.Visible = false;
            Coin4.Visible = false;
            Coin5.Visible = false;
            Coin6.Visible = false;
            axWindowsMediaPlayer2.settings.rate = 8;
            axWindowsMediaPlayer2.Ctlcontrols.play();
            timer7.Dispose();
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            if (map == 0)
            {
                changemap(none,screen, pictureBox2 , pictureBox1);         
            }
            else if (map == 1)
            {
                pictureBox1.Visible = true;                
                changemap(screen,pictureBox2, pictureBox1,none);
            }
            else
            {
                changemap2(pictureBox2, pictureBox1, screen, none);
            }
        }

        private void timer9_Tick(object sender, EventArgs e)
        {
            player.Top += 5;
            if (player.Bottom >= pictureBox2.Height)
            {
                player.Image = Image.FromFile("die.gif"); 
                gameover = 1;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer4.Ctlcontrols.play();
                timer10.Dispose();
                timer9.Dispose();
                timer8.Dispose();
                timer7.Dispose();
                timer6.Dispose();
                timer5.Dispose();
                timer4.Dispose();
                timer3.Dispose();
                timer2.Dispose();
                timer1.Dispose();
                timer10.Enabled = true;
                timer9.Enabled = false;
                timer8.Enabled = false;
                timer7.Enabled = false;
                timer6.Enabled = false;
                timer5.Enabled = false;
                timer4.Enabled = false;
                timer3.Enabled = false;
                timer2.Enabled = false;
                timer1.Enabled = false;
            }
        }

        private void timer10_Tick(object sender, EventArgs e)
        {
            if (replay2 > 8)
            {
                player.Top += 10;
                replay2++;
                if (replay2 > 30)
                {
                    MinusLife();
                    ZeroLife();
                    timer10.Enabled = false;
                    mapchange = 0;
                    this.player.Image = Image.FromFile("Smallstand_r.png");
                    right = false;
                    left = false;
                    jump = false;
                    timer1.Enabled = true;
                    timer8.Enabled = true;
                    Form1_Load(sender,e);
                }
            }
            else if (replay2 == 0)
            {
                player.Top = pictureBox2.Height - background_1.Height*2;
                replay2++;
            }
            else
            {
                player.Top -= 5;
                replay2++;
            }
        }

        private void fixedposition1230(PictureBox screen)
        {
            this.gameplayer.Parent = screen;
            this.life.Parent = screen;
            this.money.Parent = screen;
            this.point.Parent = screen;
            this.label1.Parent = screen;
            this.label3.Parent = screen;
            this.moneyimage.Parent = screen;
            this.pass.Parent = screen;
            this.time.Parent = screen;
            this.gameplayer.Left = 56;
            this.life.Left = 790;
            this.money.Left = 523;
            this.point.Left = 198;
            this.label1.Left = 495;
            this.label3.Left = 198;
            this.moneyimage.Left = 469;
            this.pass.Left = 539;
            this.time.Left = 1010;
        }

        private void fixedposition0(PictureBox screen) 
        {
            this.gameplayer.Parent = screen;
            this.life.Parent = screen;
            this.money.Parent = screen;
            this.point.Parent = screen;
            this.label1.Parent = screen;
            this.label3.Parent = screen;
            this.moneyimage.Parent = screen;
            this.pass.Parent = screen;
            this.time.Parent = screen;
            this.gameplayer.Left = 56;
            this.life.Left = 790;
            this.money.Left = 523;
            this.point.Left = 198;
            this.label1.Left = 495;
            this.label3.Left = 198;
            this.moneyimage.Left = 469;
            this.pass.Left = 539;
            this.time.Left = 1010;
        }

        private void Left1230(Label label, PictureBox screen)
        {
            if (player.Left + 15 >= 1230)
            {
                label.Parent = screen;
                if (label.Name == gameplayer.Name)
                {
  
                    this.gameplayer.Left = 56;
                }
                else if (label.Name == life.Name)
                {
                    this.life.Left = 790;
                }
                else if (label.Name == money.Name)
                {
                    this.money.Left = 523;
                }
                else if (label.Name == point.Name)
                {
                    this.point.Left = 198;
                }
                else if (label.Name == label1.Name)
                {
                    this.label1.Left = 495;
                }
                else if (label.Name == label3.Name)
                {
                    this.label3.Left = 198;
                }
            }
        }

        private void Left0(Label label, PictureBox screen)
        {
            if (player.Left - 15 < 0)
            {
                label.Parent = screen;
                if (label.Name == gameplayer.Name)
                {
                    this.gameplayer.Left = 56;
                }
                else if (label.Name == life.Name)
                {
                    this.life.Left = 790;
                }
                else if (label.Name == money.Name)
                {
                    this.money.Left = 523;
                }
                else if (label.Name == point.Name)
                {
                    this.point.Left = 198;
                }
                else if (label.Name == label1.Name)
                {
                    this.label1.Left = 495;
                }
                else if (label.Name == label3.Name)
                {
                    this.label3.Left = 198;
                }
            }
        }

        private void Left1230(PictureBox label, PictureBox screen)
        {
            if (label.Left + 15 >= 1230)
            {
                label.Parent = screen;
                this.moneyimage.Left = 469;
            }
        }

        private void Left0(PictureBox label, PictureBox screen)
        {
            if (label.Left - 15 < 0)
            {
                label.Parent = screen;
                this.moneyimage.Left = 469;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void AddCoin()
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<User> collection = database.GetCollection<User>("Alldata");
            var query = Query.EQ("Name", Name);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    user.mario.Money += 1;
                    collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Money, user.mario.Money));
                    money.Text = string.Format("{0:D3}", user.mario.Money);
                    user.mario.Point += 100;
                    collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Point, user.mario.Point));
                    point.Text = string.Format("{0:D6}", user.mario.Point);
                }

            }
            else
            {
                MessageText = "找不到此用戶";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            server.Disconnect();
        }

        private void MinusLife()
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<User> collection = database.GetCollection<User>("Alldata");
            var query = Query.EQ("Name", Name);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    user.mario.Life -= 1;
                    collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Life, user.mario.Life));
                    life.Text = "生命: " + user.mario.Life;
                }

            }
            else
            {
                MessageText = "找不到此用戶";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            server.Disconnect();
        }

        private void ZeroLife()
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<User> collection = database.GetCollection<User>("Alldata");
            var query = Query.EQ("Name", Name);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    if (user.mario.Life <= 0)
                    {
                        user.mario.Life = 5;
                        collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Life, user.mario.Life));
                        life.Text = "生命: " + user.mario.Life;
                        user.mario.Money = 0;
                        collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Money, user.mario.Money));
                        money.Text = string.Format("{0:D3}", user.mario.Money);
                        user.mario.Point = 0;
                        collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Point, user.mario.Point));
                        point.Text = string.Format("{0:D6}", user.mario.Point);
                    }
                }

            }
            else
            {
                MessageText = "找不到此用戶";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            server.Disconnect();
        }

        private void timer11_Tick(object sender, EventArgs e)
        {
            //Right Side Collision
            if (mushroom.Left - 4 <= player.Right && mushroom.Right >= player.Right && mushroom.Top <= player.Bottom - 4 && mushroom.Bottom >= player.Top)
            {
                mushroom.Width = 0;
                mushroom.Height = 0;
                mushroom.Visible = false;
                PlayerGrowup();
                MushroomPoint();
                timer11.Enabled = false;
            }
            //Left Side Collision
            if (mushroom.Right + 6 >= player.Left && mushroom.Left <= player.Left && mushroom.Top <= player.Bottom - 4 && mushroom.Bottom >= player.Top)
            {
                mushroom.Width = 0;
                mushroom.Height = 0;
                mushroom.Visible = false;
                PlayerGrowup();
                MushroomPoint();
                timer11.Enabled = false;
            }
            // Top Side Collision

            if (player.Left + player.Width >= mushroom.Left && player.Left + player.Width <= mushroom.Left + mushroom.Width + player.Width && player.Top + player.Height >= mushroom.Top && player.Top <= mushroom.Top)
            {
                mushroom.Width = 0;
                mushroom.Height = 0;
                mushroom.Visible = false;
                PlayerGrowup();
                MushroomPoint();
                timer11.Enabled = false;
            }
            //Bottom Side Collision
            if (mushroom.Bottom >= player.Top - 4 && mushroom.Right >= player.Right - player.Size.Width && mushroom.Left <= player.Left + player.Size.Width && mushroom.Top <= player.Top + 4)
            {
                mushroom.Width = 0;
                mushroom.Height = 0;
                mushroom.Visible = false;
                PlayerGrowup();
                MushroomPoint();
                timer11.Enabled = false;
            }
            Allrlcollision(mushroom);
            Alltbcollision(mushroom,1);
            if (mushroom.Top + mushroom.Height >= background_1.Top)
            {
                if (mushroom.Left >= 826 && mushroom.Right - mushroom.Width <= 931)
                {
                    mushroom.Left += 2;
                    mushroom.Top += 5;
                }
                else
                {
                    mushroom.Left += 2;
                }
            }else if(mushroom.Top + mushroom.Height >= screen.Height)
            {
                timer11.Enabled = false;
            }
            else
            {
                mushroom.Top += 5;
                mushroom.Left += 2;
            }
        }

        private void MushroomPoint()
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<User> collection = database.GetCollection<User>("Alldata");
            var query = Query.EQ("Name", Name);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    user.mario.Point += 500;
                    collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.mario.Point, user.mario.Point));
                    point.Text = string.Format("{0:D6}", user.mario.Point);
                }

            }
            else
            {
                MessageText = "找不到此用戶";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            server.Disconnect();
        }

        private void PlayerGrowup()
        {
            state = 1;
            growup = 1;
            timer11.Dispose();
            timer10.Dispose();
            timer9.Dispose();
            timer8.Dispose();
            timer7.Dispose();
            timer6.Dispose();
            timer5.Dispose();
            timer4.Dispose();
            timer3.Dispose();
            timer2.Dispose();
            timer1.Dispose();
            timer11.Enabled = false;
            timer10.Enabled = false;
            timer9.Enabled = false;
            timer8.Enabled = false;
            timer7.Enabled = false;
            timer6.Enabled = false;
            timer5.Enabled = false;
            timer4.Enabled = false;
            timer3.Enabled = false;
            timer2.Enabled = false;
            timer1.Enabled = false;
            if (judge == "Smalljump_r.png" || judge == "Smallstand_r.png" || judge == "Smallwalk_r.gif")
            {
                player.Image = Image.FromFile("Smallstand_r.png");
                judge = "Smallstand_r.png";
            }
            else if (judge == "Smalljump_l.png" || judge == "Smallstand_l.png" || judge == "Smallwalk_l.gif")
            {
                player.Image = Image.FromFile("Smallstand_l.png");
                judge = "Smallstand_l.png";
            }
            timer12.Enabled = true;
        }

        private void timer12_Tick(object sender, EventArgs e)
        {
            ++grow;
            player.Top -= 2;
            player.Height += 2;
            if (judge == "Smallstand_r.png")
            {
                player.Image = Image.FromFile("Smallstand_r" + grow + ".png");
            }
            else if (judge == "Smallstand_l.png")
            {
                player.Image = Image.FromFile("Smallstand_l" + grow + ".png");
            }
            if (player.Height >= 38)
            {
                if (judge == "Smallstand_r.png")
                { 
                    player.Image = Image.FromFile("Bigstand_r.png");
                    judge = "Bigstand_r.png";
                }
                else if (judge == "Smallstand_l.png")
                {
                    player.Image = Image.FromFile("Bigstand_l.png");
                    judge = "Bigstand_l.png";
                }
                player.Width = 25;
                player.Height = 40;
                growup = 0;
                grow = 1;
                right = false;
                left = false;
                jump = false;
                timer1.Enabled = true;
                timer8.Enabled = true;
                timer12.Enabled = false;                
            }
        }

        private void timer13_Tick(object sender, EventArgs e)
        {
            string connectstring = "mongodb://127.0.0.1:27017";
            MongoClient client = new MongoClient(connectstring);
            MongoServer server = client.GetServer();
            server.Connect();
            //已連線到資料庫
            MongoDatabase database = server.GetDatabase("Mario");
            //取得資料表
            MongoCollection<User> collection = database.GetCollection<User>("Alldata");
            var query = Query.EQ("Name", Name);
            var all_user = collection.Find(query);
            if (all_user.Count() >= 1)
            {
                foreach (var user in all_user)
                {
                    user.pass.Time -= 1;
                    collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.pass.Time, user.pass.Time));
                    time.Text = "剩餘時間:\n    " + string.Format("{0:D3}", user.pass.Time);
                    if (user.pass.Time <= 0)
                    {
                        timer14.Enabled = true;
                        user.pass.Time = 300;
                        collection.Update(Query.EQ("Name", Name), Update<User>.Set(mario => mario.pass.Time, user.pass.Time));
                        timer13.Enabled = false;
                    }
                }

            }
            else
            {
                MessageText = "找不到此用戶";
                MessageBox.Show(MessageText, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            server.Disconnect();
        }

        private void timer14_Tick(object sender, EventArgs e)
        {
            timer10.Dispose();
            timer9.Dispose();
            timer8.Dispose();
            timer7.Dispose();
            timer6.Dispose();
            timer5.Dispose();
            timer4.Dispose();
            timer3.Dispose();
            timer2.Dispose();
            timer1.Dispose();
            timer10.Enabled = false;
            timer9.Enabled = false;
            timer8.Enabled = false;
            timer7.Enabled = false;
            timer6.Enabled = false;
            timer5.Enabled = false;
            timer4.Enabled = false;
            timer3.Enabled = false;
            timer2.Enabled = false;
            timer1.Enabled = false;
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer4.Ctlcontrols.play();
            player.Image = Image.FromFile("die.gif");
            gameover = 1;
            replay3++;
            if (replay3 >=12)
            {
                player.Top += 5;
                if (replay3 >= 32)
                {
                    MinusLife();
                    ZeroLife();
                    mapchange = 0;
                    this.player.Image = Image.FromFile("Smallstand_r.png");
                    judge = "Smallstand_r.png";
                    timer14.Enabled = false;
                    timer13.Enabled = true;
                    right = false;
                    left = false;
                    jump = false;
                    timer1.Enabled = true;
                    timer8.Enabled = true;
                    Form1_Load(sender, e);
                }
            }
            else
            {
                player.Top -= 5;
            }
        }
    }
}
