
using System;
using System.Drawing;
using System.Windows.Forms;

//передача значения таймера для определения созревания растения

namespace LabOOP1
{
    public partial class Form1 : Form
    {
        private int timerCounter = 0;
        private int resolution;
        private int densityAnimals;
        private int densityPlants;
        private int rows;
        private int cols;
        private Graphics graphics;
        private MapObjectsControl mapController;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = (++timerCounter).ToString();
            mapController.LiveOneCicle();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void buttonStop_Click(object sender, EventArgs e)
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

            resolution = (int)numResolution.Value;
            densityAnimals = (int)numDensity1.Value;
            densityPlants = (int)numDensity2.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            mapController = new MapObjectsControl(rows, cols, densityAnimals, densityPlants, resolution, pictureBox1, graphics);
            mapController.CreateFirstGeneration();

            timer1.Start();
        }

       
    }
}