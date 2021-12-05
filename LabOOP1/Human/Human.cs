using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LabOOP1
{
    public class Human : OmnivorousAnimal
    {
        private Dictionary<FoodTypes, int> _foodStocks = new()
        {
            { FoodTypes.meat, 0 },
            { FoodTypes.fruit, 0 },
            { FoodTypes.plant, 0 }
        };
        private Dictionary<ResourceTypes, int> _resourceStocks = new()
        {
            { ResourceTypes.wood, 0 },
            { ResourceTypes.stone, 0 },
            { ResourceTypes.iron, 0 },
            { ResourceTypes.gold, 0 }
        };
        private Dictionary<Type, Animal> _domesticatedAnimal = new()
        {
            { typeof(Wolf), null },
            { typeof(Pig), null },
            { typeof(Sheep), null }
        };

        List<FoodTypes> desiredFoodTypesList = new();
        List<Type> desiredAnimalsList = new();

        private Human partner = null;
        private bool needToBuildHouse = false;
        public Human((int, int) pos) : base(pos) { }


        //--------------------------------------------------------< override >-----------------------------------------------------------

        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 200; } }
        protected override bool CheckForEating(FoodForOmnivorous food)
        {
            return (food is FoodForHerbivorous f && f.IsHealthy ||
                (food is Animal animal && !food.Equals(this) &&
                animal.GetType() != GetType() && animal.IsDead));
        }

        protected override void Reproduce(List<Animal> listOfHumans)
        {
            listOfHumans.Add(new Human(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCOrdinary(currentPosition);
        }

        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            return movement.MoveToTargetFor8Cells(currentPosition, target.GetPosition());
        }

        //--------------------------------------------------------< main cicle >-----------------------------------------------------------

        public void LiveHumanCicle(List<Animal> listOfHumans, List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits, List<House> listOfHouses)
        {
            if (CheckTimeToDie())
                DieHuman(listOfHumans);

            else
            {
                GeneralVoidsForLiveCicle(20);


                if (_isHungry && (CheckAbleToEat(listOfFoodForOmnivorous, CheckForEating) || CheckStocks()))
                {
                    if (CheckStocks())
                    {
                        EatSmthFromStocks(this);
                    }
                    else
                    {
                        EatingProcess(listOfAnimals, listOfPlants, listOfFruits, listOfFoodForOmnivorous);
                    }

                }

                else if (_isReadyToReproduce)
                {
                    if (CheckHavePartner() && CheckPartnerReadiness(partner))
                    {
                        MoveToTarget(partner);
                        myGoal = PurposeOfMovement.goToPartner;
                    }
                    else if (!CheckHavePartner())
                    {
                        GoToMakePair(listOfHumans);
                    }
                    if (partner?.GetPosition() == currentPosition)
                    {
                        Reproduce(listOfHumans);
                        UpdateReproduceCharacters(partner);
                    }

                }

                else
                {

                    //if инвентарь заполнен, то идти к хранилищу  
                    // else 

                    if (CheckStockNotReachedLimit(FoodTypes.plant) && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is EdiblePlant f && f.IsHealthy && f.Stage != PlantStage.seed))
                    {
                        GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Plant pl && pl.IsHealthy);
                    }
                    else if (CheckStockNotReachedLimit(FoodTypes.fruit) && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is Fruit f && f.IsHealthy))
                    {
                        GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Fruit fr && fr.IsHealthy);
                    }
                    else if (CheckStockNotReachedLimit(FoodTypes.meat) && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is Animal an && an.IsDead))
                    {
                        GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Animal an && an.IsDead);
                    }


                    else if (!CheckDomesticatedAnimalFullness() && CheckStoksFullness(1))
                    {
                        if (!CheckDomesticatedAnimalFullness())
                        {
                            GoTameAnimals(listOfFoodForOmnivorous);
                        }
                    }
                    else if (needToBuildHouse)
                    {
                        BuildHouse(listOfHouses);
                    }
                    else
                    {
                        
                        MoveToRandomCell();
                        myGoal = PurposeOfMovement.goToRandomCell;
                    }

                }
                if (_noTarget == true)
                {
                    MoveToRandomCell();
                    myGoal = PurposeOfMovement.goToRandomCell;
                }
            }
        }

        private void BuildHouse(List<House> listOfHouses)
        {
            listOfHouses.Add(new House(currentPosition, _foodStocks));
        }

        void GoToMakePair(List<Animal> listOfHumans)
        {
            // var target = FindPartner(listOfHumans);
            var target = (Human)FindTarget(listOfHumans, CheckPartner);
            if (target != null)
            {
                MoveToTarget(target);
                myGoal = PurposeOfMovement.goToPartner;

                if (target.GetPosition() == currentPosition && gender == Gender.female)
                {
                    partner = target;
                    target.partner = this;
                    target.needToBuildHouse = true;
                }
            }
            else
                _noTarget = true;

        }

        //--------------------------------------------------------< eat >-----------------------------------------------------------
        private bool CheckAbleToFindFood(List<FoodForOmnivorous> listOfFoodForOmnivorous, Func<FoodForOmnivorous, bool> check)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (check(food))
                    return true;
            }
            return false;
        }


        //--------------------------------------------------------< collect food >-----------------------------------------------------------
        private void GoCollectPlantsMyself(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Plant> listOfAllPlants, List<Fruit> listOfFruits, Func<FoodForOmnivorous, bool> myFunc)
        {

            var target = FindTarget(listOfFoodForOmnivorous, myFunc);

            if (target != null)
            {
                MoveToTarget(target);
                myGoal = PurposeOfMovement.goToCollectFood;
                if (currentPosition == target.GetPosition())
                {
                    CollectFood(target, listOfAllPlants, listOfFruits);
                }
            }
            else
                _noTarget = true;
        }
        private void CollectFood(FoodForOmnivorous target, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is Plant pl)
            {
                pl.Die(listOfAllPlants);
                _foodStocks[FoodTypes.plant]++;
            }
            else if (target is Fruit fr)
            {
                fr.Die(listOfFruits);
                _foodStocks[FoodTypes.fruit]++;
            }
            else if (target is Animal an)
            {
                an.WasEaten = true;
                _foodStocks[FoodTypes.meat]++;
            }
        }


        //--------------------------------------------------------< Tame >-----------------------------------------------------------
        //private Type GetMissingAnimal()
        //{
        //    foreach (var pair in _domesticatedAnimal)
        //    {
        //        if (pair.Value == null)
        //            return pair.Key;
        //    }
        //    return null;
        //}

        private void GetListOfDesiredAnimals()
        {
            desiredAnimalsList.Clear();

            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value == null)
                    desiredAnimalsList.Add(pair.Key);
            }
        }

        private void GoTameAnimals(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            GetListOfDesiredAnimals();
            foreach (Type type in desiredAnimalsList)
            {
                if (_domesticatedAnimal[type] == null)
                {
                    Animal target = (Animal)FindTarget(listOfFoodForOmnivorous, (obj) => obj.GetType() == type && obj is Animal an && !an.IsDomesticated);
                    if (target != null)
                    {
                        _noTarget = false;
                        MoveToTarget(target);
                        myGoal = PurposeOfMovement.goToTame;
                        if (currentPosition == target.GetPosition())
                        {
                            TameAnimal(target);
                        }
                        break;
                    }
                    else
                        _noTarget = true;
                }
            }


        }

        private void TameAnimal(Animal target)
        {
            target.IsDomesticated = true;
            target.Owner = this;
            //target.BasisCellPosition = target.GetPosition();
            _domesticatedAnimal[target.GetType()] = target;
        }

        private bool CheckDomesticatedAnimalFullness()
        {
            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value == null)
                    return false;
            }
            return true;
        }


        //--------------------------------------------------------< Partner & Reproduce >-----------------------------------------------------------


        private bool CheckPartnerReadiness(Human h)
        {
            return h._isReadyToReproduce;
        }

        //private Human FindPartner(List<Animal> listOfHumans)
        //{
        //    var minDist = Constants.ImpVal;
        //    Human target = null;

        //    foreach (Human h in listOfHumans)
        //    {
        //        if (CheckPartner(h))
        //        {
        //            double dist = movement.CountDistL1(currentPosition, h.GetPosition());

        //            if (dist < minDist)
        //            {
        //                minDist = dist;
        //                target = h;
        //            }
        //        }
        //    }

        //    return target;
        //}

        private bool CheckPartner(Human p)
        {
            return (p.partner == null && p.gender != gender && !p._isHungry && !p.Equals(this) && p._age > 20);
        }

        private bool CheckHavePartner()
        {
            return partner != null;
        }


        //--------------------------------------------------------< Eat >-----------------------------------------------------------

        private void GetListOfDesiredFood()
        {
            desiredFoodTypesList.Clear();

            foreach (var pair in _foodStocks)
            {
                if (pair.Value != Constants.MaxCountOfFoodStock)
                    desiredFoodTypesList.Add(pair.Key);
            }
        }

        private void EatSmthFromStocks(Animal eater)
        {
            myGoal = PurposeOfMovement.stand;

            FoodTypes key = FoodTypes.any;
            switch (eater)
            {
                case Human:
                    var tmp1 = _foodStocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
                    key = tmp1.Key;
                    break;

                case Wolf:
                    key = FoodTypes.meat;
                    break;

                case Pig:
                    var tmp2 = _foodStocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
                    key = tmp2.Key;

                    break;

                case Sheep:
                    if (_foodStocks[FoodTypes.fruit] >= _foodStocks[FoodTypes.plant])
                        key = FoodTypes.fruit;
                    else
                        key = FoodTypes.plant;
                    break;
            }

            _foodStocks[key]--;
            eater.RiseSatiety();
        }

        internal void ReportDeath(Animal animal)
        {
            switch (animal)
            {
                case Wolf:
                    _domesticatedAnimal[typeof(Wolf)] = null;
                    break;
                case Pig:
                    _domesticatedAnimal[typeof(Pig)] = null;
                    break;
                case Sheep:
                    _domesticatedAnimal[typeof(Sheep)] = null;
                    break;
            }
        }

        internal void GetFoodFromOwner(Animal animal)
        {
            EatSmthFromStocks(animal);
        }

        //--------------------------------------------------------< hunt >-----------------------------------------------------------


        private void GoHuntWithWolf(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            var target = FindTarget(listOfFoodForOmnivorous, (food) => food is Animal a && !a.IsDomesticated);
            if (target != null)
            {
                CallWolfForHelp(target);
                MoveToTarget(target);
                myGoal = PurposeOfMovement.goToCollectFood;
                if (currentPosition == target.GetPosition() && _domesticatedAnimal[typeof(Wolf)].GetPosition() == target.GetPosition())
                {
                    CollectFood(target, listOfAllPlants, listOfFruits);
                }
            }
            else
            {
                _noTarget = true;
            }

        }



        private void CallWolfForHelp(FoodForOmnivorous target)
        {

        }

        //--------------------------------------------------------< stocks >-----------------------------------------------------------

        private bool CheckStoksFullness(int count = Constants.MaxCountOfFoodStock)
        {
            foreach (var pair in _foodStocks)
            {
                if (CheckStockNotReachedLimit(pair.Key, count))
                    return false;
            }
            return true;
        }
        private bool CheckStockNotReachedLimit(FoodTypes ft, int count = Constants.MaxCountOfFoodStock)
        {
            return _foodStocks[ft] < count;
        }

        public bool CheckStocks(FoodTypes food = FoodTypes.any)
        {
            if (food == FoodTypes.any)
            {
                foreach (var pair in _foodStocks)
                {
                    if (pair.Value != 0)
                        return true;
                }
            }
            else
            {
                return (_foodStocks[food] != 0);
            }

            return false;
        }

        private KeyValuePair<FoodTypes, int> FindMaxValueAndItsKey()
        {
            return _foodStocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
        }


        //--------------------------------------------------------< Die >-----------------------------------------------------------


        private void DieHuman(List<Animal> listOfHumans)
        {
            if (CheckHavePartner())
            {
                partner.partner = null;
            }
            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value != null)
                {
                    var animal = pair.Value;
                    animal.IsDomesticated = false;
                    animal.Owner = null;
                    animal.BasisCellPosition = animal.GetPosition();
                }
            }
            listOfHumans.Remove(this);
            IsDead = true;
        }

        //--------------------------------------------------------< Info >-----------------------------------------------------------

        protected override string GetInfo()
        {
            if (IsDead) { return "I'm dead... :("; }
            var linesS = _foodStocks.Select(kvp => "- " + kvp.Key + ": " + kvp.Value + "/" + Constants.MaxCountOfFoodStock);
            string domAnimals = "";
            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value != null)
                {
                    domAnimals += pair.Key.Name + pair.Value.GetPosition() + ", ";
                }
            }

            string result = string.Concat("Hey! I am a ", gender, " and I'm an ", Nutrition.ToString(),
                "\r\n", "My health level is ", _currentHealth,
                "\r\n", "My satiety level is ", _currentSatiety,
                "\r\n", "My position is ", currentPosition,
                "\r\n", "My age is ", _age,
                "\r\n", "Now I am ", myGoal,
                "\r\n", partner == null ? "No partner yet" : "My patner's coordinate: " + partner.GetPosition(),
                "\r\n", "My stocks:\r\n", string.Join(Environment.NewLine, linesS),
                "\r\n", "And my animals:\r\n", domAnimals);
            //"\r\n", "Max count of food in stocks is ", Constants.MaxCountOfStock);

            return result;
        }
    }
}
