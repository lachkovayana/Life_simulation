using System;

public abstract class FoodForOmnivorous
{
    protected (int, int) position;
    public FoodForOmnivorous((int, int) pos)
    {
        position = pos;
    }
    internal  (int, int) GetPosition()  => position;

    public virtual string GetTextInfo() { return ""; }
}
