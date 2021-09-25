using System;

public class Plant
{
	private (int, int) position;

	public Plant((int, int) pos)
	{
		position = pos;
	}
	void wasEaten()
	{
		//deletefrommap
	}
	public (int, int) getPosition()
	{
		return position;
	}
}
