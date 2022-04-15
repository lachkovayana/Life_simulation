using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class Animal : FoodForOmnivorous
    {
        //--------------------------------------------------< fields >---------------------------------------------------------------

        private bool _isInHibernation = false;
        private bool _wasEaten = false;
        private bool _wasKilled = false;
        private bool _isDead = false;
        private bool _isDomesticated = false;
        private Human _owner = null;

        protected bool _noTarget = false;
        protected int _timeSinceBreeding = 0;
        protected int _timeSinceDeath = 0;
        protected int _currentSatiety;
        protected int _currentHealth;
        protected int _age = 0;
        protected bool _isHungry = false;
        protected bool _isReadyToReproduce = false;
        protected NutritionMethod Nutrition = NutritionMethod.omnivorous;
        protected PurposeOfMovement myGoal = PurposeOfMovement.goToRandomCell;
        protected Movement movement = new();

        public Gender gender = Gender.female;
        public (int, int) BasisCellPosition;

        protected abstract int MaxHealth { get; }
        protected abstract int MaxSatiety { get; }
        protected virtual bool IsAbleToHibernate { get { return false; } }
        public bool WasEaten { get => _wasEaten; set { _wasEaten = value; _isDead = value; } }
        public bool WasKilled { get => _wasKilled; set { _wasKilled = value; _isDead = value; } }
        public bool IsDead { get => _isDead; set { _isDead = value; } }
        public bool IsDomesticated { get => _isDomesticated; set { _isDomesticated = value; } }

        public Human Owner { get => _owner; set { _owner = value; } }

        //--------------------------------------------------< class constructor >---------------------------------------------------------------

        Random random = new();

        public Animal((int, int) pos) : base(pos)
        {
            //позиция рождения
            BasisCellPosition = pos;

            _currentSatiety = MaxSatiety;
            _currentHealth = MaxHealth;

            //_isAbleToHibernate = random.Next(100) <= 50;
            gender = (Gender)random.Next(0, 2);

            SetNutrition();
        }


        //--------------------------------------------------< abstract methods >---------------------------------------------------------------

        protected abstract (int, int) MoveToRandomCellOver();
        protected abstract (int, int) MoveToTargetOver((int, int) position);
        protected abstract void Reproduce(List<Animal> listOfAnimals);
        protected abstract bool CheckOwnerStocks();

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
            _isInHibernation = (MapObjectsControl.s_currentSeason == Season.winter && IsAbleToHibernate);
        }


        //-------------< update the basis cell if the animal became hungry or its target dead while it going to the previous goal >-----------------
        private void CheckForUpdatingCellAndGoal()
        {
            if (myGoal == PurposeOfMovement.goToFood || myGoal == PurposeOfMovement.goToPartner)
            {
                BasisCellPosition = currentPosition;
            }
        }

        //------------------------------------------------< find a food >------------------------------------------------------------

        protected T FindTarget<T>(List<T> listOfFoodForOmnivorous, Func<T, bool> Check) where T : FoodForOmnivorous
        {
            var minDist = (double)Constants.ImpVal;
            T target = null;

            foreach (T f in listOfFoodForOmnivorous)
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

        //--------------------------------------------------< eating >---------------------------------------------------------------

        protected void Eat(FoodForOmnivorous target, List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is FoodForHerbivorous f)
            {
                if (target is Fruit fruit)
                    fruit.Die(listOfFruits);
                else if (target is EdiblePlant plant)
                    plant.Die(listOfAllPlants);

                if (f.IsHealthy)
                    RiseSatiety();
                else
                    DecreaseHealthByZero();
            }

            else if (target is Animal animal)
            {
                //animal.DieImmediately(listOfAnimals);
                animal.WasEaten = true;
                RiseSatiety();
            }

            if (!_isDomesticated)
                BasisCellPosition = currentPosition;
        }
        protected bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous, Func<FoodForOmnivorous, bool> Check)
        {

            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (Check(food))
                    return true;
            }
            return false;
        }

        internal void Feed()
        {
            RiseSatiety();
        }

        //---------------------------------------------< reproduce characters >-------------------------------------------------------

        protected bool CheckAbleToReproduce(List<Animal> listOfAnimals)
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
            partner._timeSinceBreeding = 0;
            partner._isReadyToReproduce = false;


            if (!_isDomesticated)
                BasisCellPosition = currentPosition;
            if (!partner._isDomesticated)
                partner.BasisCellPosition = partner.GetPosition();

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
        protected virtual void UpdateReadiness(int periodOfReproducing, bool additionalCheck = true)
        {
            _timeSinceBreeding++;
            if (_timeSinceBreeding >= periodOfReproducing && !(_isHungry) && additionalCheck)
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
            return (_currentHealth == 0 || _age == 200);
        }

        public void DieImmediately(List<Animal> listOfAnimals)
        {
            listOfAnimals.Remove(this);
            if (_isDomesticated)
                Owner.ReportDeath(this);
        }


        //--------------------------------------------------< moving >---------------------------------------------------------------

        protected void MoveToRandomCell()
        {
            var newPosAn = MoveToRandomCellOver();
            SetPosition(newPosAn);
        }

        protected void MoveToTarget((int, int) position)
        {
            var newPosAn = MoveToTargetOver(position);
            SetPosition(newPosAn);
        }

        //--------------------------------------------------< set new position >---------------------------------------------------------------

        protected void SetPosition((int, int) pos)
        {
            currentPosition = pos;
        }

        //--------------------------------------------------< processes >---------------------------------------------------------------

        private void ReproducingProcess(List<Animal> listOfAnimals)
        {
            Animal partner = FindTarget(listOfAnimals, CheckPartner);

            MoveToTarget(partner.GetPosition());
            if (_isDomesticated)
            {
                myGoal = PurposeOfMovement.moveAwayFromOwnerToReproduce;
            }
            else
                myGoal = PurposeOfMovement.goToPartner;

            if (partner.GetPosition() == currentPosition && gender == Gender.female)
            {
                Reproduce(listOfAnimals);
                UpdateReproduceCharacters(partner);
            }
        }

        protected void EatingProcess(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            var target = FindTarget(listOfFoodForOmnivorous, CheckForEating);
            if (target != null)
            {
                MoveToTarget(target.GetPosition());
                if (_isDomesticated)
                {
                    myGoal = PurposeOfMovement.moveAwayFromOwnerToEat;
                }
                else
                    myGoal = PurposeOfMovement.goToFood;

                if (target.GetPosition() == currentPosition)
                    Eat(target, listOfAnimals, listOfPlants, listOfFruits);
            }
            else
            {
                _noTarget = true;
            }
        }

        private void EatingOwnersFoodProcess()
        {
            MoveToTarget(Owner.GetPosition());
            myGoal = PurposeOfMovement.goToOwner;

            if (Owner.GetPosition() == currentPosition)
            {
                Owner.FeedAnimal(this);
            }
        }
        private void WalkingProcess()
        {

            //if (_isDomesticated && myGoal != PurposeOfMovement.walkNextToOwner)
            //{
            //    MoveToTarget(Owner);
            //    myGoal = PurposeOfMovement.goToOwner;

            //    if (Owner.GetPosition() == currentPosition)
            //    {
            //        myGoal = PurposeOfMovement.walkNextToOwner;
            //    }
            //}
            //else if (_isDomesticated && myGoal == PurposeOfMovement.walkNextToOwner)
            //{
            //    BasisCellPosition = Owner.GetPosition();
            //    MoveToRandomCell();
            //}

            if (_isDomesticated)
            {
                BasisCellPosition = Owner.GetPosition();
                MoveToTarget(Owner.GetPosition());
                myGoal = PurposeOfMovement.goToOwner;
            }

            if (!_isDomesticated)
            {
                CheckForUpdatingCellAndGoal();
                myGoal = PurposeOfMovement.goToRandomCell;
                MoveToRandomCell();
            }


        }

        //--------------------------------------------------< main part >---------------------------------------------------------------
        public void LiveAnimalCicle(List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            CheckHibernationSeason();
            if (_isInHibernation)
            {
                RiseHealth();
                myGoal = PurposeOfMovement.sleep;
            }

            else
            {
                if (_wasEaten || _wasKilled)
                {
                    DieImmediately(listOfAnimals);
                }
                else
                {
                    if (CheckTimeToDie())
                        _isDead = true;

                    if (_isDead)
                    {
                        if (_isDomesticated)
                            Owner.ReportDeath(this);
                        _timeSinceDeath++;
                        CheckTimeForRemoveFromList(listOfAnimals);
                    }
                    else
                    {
                        GeneralVoidsForLiveCicle(10);

                        if (_isHungry && (CheckAbleToEat(listOfFoodForOmnivorous, CheckForEating) || _isDomesticated && CheckOwnerStocks()))
                        {
                            if (_isDomesticated && CheckOwnerStocks())
                            {
                                EatingOwnersFoodProcess();
                            }
                            else
                            {
                                EatingProcess(listOfAnimals, listOfPlants, listOfFruits, listOfFoodForOmnivorous);
                            }
                        }
                        else if (_isReadyToReproduce && CheckAbleToReproduce(listOfAnimals))
                        {
                            ReproducingProcess(listOfAnimals);
                        }
                        else
                        {
                            WalkingProcess();
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

        protected void GeneralVoidsForLiveCicle(int time, bool additionalCheck = true)
        {
            _noTarget = false;
            UpdateReadiness(time, additionalCheck);
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
                ".\r\n", _owner == null ? "No owner yet" : "My owner position now is" + _owner.GetPosition(),
                "\r\n", "My age is ", _age,
                ".\r\n", "And now I am ", myGoal);

            return result;
        }

    }
}
