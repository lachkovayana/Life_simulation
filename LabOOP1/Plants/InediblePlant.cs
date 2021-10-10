using System;

public class InediblePlant: Plant
{

    private const int _densityHealthyPlant = 4;
    private bool _isHealthy = true;
    public InediblePlant((int, int) pos) : base(pos)
    {
        Random random = new();
        if (random.Next(_densityHealthyPlant) == 0)
        {
            _isHealthy = false;
        }

    }

    public bool GetStatus()
    {
        return _isHealthy;
    }
    public void SetStatus(bool status)
    {
        _isHealthy = status;
    }
}
