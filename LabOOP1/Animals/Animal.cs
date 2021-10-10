using System;
using System.Collections.Generic;


public class Animal
{
    private (int, int) _position;
    private int _health = 100;
    private int _satiety = 100;
    private int _rows;
    private int _cols;
    private bool _isHungry = false;

    public Animal(int rows, int cols, (int, int) pos)
    {
        _rows = rows;
        _cols = cols;
        _position = pos;
    }

    private void RiseHealth()
    {
        _health += 50;
    }
    private void RiseSatiety()
    {
        _satiety = 100;
        _isHungry = false;
        RiseHealth();
    }
    private void DecreaseHealth(List<Animal> removeList)
    {
        _health -= 5;
        if (_health <= 0)
            Die(removeList);
    }
    private void DecreaseHealthByZero()
    {
        _health = 0;
    }

    private void DecreaseSatiety(List<Animal> removeList)
    {
        _satiety -= 5;
        if (_satiety <= 30)
        {
            _isHungry = true;
            DecreaseHealth(removeList);
        }
    }

    private void Die(List<Animal> removeList)
    {
        removeList.Add(this);
    }

    private void MoveToRandomCell()
    {
        Random rnd = new Random();

        int x, y;
        do
        {
            x = _position.Item1 + rnd.Next(-1, 2);
        }
        while (x < 0 || x >= _cols);

        do
        {
            y = _position.Item2 + rnd.Next(-1, 2);
        }
        while (y < 0 || y >= _rows);

        SetPosition((x, y));

    }

    private (int, int, int) ChooseTarget(Fruit targetFruit, Plant targetPlant, (int, int) posAn)
    {
        if ((targetFruit.GetPosition().Item1 + targetFruit.GetPosition().Item2) < (targetPlant.GetPosition().Item1 + targetPlant.GetPosition().Item2))
        {
            var x = posAn.Item1 - targetFruit.GetPosition().Item1;
            var y = posAn.Item2 - targetFruit.GetPosition().Item2;
            return (x, y, 1);
        }
        var distx = posAn.Item1 - targetPlant.GetPosition().Item1;
        var disty = posAn.Item2 - targetPlant.GetPosition().Item2;
        return (distx, disty, 2);

    }

    private void MoveToTarget(List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
    {
        var posAn = _position;
        var minPl = 100000;
        var minFr = 100000;
        EdiblePlant targetPlant = new((-1, -1));
        Fruit targetFruit = new((1001, 1001));
        (int, int) newPosAn = posAn;

        foreach (Plant plant in listOfAllPlants)
        {
            if (plant is EdiblePlant && plant.Stage != PlantStage.seed)
            {
                var posPl = plant.GetPosition();
                var tmpx = Math.Abs(posAn.Item1 - posPl.Item1);
                var tmpy = Math.Abs(posAn.Item2 - posPl.Item2);
                if (tmpx + tmpy < minPl)
                {
                    minPl = tmpx + tmpx;
                    targetPlant = (EdiblePlant)plant;
                }
            }
        }
        foreach (Fruit fruit in listOfFruits)
        {
            var posFr = fruit.GetPosition();
            var tmpx = Math.Abs(posAn.Item1 - posFr.Item1);
            var tmpy = Math.Abs(posAn.Item2 - posFr.Item2);
            if (tmpx + tmpy < minFr)
            {
                minFr = tmpx + tmpx;
                targetFruit = fruit;
            }
        }

        var target = ChooseTarget(targetFruit, targetPlant, posAn);

        var distx = target.Item1;
        var disty = target.Item2;

        //var distx = posAn.Item1 - targetFruit.GetPosition().Item1;
        //var disty = posAn.Item2 - targetFruit.GetPosition().Item2;

        if (distx < 0)
        {
            if (disty > 0)
            {
                newPosAn = MoveToDirection(posAn, "right");
                newPosAn = MoveToDirection(newPosAn, "down");
            }
            else if (disty < 0)
            {
                newPosAn = MoveToDirection(posAn, "right");
                newPosAn = MoveToDirection(newPosAn, "up");
            }
            else
            {
                newPosAn = MoveToDirection(posAn, "right");
            }
        }
        else if (distx > 0)
        {
            if (disty > 0)
            {
                newPosAn = MoveToDirection(posAn, "left");
                newPosAn = MoveToDirection(newPosAn, "down");
            }
            else if (disty < 0)
            {
                newPosAn = MoveToDirection(posAn, "left");
                newPosAn = MoveToDirection(newPosAn, "up"); ;
            }
            else
            {
                newPosAn = MoveToDirection(posAn, "left");
            }
        }
        else
        {
            if (disty > 0)
            {
                newPosAn = MoveToDirection(posAn, "down"); ;

            }
            else if (disty < 0)
            {
                newPosAn = MoveToDirection(posAn, "up"); ;

            }
            else
            {
                if (target.Item3 == 2)
                {
                    if (targetPlant.IsHealthy())
                    {
                        RiseSatiety();
                    }
                    else
                    {
                        DecreaseHealthByZero();
                    }

                    listOfAllPlants.Remove(targetPlant);
                }
                else if (target.Item3 == 1)
                {
                    if (targetFruit.IsHealthy())
                    {
                        RiseSatiety();
                    }
                    else
                    {
                        DecreaseHealthByZero();
                    }
                    listOfFruits.Remove(targetFruit);

                }
            }
        }
        SetPosition(newPosAn);
    }

    private (int, int) MoveToDirection((int, int) pos, string direction)
    {
        int x = pos.Item1;
        int y = pos.Item2;

        switch (direction)
        {
            case "right":
                return (x + 1, y);
            case "left":
                return (x - 1, y);
            case "up":
                return (x, y + 1);
            case "down":
                return (x, y - 1);
        }
        return (x, y);
    }

    private void SetPosition((int, int) pos)
    {
        _position = pos;
    }

    public (int, int) GetPosition()
    {
        return _position;
    }

    public void LiveAnimalCicle(List<Plant> listOfAllPlants, List<Fruit> listOfFruits, List<Animal> removeList)
    {
        DecreaseSatiety(removeList);

        if (_isHungry)
        {
            MoveToTarget(listOfAllPlants, listOfFruits);
        }
        else
        {
            MoveToRandomCell();
        }
    }

}
