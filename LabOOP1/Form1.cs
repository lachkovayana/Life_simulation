using System;
using System.Drawing;
using System.Windows.Forms;

//передача значения таймера для определения созревания растения

namespace LabOOP1
{
    public partial class Form1 : Form
    {
        private int timerCounter = 0;

        public static int s_resolution;
        public static int s_densityAnimals;
        public static int s_densityPlants;
        public static Graphics s_graphics;
        public static int s_rows;
        public static int s_cols;
        public static PictureBox s_pictureBox;

        private MapObjectsControl mapController; 


        public Form1()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = (++timerCounter).ToString();
            mapController.LiveOneCicle();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }
        private void buttonRestart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        public void StopGame()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();
            buttonStart.Enabled = true;
            buttonContinue.Enabled = true;
            numResolution.Enabled = true;
            numDensity1.Enabled = true;
            numDensity2.Enabled = true;
        }

        public void StartGame()
        {

            timerCounter = 0;
            buttonStart.Enabled = false;
            buttonContinue.Enabled = false;
            numResolution.Enabled = false;
            numDensity1.Enabled = false;
            numDensity2.Enabled = false;

            s_resolution = (int)numResolution.Value;
            s_densityAnimals = (int)numDensity1.Value;
            s_densityPlants = (int)numDensity2.Value;
            s_rows = pictureBox1.Height / s_resolution;
            s_cols = pictureBox1.Width / s_resolution;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            s_graphics = Graphics.FromImage(pictureBox1.Image);

            s_pictureBox = pictureBox1;

            mapController = new MapObjectsControl();
            mapController.CreateFirstGeneration();

            timer1.Start();
        }

       
    }
}