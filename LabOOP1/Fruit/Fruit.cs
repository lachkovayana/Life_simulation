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
        public override string GetTextInfo()
        {
            string name = GetType().ToString().Substring(GetType().ToString().IndexOf(".") + 1).ToLower();
            string result = string.Concat("Hey! I am an ", name,
                ".\r\nMy position now is ", currentPosition);
            return result;
        }
    }
}
