using System;
using System.Collections.Generic;
using System.Linq;

namespace LabOOP1
{
    public class MapObjectsControl
    {
        private const int _densityAnimals = 30;
        private const int _densityPlants = 50;
        private const int _densityHumans = 50;
        private const int _densitySources = 1040;

        private List<Animal> _listOfAnimals = new();
        private List<Plant> _listOfAllPlants = new();
        private List<Fruit> _listOfFruits = new();
        private List<Animal> _listOfHumans = new();
        private List<Source> _listOfSources = new();
        private List<FoodForHerbivorous> _listOfFoodForHerbivorous = new();
        private List<FoodForOmnivorous> _listOfFoodForOmnivorous = new();

        private readonly Rendering _rendering = new();

        internal static Season s_currentSeason;
        internal static List<Building> ListOfBuildings = new();
        internal static List<Animal> listOfAnimalsCopy = new();

        internal static List<MapObject>[,] FieldOfAllMapObjects = new List<MapObject>[Form1.s_cols, Form1.s_rows];
        internal static List<List<MapObject>> ListOfVillages = new();


        public MapObjectsControl()
        {
            s_currentSeason = Season.summer;

        }

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

        private void UpdateMen()
        {
            foreach (Human human in _listOfHumans.ToArray())
            {
                human.LiveHumanCicle(_listOfHumans, _listOfFoodForOmnivorous, _listOfAnimals, _listOfAllPlants, _listOfFruits);
            }
        }

        private void UpdateSeason(int timerValue)
        {
            if (timerValue % 25 == 0)
                s_currentSeason = (timerValue % 50 == 0) ? Season.summer : Season.winter;
        }

        public void LiveOneCicle(int timerValue)
        {
            UpdateSeason(timerValue);
            UpdateAnimals();
            UpdatePlants();
            UpdateFood();
            UpdateMen();
            ClearField();
            _rendering.DrawField(_listOfAnimals, _listOfAllPlants, _listOfFruits, _listOfHumans);
            listOfAnimalsCopy = _listOfAnimals;
        }

        private void ClearField()
        {
            for (int x = 0; x < Form1.s_cols; x++)
            {
                for (int y = 0; y < Form1.s_rows; y++)
                {
                    FieldOfAllMapObjects[x, y].Clear();
                }
            }
        }

        public void CreateFirstGeneration()
        {
            Random random = new();
            for (int x = 0; x < Form1.s_cols; x++)
            {
                for (int y = 0; y < Form1.s_rows; y++)
                {
                    FieldOfAllMapObjects[x, y] = new List<MapObject>();
                    if (random.Next(_densityAnimals) == 0)
                    {
                        switch (random.Next(9))
                        {
                            case 0:
                                _listOfAnimals.Add(new Rabbit((x, y)));
                                break;
                            case 1:
                                _listOfAnimals.Add(new Horse((x, y)));
                                break;
                            case 2:
                                _listOfAnimals.Add(new Sheep((x, y)));
                                break;
                            case 3:
                                _listOfAnimals.Add(new Tiger((x, y)));
                                break;
                            case 4:
                                _listOfAnimals.Add(new Wolf((x, y)));
                                break;
                            case 5:
                                _listOfAnimals.Add(new Fox((x, y)));
                                break;
                            case 6:
                                _listOfAnimals.Add(new Bear((x, y)));
                                break;
                            case 7:
                                _listOfAnimals.Add(new Pig((x, y)));
                                break;
                            case 8:
                                _listOfAnimals.Add(new Rat((x, y)));
                                break;
                        }
                    }
                    else if (random.Next(_densityPlants) == 0)
                    {
                        switch (random.Next(0, 2))
                        {
                            case 0:
                                _listOfAllPlants.Add(new EdiblePlant((x, y)));
                                break;

                            case 1:
                                _listOfAllPlants.Add(new InediblePlant((x, y)));
                                //_listOfSources.Add(new Wood((x, y)));
                                break;
                        }

                    }
                    else if (random.Next(_densityHumans) == 0)
                    {
                        _listOfHumans.Add(new Human((x, y)));

                    }
                    else if (random.Next(_densitySources) == 0)
                    {
                        switch (random.Next(4))
                        {
                            case 0:
                                _listOfSources.Add(new GoldSource((x, y)));
                                break;
                            case 1:
                                _listOfSources.Add(new StoneSource((x, y)));

                                break;
                            case 2:
                                _listOfSources.Add(new IronSource((x, y)));

                                break;
                            case 3:
                                _listOfSources.Add(new WoodSource((x, y)));

                                break;

                        }
                    }
                    listOfAnimalsCopy = _listOfAnimals;
                }
            }
            ListOfBuildings.Clear();
            ListOfVillages.Clear();
        }
       
        private static void ChangeIndex(int replaceableIndex, int newIndex)
        {
            foreach (MapObject mapObj in ListOfVillages[replaceableIndex])
            {
                if (mapObj is Human human)
                {
                    human.indexOfVillage = newIndex;
                }
                if (mapObj is House house)
                {
                    house.indexOfVillage = newIndex;
                }
                ListOfVillages[newIndex].Add(mapObj);
            }
            ListOfVillages[replaceableIndex].Clear();
        }
        public static void DefineIndices(((int, int), (int, int, int)) data)
        {
            var newHousePosition = data.Item1;
            var indexOfNewHouse = data.Item2.Item3;

            for (int x = newHousePosition.Item1 - 1; x <= newHousePosition.Item1 + 1; x++)
            {
                for (int y = newHousePosition.Item2 - 1; y <= newHousePosition.Item2 + 1; y++)
                {
                    if (x >= 0 && y >= 0 && x < Form1.s_cols && y < Form1.s_rows)
                    {
                        //если рядом есть дом
                        House houseAtThisPosition = FieldOfAllMapObjects[x, y].OfType<House>().FirstOrDefault();
                        if (houseAtThisPosition != null)
                        {
                            //и индекс деревни этого дома отличается от индекса деревни нашего строящегося дома
                            var indexOfNearbyHouse = houseAtThisPosition.indexOfVillage;
                            if (indexOfNearbyHouse != indexOfNewHouse)
                            {
                                //объединяем эти две деревни, индексом обеих становится минимальный индекс
                                ChangeIndex(Math.Max(indexOfNewHouse, indexOfNearbyHouse), Math.Min(indexOfNewHouse, indexOfNearbyHouse));
                            }
                        }
                    }
                }
            }
        }
    }
}
