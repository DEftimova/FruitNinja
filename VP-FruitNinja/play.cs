using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace VP_FruitNinja
{
    public partial class FruitNinja : Form
    {
        public int score = 0, level = 1;
        public string username = "";
        private SoundPlayer sliceSound;
        System.Random rx = new System.Random();//random X number
        System.Random ry = new System.Random();//random Y number
        int a, b;

        public FruitNinja(String username)
        {
            InitializeComponent();
            sliceSound = new SoundPlayer("KnifeSlice.wav");
            this.username = username;

            Icon cur = new Icon("Resources/knife_oyJ_icon.ico");
            this.Cursor = new Cursor(cur.Handle);
        }

        public void play_Load(object sender, EventArgs e)
        {
            Icon cur = new Icon("Resources/knife_oyJ_icon.ico");
            this.Cursor = new Cursor(cur.Handle);
            a = rx.Next(0, ClientSize.Width - 200);
            b = ry.Next(0, ClientSize.Height / 2);
            for (int i = 0; i < Controls.Count; i++)
            {
                for (int j = 0; j < Controls.Count; j++)
                {
                    if (Controls[i].Name == "pictureBox" + j.ToString() || Controls[i].Name == "Bomb" + j.ToString())
                    {
                        Controls[i].Location = new System.Drawing.Point(a, ClientSize.Height);
                    }
                }
            }
            next = n.Next(1, 6);
            timer1.Start();
        }
        int next = 0;
        Random n = new Random();
        int f = 0, g = 0, h = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            Icon cur = new Icon("Resources/knife_oyJ_icon.ico");
            this.Cursor = new Cursor(cur.Handle);
            for (int i = 0; i < Controls.Count; i++)
            {
                for (int j = 0; j < Controls.Count; j++)
                {
                    if (Controls[i].Name == "pictureBox" + j.ToString())
                    {
                        Controls[i].Hide();
                    }
                    if (Controls[i].Name == "picturecutt" + j.ToString())
                    {
                        Controls[i].Hide();
                    }
                    if (Controls[i].Name == "picturecut" + j.ToString())
                    {
                        Controls[i].Hide();
                    }
                    if (Controls[i].Name == "Bomb" + j.ToString())
                    {
                        Controls[i].Hide();
                    }

                    if (Controls[i].Name == "Bomb" + next.ToString())
                    {
                        Controls[i].Show();
                        f = i;

                    }
                    if (Controls[i].Name == "pictureBox" + next.ToString())
                    {
                        Controls[i].Show();
                        f = i;
                    }
                    if (Controls[i].Name == "picturecutt" + next.ToString())
                    {
                        Controls[i].Show();
                        g = i;
                    }
                    if (Controls[i].Name == "picturecut" + next.ToString())
                    {
                        Controls[i].Show();
                        h = i;
                    }
                }
            }

            if (Controls[f].Location.Y < b)
            {
                timer1.Stop();
                timer2.Start();
            }

            else
            {
                if (a < ClientSize.Width / 2)
                {
                    Controls[f].Location = new Point(Controls[f].Location.X + 6, Controls[f].Location.Y - 15);
                    Controls[g].Location = Controls[f].Location;
                    Controls[h].Location = Controls[f].Location;
                }
                else if (a > ClientSize.Width / 2)
                {
                    Controls[f].Location = new Point(Controls[f].Location.X - 6, Controls[f].Location.Y - 15);
                    Controls[g].Location = Controls[f].Location;
                    Controls[h].Location = Controls[f].Location;
                }
            }
        }

        bool crop = false;

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Controls[f].Location.Y >= ClientSize.Height)
            {
                timer2.Stop();
                if (crop == false && Controls[f].Visible == true && Controls[f].Name.ToString() != "Bomb" + 6)
                {
                    score = score - 1;
                    label1.Text = score.ToString();
                }
                crop = false;
                next = n.Next(1, 7);
                a = rx.Next(0, ClientSize.Width - 200);
                b = ry.Next(0, ClientSize.Height / 2);
                for (int i = 0; i < Controls.Count; i++)
                {
                    for (int j = 0; j < Controls.Count; j++)
                    {
                        if (Controls[i].Name == "pictureBox" + j.ToString() || Controls[i].Name == "Bomb" + j.ToString())
                        {
                            Controls[i].Location = new System.Drawing.Point(a, ClientSize.Height);
                        }
                    }
                }
                timer1.Start();

            }
            else if (Controls[g].Location.Y >= ClientSize.Height && Controls[h].Location.Y >= ClientSize.Height)
            {
                Controls[f].Location = new System.Drawing.Point(a, ClientSize.Height);

                timer2.Stop();

                crop = false;
                next = n.Next(1, 7);
                a = rx.Next(0, ClientSize.Width - 200);
                b = ry.Next(0, ClientSize.Height / 2);
                timer1.Start();
            }
            else if (crop)
            {
                Controls[g].Location = new Point(Controls[g].Location.X + 6, Controls[g].Location.Y + 20);
                Controls[h].Location = new Point(Controls[h].Location.X - 6, Controls[h].Location.Y + 20);

            }
            else
            {
                if (a < ClientSize.Width / 2)
                {
                    Controls[f].Location = new Point(Controls[f].Location.X + 6, Controls[f].Location.Y + 15);
                    Controls[g].Location = Controls[f].Location;
                    Controls[h].Location = Controls[f].Location;
                }
                else if (a > ClientSize.Width / 2)
                {
                    Controls[f].Location = new Point(Controls[f].Location.X - 6, Controls[f].Location.Y + 15);
                    Controls[g].Location = Controls[f].Location;
                    Controls[h].Location = Controls[f].Location;
                }
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            sliceSound.Play();
            pictureBox1.Visible = false;
            crop = true;
            score = score + 1;
            if (score % 5 == 0 && timer1.Interval > 5 && timer2.Interval > 5)
            {
                timer1.Interval -= 5;
                timer2.Interval -= 5;
                level++;
                lblLevel.Text = level.ToString();
            }

            timer1.Stop();
            timer2.Start();
            label1.Text = score.ToString();
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            sliceSound.Play();
            pictureBox2.Visible = false;
            crop = true;
            score = score + 1;
            if (score % 5 == 0 && timer1.Interval > 5 && timer2.Interval > 5)
            {
                timer1.Interval -= 5;
                timer2.Interval -= 5;
                level++;
                lblLevel.Text = level.ToString();
            }

            timer1.Stop();
            timer2.Start();
            label1.Text = score.ToString();
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            sliceSound.Play();
            pictureBox3.Visible = false;
            crop = true;
            score = score + 1;
            if (score % 5 == 0 && timer1.Interval > 5 && timer2.Interval > 5)
            {
                timer1.Interval -= 5;
                timer2.Interval -= 5;
                level++;
                lblLevel.Text = level.ToString();
            }

            timer1.Stop();
            timer2.Start();
            label1.Text = score.ToString();
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            sliceSound.Play();
            pictureBox4.Visible = false;
            crop = true;
            score = score + 1;
            if (score % 5 == 0 && timer1.Interval > 5 && timer2.Interval > 5)
            {
                timer1.Interval -= 5;
                timer2.Interval -= 5;
                level++;
                lblLevel.Text = level.ToString();
            }

            timer1.Stop();
            timer2.Start();
            label1.Text = score.ToString();
        }


        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            sliceSound.Play();
            pictureBox5.Visible = false;
            crop = true;
            score = score + 1;
            if (score % 5 == 0 && timer1.Interval > 5 && timer2.Interval > 5)
            {
                timer1.Interval -= 5;
                timer2.Interval -= 5;
                level++;
                lblLevel.Text = level.ToString();
            }

            timer1.Stop();
            timer2.Start();
            label1.Text = score.ToString();
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            Bomb6.Visible = false;
            crop = true;
            timer1.Stop();
            timer2.Stop();
            label1.Hide();
            label2.Hide();

            GameOver go = new GameOver(username, score);
            this.Hide();
            go.Show();
        }

        private void FruitNinja_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
