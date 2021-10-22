using System;

namespace LabOOP1
{
    public class Fruit: FoodForHerbivores
    {
        private (int, int) _position;
        private int _densityHealthyPlant = 4;

        public bool _isHealthy = true;

        public Fruit((int, int) pos)
        {
            _position = pos;
            Random random = new();
            if (random.Next(_densityHealthyPlant) == 0)
            {
                _isHealthy = false;
            }
        }
        public (int, int) GetPosition()
        {
            return _position;
        }

        public bool IsHealthy()
        {
            return _isHealthy;
        }
        public void SetStatus(bool status)
        {
            _isHealthy = status;
        }
    }
}
