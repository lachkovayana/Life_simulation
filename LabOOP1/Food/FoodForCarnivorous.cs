using System;

public abstract class FoodForCarnivorous: FoodForOmnivores
{
	public FoodForCarnivorous((int, int) pos) : base(pos)
	{
	}
}
