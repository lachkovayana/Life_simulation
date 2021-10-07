using System;

public abstract class Plant
{
    private (int, int) _position;
    private int _age = 0;
    public int stage = 1;

    public Plant((int, int) pos)
    {
        _position = pos;
    }

    public (int,int) GetPosition()
    {
        return _position;
    }


    public void UpdateAge()
    {
        _age++;
        if (_age >= 20)
        {
            stage = 3;
        }
        else if (_age >= 10)
        {
            stage = 2;
        }
    }

}
