using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VP_FruitNinja
{
    public partial class GameOver : Form
    {
        string username = "";
        int score = 0;
        private SoundPlayer soundtrack;
        public GameOver(String username, int score)
        {
            InitializeComponent();
            soundtrack = new SoundPlayer("FruitNinjaFullSoundtrack.wav");
            soundtrack.Play();
            GenerateHighScores();
            //lblGameOver.Text = username + " " + score.ToString();
            this.username = username;
            this.score = score;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            //this.Close();
            DialogResult r = MessageBox.Show("Are you sure you want to quit?",
                "Quit Game", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
                Application.Exit();
            else
                return;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            FruitNinja fn = new FruitNinja(username);
            soundtrack.Stop();
            this.Hide();
            fn.Show();
            fn.play_Load(sender, e);
        }

        private void btnHighscores_Click(object sender, EventArgs e)
        {
            ShowHighScores();
        }

        private void GameOver_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void GenerateHighScores()
        {
            try
            {
                if (!File.Exists("HighScores.bin"))
                {
                    List<Player> list1 = new List<Player>();

                    list1.Add(new Player("Ema", 25));
                    list1.Add(new Player("Dijana", 20));
                    list1.Add(new Player("Daniel", 18));
                    list1.Add(new Player("Marko", 15));
                    list1.Add(new Player("Stefan", -3));

                    Stream stream = File.Create("HighScores.bin");

                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Context = new StreamingContext(StreamingContextStates.CrossAppDomain);
                    bf.Serialize(stream, list1);

                    stream.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ShowHighScores()
        {
            saveHighScore();
            Stream stream1 = File.OpenRead("HighScores.bin");
            BinaryFormatter bf1 = new BinaryFormatter();

            List<Player> list = (List<Player>)bf1.Deserialize(stream1);

            list.Sort(
                delegate (Player p1, Player p2)
                {
                    return -(p1.Score.CompareTo(p2.Score));
                }
            );
            stream1.Close();

            lblGameOver.Font = new Font(lblGameOver.Font.FontFamily.Name, 15);
            lblGameOver.Text = "";
            for (int i = 0; i < 5; i++)
            {
                lblGameOver.Text = lblGameOver.Text + Environment.NewLine + list[i].Username + "     " + list[i].Score.ToString();
            }
        }

        public void saveHighScore()
        {
            Stream stream = File.OpenRead("HighScores.bin");
            BinaryFormatter bf = new BinaryFormatter();

            List<Player> list = (List<Player>)bf.Deserialize(stream);

            Player p = new Player(username, score);
            bool found = false;
            for (int i = 0; i < list.Count; i++)
                if (list[i].Username == username)
                {
                    found = true;
                    if (list[i].Score < score)
                        list[i].Score = score;
                }
            if (!found)
                list.Add(p);

            stream.Close();
            File.Create("HighScores.bin").Dispose();
            stream = File.Create("HighScores.bin");

            bf = new BinaryFormatter();
            bf.Context = new StreamingContext(StreamingContextStates.CrossAppDomain);
            bf.Serialize(stream, list);
            stream.Flush();
            stream.Close();
        }
    }
}
