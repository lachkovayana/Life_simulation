using System;

public abstract class Plant
{
    private (int, int) _position;
    private int _age = 0;
    public PlantStage stage = PlantStage.seed;

    public Plant((int, int) pos)
    {
        _position = pos;
    }

    public (int, int) GetPosition()
    {
        return _position;
    }

    public void StartGrowingSeed()
    {
        Random rnd = new();
        for (int i = 1; i < rnd.Next(4); i++)
        {

        }
    }

    public void UpdateAge()
    {
        _age++;
        if (_age == 45)
        {
            stage = PlantStage.dead;
            StartGrowingSeed();
        }
        if (_age == 30)
        {
            stage = PlantStage.grown ;
            StartGrowingSeed();
        }
        if (_age == 15)
        {
            stage = PlantStage.sprout;
        }
    }

}
