using System;
using System.Collections.Generic;

public abstract class Plant
{
    private (int, int) _position;
    private int _age = 0;
    public PlantStage Stage = PlantStage.seed;

    public Plant((int, int) pos)
    {
        _position = pos;
    }

    private (int, int) FindNewCell()
    {
        Random rnd = new Random();

        int x, y;
        do
        {
            x = _position.Item1 + rnd.Next(-1, 2);
        }
        while (x < 0 || x >= 50);

        do
        {
            y = _position.Item2 + rnd.Next(-1, 2);
        }
        while (y < 0 || y >= 50);
        return (x, y);
    }


    public (int, int) GetPosition()
    {
        return _position;
    }

    public void GrowFruit(List<Fruit> listOfFruits)
    {
        Random rnd = new();
        for (int i = 0; i < rnd.Next(3); i++)
        {
            Fruit fruit = new(FindNewCell());
            listOfFruits.Add(fruit);
        }
    }
    //public void GrowSeed()
    //{

    //    Random rnd = new();
    //    for (int i = 1; i < rnd.Next(4); i++)
    //    {
    //        FindNewCell();

    //        // посев семени на клетку с найденными координатами
    //    }
    //}

    public void CheckGrowing(List<Fruit> listOfFruits)
    {
        if (this is EdiblePlant && Stage == PlantStage.grown && (_age % 10 == 0))
        {
            GrowFruit(listOfFruits);
        }
    }
        public void UpdateAge()
        {
            _age++;
            if (_age == 10)
            {
                Stage = PlantStage.sprout;
            }
            if (_age == 18)
            {
                Stage = PlantStage.grown;
            }
            if (_age == 35)
            {
                Stage = PlantStage.dead;
            }
        }

        public void LivePlantCicle(List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            UpdateAge();
            CheckGrowing(listOfFruits);
        }
    }
