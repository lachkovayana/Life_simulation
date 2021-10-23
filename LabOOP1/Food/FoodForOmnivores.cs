using System;

public abstract class FoodForOmnivores
{
    private (int, int) _position;
    public FoodForOmnivores((int, int) pos)
    {
        _position = pos;
    }
    public virtual (int, int) GetPosition()
    {
        return _position;
    }
}
