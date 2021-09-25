using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabOOP1
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private int densityAnimals;
        private int densityPlants;
        private int rows;
        private int cols;
        private int[,] field;

        private MapController mapController;
        private Rendering rendering;


        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawFirstGeneration();
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }
        public void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            numResolution.Enabled = true;
            numDensity1.Enabled = true;

        }
        public void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }
            numResolution.Enabled = false;
            numDensity1.Enabled = false;
            resolution = (int)numResolution.Value;
            densityAnimals = (int)numDensity1.Value;
            densityPlants = (int)numDensity2.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            mapController = new MapController(rows, cols, densityAnimals, densityPlants);

            field = mapController.GetField();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void DrawFirstGeneration()
        {
            rendering = new Rendering(cols, rows, resolution, field, pictureBox1, graphics);
            rendering.drawFirstGeneration();
        }


    }
}