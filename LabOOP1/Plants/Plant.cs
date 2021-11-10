using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class Plant : FoodForHerbivorous
    {
        private (int, int) _position;
        private int _age = 0;
        protected bool _isFruiting = true;
        private bool _isAbleToSurviveTheWinter = true;
        private bool _isStopsFruits = false;

        protected Movement movement = new();
        public PlantStage Stage = PlantStage.seed;

        public Plant((int, int) pos) : base(pos)
        {
            _position = pos;

            Random random = new();
            _isFruiting = random.Next(10) <= 5;
            _isAbleToSurviveTheWinter = random.Next(10) <= 3;
            _isStopsFruits = random.Next(10) <= 5;
        }

        private bool CheckGrowth()
        {
            if (_isFruiting && Stage == PlantStage.grown && (_age % 10 == 0))
            {
                if (MapObjectsControl.s_currentSeason == Season.winter)
                {
                    if (!_isStopsFruits)
                        return true;
                }
                else 
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
        protected virtual void FormSeeds(List<Plant> listOfAllPlants) { }

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

            if (_age > 100 || (MapObjectsControl.s_currentSeason == Season.winter && !_isAbleToSurviveTheWinter && Stage != PlantStage.seed))
                Die(listOfAllPlants);

            else if (!(MapObjectsControl.s_currentSeason == Season.winter && !_isAbleToSurviveTheWinter && Stage == PlantStage.seed))
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
            if (this is EdiblePlant)
            {
                name = (_isHealthy ? "healthy" : "poisonous") + " " + name;
            }
             name += (_isAbleToSurviveTheWinter ? " and I am able to survive the winter" : " and I will die when winter comes... ");
             name += "\r\n" + (_isStopsFruits ? "Also, I stop growing fruit" : "But I can grow fruit every season, you know!");
            
            string result = string.Concat("Hey! I am an ", name,
                ".\r\nI'm a ", Stage, " now",
                ".\r\nMy position now is ", currentPosition);
            return result;
        }
    }
}
