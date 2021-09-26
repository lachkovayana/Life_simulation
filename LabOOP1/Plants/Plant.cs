using System;

public class Plant
{
	private (int, int) position;

	public Plant((int, int) pos)
	{
		position = pos;
	}
	public void WasEaten()
	{
		//deletefrommap
	}
	public (int, int) GetPosition()
	{
		return position;
	}
}
