using System;
using System.Collections.Generic;

namespace LabOOP1
{
    class Constants
    {
        public const int ImpVal = 1000000000; 
    }
    public class Animal
    {
        private (int, int) _position;
        private int _health = 100;
        private int _satiety = 100;
        private bool _isHungry = false;
        private NutritionMethod nm = NutritionMethod.herbivore;

        public Animal((int, int) pos)
        {
            _position = pos;
            Random random = new();
            nm = (NutritionMethod)random.Next(0, 3);

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

        // дублирование
        private static (int, int, int) ChooseTarget(Fruit targetFruit, Plant targetPlant, (int, int) posAn)
        {
            //var target;
            if ((targetFruit.GetPosition().Item1 + targetFruit.GetPosition().Item2) < (targetPlant.GetPosition().Item1 + targetPlant.GetPosition().Item2))
            {
                var x = posAn.Item1 - targetFruit.GetPosition().Item1;
                var y = posAn.Item2 - targetFruit.GetPosition().Item2;
                //target = targetFruit;
                return (x, y, 1);
            }
            var distx = posAn.Item1 - targetPlant.GetPosition().Item1;
            var disty = posAn.Item2 - targetPlant.GetPosition().Item2;
            // target = plant

            //return target;
            return (distx, disty, 2);

        }

        private void MoveToTarget(List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            var posAn = _position;
            var minPl = Constants.ImpVal;
            var minFr = Constants.ImpVal;
            EdiblePlant targetPlant = new((Constants.ImpVal, Constants.ImpVal));
            Fruit targetFruit = new((Constants.ImpVal, Constants.ImpVal));
            (int, int) newPosAn = posAn;



            //массив listOfFood из двух разных классов Fruit + Plant  

            // дублирование foreach

            foreach (Plant plant in listOfAllPlants)
            {
                if (plant is EdiblePlant plant1 && plant.Stage != PlantStage.seed)
                {
                    var posPl = plant.GetPosition();
                    var tmpx = Math.Abs(posAn.Item1 - posPl.Item1);
                    var tmpy = Math.Abs(posAn.Item2 - posPl.Item2);
                    if (tmpx + tmpy < minPl)
                    {
                        minPl = tmpx + tmpx;
                        targetPlant = plant1;
                    }
                }
            }
            //var target = (targetPlant.GetPosition().Item1, targetPlant.GetPosition().Item2, 2);
            //if (listOfFruits.Count != 0)
            //{
                foreach (Fruit fruit in listOfFruits)
                {
                    var posFr = fruit.GetPosition();
                    var tmpx = Math.Abs(posAn.Item1 - posFr.Item1);
                    var tmpy = Math.Abs(posAn.Item2 - posFr.Item2);
                    if (tmpx + tmpy < minFr)
                    {
                        minFr = tmpx + tmpx;
                        targetFruit = fruit;
                    }
                }

                var target = ChooseTarget(targetFruit, targetPlant, posAn);

            //}

            var distx = target.Item1;
            var disty = target.Item2;

            if (distx < 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(posAn, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(posAn, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.up);
                }
                else
                {
                    newPosAn = MoveToDirection(posAn, Direction.right);
                }
            }
            else if (distx > 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(posAn, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(posAn, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.up); ;
                }
                else
                {
                    newPosAn = MoveToDirection(posAn, Direction.left);
                }
            }
            else
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(posAn, Direction.down); ;

                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(posAn, Direction.up); ;

                }
                else
                {
                    if (target.Item3 == 2)
                    {
                        if (targetPlant.IsHealthy())
                        {
                            RiseSatiety();
                        }
                        else
                        {
                            DecreaseHealthByZero();
                        }

                        listOfAllPlants.Remove(targetPlant);
                    }
                    else if (target.Item3 == 1)
                    {
                        if (targetFruit.IsHealthy())
                        {
                            RiseSatiety();
                        }
                        else
                        {
                            DecreaseHealthByZero();
                        }
                        listOfFruits.Remove(targetFruit);

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
            }
            return (x, y);
        }

        private void SetPosition((int, int) pos)
        {
            _position = pos;
        }

        public (int, int) GetPosition()
        {
            return _position;
        }

        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            DecreaseSatiety(listOfAnimals);

            if (_isHungry)
            {
                MoveToTarget(listOfAllPlants, listOfFruits);
            }
            else
            {
                MoveToRandomCell();
            }
        }

    }
}
