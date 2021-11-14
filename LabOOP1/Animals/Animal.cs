using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class Animal : FoodForOmnivorous
    {
        //--------------------------------------------------< fields >---------------------------------------------------------------

        private bool _isInHibernation = false;
        private bool _isAbleToHibernate = false;
        private bool _wasEaten = false;
        private bool _wasCalled = false;
        private bool _isDead = false;
        private bool _isDomesticated = false;
        protected PurposeOfMovement myGoal = PurposeOfMovement.goingToRandomCell;

        protected Movement movement = new();
        protected int _timeSinceBreeding = 0;
        protected int _timeSinceDeath = 0;
        protected int _currentSatiety;
        protected int _currentHealth;
        protected int _age = 0;
        protected bool _isHungry = false;
        protected bool _isReadyToReproduce = false;
        protected NutritionMethod Nutrition = NutritionMethod.omnivorous;

        protected (int, int) BasisCellPosition;

        public Gender gender = Gender.female;

        protected abstract int MaxHealth { get; }
        protected abstract int MaxSatiety { get; }
        public bool WasEaten { get => _wasEaten; set { _wasEaten = value; } }
        public bool WasCalled { get => _wasCalled; set { _wasCalled = value; } }
        public bool IsDead { get => _isDead; set { _isDead = value; } }
        public bool IsDomesticated { get => _isDomesticated; set { _isDomesticated = value; } }

        public Human Owner;

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

        protected void RiseHealth(int coef = 50)
        {
            _currentHealth += Math.Min(coef, MaxHealth - _currentHealth);
        }
        public void RiseSatiety()
        {
            _currentSatiety = MaxSatiety;
            _isHungry = false;
            RiseHealth();
        }
        protected void DecreaseHealthByZero()
        {
            _currentHealth = 0;
        }
        protected void DecreaseHealth()
        {
            _currentHealth = Math.Max(0, _currentHealth - 5);
        }
        protected void DecreaseSatiety()
        {
            int decreaseCoef = MapObjectsControl.s_currentSeason == Season.winter ? 5 : 3;
            _currentSatiety = Math.Max(0, _currentSatiety - decreaseCoef);
            if (_currentSatiety <= 50)
            {
                _isHungry = true;
                DecreaseHealth();
            }
        }


        //--------------------------------------------------< check for hebirnation >---------------------------------------------------------------

        private void CheckHibernationSeason()
        {
            _isInHibernation = (MapObjectsControl.s_currentSeason == Season.winter && _isAbleToHibernate);
        }


        //-------------< update the basis cell if the animal became hungry or its target dead while it going to the previous goal >-----------------
        private void CheckForUpdatingCellAndGoal()
        {
            if (myGoal == PurposeOfMovement.goingToFood || myGoal == PurposeOfMovement.goingToPartner)
            {
                BasisCellPosition = currentPosition;
                myGoal = PurposeOfMovement.goingToRandomCell;
            }
        }

        //------------------------------------------------< find a food >------------------------------------------------------------



        protected FoodForOmnivorous FindTarget(List<FoodForOmnivorous> listOfFoodForOmnivorous, Func<FoodForOmnivorous, bool> Check)
        {
            var minDist = Constants.ImpVal;
            FoodForOmnivorous target = null;

            foreach (FoodForOmnivorous f in listOfFoodForOmnivorous)
            {
                if (Check(f))
                {
                    double dist = movement.CountDistL1(currentPosition, f.GetPosition());

                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = f;
                    }
                }
            }

            return target;
        }

        ////-----------------------------------------------< find a partner >----------------------------------------------------------

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
                animal.DieImmediately(listOfAnimals);
                RiseSatiety();
            }

            BasisCellPosition = currentPosition;
        }

        //---------------------------------------------< reproduce characters >-------------------------------------------------------

        private bool CheckAbleToReproduce(List<Animal> listOfAnimals)
        {
            foreach (Animal animal in listOfAnimals)
            {
                if (CheckPartner(animal))
                    return true;
            }
            return false;
        }
        protected void UpdateReproduceCharacters(Animal partner)
        {
            _timeSinceBreeding = 0;
            _isReadyToReproduce = false;
            BasisCellPosition = currentPosition;
            partner.BasisCellPosition = partner.GetPosition();
            partner._timeSinceBreeding = 0;
            partner._isReadyToReproduce = false;
        }
        protected virtual bool CheckPartner(FoodForOmnivorous an)
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
        protected virtual void UpdateReadiness(int ageForReproduce)
        {
            _timeSinceBreeding++;
            if (_timeSinceBreeding >= ageForReproduce && !(_isHungry))
            {
                _isReadyToReproduce = true;
            }
        }

        //--------------------------------------------------< age >---------------------------------------------------------------

        protected void UpdateAge()
        {
            _age++;
        }

        //--------------------------------------------------< die >---------------------------------------------------------------
        protected bool CheckTimeToDie()
        {
            return (_currentHealth == 0 || _age == 100);
        }

        public void DieImmediately(List<Animal> listOfAnimals)
        {
            listOfAnimals.Remove(this);
            //_isDead = true;
        }


        //--------------------------------------------------< moving >---------------------------------------------------------------

        protected void MoveToRandomCell()
        {
            var newPosAn = MoveToRandomCellOver();
            SetPosition(newPosAn);
        }

        protected void MoveToTarget(FoodForOmnivorous target)
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
            myGoal = PurposeOfMovement.goingToPartner;

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
            myGoal = PurposeOfMovement.goingToFood;

            var target = FindTarget(listOfFoodForOmnivorous, CheckForEating);
            if (target != null)
            {
                MoveToTarget(target);

                if (target.GetPosition() == currentPosition)
                    Eat(target, listOfAnimals, listOfPlants, listOfFruits);
            }
        }




        //--------------------------------------------------< main part >---------------------------------------------------------------
        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            CheckHibernationSeason();
            if (_isInHibernation)
                RiseHealth();

            //else if (_isDomesticated)
            //{
            //    if (movement.CountDistL1(currentPosition, Owner.GetPosition()) > 2)
            //    {
            //        MoveToTarget(Owner);
            //    }
            //    else
            //    {
            //        BasisCellPosition = Owner.GetPosition();
            //    }
            //}


            else
            {

                if (_wasEaten)
                {
                    DieImmediately(listOfAnimals);
                }
                else
                {
                    if (CheckTimeToDie())
                        _isDead = true;

                    if (_isDead)
                    {
                        _timeSinceDeath++;
                        if (this is Pig)
                        {
                            DieImmediately(listOfAnimals);
                        }
                        CheckTimeForRemoveFromList(listOfAnimals);
                    }

                    else
                    {

                        GeneralVoidsForLiveCicle(10);
                        if (_wasCalled)
                        {
                            MoveToTarget(Owner);
                        }
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
            }
        }

        private void CheckTimeForRemoveFromList(List<Animal> listOfAnimals)
        {
            if (_timeSinceDeath > 15)
            {
                DieImmediately(listOfAnimals);
            }
        }

        protected void GeneralVoidsForLiveCicle(int time)
        {
            UpdateReadiness(time);
            UpdateAge();
            DecreaseSatiety();
        }

        //--------------------------------------------------< textbox info >---------------------------------------------------------------

        protected override string GetInfo()
        {
            if (_isDead) { return "I'm dead... Sorry :("; }
            string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();
            string result = string.Concat("Hey! I am a ", name, " and I'm an ", Nutrition.ToString(),
                ".\r\n", "My health level is ", _currentHealth,
                ".\r\n", "My satiety level is ", _currentSatiety,
                ".\r\n", "My position now is ", currentPosition,
                "\r\n", "My age is ", _age,
                ".\r\n", "And now I am ", myGoal);

            return result;
        }

    }
}
