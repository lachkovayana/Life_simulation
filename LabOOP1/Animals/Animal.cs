﻿using System;

public class Animal
{
    private (int, int) position;
    private int health = 100;
    private int satiety = 100;

    public Animal((int, int) pos)
    {

        position = pos;
    }
    private void riseHealth()
    {
        health += 5;
    }
    private void riseSatiety()
    {
        satiety += 5;
        if (health <= 95)
            health += 5;
    }
    private void decreaseHealth()
    {
        if (health - 5 == 0)
            Die();
        else
            health -= 5;
    }
    private void decreaseSatiety()
    {
        if (satiety < 30 && satiety > 0)
            decreaseHealth();
        else
            Die();
    }

    public void Die()
    {
        //deletefrommap
    }

    public (int, int) GetHP()
    {
        return (health, satiety);
    }
    //(getHp()).Item1..Item2

    public (int, int) GetPosition()
    {
        return position;
    }
    public (int, int) SetPosition((int,int) pos)
    {
        this.position = pos;
        return position;
    }


}
