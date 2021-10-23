﻿using System;
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
        private List<FoodForherbivorous> _listOfFoodForherbivorous = new();
        private List<FoodForOmnivores> _listOfFoodForOmnivores = new();

        private readonly Rendering _rendering = new();

        private void UpdatePlants()
        {
            foreach (Plant plant in _listOfAllPlants.ToArray())
            {
                plant.LivePlantCicle(_listOfFruits, _listOfAllPlants);
            }
        }

        private void UpdateAnimals()
        {
            foreach (Animal animal in _listOfAnimals.ToArray())
            {
                animal.LiveAnimalCicle(_listOfAnimals, _listOfAllPlants, _listOfFruits, _listOfFoodForherbivorous, _listOfFoodForOmnivores);
            }
        }
        private void UpdateFood()
        {
            _listOfFoodForherbivorous = new();
            _listOfFoodForOmnivores = new();

            foreach (Plant plant in _listOfAllPlants.ToArray())
            {
                if (plant is EdiblePlant && plant.Stage != PlantStage.seed)
                {
                    _listOfFoodForherbivorous.Add(plant);
                }
            }
            foreach (Fruit fruit in _listOfFruits)
            {
                _listOfFoodForherbivorous.Add(fruit);
            }
            foreach (FoodForherbivorous f in _listOfFoodForherbivorous)
            {
                _listOfFoodForOmnivores.Add(f);
            }
            foreach (Animal an in _listOfAnimals)
            {
                _listOfFoodForOmnivores.Add(an);
            }
        }

        public void LiveOneCicle()
        {
            UpdateFood();
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
                        Animal an = new Animal((x, y));
                        _listOfAnimals.Add(an);
                        switch (an.Nutrition)
                        {
                            case NutritionMethod.herbivorous:
                                _rendering.DrawFirstGeneration(MapObject.animalHerbivorous, x, y);
                                break;
                            case NutritionMethod.carnivorous:
                                _rendering.DrawFirstGeneration(MapObject.animalCarnivorous, x, y);
                                break;
                            case NutritionMethod.omnivorous:
                                _rendering.DrawFirstGeneration(MapObject.animalOmnivorous, x, y);
                                break;
                        }
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
