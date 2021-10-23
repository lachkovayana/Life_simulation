using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class Plant : FoodForHerbivorous
    {
        private const int _density = 6;
        private (int, int) _position;
        private int _age = 0;
        internal bool _isFruiting = true;

        public PlantStage Stage = PlantStage.seed;

        public Plant((int, int) pos) : base(pos)
        {
            _position = pos;

            Random random = new();

            if (random.Next(_density) == 0)
            {
                _isFruiting = false;
            }

        }

        internal (int, int) FindNewCell()
        {
            Random rnd = new Random();

            int x, y;
            do
            {
                x = _position.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols);

            do
            {
                y = _position.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows);
            return (x, y);
        }


        private bool CheckGrowth()
        {
            if (_isFruiting && Stage == PlantStage.grown && (_age % 10 == 0))
            {
                return true;
            }
            return false;
        }
        private bool CheckForm()
        {
            if (Stage == PlantStage.grown && (_age % 10 == 0))
            {
                return true;
            }
            return false;
        }

        private void UpdateAge()
        {
            _age++;
            if (_age == 10)
            {
                Stage = PlantStage.sprout;
            }
            if (_age == 30)
            {
                Stage = PlantStage.grown;
            }
            if (_age == 55)
            {
                Stage = PlantStage.dead;
            }
        }
        private void GrowFruit(List<Fruit> listOfFruits)
        {
            Random rnd = new();
            for (int i = 0; i < rnd.Next(3); i++)
            {
                Fruit fruit = new(FindNewCell());
                listOfFruits.Add(fruit);
            }
        }
        public virtual void FormSeeds(List<Plant> listOfAllPlants) { }

        public bool IsFruiting()
        {
            return _isFruiting;
        }

        public void LivePlantCicle(List<Fruit> listOfFruits, List<Plant> listOfAllPlants)
        {
            UpdateAge();
            if (CheckGrowth())
            {
                GrowFruit(listOfFruits);
            }
            if (CheckForm())
            {
                FormSeeds(listOfAllPlants);
            }
        }
    }
}
