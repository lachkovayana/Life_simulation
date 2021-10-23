using System;
using System.Collections.Generic;

namespace LabOOP1
{
    class Constants
    {
        public const int ImpVal = 1000000000;
    }

    public class Animal : FoodForOmnivores
    {
        private (int, int) _position;
        private int _health = 100;
        private int _satiety = 100;
        private int _timeSinceBreeding = 0;

        private bool _isHungry = false;

        private NutritionMethod _nutritionMethod = NutritionMethod.herbivore;

        public Animal((int, int) pos):base(pos)
        {
            _position = pos;
            Random random = new();
            _nutritionMethod = (NutritionMethod)random.Next(0, 3);
        }

        private void RiseHealth()
        {
            _health += 50;
        }
        private void RiseSatiety()
        {
            _satiety = 100;
            _isHungry = false;
            RiseHealth();
        }
        private void DecreaseHealth(List<Animal> listOfAnimals)
        {
            _health -= 5;
            if (_health <= 0)
                listOfAnimals.Remove(this);
        }
        private void DecreaseHealthByZero()
        {
            _health = 0;
        }

        private void DecreaseSatiety(List<Animal> listOfAnimals)

        {
            _satiety -= 5;
            if (_satiety <= 30)
            {
                _isHungry = true;
                DecreaseHealth(listOfAnimals);
            }
        }

        private void MoveToRandomCell()
        {
            Random rnd = new();

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

            SetPosition((x, y));

        }
        public override (int, int) GetPosition()
        {
            return _position;
        }

        private void MoveToTarget(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits, List<FoodForHerbivores> listOfFoodForHerbivores, List<FoodForOmnivores> _listOfFoodForOmnivores)
        {
            var minDist = Constants.ImpVal;
            (int, int) newPosAn = _position;


            FoodForOmnivores target = this;
           
            foreach (FoodForOmnivores f in _listOfFoodForOmnivores)
            {
                if ((_nutritionMethod == NutritionMethod.herbivore && (f is Fruit || f is EdiblePlant)) ||
                    (_nutritionMethod == NutritionMethod.carnivorous && f is Animal && !f.Equals(this)) ||
                    (_nutritionMethod == NutritionMethod.omnivorous && !f.Equals(this)))
                {
                    var posFood = f.GetPosition();
                    var tmpx = Math.Abs(_position.Item1 - posFood.Item1);
                    var tmpy = Math.Abs(_position.Item2 - posFood.Item2);
                    if (tmpx + tmpy < minDist)
                    {
                        minDist = tmpx + tmpx;
                        target = f;
                    }
                }
            }

            var distx = _position.Item1 - target.GetPosition().Item1;
            var disty = _position.Item2 - target.GetPosition().Item2;

            if (distx < 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(_position, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(_position, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.up);
                }
                else
                {
                    newPosAn = MoveToDirection(_position, Direction.right);
                }
            }
            else if (distx > 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(_position, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(_position, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.up); ;
                }
                else
                {
                    newPosAn = MoveToDirection(_position, Direction.left);
                }
            }
            else
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(_position, Direction.down); ;

                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(_position, Direction.up); ;

                }
                else
                {
                    if (target is FoodForHerbivores f)
                    {
                        if (f.IsHealthy())
                        {
                            RiseSatiety();
                        }
                        else
                        {
                            DecreaseHealthByZero();
                        }
                    }
                    if (target is Fruit target2)
                    {
                        listOfFruits.Remove(target2);
                    }
                    else if (target is EdiblePlant target3)
                    {
                        listOfAllPlants.Remove(target3);
                    }
                    else if (target is Animal target4)
                    {
                        listOfAnimals.Remove(target4);
                    }
                }
            }
            SetPosition(newPosAn);
        }

        private static (int, int) MoveToDirection((int, int) pos, Direction direction)
        {
            int x = pos.Item1;
            int y = pos.Item2;

            switch (direction)
            {
                case Direction.right:
                    return (x + 1, y);
                case Direction.left:
                    return (x - 1, y);
                case Direction.up:
                    return (x, y + 1);
                case Direction.down:
                    return (x, y - 1);
                default:
                    break;
            }
            return (x, y);
        }
        private void UpdateTime()
        {
            _timeSinceBreeding++;
        }

        private void SetPosition((int, int) pos)
        {
            _position = pos;
        }


        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForHerbivores> listOfFoodForHerbivores, List<FoodForOmnivores> listOfFoodForOmnivores)
        {
            UpdateTime();
            DecreaseSatiety(listOfAnimals);

            if (_isHungry)
            {
                MoveToTarget(listOfAnimals, listOfPlants, listOfFruits, listOfFoodForHerbivores, listOfFoodForOmnivores);
            }
            else
            {
                MoveToRandomCell();
            }
        }
        //160 273
    }
}
