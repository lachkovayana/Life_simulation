using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public class MapObjectsControl
    {
        private List<Animal> _listOfAnimals = new();
        private List<Plant> _listOfAllPlants = new();
        private List<Fruit> _listOfFruits = new();
        private List<FoodForHerbivorous> _listOfFoodForHerbivorous = new();
        private List<FoodForOmnivorous> _listOfFoodForOmnivorous = new();
        private readonly Rendering _rendering = new();

        public static Season s_currentSeason = Season.summer;

        public static List<Animal> listOfAnimalsCopy = new();

        private const int _densityAnimals = 80;
        private const int _densityPlants = 80;


        private void UpdateAnimals()
        {
            foreach (Animal animal in _listOfAnimals.ToArray())
            {
                animal.LiveAnimalCicle(_listOfAnimals, _listOfAllPlants, _listOfFruits, _listOfFoodForOmnivorous);
            }
        }
        private void UpdatePlants()
        {
            foreach (Plant plant in _listOfAllPlants.ToArray())
            {
                plant.LivePlantCicle(_listOfFruits, _listOfAllPlants);
            }
        }
        private void UpdateFood()
        {
            _listOfFoodForHerbivorous = new();
            _listOfFoodForOmnivorous = new();

            foreach (Plant plant in _listOfAllPlants.ToArray())
            {
                if (plant is EdiblePlant && plant.Stage != PlantStage.seed)
                {
                    _listOfFoodForHerbivorous.Add(plant);
                }
            }
            foreach (Fruit fruit in _listOfFruits)
            {
                _listOfFoodForHerbivorous.Add(fruit);
            }
            foreach (FoodForHerbivorous f in _listOfFoodForHerbivorous)
            {
                _listOfFoodForOmnivorous.Add(f);
            }
            foreach (Animal an in _listOfAnimals)
            {
                _listOfFoodForOmnivorous.Add(an);
            }
        }
        private void UpdateSeason(int timerValue)
        {
            if (timerValue % 10 == 0)
                s_currentSeason = (timerValue % 20 == 0) ? Season.summer : Season.winter;
        }

        public void LiveOneCicle(int timerValue)
        {
            UpdateSeason(timerValue);
            UpdateAnimals();
            UpdatePlants();
            UpdateFood();
            _rendering.UpdateField(_listOfAnimals, _listOfAllPlants, _listOfFruits);
            listOfAnimalsCopy = _listOfAnimals;
        }


        public void CreateFirstGeneration()
        {
            Random random = new();
            for (int x = 0; x < Form1.s_cols; x++)
            {
                for (int y = 0; y < Form1.s_rows; y++)
                {
                    if (random.Next(_densityAnimals) == 0)
                    {
                        switch (random.Next(9))
                        {
                            case 0:
                                _listOfAnimals.Add(new Rabbit((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.rabbit, x, y);
                                break;
                            case 1:
                                _listOfAnimals.Add(new Horse((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.horse, x, y);
                                break;
                            case 2:
                                _listOfAnimals.Add(new Giraffe((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.giraffe, x, y);
                                break;
                            case 3:
                                _listOfAnimals.Add(new Tiger((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.tiger, x, y);
                                break;
                            case 4:
                                _listOfAnimals.Add(new Wolf((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.wolf, x, y);
                                break;
                            case 5:
                                _listOfAnimals.Add(new Fox((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.fox, x, y);
                                break;
                            case 6:
                                _listOfAnimals.Add(new Bear((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.bear, x, y);
                                break;
                            case 7:
                                _listOfAnimals.Add(new Pig((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.pig, x, y);
                                break;
                            case 8:
                                _listOfAnimals.Add(new Rat((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.rat, x, y);
                                break;
                        }
                    }
                    else
                    {
                        switch (random.Next(_densityPlants))
                        {
                            case 0:
                                EdiblePlant newEPlant = new((x, y));
                                _listOfAllPlants.Add(newEPlant);
                                //if (newEPlant.IsHealthy())
                                //    _rendering.DrawFirstGeneration(MapObject.ediblePlantHealthy, x, y);
                                //else
                                //    _rendering.DrawFirstGeneration(MapObject.ediblePlantPoisonous, x, y);
                                break;

                            case 1:
                                _listOfAllPlants.Add(new InediblePlant((x, y)));
                                //_rendering.DrawFirstGeneration(MapObject.inediblePlant, x, y);
                                break;
                        }

                    }
                    listOfAnimalsCopy = _listOfAnimals;
                }
            }

        }
    }
}
