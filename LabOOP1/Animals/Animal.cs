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
        //--------------------------------------------------< fields >---------------------------------------------------------------

        protected abstract int MaxHealth { get; }
        protected abstract int MaxSatiety { get; }

        protected (int, int) BasisCellPosition;

        private GoalOfTheLastStep myGoal;

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

        protected Movement movement = new();


        //--------------------------------------------------< class constructor >---------------------------------------------------------------


        public Animal((int, int) pos) : base(pos)
        {
            //позиция рождения
            BasisCellPosition = pos;

            _currentSatiety = MaxSatiety;
            _currentHealth = MaxHealth;

            Random random = new();
            _isAbleToHibernate = random.Next(100) <= 50 ? true : false;
            gender = (Gender)random.Next(0, 2);

            SetNutrition();
        }


        //--------------------------------------------------< abstract methods >---------------------------------------------------------------

        protected abstract (int, int) MoveToRandomCellOver();
        protected abstract (int, int) MoveToTargetOver(FoodForOmnivorous target);
        protected abstract void Reproduce(List<Animal> listOfAnimals);
        protected abstract bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous);
        protected abstract bool CheckForEating(FoodForOmnivorous food);
        protected abstract void SetNutrition();


        //------------------------------------------< methods for update health and satiety >-----------------------------------------------------

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


        //--------------------------------------------------< check for hebirnation >---------------------------------------------------------------

        private void CheckSeason()
        {
            _isInHibernation = (MapObjectsControl.s_currentSeason == Season.winter && _isAbleToHibernate);
        }


        //-------------< update the basis cell if the animal became hungry or its target dead while it going to the previous goal >-----------------
        private void CheckForUpdatingCellAndGoal()
        {
            if (myGoal == GoalOfTheLastStep.goingToFood || myGoal == GoalOfTheLastStep.goingToPartner)
            {
                BasisCellPosition = currentPosition;
                myGoal = GoalOfTheLastStep.goingToRandomCell;
            }
        }

        //--------------------------------------------------< find a food >---------------------------------------------------------------



        protected FoodForOmnivorous FindTarget(List<FoodForOmnivorous> listOfFoodForOmnivorous, Func<FoodForOmnivorous, bool> Check)
        {

            var minDist = Constants.ImpVal;
            FoodForOmnivorous target = this;

            foreach (FoodForOmnivorous f in listOfFoodForOmnivorous)
            {
                if (Check(f))
                {
                    double dist = movement.CountDistL1(currentPosition, f.GetPosition());

                    //костыль if this.moveway = enum.Euclid4Cells
                    //if (Nutrition == NutritionMethod.carnivorous)
                    //{
                    //    dist = movement.CountDistEuclid(currentPosition, f.GetPosition());
                    //}

                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = f;
                    }
                }
            }
            return target;
        }

        //--------------------------------------------------< find a partner >---------------------------------------------------------------

        //private Animal FindPartner(List<Animal> listOfAnimals)
        //{
        //    var minDist = Constants.ImpVal;
        //    var partner = this;

        //    foreach (Animal animal in listOfAnimals)
        //    {
        //        if (CheckPartner(animal))
        //        {
        //            var dist = movement.CountDistL1(currentPosition, animal.GetPosition());
        //            if (dist < minDist)
        //            {
        //                minDist = dist;
        //                partner = animal;
        //            }
        //        }
        //    }

        //    return partner;
        //}



        //--------------------------------------------------< eating >---------------------------------------------------------------

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

        //--------------------------------------------------< reproduce characters >---------------------------------------------------------------

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

        private bool CheckPartner(FoodForOmnivorous an)
        {
            if (an is Animal animal)
            {
                if (animal.GetType() == GetType() && !animal.Equals(this) &&
                    animal.gender != gender && animal._isReadyToReproduce &&
                    !animal._isInHibernation && !animal._isHungry)
                    return true;
            }
            return false;
        }
        private bool CheckAbleToReproduce(List<Animal> listOfAnimals)
        {
            foreach (Animal animal in listOfAnimals)
            {
                if (CheckPartner(animal))
                    return true;
            }
            return false;
        }



        //--------------------------------------------------< age >---------------------------------------------------------------

        protected void UpdateAge()
        {
            _age++;
        }

        //--------------------------------------------------< die >---------------------------------------------------------------

        protected void Die(List<Animal> listOfAnimals)
        {
            listOfAnimals.Remove(this);
            _isDead = true;
        }


        //--------------------------------------------------< moving >---------------------------------------------------------------

        private void MoveToRandomCell()
        {
            var newPosAn = MoveToRandomCellOver();
            SetPosition(newPosAn);
        }

        private void MoveToTarget(FoodForOmnivorous target)
        {
            var newPosAn = MoveToTargetOver(target);
            SetPosition(newPosAn);
        }

        //--------------------------------------------------< set new position >---------------------------------------------------------------

        protected void SetPosition((int, int) pos)
        {
            currentPosition = pos;
        }

        //--------------------------------------------------< processes >---------------------------------------------------------------

        private void ReproducingProcess(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Animal> listOfAnimals)
        {
            myGoal = GoalOfTheLastStep.goingToPartner;

            Animal partner = (Animal)FindTarget(listOfFoodForOmnivorous, CheckPartner);

            MoveToTarget(partner);

            if (partner.GetPosition() == currentPosition && gender == Gender.female)
            {
                Reproduce(listOfAnimals);
                UpdateReproduceCharacters(partner);
            }
        }

        private void EatingProcess(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            myGoal = GoalOfTheLastStep.goingToFood;

            var target = FindTarget(listOfFoodForOmnivorous, CheckForEating);

            MoveToTarget(target);

            if (target.GetPosition() == currentPosition)
                Eat(target, listOfAnimals, listOfPlants, listOfFruits);
        }




        //--------------------------------------------------< main part >---------------------------------------------------------------
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
                        EatingProcess(listOfAnimals, listOfPlants, listOfFruits, listOfFoodForOmnivorous);

                    }
                    else if (_isReadyToReproduce && CheckAbleToReproduce(listOfAnimals))
                    {
                        ReproducingProcess(listOfFoodForOmnivorous, listOfAnimals);
                    }
                    else
                    {
                        CheckForUpdatingCellAndGoal();
                        MoveToRandomCell();
                    }
                }
            }
        }


        //--------------------------------------------------< textbox info >---------------------------------------------------------------

        protected override string GetInfo()
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
