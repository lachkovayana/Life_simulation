using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

public class Rendering
{
    private Graphics _graphics;
    private int _resolution;
    private int _rows;
    private int _cols;
    //private int[,] field;
    private PictureBox _pictureBox;

    public Rendering(int cols, int rows, int resolution, PictureBox pictureBox, Graphics graphics)
    {
        _rows = rows;
        _cols = cols;
        _pictureBox = pictureBox;
        _resolution = resolution;
        _graphics = graphics;
    }

    public void DrawAnimal(int x, int y)
    {
        _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
        _pictureBox.Refresh();

    }
    public void DrawPlant(int x, int y)
    {
        _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
        _pictureBox.Refresh();

    }


    //public void UpdateField(int[,] field)
    //{
    //    graphics.Clear(Color.Black);
    //    for (int x = 0; x < cols; x++)
    //    {
    //        for (int y = 0; y < rows; y++)
    //        {
    //            if (field[x, y] == 1)
    //            { graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution, resolution); }
    //            if (field[x, y] == 2)
    //            { graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution); }

    //        }
    //    }
    //    _pictureBox.Refresh();
    //}

    public void UpdateField(List<Animal> listOfAnimals, List<Plant> listOfPlants)
    {
        _graphics.Clear(Color.Black);
        foreach (Plant plant in listOfPlants)
        {
            int x = plant.GetPosition().Item1;
            int y = plant.GetPosition().Item2;

            _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
        }
        foreach (Animal animal in listOfAnimals)
        {
            int x = animal.GetPosition().Item1;
            int y = animal.GetPosition().Item2;

            _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
        }
        _pictureBox.Refresh();

    }
}

