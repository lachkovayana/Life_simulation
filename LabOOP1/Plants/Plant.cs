using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class Plant : FoodForHerbivorous
    {
        private const int _density = 10;
        private (int, int) _position;
        private int _age = 0;
        protected bool _isFruiting = true;

        public PlantStage Stage = PlantStage.seed;
        protected Movement movement = new();

        public Plant((int, int) pos) : base(pos)
        {
            _position = pos;

            Random random = new();

            if (random.Next(_density) == 0)
            {
                _isFruiting = false;
            }

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
            if (_age == 20)
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
            for (int i = 0; i < rnd.Next(2); i++)
            {
                Fruit fruit = new(movement.FindNewCell(_position));
                listOfFruits.Add(fruit);
            }
        }
        public virtual void FormSeeds(List<Plant> listOfAllPlants) { }

        public bool IsFruiting()
        {
            return _isFruiting;
        }

        public void Die(List<Plant> listOfAllPlants)
        {
            listOfAllPlants.Remove(this);
        }

        public void LivePlantCicle(List<Fruit> listOfFruits, List<Plant> listOfAllPlants)
        {
            UpdateAge();

            if (_age > 100)
                Die(listOfAllPlants);
            else
            {
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
        public override string GetTextInfo()
        {
            string name = GetType().ToString().Substring(GetType().ToString().IndexOf(".") + 1).ToLower();
            string result = string.Concat("Hey! I am an ", name,
                ".\r\nI'm a ", Stage, " now",
                ".\r\nMy position now is ", position);
            return result;
        }
    }
}
