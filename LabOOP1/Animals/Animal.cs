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

    private void MoveToTarget(List<Plant> listOfAllPlants)
    {
        var posAn = _position;
        var min = 100000;
        EdiblePlant target = new((-1, -1));
        (int, int) newPosAn = posAn;

        foreach (Plant plant in listOfAllPlants)
        {
            if (plant is EdiblePlant && plant.stage != PlantStage.seed) {
                    var posPl = plant.GetPosition();
                    var tmpx = Math.Abs(posAn.Item1 - posPl.Item1);
                    var tmpy = Math.Abs(posAn.Item2 - posPl.Item2);
                    if (tmpx + tmpy < min)
                    {
                        min = tmpx + tmpx;
                        target = (EdiblePlant)plant;
                    }
                
            }
        }
        
        var distx = posAn.Item1 - target.GetPosition().Item1;
        var disty = posAn.Item2 - target.GetPosition().Item2;

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
                if (target.IsHealthy)
                {
                    RiseSatiety();
                }
                else
                {
                    DecreaseHealthByZero();
                }

                listOfAllPlants.Remove(target);
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

    public void LiveAnimalCicle(List<Plant> listOfAllPlants, List<Animal> removeList)
    {
        DecreaseSatiety(removeList);
        
        if (_isHungry)
        {
            MoveToTarget(listOfAllPlants);
        }
        else
        {
            MoveToRandomCell();
        }
    }

}
