
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LabOOP1
{
    public class MapObjectsControl
    {
        private List<Animal> _listOfAnimals = new();
        private List<Plant> _listOfAllPlants = new();
        private List<Fruit> _listOfFruits = new();
        private List<Plant> _listOfNewPlants = new();
        private List<Animal> _removeList = new();


        private readonly Rendering _rendering;
        public MapObjectsControl()
        {
            _rendering = new Rendering();
        }


        private void UpdatePlants()
        {
            foreach (Plant plant in _listOfAllPlants)
            {
                plant.LivePlantCicle(_listOfFruits, _listOfNewPlants);
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
            for (int x = 0; x < Form1.s_cols; x++)
            {
                for (int y = 0; y < Form1.s_rows; y++)
                {
                    if (random.Next(Form1.s_densityAnimals) == 0)
                    {
                        _listOfAnimals.Add(new Animal((x, y)));
                        _rendering.DrawFirstGeneration(MapObject.animal, x, y);
                    }
                    else if (random.Next(Form1.s_densityPlants) == 0)
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
                    else if (random.Next(Form1.s_densityPlants) == 1)
                    {
                        InediblePlant newPlant = new((x, y));
                        _listOfAllPlants.Add(newPlant);
                        _rendering.DrawFirstGeneration(MapObject.inediblePlant, x, y);

                    }

                }
            }
        }

    }
}