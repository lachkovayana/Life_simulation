using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Constants
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

        private void MoveToRandomCell()
        {
            var newPosition = MoveWay.MoveToRandomCell(_position);
            SetPosition(newPosition);
        }
        public override (int, int) GetPosition()
        {
            return _position;
        }

        private void MoveToFood(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits, List<FoodForOmnivores> _listOfFoodForOmnivores)
        {
            var target = FindFood(_listOfFoodForOmnivores);

            var newPosAn = MoveWay.MoveToTarget(_position, target);
            SetPosition(newPosAn);

            if (target.GetPosition() == _position)
                Eat(target, listOfAnimals, listOfAllPlants, listOfFruits);
        }

        private FoodForOmnivores FindFood(List<FoodForOmnivores> listOfFoodForOmnivores)
        {
            var minDist = Constants.ImpVal;

            FoodForOmnivores target = this;

            foreach (FoodForOmnivores f in listOfFoodForOmnivores)
            {
                if ((Nutrition == NutritionMethod.herbivorous && (f is Fruit || f is EdiblePlant)) ||
                    (Nutrition == NutritionMethod.carnivorous && f is Animal animal && !f.Equals(this) && animal.Nutrition != Nutrition) ||
                    (Nutrition == NutritionMethod.omnivorous &&  ((f is Fruit || f is EdiblePlant) || (f is Animal animal1 && !f.Equals(this) && animal1.Nutrition != Nutrition))))
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
            return target;
        }
       
        private void Eat(FoodForOmnivores target, List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
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
                    !(animal.Equals(this)) && animal.Nutrition == Nutrition)
                {
                    var posPartner = animal.GetPosition();
                    var tmpx = Math.Abs(_position.Item1 - posPartner.Item1);
                    var tmpy = Math.Abs(_position.Item2 - posPartner.Item2);
                    if (tmpx + tmpy < minDist)
                    {
                        minDist = tmpx + tmpx;
                        partner = animal;
                    }
                }
            }

            return partner;
        }
        private void MoveToPartner(List<Animal> listOfAnimals, Animal partner)
        {
            var newPosAn = MoveWay.MoveToTarget(_position, partner);
            SetPosition(newPosAn);
        }
        private void Reproduce(List<Animal> listOfAnimals)
        {
            Animal newAnimal = new(_position);
            newAnimal.SetNutrition(Nutrition);
            //newAnimal.SetKind(Kind);
            listOfAnimals.Add(newAnimal);
        }

        private void SetNutrition(NutritionMethod nutrition)
        {
            this.Nutrition = nutrition;
        }

        private void UpdateTime()
        {
            _timeSinceBreeding++;
        }

        private void SetPosition((int, int) pos)
        {
            _position = pos;
        }


        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivores> listOfFoodForOmnivores)
        {
            UpdateTime();
            DecreaseSatiety(listOfAnimals);

            if (_isHungry)
            {
                MoveToFood(listOfAnimals, listOfPlants, listOfFruits, listOfFoodForOmnivores);
            }
            else
            {
                if (_timeSinceBreeding >= 15)
                {
                    var partner = FindPartner(listOfAnimals);
                    if (!partner.Equals(this))
                    {
                        MoveToPartner(listOfAnimals, partner);

                        if (partner.GetPosition() == _position && gender == Gender.female)
                        {
                            Reproduce(listOfAnimals);
                        }
                    }
                }
                else
                    MoveToRandomCell();
            }
        }

       
    }
}
