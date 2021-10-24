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
        //--------------------------------------------------<fields>---------------------------------------------------------------

        protected abstract int MaxHealth { get;}
        protected abstract int MaxSatiety { get; }
        //protected virtual int CurrentHealth { get; set; }
        //protected virtual int CurrentSatiety { get; set; }

        private int _timeSinceBreeding = 0;

        private bool _isHungry = false;
        private bool _isReadyToReproduce = false;

        Movement MoveWay = new();

        private int CurrentSatiety;
        private int CurrentHealth;

        private Gender gender = Gender.female;
        public NutritionMethod Nutrition = NutritionMethod.omnivorous;

        //--------------------------------------------------<class constructot>---------------------------------------------------------------


        public Animal((int, int) pos) : base(pos)
        {
            CurrentSatiety = MaxSatiety;
            CurrentHealth = MaxHealth;
            
            Random random = new();
            gender = (Gender)random.Next(0, 2);

            
        }


        //--------------------------------------------------<abstract methods>---------------------------------------------------------------

        protected abstract void MoveToRandomCell();
        protected abstract void MoveToFood(FoodForOmnivorous target);
        protected abstract void Reproduce(List<Animal> listOfAnimals);
        protected abstract bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous);
        protected abstract void SetNutrition();


        //--------------------------------------------------<methods for update health and satiety>---------------------------------------------------------------

        private void RiseHealth()
        {
            CurrentHealth += Math.Min(50, MaxHealth - CurrentHealth);
        }
        private void RiseSatiety()
        {
            CurrentSatiety = MaxSatiety;
            _isHungry = false;
            RiseHealth();
        }
        private void DecreaseHealthByZero()
        {
            CurrentHealth = 0;
        }
        private void DecreaseHealth(List<Animal> listOfAnimals)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - 5);
            if (CurrentHealth == 0)
                listOfAnimals.Remove(this);
        }
        private void DecreaseSatiety(List<Animal> listOfAnimals)
        {
            CurrentSatiety = Math.Max(0, CurrentSatiety - 5);
            if (CurrentSatiety <= 30)
            {
                _isHungry = true;
                DecreaseHealth(listOfAnimals);
            }
        }


        //--------------------------------------------------<food search>---------------------------------------------------------------


        private bool CheckForHerbivorous(FoodForOmnivorous f)
        {
            if (Nutrition == NutritionMethod.herbivorous && f is FoodForHerbivorous)
                return true;
            return false;
        }
        private bool CheckForCarnivorous(FoodForOmnivorous f)
        {
            if (Nutrition == NutritionMethod.carnivorous && f is Animal animal && !f.Equals(this) && animal.Nutrition != Nutrition)
                return true;
            return false;
        }
        private bool CheckForOmniivorous(FoodForOmnivorous f)
        {
            if (Nutrition == NutritionMethod.omnivorous && ((f is FoodForHerbivorous) || (f is Animal animal1 && !f.Equals(this) && animal1.Nutrition != Nutrition)))
                return true;
            return false;
        }

        private int CountDist(FoodForOmnivorous f)
        {
            var posFood = f.GetPosition();
            var tmpx = Math.Abs(position.Item1 - posFood.Item1);
            var tmpy = Math.Abs(position.Item2 - posFood.Item2);
            return tmpx + tmpy;
        }

        private FoodForOmnivorous FindFood(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            var minDist = Constants.ImpVal;
            FoodForOmnivorous target = this;

            foreach (FoodForOmnivorous f in listOfFoodForOmnivorous)
            {
                if (CheckForHerbivorous(f) ||
                    CheckForCarnivorous(f) ||
                    CheckForOmniivorous(f))
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

        //--------------------------------------------------<eating>---------------------------------------------------------------

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

        //--------------------------------------------------<find and move to partner>---------------------------------------------------------------

        private Animal FindPartner(List<Animal> listOfAnimals)
        {
            var minDist = Constants.ImpVal;
            var partner = this;

            foreach (Animal animal in listOfAnimals)
            {
                if (animal.gender != gender && !(animal.Equals(this))
                    && animal.Nutrition == Nutrition && animal._isReadyToReproduce)
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
        private void MoveToPartner(Animal partner)
        {
            var newPosAn = MoveWay.MoveToTarget1(position, partner);
            SetPosition(newPosAn);
        }

        //--------------------------------------------------<reproduce characters>---------------------------------------------------------------

        private void UpdateReadiness()
        {
            _timeSinceBreeding++;
            if (_timeSinceBreeding >= 5 && !(_isHungry))
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

        private bool CheckAbleToReoroduce(List<Animal> listOfAnimals)
        {
            foreach (Animal animal in listOfAnimals)
            {
                if (animal.GetType() == GetType() && !(animal.Equals(this)) &&
                    animal.gender != gender && animal.Nutrition == Nutrition &&
                    animal._isReadyToReproduce)

                    return true;
            }
            return false;
        }

        //--------------------------------------------------<set new position>---------------------------------------------------------------

        protected void SetPosition((int, int) pos)
        {
            position = pos;
        }



        //--------------------------------------------------<main part>---------------------------------------------------------------
        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            UpdateReadiness();
            DecreaseSatiety(listOfAnimals);

            if (_isHungry && CheckAbleToEat(listOfFoodForOmnivorous))
            {
                var target = FindFood(listOfFoodForOmnivorous);

                MoveToFood(target);

                if (target.GetPosition() == position)
                    Eat(target, listOfAnimals, listOfPlants, listOfFruits);
            }
            else if (_isReadyToReproduce && CheckAbleToReoroduce(listOfAnimals))
            {
                var partner = FindPartner(listOfAnimals);

                MoveToPartner(partner);

                if (partner.GetPosition() == position && gender == Gender.female)
                {
                    Reproduce(listOfAnimals);
                    UpdateTime(partner);
                }

            }
            else
                MoveToRandomCell();
        }


    }


}
