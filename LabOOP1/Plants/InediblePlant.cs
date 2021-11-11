using System;
using System.Collections.Generic;


namespace LabOOP1
{
    public class InediblePlant : Plant
    {
        public InediblePlant((int, int) pos) : base(pos) { }

        private void SetStatus(bool status)
        {
            _isFruiting = status;
        }
        protected override void FormSeeds(List<Plant> listOfAllPlants)
        {
            InediblePlant newPlant = new(movement.GetClosestCell(currentPosition));
            newPlant.SetStatus(_isFruiting);
            listOfAllPlants.Add(newPlant);
        }
    }
}
