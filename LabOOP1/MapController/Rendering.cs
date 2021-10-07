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
    private PictureBox _pictureBox;

    public Rendering(int cols, int rows, int resolution, PictureBox pictureBox, Graphics graphics)
    {
        _rows = rows;
        _cols = cols;
        _pictureBox = pictureBox;
        _resolution = resolution;
        _graphics = graphics;
    }

    //public void DrawAnimal(int x, int y)
    //{
    //    _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
    //    _pictureBox.Refresh();

    //}
    //public void DrawEdiblePlant(int x, int y)
    //{
    //    _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
    //    _pictureBox.Refresh();

    //}
    //public void DrawInediblePlant(int x, int y)
    //{
    //    _graphics.FillRectangle(Brushes.Blue, x * _resolution, y * _resolution, _resolution, _resolution);
    //    _pictureBox.Refresh();
    //}

    public void Draw(string element, int x, int y)
    {
        switch (element)
        {
            case "animal":
                _graphics.FillRectangle(Brushes.Gray, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
            case "ediblePlant":
                _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
            case "inediblePlant":
                _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
        }
        _pictureBox.Refresh();


    }

    public void UpdateField(List<Animal> listOfAnimals, List<EdiblePlant> listOfEdiblePlants, List<InediblePlant> listOfInediblePlants)
    {
        _graphics.Clear(Color.Black);
        foreach (EdiblePlant plant in listOfEdiblePlants)
        {
            int x = plant.GetPosition().Item1;
            int y = plant.GetPosition().Item2;
            switch (plant.stage)
            {
                case 1:
                    _graphics.FillEllipse(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                    break;
                case 2:
                    _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                    break;
                case 3:
                    _graphics.FillRectangle(Brushes.Blue, x * _resolution, y * _resolution, _resolution, _resolution);
                    break;
            }

        }
        foreach (InediblePlant plant in listOfInediblePlants)
        {
            int x = plant.GetPosition().Item1;
            int y = plant.GetPosition().Item2;

            switch (plant.stage)
            {
                case 1:
                    _graphics.FillEllipse(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                    break;
                case 2:
                    _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                    break;
                case 3:
                    _graphics.FillRectangle(Brushes.Blue, x * _resolution, y * _resolution, _resolution, _resolution);
                    break;
            }
        }
        foreach (Animal animal in listOfAnimals)
        {
            int x = animal.GetPosition().Item1;
            int y = animal.GetPosition().Item2;

            _graphics.FillRectangle(Brushes.Gray, x * _resolution, y * _resolution, _resolution, _resolution);
        }
        _pictureBox.Refresh();

    }

    internal void UpgradeField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants)
    {
        _graphics.Clear(Color.Black);
       
        foreach (Plant plant in listOfAllPlants)
        {
            int x = plant.GetPosition().Item1;
            int y = plant.GetPosition().Item2;
            switch (plant)
            {
                case EdiblePlant:
                    switch (plant.stage)
                    {
                        case 1:
                            _graphics.FillEllipse(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case 2:
                            _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case 3:
                            _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                    }
                    break;
                    case InediblePlant:
                    switch (plant.stage)
                    {
                        case 1:
                            _graphics.FillEllipse(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case 2:
                            _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case 3:
                            _graphics.FillRectangle(Brushes.Blue, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                    }
                    break;
            }
            
        }
        foreach (Animal animal in listOfAnimals)
        {
            int x = animal.GetPosition().Item1;
            int y = animal.GetPosition().Item2;

            _graphics.FillRectangle(Brushes.Gray, x * _resolution, y * _resolution, _resolution, _resolution);
        }
        _pictureBox.Refresh();

    }
}


