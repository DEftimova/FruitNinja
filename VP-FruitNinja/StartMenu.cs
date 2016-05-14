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
    public partial class StartMenu : Form
    {
        private SoundPlayer soundtrack;

        public StartMenu()
        {
            InitializeComponent();
            GenerateHighScores();
            soundtrack = new SoundPlayer("FruitNinjaFullSoundtrack.wav");
            soundtrack.Play();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            lblWelcome.Visible = false;
            lblEnterUsername.Visible = true;
            tbUserName.Visible = true;
            btnContinue.Visible = true;
            tbUserName.Focus();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.Hide();
            FruitNinja go = new FruitNinja(tbUserName.Text);
            go.Show();
            go.play_Load(sender, e);
            soundtrack.Stop();
        }

        private void tbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (tbUserName.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(tbUserName, "Please enter an username.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(tbUserName, null);
                e.Cancel = false;
            }
        }

        private void btnHighScores_Click(object sender, EventArgs e)
        {
            ShowHighScores();
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

            lblWelcome.Font = new Font(lblWelcome.Font.FontFamily.Name, 15);
            lblWelcome.Text = "";
            for (int i = 0; i < 5; i++)
            {
                lblWelcome.Text = lblWelcome.Text + Environment.NewLine + list[i].Username + "     " + list[i].Score.ToString();
            }
        }

        private void StartMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnHowToPlay_Click(object sender, EventArgs e)
        {
            lblWelcome.Font = new Font(lblWelcome.Font.FontFamily.Name, 15);
            lblWelcome.Text = "It's very simple!" +
                Environment.NewLine +
                Environment.NewLine +
                ">>>> Cut the fruits <<<<" +
                Environment.NewLine +
                ">>> Avoid the bombs <<<" +
                Environment.NewLine +
                Environment.NewLine +
                Environment.NewLine +
                "Good luck!";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            lblWelcome.Font = new Font(lblWelcome.Font.FontFamily.Name, 15);
            lblWelcome.Text = "The game was developed as a project for the Visual Programming course." +
                Environment.NewLine + "No copyright infringement intended!" +
                Environment.NewLine + Environment.NewLine +  "Copyright @FINKI";
        }
    }
}
