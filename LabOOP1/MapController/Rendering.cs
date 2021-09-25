using System;
using System.Windows.Forms;
using System.Drawing;

public class Rendering
{
    private Graphics grph;
    private int newResolution;
    private int newRows;
    private int newCols;
    private int[,] newField;
    private PictureBox pictureBox;

    public Rendering(int cols, int rows, int resolution, int[,] field, PictureBox pbx, Graphics graphics)
	{
        newRows = rows;
        newCols = cols;
        newField = field;
        pictureBox = pbx;
        newResolution = resolution;
        grph = graphics;
    }

	public void drawFirstGeneration()
    {
        grph.Clear(Color.Black);
        for (int x = 0; x < newCols; x++)
        {
            for (int y = 0; y < newRows; y++)
            {
                if (newField[x, y] == 1)
                {
                    grph.FillRectangle(Brushes.Blue, x * newResolution, y * newResolution, newResolution, newResolution);
                }
                else if (newField[x, y] == 2)
                {
                    grph.FillRectangle(Brushes.Crimson, x * newResolution, y * newResolution, newResolution, newResolution);
                }
            }
        }
        pictureBox.Refresh();
    }
}
