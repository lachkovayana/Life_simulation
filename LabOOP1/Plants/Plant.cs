using System;

public class Plant
{
	private (int, int) _position;

	public Plant((int, int) pos)
	{
		_position = pos;
	}
	public (int, int) GetPosition()
	{
		return _position;
	}



}
