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

        private Human _partner = null;
        private bool _needToBuildHouse = false;
        private bool _haveAHouse = false;

        private House _house;
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

        protected override (int, int) MoveToTargetOver((int, int) position)
        {
            return movement.MoveToTargetFor8Cells(currentPosition, position);
        }

        //--------------------------------------------------------< main cicle >-----------------------------------------------------------

        public void LiveHumanCicle(List<Animal> listOfHumans, List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits)
        {
            if (CheckTimeToDie())
                DieHuman(listOfHumans);

            else
            {
                GeneralVoidsForLiveCicle(20, _haveAHouse && CheckHavePartner());


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
                else if (CheckAgeForRelationship() && !CheckHavePartner())
                {
                    GoToMakePair(listOfHumans);
                }

                else if (_needToBuildHouse)
                {
                    var positions = GetPositionsOfClosestHouses();
                    if (positions.Count == 0)
                    {
                        BuildHouse(currentPosition);
                    }
                    else
                    {
                        var coor = CheckNearbyArea(positions);
                        if (coor != default)
                            BuildHouse(coor);
                        else
                            _noTarget = true;
                    }
                }

                else if (_isReadyToReproduce)
                {
                    if (CheckPartnerReadiness(_partner))
                    {
                        MoveToTarget(_house.GetPosition());
                        myGoal = PurposeOfMovement.goToHouse;
                    }
                    if (_partner.GetPosition() == _house.GetPosition() && _house.GetPosition() == currentPosition)
                    {
                        Reproduce(listOfHumans);
                        UpdateReproduceCharacters(_partner);
                    }

                }

                else
                {

                    if (_haveAHouse && CheckStoksFullness())
                    {
                        (int, int) hp = _house.GetPosition();
                        MoveToTarget(hp);
                        if (currentPosition == hp)
                        {
                            foreach (var pair in _foodStocks)
                            {
                                _house.PutFood(pair.Key, pair.Value);
                            }
                        }
                    }


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



        //------------------------------------------------------------< house >---------------------------------------------------------------

        private List<(int, int)> GetPositionsOfClosestHouses()
        {
            List<(int, int)> positionsOfHouses = new();
            for (int x = currentPosition.Item1 - 3; x < currentPosition.Item1 + 3; x++)
            {
                for (int y = currentPosition.Item2 - 3; y < currentPosition.Item2 + 3; y++)
                {
                    if (x >= 0 && y >= 0 && x < Form1.s_cols && y < Form1.s_rows)
                    {
                        if (MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<House>().FirstOrDefault() != default)
                        {
                            positionsOfHouses.Add((x, y));
                        }
                    }
                }
            }
            return positionsOfHouses;
        }
        private (int, int) CheckNearbyArea(List<(int, int)> positions)
        {
            var orderedPositions = positions.OrderBy(p => Math.Abs(p.Item1 - currentPosition.Item1)).ThenBy(p => Math.Abs(p.Item2 - currentPosition.Item2));
            foreach (var pairCoor in orderedPositions)
            {
                for (int x = pairCoor.Item1 - 1; x < pairCoor.Item1 + 1; x++)
                {
                    for (int y = pairCoor.Item2 - 1; y < pairCoor.Item2 + 1; y++)
                    {
                        if (x >= 0 && y >= 0 && x <= Form1.s_cols && y <= Form1.s_rows)
                            if (MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<House>().FirstOrDefault() == null)
                            {
                                return (x, y);
                            }
                    }
                }
            }
            return default;
        }

        private void BuildHouse((int, int) pos)
        {
            var nh = new House(currentPosition);
            //listOfHouses.Add(nh);
            MapObjectsControl.ListOfHouses.Add(nh);
            _haveAHouse = true;
            _partner._haveAHouse = true;
            _house = nh;
            _partner._house = nh;
            _house.FemaleOwner = _partner;
            _house.MaleOwner = this;

            _needToBuildHouse = false;
        }


        //------------------------------------------------------------< eat >---------------------------------------------------------------
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
                MoveToTarget(target.GetPosition());
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


        //--------------------------------------------------------< tame >-----------------------------------------------------------
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
                        MoveToTarget(target.GetPosition());
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

        private bool CheckHavePartner()
        {
            return _partner != null;
        }
        private bool CheckAgeForRelationship()
        {
            return _age > 5;
        }
        private bool CheckHumanPartner(FoodForOmnivorous an)
        {
            if (an is Human human)
            {
                if (!human.Equals(this) && human.gender != gender &&
                    human.CheckAgeForRelationship() && !human._isHungry)
                    return true;
            }
            return false;
        }
        void GoToMakePair(List<Animal> listOfHumans)
        {
            var target = (Human)FindTarget(listOfHumans, CheckHumanPartner);
            if (target != null)
            {
                MoveToTarget(target.GetPosition());
                myGoal = PurposeOfMovement.goToPartner;

                if (target.GetPosition() == currentPosition && gender == Gender.male)
                {
                    _partner = target;
                    target._partner = this;
                    _needToBuildHouse = true;
                }
            }
            else
                _noTarget = true;

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
                MoveToTarget(target.GetPosition());
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
                _partner._partner = null;
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
                "\r\n", _partner == null ? "No partner yet" : "My patner's coordinates: " + _partner.GetPosition(),
                "\r\n", _house == null ? "No house yet" : "Сoordinates of my house: " + _house.GetPosition(),
                "\r\n", "My stocks:\r\n", string.Join(Environment.NewLine, linesS),
                "\r\n", "And my animals:\r\n", domAnimals);
            //"\r\n", "Max count of food in stocks is ", Constants.MaxCountOfStock);

            return result;
        }
        public string InfoForHouse()
        {
            string result = string.Concat(gender,
                " position is ", currentPosition,
                "\r\n", _partner == null ? "No partner yet" : "Patner's coordinates: " + _partner.GetPosition(),
                "\r\n", _house == null ? "No house yet" : "Сoordinates of my house: " + _house.GetPosition());

            return result;
        }
    }
}
