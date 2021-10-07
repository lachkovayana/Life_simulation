﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class MapObjectsControl
{
    private List<Animal> _listOfAnimals = new();
    private List<Plant> _listOfAllPlants = new();
    private List<EdiblePlant> _listOfEdiblePlants = new();
    private List<InediblePlant> _listOfInediblePlants = new();

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

    public void CreateFirstGeneration()
    {
        Random random = new();
        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                if (random.Next(_densityAnimals) == 0)
                {
                    _rendering.Draw("animal", x, y);
                    _listOfAnimals.Add(new Animal(_rows, _cols, (x, y)));
                }
                else if (random.Next(_densityPlants) == 1)
                {
                    _rendering.Draw("inediblePlant", x, y);
                    _listOfAllPlants.Add(new InediblePlant((x, y)));
                    _listOfInediblePlants.Add(new InediblePlant((x, y)));

                }
                else if (random.Next(_densityPlants) == 0)
                {
                    _rendering.Draw("ediblePlant", x, y);
                    _listOfAllPlants.Add(new EdiblePlant((x, y)));
                    _listOfEdiblePlants.Add(new EdiblePlant((x, y)));

                }
            }
        }
    }

    private void LivePlantCicle()
    {
        foreach (Plant plant in _listOfAllPlants)
        {
            plant.UpdateAge();
        }
    }

    private void UpdateAnimals()
    {
        foreach (Animal animal in _listOfAnimals)
        {
            animal.LiveAnimalCicle(_listOfAllPlants, _removeList);
        }
        foreach (Animal animal in _removeList)
        {
            _listOfAnimals.Remove(animal);
        }
        _removeList = new();
    }

    public void LiveOneCicle()
    {
        UpdateAnimals();
        LivePlantCicle();
        //_rendering.UpdateField(_listOfAnimals, _listOfEdiblePlants, _listOfInediblePlants);
        _rendering.UpgradeField(_listOfAnimals, _listOfAllPlants);


    }

}
