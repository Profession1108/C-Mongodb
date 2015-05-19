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
        int map = 0,map2 = 0;
        int i = 0;
        int[] stoptimer = new int[]{0,0};
        int[] aftercheck = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        int replay = 0;
        int jumpheight = 15;
        public Rock1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            axWindowsMediaPlayer1.settings.setMode("loop", true);
            judge = "stand_r.png";
            player.Top = screen.Height - 127;            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            screen.Image = Image.FromFile("C:\\background_2.png");
            pictureBox2.Image = Image.FromFile("C:\\background_2.png");
            pictureBox1.Image = Image.FromFile("C:\\background_2.png");
            this.player.BackColor = Color.Transparent;
            this.player.Parent = this.screen;
            this.Coin.BackColor = Color.Transparent;
            this.Coin.Parent = this.screen;            
            this.Box1.BackColor = Color.Transparent;
            this.Box1.Parent = this.screen;
            this.Coin2.BackColor = Color.Transparent;
            this.Coin2.Parent = this.screen;
            this.Box2.BackColor = Color.Transparent;
            this.Box2.Parent = this.screen;
            this.Coin3.BackColor = Color.Transparent;
            this.Coin3.Parent = this.screen;
            this.Box3.BackColor = Color.Transparent;
            this.Box3.Parent = this.screen;
            this.Coin4.BackColor = Color.Transparent;
            this.Coin4.Parent = this.screen;
            this.Box4.BackColor = Color.Transparent;
            this.Box4.Parent = this.screen;
            this.Rock.BackColor = Color.Transparent;
            this.Rock.Parent = this.screen;
            this.Rock2.BackColor = Color.Transparent;
            this.Rock2.Parent = this.screen;
            this.Rock3.BackColor = Color.Transparent;
            this.Rock3.Parent = this.screen;
            axWindowsMediaPlayer1.URL = "music.mp3";
            axWindowsMediaPlayer2.URL = "Coin_Sound.mp3";
            axWindowsMediaPlayer2.settings.volume = 100;
            axWindowsMediaPlayer2.Ctlcontrols.stop();
            axWindowsMediaPlayer3.URL = "Jump_Sound.mp3";
            axWindowsMediaPlayer3.settings.volume = 10;
            axWindowsMediaPlayer3.Ctlcontrols.stop();

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            timer1.Enabled = true;           
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
                    jump = true;
                    jumpheight = 22;
                    axWindowsMediaPlayer3.Ctlcontrols.play();
                    if (judge == "stand_r.png")
                    {
                        judge = "jump_r.png";
                        standright = true;
                        timer5.Enabled = true;
                    }
                    if (judge == "stand_l.png")
                    {
                        judge = "jump_l.png";
                        standleft = true;
                        timer5.Enabled = true;
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                right = false;
                judge = "stand_r.png";
                timer3.Enabled = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                left = false;
                judge = "stand_l.png";
                timer3.Enabled = true;
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
        private void tbcollision(PictureBox player, PictureBox block,int coin,bool check)
        {
            // Top Side Collision
            if (player.Left + player.Width >= block.Left && player.Left + player.Width <= block.Left + block.Width + player.Width && player.Top + player.Height >= block.Top && player.Top <= block.Top)
            {
                jump = false;
                jumpheight = 0;
                player.Top = block.Location.Y - player.Height;
            }
            //Bottom Side Collision
            if (block.Bottom >= player.Top - 4 && block.Right >= player.Right - player.Size.Width && block.Left <= player.Left + player.Size.Width && block.Top <= player.Top + 4)
            {
                jumpheight = -1;
                i++;
                switch(coin)
                {
                    case 1:
                        if (check == true && aftercheck[1] != 1)
                        {
                            Coin.Visible = true;
                            Box1.Image = Image.FromFile("Box_check.png");
                            aftercheck[1] = 1;
                            timer7.Enabled = true;
                        }
                        break;
                    case 2:
                        if (check == true && aftercheck[2] != 1)
                        {
                            Coin2.Visible = true;
                            Box2.Image = Image.FromFile("Box_check.png");
                            aftercheck[2] = 1;
                            timer7.Enabled = true;
                        }
                        break;
                    case 3:
                        if (check == true && aftercheck[3] != 1)
                        {
                            Coin3.Visible = true;
                            Box3.Image = Image.FromFile("Box_check.png");
                            aftercheck[3] = 1;
                            timer7.Enabled = true;
                        }
                        break;
                    case 4:
                        if (check == true && aftercheck[4] != 1)
                        {
                            Coin4.Visible = true;
                            Box4.Image = Image.FromFile("Box_check.png");
                            aftercheck[4] = 1;
                            timer7.Enabled = true;
                        }
                        break;
                    default:
                        break;
                }                
            }
        }

        private void dropposition(PictureBox player, PictureBox block)
        {
            if (player.Top + player.Height >= background_1.Top)
            {
                player.Top = background_1.Top - player.Height;
                jump = false;
            }
            else if (player.Top + player.Height <= block.Top)
            {
                if (player.Right < block.Left || player.Left > block.Right)
                {
                    player.Top += 5;
                }
            }
            else
            {
                player.Top += 5;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            rlcollision(player,Box1);
            rlcollision(player, Box2);
            rlcollision(player, Box3);
            rlcollision(player, Box4);
            rlcollision(player, Rock);
            rlcollision(player, Rock2);
            rlcollision(player, Rock3);
            //只按右鍵
            if (right == true) { player.Left += 7; }
            //只按左鍵
            if (left == true) { player.Left -= 7; }
            
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
            dropposition(player, Box1);

            tbcollision(player, Box1 , 1 , true);
            tbcollision(player, Box2, 2, true);
            tbcollision(player, Box3, 3, true);
            tbcollision(player, Box4, 4, true);
            tbcollision(player, Rock, 1, false);
            tbcollision(player, Rock2, 2, false);
            tbcollision(player, Rock3, 3, false);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (right == true && timer4.Enabled == false)
            {
                player.Image = Image.FromFile("walk_r.gif");
                judge = "stand_r.png";
                timer2.Dispose();
                timer4.Enabled = true;                
            }
            if (left == true && timer4.Enabled == false)
            {
                player.Image = Image.FromFile("walk_l.gif");
                judge = "stand_l.png"; 
                timer2.Dispose();
                timer4.Enabled = true;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (right == false && standright == true)
            {
                player.Image = Image.FromFile("stand_r.png");
                judge = "stand_r.png";
                standright = false;
                timer3.Dispose();
            }
            if (left == false && standleft == true)
            {
                player.Image = Image.FromFile("stand_l.png");
                judge = "stand_l.png";
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
                player.Image = Image.FromFile("walk_r.gif");
                timer4.Dispose();
            }
            if (left == true && replay % 10 == 0)
            {
                replay = 0;
                player.Image = Image.FromFile("walk_l.gif");
                timer4.Dispose();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            if (standright == true)
            {
                player.Image = Image.FromFile("jump_r.png");
                timer5.Dispose();
                timer6.Enabled = true;
            }
            if (standleft == true)
            {
                player.Image = Image.FromFile("jump_l.png");
                timer5.Dispose();
                timer6.Enabled = true;
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            if (judge == "jump_r.png")
            {
                player.Image = Image.FromFile("stand_r.png");
                standright = true;
                judge = "stand_r.png";          
                timer6.Dispose();
                timer4.Enabled = true;
                
            }
            if (judge == "jump_l.png")
            {
                player.Image = Image.FromFile("stand_l.png");
                standleft = true;
                judge = "stand_l.png";
                timer6.Dispose();
                timer4.Enabled = true;
            }       
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            Coin.Visible = false;
            Coin2.Visible = false;
            Coin3.Visible = false;
            Coin4.Visible = false;
            axWindowsMediaPlayer2.settings.rate = 8;
            axWindowsMediaPlayer2.Ctlcontrols.play();
            timer7.Dispose();
        }

        private void changemap(PictureBox before, PictureBox screen, PictureBox pictureBox2, PictureBox pictureBox1)
        {
            if (right == true)
            {
                screen.Left -= 3;
                pictureBox2.Left -= 3;
                pictureBox2.Width += 3;
                //

                if (pictureBox2.Location.X <= 0)
                {
                    pictureBox2.Left = 0;
                    pictureBox2.Width = 1230;
                    pictureBox2.Height = 720;

                    pictureBox1.Left = 1230;
                    pictureBox1.Width = 50;
                    pictureBox1.Height = 720;  
                    map++;                  
                }
                if (player.Left >= 1230)
                {
                    this.player.BackColor = Color.Transparent;
                    this.player.Parent = pictureBox2;
                    player.Left = 0;
                    map2++;
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
                        pictureBox2.Width -= 3;
                    }
                    if (player.Left < 0)
                    {
                        this.player.BackColor = Color.Transparent;
                        this.player.Parent = screen;
                        player.Left = 0;

                    }
                }
                else
                {
                    if (screen.Left > 0)
                    {
                        screen.Left -= 3;
                        before.Left -= 3;
                        before.Width += 3;
                        pictureBox2.Left -= 3;
                        pictureBox2.Width += 3;
                    }
                    else
                    {   
                        screen.Left += 3;
                        before.Left += 3;
                        before.Width -= 3;
                        pictureBox2.Left += 3;
                        pictureBox2.Width -= 3;
                    }
                    if (player.Left < 0)
                    {
                        this.player.BackColor = Color.Transparent;
                        this.player.Parent = before;
                        player.Left = 1230;
                        map2--;

                    }
                }
                
            }
            label1.Text = Convert.ToString( player.Left);
        }

        private void changemap2(PictureBox before, PictureBox screen, PictureBox pictureBox2, PictureBox pictureBox1)
        {
            if (right == true)
            {
                screen.Left -= 3;

                if (screen.Location.X <= 0)
                {
                    screen.Left = 0;
                    screen.Width = 1280;
                    screen.Height = 720;
                }
                if (player.Left >= 1230)
                {
                    this.player.BackColor = Color.Transparent;
                    this.player.Parent = screen;
                    player.Left = 1230;
                }
            }
            if (left == true)
            {
                if(map == 2)
                {
                    if (screen.Left > 0)
                    {
                        screen.Left -= 3;
                        before.Left -= 3;
                        before.Width += 3;
                    }
                    else
                    {
                        screen.Left += 3;
                        before.Left += 3;
                        before.Width -= 3;
                    }
                    if (player.Left < 0)
                    {
                        this.player.BackColor = Color.Transparent;
                        this.player.Parent = before;
                        player.Left = 1230;
                        map2--;
                    }
                }

            }
        }
        private void timer8_Tick(object sender, EventArgs e)
        {
            if (map == 0)
            {
                PictureBox none = new PictureBox();
                changemap(none,screen, pictureBox2 , pictureBox1);         
            }
            else if (map == 1)
            {
                pictureBox1.Visible = true;                
                changemap(screen,pictureBox2, pictureBox1,screen);
            }
            else
            {
                changemap2(pictureBox2, pictureBox1, screen, screen);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

    }
}
