using System;
using System.Drawing;
using System.Windows.Forms;

//передача значения таймера для определения созревания растения

namespace LabOOP1
{
    public partial class Form1 : Form
    {
        public static int s_resolution = 35;
        public static Graphics s_graphics;
        public static int s_rows;
        public static int s_cols;
        public static PictureBox s_pictureBox;

        private MapObjectsControl mapController;
        private FoodForOmnivorous currentObj = null;
        private int timerValue = 0;

        private string startMessage = "There is nothing to show yet. \r\nClick on any object on the map!";
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(1000,700);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = (++timerValue).ToString() ;
            mapController.LiveOneCicle(timerValue);
            ShowMessage();
        }

        protected virtual void ShowMessage()
        {
            if (currentObj == null)
                SetMessage(startMessage);
            else
                SetMessage(currentObj.GetTextInfo());
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

        //Image ZoomPicture(Size size)
        //{

        //    Bitmap bm = new (pictureBox1.Image, Convert.ToInt32(pictureBox1.Image.Width * size.Width),
        //        Convert.ToInt32(pictureBox1.Image.Height * size.Height));
        //    Graphics gpu = Graphics.FromImage(bm);
        //    gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    return bm;
        //}
        //PictureBox org;
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered = true;
            //org = new PictureBox();
            //org.Image = pictureBox1.Image;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //    pictureBox1.Image = ZoomPicture( new Size(trackBar1.Value, trackBar1.Value));

        }

        void SetMessage(string msg)
        {
            textBox2.Text = msg;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int CurX = e.X / s_resolution;
            int CurY = e.Y / s_resolution;
            currentObj = Rendering.FieldOfAllMapObjects[CurX, CurY];
           
            if (Rendering.FieldOfAllMapObjects[CurX, CurY] == null)
            {
               SetMessage("Whoops! Missclick! \r\nNothing at position (" + CurX + ";" + CurY + ")");
            }
            else
            {
                SetMessage(Rendering.FieldOfAllMapObjects[CurX, CurY].GetTextInfo());
            }

        }
    }
}