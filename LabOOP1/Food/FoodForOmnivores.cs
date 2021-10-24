using System;

public abstract class FoodForOmnivorous
{
    protected (int, int) position;
    public FoodForOmnivorous((int, int) pos)
    {
        position = pos;
    }
    internal (int, int) GetPosition()
    {
        return position;
    }
}
