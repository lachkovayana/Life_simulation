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
        private int rows;
        private int cols;
        private bool[,] field;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }


        private void buttonStop_Click(object sender, EventArgs e)
        {

        }
        public void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }
            
            numResolution.Enabled = false;
            numDensity.Enabled = false;
            resolution = (int)numResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)numDensity.Value) == 0;
                }

            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (field[x, y])
                    {
                        graphics.FillRectangle(Brushes.Blue, x * resolution, y * resolution, resolution, resolution);

                    }
                }
            }
            pictureBox1.Refresh();
        }

        private void numResolution_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
