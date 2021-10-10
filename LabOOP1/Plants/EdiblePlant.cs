using System;
//наследование
public class EdiblePlant : Plant
{

    private const int _densityHealthyPlant = 4;
    public bool IsHealthy = true;
    public EdiblePlant((int, int) pos) : base(pos)
    {
        Random random = new();
        if (random.Next(_densityHealthyPlant) == 0)
        {
            IsHealthy = false;
        }
        
    }
    
    public bool GetStatus()
    {
        return IsHealthy;
    }
  
}