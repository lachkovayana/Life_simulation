using System;
using System.Collections.Generic;

public abstract class Plant
{
    private const int _density = 3;
    private (int, int) _position;
    private int _age = 0;
    private bool _isHealthy = true;
    private bool _isGrowing = true;
    public PlantStage Stage = PlantStage.seed;

    public Plant((int, int) pos)
    {
        _position = pos;
        Random random = new Random();
        if (random.Next(_density) == 0)
        {
            _isHealthy = false;
        }if (random.Next(_density) == 0)
        {
            _isGrowing = false;
        }

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

   
    private void SetStatus(bool statusHealth, bool statusGrowth)
    {
        _isHealthy = statusHealth;
        _isGrowing = statusGrowth;

    }
    public bool IsHealthy()
    {
        return _isHealthy;
    }
    public bool IsGrowth()
    {
        return _isGrowing;
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
        if (this is EdiblePlant)
        {
            var newPlant = new EdiblePlant(FindNewCell());
            newPlant.SetStatus(IsHealthy(), IsGrowth());
            listOfNewPlants.Add(newPlant);
        }
        else
        {
            var newPlant = new InediblePlant(FindNewCell());
            newPlant.SetStatus(IsHealthy(), IsGrowth());
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
        if (_age == 30)
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
