using System;

public abstract class FoodForOmnivorous
{
    protected (int, int) currentPosition;
    protected (int, int) birthPosiiton;
    public FoodForOmnivorous((int, int) pos)
    {
        //для статических элементов это актуальная позиция, для движущихся - точка рождения
        birthPosiiton = pos;

        //текущая позиция позиция
        currentPosition = pos;
    }
    internal  (int, int) GetPosition()  => currentPosition;

    public virtual string GetTextInfo() { return ""; }
}
