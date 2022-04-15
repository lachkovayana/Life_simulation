using System;
using System.Collections.Generic;


namespace LabOOP1
{
    public class EdiblePlant : Plant
    {
        private const int _density = 6;
        public EdiblePlant((int, int) pos) : base(pos)
        {
            Random random = new();
            if (random.Next(_density) == 0)
            {
                _isHealthy = false;
            }
        }

        private void SetStatus(bool statusHealth, bool statusGrowth)
        {
            _isHealthy = statusHealth;
            _isFruiting = statusGrowth;
        }
        protected override void FormSeeds(List<Plant> listOfAllPlants)
        {
            EdiblePlant newPlant = new(movement.GetClosestCell(currentPosition));
            newPlant.SetStatus(_isHealthy, _isFruiting);
            listOfAllPlants.Add(newPlant);
        }
    }
}
