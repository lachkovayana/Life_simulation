using System;

public abstract class FoodForOmnivorous
{
    private (int, int) _position;
    public FoodForOmnivorous((int, int) pos)
    {
        _position = pos;
    }
    internal virtual (int, int) GetPosition()
    {
        return _position;
    }
}
