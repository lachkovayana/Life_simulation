using System;
using System.Collections.Generic;


public class Animal
{
    private (int, int) _position;
    private int _health = 100;
    private int _satiety = 100;
    public bool isHungry = false;
    private int _rows;
    private int _cols;

    public Animal(int rows, int cols, (int, int) pos)
    {
        _rows = rows;
        _cols = cols;
        _position = pos;
    }
    private void RiseHealth()
    {
            _health += 15;
    }
    public void RiseSatiety()
    {
        if (_satiety >= 30)
            isHungry = false;
        if (_satiety < 100)
            _satiety += 15;
        if (_health <= 85)
            RiseHealth();
    }
    private void DecreaseHealth(List<Animal> removeList)
    {
        if (_health <= 0)
            Die(removeList);
        else
            _health -= 5;
    }

    public void DecreaseSatiety(List<Animal> removeList)
    {
        if (_satiety <= 30)
        {
            isHungry = true;
            DecreaseHealth(removeList);
        }
        _satiety -= 5;
    }

    public void Die(List<Animal> removeList)
    {
        removeList.Add(this);
    }

    public int GetHealth()
    {
        return _health;
    }

    public bool IsHungry()
    {
        if (_satiety < 30)
        {
            return true;
        }
        return false;
    }

    public (int, int) GetPosition()
    {
        return _position;
    }
    public (int, int) SetPosition((int, int) pos)
    {
        _position = pos;
        return _position;
    }

    public void MoveToRandomCell()
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

    public void MoveToTarget(List<Plant> listOfPlants)
    {
        var posAn = _position;
        var min = 100000;
        Plant target = new((-1, -1));
        (int, int) newPosAn = posAn;

        foreach (Plant plant in listOfPlants)
        {
            var posPl = plant.GetPosition();
            var tmpx = Math.Abs(posAn.Item1 - posPl.Item1);
            var tmpy = Math.Abs(posAn.Item2 - posPl.Item2);

            if (tmpx + tmpy < min)
            {
                min = tmpx + tmpx;
                target = plant;
            }
        }

        var distx = posAn.Item1 - target.GetPosition().Item1;
        var disty = posAn.Item2 - target.GetPosition().Item2;

        if (distx < 0)
        {
            if (disty > 0)
            {
                newPosAn = MoveToDirection(posAn, "right");
                newPosAn = MoveToDirection(posAn, "down");
            }
            else if (disty < 0)
            {
                newPosAn = MoveToDirection(posAn, "right");
                newPosAn = MoveToDirection(posAn, "up");
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
                newPosAn = MoveToDirection(posAn, "down");
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
                RiseSatiety();

                listOfPlants.Remove(target);

                //Plant newPlant = new Plant(FindFreeCell());
                //listOfPlants[listOfPlants.FindIndex(ind => ind.Equals(target))] = newPlant;
                //rendering.DrawPlant(newPlant.GetPosition().Item1, newPlant.GetPosition().Item2);
            }
        }
        //rendering.removeFromField(posAn);
        SetPosition(newPosAn);
        //rendering.Draw(newPosAn);
    }

    private (int, int) MoveToDirection((int, int) pos, string direction)
    {
        switch (direction)
        {
            case "right":
                return (pos.Item1 + 1, pos.Item2);
            case "left":
                return (pos.Item1 - 1, pos.Item2);
            case "up":
                return (pos.Item1, pos.Item2 + 1);
            case "down":
                return (pos.Item1, pos.Item2 - 1);
        }
        return (pos.Item1, pos.Item2);
    }


    public void Move(List<Plant> listOfPlants)
    {
        if (IsHungry() && listOfPlants.Count != 0)
        {
            MoveToTarget(listOfPlants);
        }
        else
        {
            MoveToRandomCell();
        }
    }

}
