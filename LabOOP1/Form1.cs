using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
//передача значения таймера для определения созревания растения

namespace LabOOP1
{
    public partial class Form1 : Form
    {
        //public static int s_resolution = 35;
        public static int s_resolution = 35;
        public static Graphics s_graphics;
        public static int s_rows;
        public static int s_cols;
        public static PictureBox s_pictureBox;

        private MapObjectsControl mapController;
        private MapObject currentObj = null;
        private int timerValue = 0;

        private string startMessage = "There is nothing to show yet. \r\nClick on any object on the map!";
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(1500, 1050);
            //pictureBox1.Size = new Size(15000, 15000);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = (++timerValue).ToString();
            mapController.LiveOneCicle(timerValue);
            ShowMessage();
        }

        protected virtual void ShowMessage()
        {
            if (currentObj == null)
                SetMessage(startMessage);
            else
                SetMessage(currentObj.GetInfoAndLight());
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
        }

        public void StartGame()
        {
            timerValue = 0;
            currentObj = null;
            buttonStart.Enabled = false;
            buttonContinue.Enabled = false;

            s_rows = pictureBox1.Height / s_resolution;
            s_cols = pictureBox1.Width / s_resolution;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            s_graphics = Graphics.FromImage(pictureBox1.Image);

            s_pictureBox = pictureBox1;

            mapController = new MapObjectsControl();
            mapController.CreateFirstGeneration();
            timer1.Start();
        }

       

        void SetMessage(string msg)
        {
            textBox2.Text = msg;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int CurX = e.X / s_resolution;
            int CurY = e.Y / s_resolution;

            if (MapObjectsControl.FieldOfAllMapObjects[CurX, CurY].Count == 0)
            {
                SetMessage("Whoops! Missclick! \r\nNothing at position (" + CurX + ";" + CurY + ")");
            }
            else
            {
                currentObj = MapObjectsControl.FieldOfAllMapObjects[CurX, CurY].Last();
                SetMessage(currentObj.GetInfoAndLight());
            }

        }
    }
}