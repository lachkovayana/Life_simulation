
namespace LabOOP1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numDensity2 = new System.Windows.Forms.NumericUpDown();
            this.numResolution = new System.Windows.Forms.NumericUpDown();
            this.numDensity1 = new System.Windows.Forms.NumericUpDown();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(59, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(8, 8);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.buttonContinue);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.numDensity2);
            this.panel2.Controls.Add(this.numResolution);
            this.panel2.Controls.Add(this.numDensity1);
            this.panel2.Controls.Add(this.buttonStop);
            this.panel2.Controls.Add(this.buttonStart);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(276, 1000);
            this.panel2.TabIndex = 2;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(80, 192);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(94, 29);
            this.buttonContinue.TabIndex = 10;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonRestart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Размер клетки(в px)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 434);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Плотность растений";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Плотность животных";
            // 
            // numDensity2
            // 
            this.numDensity2.Location = new System.Drawing.Point(59, 457);
            this.numDensity2.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numDensity2.Name = "numDensity2";
            this.numDensity2.Size = new System.Drawing.Size(150, 27);
            this.numDensity2.TabIndex = 6;
            this.numDensity2.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // numResolution
            // 
            this.numResolution.Location = new System.Drawing.Point(59, 290);
            this.numResolution.Name = "numResolution";
            this.numResolution.Size = new System.Drawing.Size(150, 27);
            this.numResolution.TabIndex = 4;
            this.numResolution.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numDensity1
            // 
            this.numDensity1.Location = new System.Drawing.Point(59, 369);
            this.numDensity1.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numDensity1.Name = "numDensity1";
            this.numDensity1.Size = new System.Drawing.Size(150, 27);
            this.numDensity1.TabIndex = 3;
            this.numDensity1.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(80, 138);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(94, 29);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(80, 83);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(94, 29);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel3.Location = new System.Drawing.Point(282, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1000, 1000);
            this.panel3.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1000, 1000);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 400;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(70, 609);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 27);
            this.textBox1.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1282, 1000);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDensity1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown numDensity1;
        private System.Windows.Forms.NumericUpDown numResolution;
        private System.Windows.Forms.NumericUpDown numDensity2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.TextBox textBox1;
    }
}

