using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class MapObjectsControl
{
    private List<Animal> _listOfAnimals = new();
    private List<Plant> _listOfAllPlants = new();
    private List<Animal> _removeList = new();


    private int _rows;
    private int _cols;
    private int _densityAnimals;
    private int _densityPlants;
    private int _resolution;

    private int[,] _field;

    private Rendering _rendering;
    private PictureBox _pictureBox;

    public MapObjectsControl(int rows, int cols, int density1, int density2, int resolution, PictureBox pictureBox, Graphics graphics)
    {
        _pictureBox = pictureBox;
        _resolution = resolution;
        _rows = rows;
        _cols = cols;
        _densityAnimals = density1;
        _densityPlants = density2;

        _field = new int[cols, rows];
        _rendering = new Rendering(_cols, _rows, _resolution, _pictureBox, graphics);
    }

    public void CreateGeneration()
    {
        Random random = new();
        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                if (random.Next(_densityAnimals) == 0)
                {
                    //_field[x, y] = 1;
                    _rendering.DrawAnimal(x, y);
                    _listOfAnimals.Add(new Animal(_rows, _cols, (x, y)));
                }
                else if (random.Next(_densityPlants) == 0)
                {
                    //_field[x, y] = 2;
                    _rendering.DrawPlant(x, y);
                    _listOfAllPlants.Add(new Plant((x, y)));
                }
                //else
                //{
                //_field[x, y] = 0;
                //}
            }
        }
    }

    public void LiveOneCicle()
    {

        LiveAnimalCicle();
        LivePlantCicle();
        _rendering.UpdateField(_listOfAnimals, _listOfAllPlants);


    }

    private void LivePlantCicle()
    {
        //foreach (Plant plant in _listOfAllPlants)
        //{

        //}
    }

    private void LiveAnimalCicle()
    {
        foreach (Animal animal in _listOfAnimals)
        {
            animal.DecreaseSatiety(_removeList);
            animal.Move(_listOfAllPlants);
        }
        foreach (Animal animal in _removeList)
        {
            _listOfAnimals.Remove(animal);
        }
        _removeList = new();
    }

}
