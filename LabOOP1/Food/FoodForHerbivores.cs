using System;
namespace LabOOP1
{
	public abstract class FoodForHerbivorous: FoodForOmnivorous
	{
		private readonly int _densityHealthyPlant = 4;
		protected bool _isHealthy = true; 
		public FoodForHerbivorous((int, int) pos) : base(pos)
		{
			Random random = new();
			if (random.Next(_densityHealthyPlant) == 0)
			{
				_isHealthy = false;
			}
		}
		public bool IsHealthy()
		{
			return _isHealthy;
		}
		public virtual void SetHealthStatus(bool status)
		{
			_isHealthy = status;
		}
	}
}
