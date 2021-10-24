using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Constants
    {
        public const int ImpVal = 1000000000;
    }

    public abstract class Animal : FoodForOmnivorous
    {
        internal (int, int) _position;
        private int _health = 100;
        private int _satiety = 100;
        private int _timeSinceBreeding = 0;

        private bool _isHungry = false;
        private bool _isReadyToReproduce = false;

        Movement MoveWay = new();

        private Gender gender = Gender.female;
        public NutritionMethod Nutrition = NutritionMethod.herbivorous;

        public Animal((int, int) pos) : base(pos)
        {
            _position = pos;
            Random random = new();
            Nutrition = (NutritionMethod)random.Next(0, 3);
            gender = (Gender)random.Next(0, 2);
        }
        internal override (int, int) GetPosition()
        {
            return _position;
        }

        private void RiseHealth()
        {
            _health += Math.Min(50, 100 - _health);
        }
        private void RiseSatiety()
        {
            _satiety = 100;
            _isHungry = false;
            RiseHealth();
        }
        private void DecreaseHealth(List<Animal> listOfAnimals)
        {
            _health = Math.Max(0, _health - 5);
            if (_health <= 0)
                listOfAnimals.Remove(this);
        }
        private void DecreaseHealthByZero()
        {
            _health = 0;
        }
        private void DecreaseSatiety(List<Animal> listOfAnimals)
        {
            _satiety = Math.Max(0, _satiety - 5);
            if (_satiety <= 30)
            {
                _isHungry = true;
                DecreaseHealth(listOfAnimals);
            }
        }

        internal abstract void MoveToRandomCell();
        internal abstract void MoveToFood(FoodForOmnivorous target);
        internal abstract void Reproduce(List<Animal> listOfAnimals);


        private FoodForOmnivorous FindFood(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            var minDist = Constants.ImpVal;

            FoodForOmnivorous target = this;

            foreach (FoodForOmnivorous f in listOfFoodForOmnivorous)
            {
                if ((Nutrition == NutritionMethod.herbivorous && (f is Fruit || f is EdiblePlant)) ||
                    (Nutrition == NutritionMethod.carnivorous && f is Animal animal && !f.Equals(this) && animal.Nutrition != Nutrition) ||
                    (Nutrition == NutritionMethod.omnivorous && ((f is Fruit || f is EdiblePlant) || (f is Animal animal1 && !f.Equals(this) && animal1.Nutrition != Nutrition))))
                {
                    var dist = CountDist(f);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = f;
                    }
                }
            }
            return target;
        }

        private int CountDist(FoodForOmnivorous f)
        {
            var posFood = f.GetPosition();
            var tmpx = Math.Abs(_position.Item1 - posFood.Item1);
            var tmpy = Math.Abs(_position.Item2 - posFood.Item2);
            return tmpx + tmpy;
        }

        private void Eat(FoodForOmnivorous target, List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is FoodForHerbivorous f)
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
            if (target is Animal)
            {
                RiseSatiety();
            }

            if (target is Fruit fruit)
            {
                listOfFruits.Remove(fruit);
            }
            else if (target is EdiblePlant plant)
            {
                listOfAllPlants.Remove(plant);
            }
            else if (target is Animal animal)
            {
                listOfAnimals.Remove(animal);
            }
        }

        private Animal FindPartner(List<Animal> listOfAnimals)
        {
            var minDist = Constants.ImpVal;
            var partner = this;

            foreach (Animal animal in listOfAnimals)
            {
                if (!(animal._isHungry) && animal.gender != gender &&
                    !(animal.Equals(this)) && animal.Nutrition == Nutrition && animal._isReadyToReproduce)
                {
                    var dist = CountDist(animal);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        partner = animal;
                    }
                }
            }

            return partner;
        }
        private void MoveToPartner(List<Animal> listOfAnimals, Animal partner)
        {
            var newPosAn = MoveWay.MoveToTarget1(_position, partner);
            SetPosition(newPosAn);
        }



        private void UpdateReadiness()
        {
            _timeSinceBreeding++;
            if (_timeSinceBreeding >= 5)
            {
                _isReadyToReproduce = true;
            }
        }
        private void UpdateTime(Animal partner)
        {
            _timeSinceBreeding = 0;
            _isReadyToReproduce = false;
            partner._timeSinceBreeding = 0;
            partner._isReadyToReproduce = false;
        }

        internal void SetPosition((int, int) pos)
        {
            _position = pos;
        }

        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            UpdateReadiness();
            DecreaseSatiety(listOfAnimals);

            if (_isHungry)
            {
                var target = FindFood(listOfFoodForOmnivorous);

                if (!target.Equals(this))
                {
                    MoveToFood(target);
                    if (target.GetPosition() == _position)
                        Eat(target, listOfAnimals, listOfPlants, listOfFruits);
                }
            }
            else if (_isReadyToReproduce)
            {
                var partner = FindPartner(listOfAnimals);

                if (!partner.Equals(this))
                {
                    MoveToPartner(listOfAnimals, partner);

                    if (partner.GetPosition() == _position && gender == Gender.female)
                    {
                        Reproduce(listOfAnimals);
                        UpdateTime(partner);
                    }
                }
            }
            else
                MoveToRandomCell();
        }
    }


}
