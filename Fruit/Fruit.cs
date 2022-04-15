using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Fruit : FoodForHerbivorous
    {
        public Fruit((int, int) pos) : base(pos) { }

        //protected override int NutritionalUnit { get;}

        public void Die(List<Fruit> list)
        {
            list.Remove(this);
        }
        protected override string GetInfo()
        {
            string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();
            string result = string.Concat("Hey! I am a ", name,
                ".\r\nMy position now is ", currentPosition);
            return result;
        }
    }
}
