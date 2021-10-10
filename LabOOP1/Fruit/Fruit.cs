using System;

public class Fruit
{
    private (int, int) _position;
    private int _densityHealthyPlant = 4;

    public bool IsHealthy = true;

    public Fruit((int, int) pos)
    {
        _position = pos;
        Random random = new();
        if (random.Next(_densityHealthyPlant) == 0)
        {
            IsHealthy = false;
        }
    }
    public (int, int) GetPosition()
    {
        return _position;
    }
}
