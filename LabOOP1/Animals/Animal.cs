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

        protected (int, int) BasisCellPosition;

        private int _timeSinceBreeding = 0;
        private int _age = 0;

        private bool _isHungry = false;
        private bool _isReadyToReproduce = false;
        private bool _isDead = false;
        private bool _isInHibernation = false;

        private int _currentSatiety;
        private int _currentHealth;

        private bool _isAbleToHibernate = false;
        private Gender gender = Gender.female;
        protected NutritionMethod Nutrition = NutritionMethod.omnivorous;

        private Movement movement = new();


        //--------------------------------------------------<class constructor>---------------------------------------------------------------


        public Animal((int, int) pos) : base(pos)
        {
            BasisCellPosition = pos;

            _currentSatiety = MaxSatiety;
            _currentHealth = MaxHealth;

            Random random = new();
            _isAbleToHibernate = random.Next(100) <= 50 ? true : false;
            gender = (Gender)random.Next(0, 2);

            SetNutrition();
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
            _currentHealth += Math.Min(50, MaxHealth - _currentHealth);
        }
        private void RiseSatiety()
        {
            _currentSatiety = MaxSatiety;
            _isHungry = false;
            RiseHealth();
        }
        private void DecreaseHealthByZero()
        {
            _currentHealth = 0;
        }
        private void DecreaseHealth()
        {
            _currentHealth = Math.Max(0, _currentHealth - 5);
        }
        private void DecreaseSatiety()
        {
            int decreaseCoef = MapObjectsControl.s_currentSeason == Season.winter ? 5 : 3;
            _currentSatiety = Math.Max(0, _currentSatiety - decreaseCoef);
            if (_currentSatiety <= 30)
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
                if (target is Fruit fruit)
                    fruit.Die(listOfFruits);
                else if (target is EdiblePlant plant)
                    plant.Die(listOfAllPlants);

                if (f.IsHealthy())
                    RiseSatiety();
                else
                    DecreaseHealthByZero();
            }

            else if (target is Animal animal)
            {
                animal.Die(listOfAnimals);
                RiseSatiety();
            }
            BasisCellPosition = currentPosition;
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

        private bool CheckPartner(Animal animal)
        {
            if (animal.GetType() == GetType() && !animal.Equals(this) &&
                animal.gender != gender && animal._isReadyToReproduce &&
                !animal._isInHibernation && !animal._isHungry)
                return true;
            return false;
        }
        private bool CheckAbleToReoroduce(List<Animal> listOfAnimals)
        {
            foreach (Animal animal in listOfAnimals)
            {
                if (CheckPartner(animal))
                    return true;
            }
            return false;
        }

        //--------------------------------------------------<find and move to partner>---------------------------------------------------------------

        private Animal FindPartner(List<Animal> listOfAnimals)
        {
            var minDist = Constants.ImpVal;
            var partner = this;

            foreach (Animal animal in listOfAnimals)
            {
                if (CheckPartner(animal))
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
            var newPosAn = movement.MoveToTarget1(currentPosition, partner.GetPosition());
            SetPosition(newPosAn);
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

        //--------------------------------------------------<check for hebirnation>---------------------------------------------------------------

        private void CheckSeason()
        {
            _isInHibernation = (MapObjectsControl.s_currentSeason == Season.winter && _isAbleToHibernate);
        }


        //--------------------------------------------------<update the basis cell if the animal becomes hungry while going to the partner>---------------------------------------------------------------
        private void UpdateBasisCell()
        {
            if (_isHungry && _timeSinceBreeding > 5)
                BasisCellPosition = currentPosition;
        }

        //--------------------------------------------------<main part>---------------------------------------------------------------
        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            CheckSeason();
            if (_isInHibernation)
                RiseHealth();

            else
            {
                UpdateReadiness();
                UpdateAge();
                DecreaseSatiety();

                if (_currentHealth == 0 || _age > 100)
                    Die(listOfAnimals);

                else
                {
                    if (_isHungry && CheckAbleToEat(listOfFoodForOmnivorous))
                    {
                        var target = FindFood(listOfFoodForOmnivorous);

                        MoveToFood(target);

                        if (target.GetPosition() == currentPosition)
                            Eat(target, listOfAnimals, listOfPlants, listOfFruits);

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
                        UpdateBasisCell();
                        MoveToRandomCell();
                    }
                }
            }
        }

        //--------------------------------------------------<textbox info>---------------------------------------------------------------

        public override string GetTextInfo()
        {
            if (_isDead) { return "I'm dead... Sorry :("; }
            string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();
            string result = string.Concat("Hey! I am a ", name, " and I'm an ", Nutrition.ToString(),
                ".\r\n", "My health level is ", _currentHealth,
                ".\r\n", "My satiety level is ", _currentSatiety,
                ".\r\n", "My position now is ", currentPosition);
            return result;
        }

    }
}
