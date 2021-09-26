using System;
using System.Windows.Forms;
using System.Drawing;

public class Rendering
{
    private Graphics graphics;
    private int resolution;
    private int rows;
    private int cols;
    private int[,] field;
    private PictureBox pictureBox;

    public Rendering(int cols, int rows, int resolution, PictureBox pictureBox, Graphics graphics)
    {
        this.rows = rows;
        this.cols = cols;
        this.pictureBox = pictureBox;
        this.resolution = resolution;
        this.graphics = graphics;
    }

    public void Draw((int, int) pos)
    {
        graphics.FillRectangle(Brushes.Green, pos.Item1 * resolution, pos.Item2 * resolution, resolution, resolution);
        pictureBox.Refresh();
    }

    public void ChangeColor((int, int) pos)
    {
        //graphics.Clear(Color.Black);
        graphics.FillRectangle(Brushes.White, pos.Item1 * resolution, pos.Item2 * resolution, resolution, resolution);
        pictureBox.Refresh();
    }

    public void removeFromField((int, int) pos)
    {
        graphics.FillRectangle(Brushes.Black, pos.Item1 * resolution, pos.Item2 * resolution, resolution, resolution);
        pictureBox.Refresh();
    }

    public void DrawAnimal(int x, int y)
    {
            graphics.FillRectangle(Brushes.Gray, x * resolution, y * resolution, resolution, resolution);
            pictureBox.Refresh();
    }
    public void DrawPlant(int x, int y)
    {
        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
        pictureBox.Refresh();
    }
}
