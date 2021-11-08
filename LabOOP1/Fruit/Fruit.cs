using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Fruit : FoodForHerbivorous
    {

        public Fruit((int, int) pos) : base(pos)
        {

        }

        public void Die(List <Fruit> list)
        {
            list.Remove(this);
        }
    }
}
