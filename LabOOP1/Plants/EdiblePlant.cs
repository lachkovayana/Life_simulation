using System;
using System.Collections.Generic;


namespace LabOOP1
{
    public class EdiblePlant : Plant
    {
        private const int _density = 6;
        private bool _isHealthy = true;

        public EdiblePlant((int, int) pos) : base(pos)
        {
            Random random = new();
            if (random.Next(_density) == 0)
            {
                _isHealthy = false;
            }
        }
        public bool IsHealthy()
        {
            return _isHealthy;
        }
        private void SetStatus(bool statusHealth, bool statusGrowth)
        {
            _isHealthy = statusHealth;
            _isFruiting = statusGrowth;
        }
        public override void FormSeeds(List<Plant> listOfNewPlants)
        {
            EdiblePlant newPlant = new(FindNewCell());
            newPlant.SetStatus(_isHealthy, _isFruiting);
            listOfNewPlants.Add(newPlant);
        }
    }
}