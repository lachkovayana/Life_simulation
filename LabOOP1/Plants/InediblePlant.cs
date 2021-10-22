using System;
using System.Collections.Generic;


namespace LabOOP1
{
    public class InediblePlant : Plant
    {

        public InediblePlant((int, int) pos) : base(pos)
        {

        }

        private void SetStatus(bool status)
        {
            _isFruiting = status;
        }
        public override void FormSeeds(List<Plant> listOfNewPlants)
        {
            InediblePlant newPlant = new(FindNewCell());
            newPlant.SetStatus(_isFruiting);
            listOfNewPlants.Add(newPlant);
        }
    }
}