using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class MapObjectsControl
{
    private List<Animal> _listOfAnimals = new();
    private List<Plant> _listOfAllPlants = new();
    private List<Fruit> _listOfFruits = new();
    private List<Plant> _listOfNewPlants = new();
    private List<Animal> _removeList = new();


    private int _rows;
    private int _cols;
    private int _densityAnimals;
    private int _densityPlants;
    private int _resolution;

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

        _rendering = new Rendering(_cols, _rows, _resolution, _pictureBox, graphics);
    }

   
    private void UpdatePlants()
    {
        foreach (Plant plant in _listOfAllPlants)
        {
            plant.LivePlantCicle(_listOfAllPlants, _listOfFruits, _listOfNewPlants);
        }
        foreach (Plant plant in _listOfNewPlants)
        {
            _listOfAllPlants.Add(plant);
        }
            _listOfNewPlants = new();
    }

    private void UpdateAnimals()
    {
        foreach (Animal animal in _listOfAnimals)
        {
            animal.LiveAnimalCicle(_listOfAllPlants, _listOfFruits, _removeList);
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
        UpdatePlants();
        _rendering.UpgradeField(_listOfAnimals, _listOfAllPlants, _listOfFruits);


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
                    _listOfAnimals.Add(new Animal(_rows, _cols, (x, y)));
                    _rendering.DrawFirstGeneration(MapObject.animal, x, y);
                }
                else if (random.Next(_densityPlants) == 0)
                {
                    EdiblePlant newPlant = new((x, y));
                    _listOfAllPlants.Add(newPlant);
                    if (newPlant.IsHealthy())
                    {
                       _rendering.DrawFirstGeneration(MapObject.ediblePlantHealthy, x, y);
                    }
                    else
                    {
                        _rendering.DrawFirstGeneration(MapObject.ediblePlantPoisonous, x, y);
                    }
                }
                else if (random.Next(_densityPlants) == 1)
                {
                    InediblePlant newPlant = new((x, y));
                    _listOfAllPlants.Add(newPlant);
                    if (newPlant.IsHealthy())
                    {
                       _rendering.DrawFirstGeneration(MapObject.inediblePlantHealthy, x, y);
                    }
                    else
                    {
                       _rendering.DrawFirstGeneration(MapObject.inediblePlantPoisonous, x, y);
                    }
                }

            }
        }
    }

}
