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

    public Rendering(int cols, int rows, int resolution, int[,] field, PictureBox pictureBox, Graphics graphics)
    {
        this.rows = rows;
        this.cols = cols;
        this.field = field;
        this.pictureBox = pictureBox;
        this.resolution = resolution;
        this.graphics = graphics;
    }

    public void DrawGeneration()
    {
        graphics.Clear(Color.Black);
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (field[x, y] == 1)
                {
                    graphics.FillRectangle(Brushes.Blue, x * resolution, y * resolution, resolution, resolution);
                }
                else if (field[x, y] == 2)
                {
                    graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }

            }
        }
        pictureBox.Refresh();
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

    internal void removeFromField((int, int) pos)
    {
        graphics.FillRectangle(Brushes.Black, pos.Item1 * resolution, pos.Item2 * resolution, resolution, resolution);

    }
}
