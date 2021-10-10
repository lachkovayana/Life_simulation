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
    
    private bool CheckGrowth()
    {
        if (Stage == PlantStage.grown && (_age % 10 == 0))
        {
            return true;
        }
        return false;
    }
    private bool CheckForm()
    {
        if (Stage == PlantStage.grown && (_age % 10 == 0))
        {
            return true;
        }
        return false;
    }

    private void FormSeeds(List<Plant> listOfNewPlants)
    {
        if (this is EdiblePlant plant1)
        {
            var newPlant = new EdiblePlant(FindNewCell());
            newPlant.SetStatus(((EdiblePlant)this).IsHealthy());
            listOfNewPlants.Add(newPlant);
        }
        else
        {
            var newPlant = new InediblePlant(FindNewCell());
            newPlant.SetStatus(((InediblePlant)this).GetStatus());
            listOfNewPlants.Add(newPlant);
        }
    }

    private void UpdateAge()
    {
        _age++;
        if (_age == 15)
        {
            Stage = PlantStage.sprout;
        }
        if (_age == 35)
        {
            Stage = PlantStage.grown;
        }
        if (_age == 55)
        {
            Stage = PlantStage.dead;
        }
    }


    public void LivePlantCicle(List<Plant> listOfAllPlants, List<Fruit> listOfFruits, List<Plant> listOfNewPlants)
    {
        UpdateAge();
        if (CheckGrowth()) { GrowFruit(listOfFruits); }
        if (CheckForm()) { FormSeeds(listOfNewPlants); }
    }
}
