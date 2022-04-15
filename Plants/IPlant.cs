using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public interface IPlant
    {
        protected (int, int) FindNewCell((int, int) position)
        {
            Random rnd = new();

            int x, y;
            do
            {
                x = position.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols);

            do
            {
                y = position.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows);
            return (x, y);
        }


        protected bool CheckGrowth(bool isFruiting, PlantStage stage, int age)
        {
            if (isFruiting && stage == PlantStage.grown && (age % 10 == 0))
            {
                return true;
            }
            return false;
        }
        protected bool CheckForm(PlantStage stage, int age)
        {
            if (stage == PlantStage.grown && (age % 10 == 0))
            {
                return true;
            }
            return false;
        }

        protected void GrowFruit(List<Fruit> listOfFruits, (int, int) position)
        {
            Random rnd = new();
            for (int i = 0; i < rnd.Next(3); i++)
            {
                Fruit fruit = new(FindNewCell(position));
                listOfFruits.Add(fruit);
            }
        }

       
    }
}
