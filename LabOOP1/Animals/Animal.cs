using System;
using System.Collections.Generic;

namespace LabOOP1
{

    public class Constants
    {
        public const double ImpVal = 1000000000;
    }

    public abstract class Animal : FoodForOmnivorous
    {
        //--------------------------------------------------<fields>---------------------------------------------------------------

        protected abstract int MaxHealth { get; }
        protected abstract int MaxSatiety { get; }

        private int _timeSinceBreeding = 0;
        private int _age = 0;
        //private (int, int) _position;

        private bool _isHungry = false;
        private bool _isReadyToReproduce = false;

        Movement MoveWay = new();

        private int CurrentSatiety;
        private int CurrentHealth;

        private Gender gender = Gender.female;
        protected NutritionMethod Nutrition = NutritionMethod.omnivorous;

        protected (int, int) BasisCellPosition;

        private Movement movement = new();

        private bool _isDead = false;

        //--------------------------------------------------<class constructor>---------------------------------------------------------------


        public Animal((int, int) pos) : base(pos)
        {
            BasisCellPosition = pos;

            CurrentSatiety = MaxSatiety;
            CurrentHealth = MaxHealth;

            Random random = new();
            gender = (Gender)random.Next(0, 2);

            //_position = pos; 

            SetNutrition();
        }


        //--------------------------------------------------<abstract methods>---------------------------------------------------------------

        protected abstract void MoveToRandomCell();
        protected abstract void MoveToFood(FoodForOmnivorous target);
        protected abstract void Reproduce(List<Animal> listOfAnimals);
        protected abstract bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous);
        protected abstract void SetNutrition();
        //protected abstract void GoToEat(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous);


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
        private void DecreaseHealth()
        {
            CurrentHealth = Math.Max(0, CurrentHealth - 5);
        }
        private void DecreaseSatiety()
        {
            CurrentSatiety = Math.Max(0, CurrentSatiety - 5);
            if (CurrentSatiety <= 30)
            {
                _isHungry = true;
                DecreaseHealth();
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


        protected FoodForOmnivorous FindFood(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            var minDist = Constants.ImpVal;
            FoodForOmnivorous target = this;
            foreach (FoodForOmnivorous f in listOfFoodForOmnivorous)
            {
                if (CheckForHerbivorous(f) ||
                    CheckForCarnivorous(f) ||
                    CheckForOmniivorous(f))
                {
                    double dist = movement.CountDistL1(currentPosition, f.GetPosition());
                    if (Nutrition == NutritionMethod.carnivorous)
                    {
                        dist = movement.CountDistEuclid(currentPosition, f.GetPosition());
                    }

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

        protected void Eat(FoodForOmnivorous target, List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is FoodForHerbivorous f)
            {
                if (f.IsHealthy())
                    RiseSatiety();
                else
                    DecreaseHealthByZero();

                if (target is Fruit fruit)
                    fruit.Die(listOfFruits);
                else if (target is EdiblePlant plant)
                    plant.Die(listOfAllPlants);
            }
            if (target is Animal animal)
            {
                RiseSatiety();
                animal.Die(listOfAnimals);
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
                    && animal.Nutrition == Nutrition && animal._isReadyToReproduce && animal._isHungry == false)
                {
                    var dist = movement.CountDistL1(currentPosition, animal.GetPosition());
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
            var newPosAn = MoveWay.MoveToTarget1(currentPosition, partner.GetPosition());
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
        private void UpdateReproduceCharacters(Animal partner)
        {
            _timeSinceBreeding = 0;
            _isReadyToReproduce = false;
            BasisCellPosition = currentPosition;
            partner.BasisCellPosition = partner.GetPosition();
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
            currentPosition = pos;
        }

        //--------------------------------------------------<age>---------------------------------------------------------------

        protected void UpdateAge()
        {
            _age++;
        }

        //--------------------------------------------------<die>---------------------------------------------------------------

        protected void Die(List<Animal> listOfAnimals)
        {
            listOfAnimals.Remove(this);
            _isDead = true;
        }

        //--------------------------------------------------<get position again>---------------------------------------------------------------

        //internal override (int, int) GetPosition()
        //{
        //    return _position;
        //}

        //--------------------------------------------------<main part>---------------------------------------------------------------
        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            UpdateReadiness();
            UpdateAge();
            DecreaseSatiety();

            if (CurrentHealth == 0 || _age > 100)
            {
                Die(listOfAnimals);
            }
            else
            {
                if (_isHungry && CheckAbleToEat(listOfFoodForOmnivorous))
                {
                    var target = FindFood(listOfFoodForOmnivorous);

                    MoveToFood(target);

                    if (target.GetPosition() == currentPosition)
                    {
                        BasisCellPosition = currentPosition;
                        Eat(target, listOfAnimals, listOfPlants, listOfFruits);
                    }

                }
                else if (_isReadyToReproduce && CheckAbleToReoroduce(listOfAnimals))
                {
                    var partner = FindPartner(listOfAnimals);

                    MoveToPartner(partner);

                    if (partner.GetPosition() == currentPosition && gender == Gender.female)
                    {
                        Reproduce(listOfAnimals);
                        UpdateReproduceCharacters(partner);
                    }
                }
                else
                {
                    //костыль
                    if (_isHungry && _timeSinceBreeding > 5)
                        BasisCellPosition = currentPosition;

                    MoveToRandomCell();
                    
                }
            }
        }

        public override string GetTextInfo()
        {
            if (_isDead) { return "I'm dead... Sorry :("; }
            string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();
            string result = string.Concat("Hey! I am a ", name, " and I'm an ", Nutrition.ToString(),
                ".\r\n", "My health level is ", CurrentHealth,
                ".\r\n", "My satiety level is ", CurrentSatiety,
                ".\r\n", "My position now is ", currentPosition);
            return result;
        }

    }
}
