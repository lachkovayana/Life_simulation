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
        private House _house;
        private bool _noPlaceForBuilding = false;
        private int _indexOfVillage = -1;
        public Human((int, int) pos) : base(pos) { }



        //--------------------------------------------------------< override >-----------------------------------------------------------

        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 500; } }
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
                GeneralVoidsForLiveCicle(20, CheckIfHasAHouse() && CheckIfHasAPartner());


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
                else if (CheckAgeForRelationship() && !CheckIfHasAPartner())
                {
                    GoToMakePair(listOfHumans);
                }

                else if (_needToBuildHouse)
                {
                    HouseBuildingProcess();
                }

                else if (_isReadyToReproduce)
                {
                    GoToTheHouseToReproduce(listOfAnimals);
                }
                else
                {
                    if (CheckIfHasAHouse() && CheckStoksFullness())
                    {
                        GoToTheHouseToPutFood();
                    }

                    else if (CheckStockNotReachedLimit(FoodTypes.plant) && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is EdiblePlant f && f.IsHealthy && f.Stage != PlantStage.seed))
                    {
                        GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is EdiblePlant f && f.IsHealthy && f.Stage != PlantStage.seed);
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
                        GoTameAnimals(listOfFoodForOmnivorous);
                    }
                    else
                    {
                        MoveToRandomCell();
                        myGoal = PurposeOfMovement.goToRandomCell;
                    }

                }
                if (_noTarget || _noPlaceForBuilding)
                {
                    MoveToRandomCell();
                    myGoal = PurposeOfMovement.goToRandomCell;
                }
            }
        }



        //------------------------------------------------------------< house >---------------------------------------------------------------
        private void HouseBuildingProcess()
        {
            var positionsWithIndexes = GetPositionsOfClosestHouses();
            if (positionsWithIndexes.Count == 0)
            {
                BuildHouse(currentPosition, default);
            }
            else
            {
                var coor = GetPositionOfFreePlaceNearHouse(positionsWithIndexes);
                if (coor != default)
                {
                    var positionOfPlaceForNewHouse = coor.Item1;
                    var baseHouse = coor.Item2;
                    BuildHouse(positionOfPlaceForNewHouse, baseHouse);
                }
                else
                    _noPlaceForBuilding = true;
            }
        }
        private void GoToTheHouseToPutFood()
        {
            (int, int) hp = _house.GetPosition();
            MoveToTarget(hp);
            myGoal = PurposeOfMovement.goToHouseToPutFood;

            if (currentPosition == hp)
            {
                foreach (var pair in _foodStocks)
                {
                    _house.PutFood(pair.Key, pair.Value);
                    _foodStocks[pair.Key]--;
                }
            }
        }

        private List<(int, int, int)> GetPositionsOfClosestHouses()
        {
            List<(int, int, int)> positionsOfHousesWithIndexes = new();
            for (int x = currentPosition.Item1 - 3; x < currentPosition.Item1 + 3; x++)
            {
                for (int y = currentPosition.Item2 - 3; y < currentPosition.Item2 + 3; y++)
                {
                    if (x >= 0 && y >= 0 && x < Form1.s_cols && y < Form1.s_rows)
                    {
                        House supposedHouse = MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<House>().FirstOrDefault();
                        if (supposedHouse != default)
                        {
                            var ind = supposedHouse.indexOfVillage;
                            positionsOfHousesWithIndexes.Add((x, y, ind));
                        }
                    }
                }
            }
            return positionsOfHousesWithIndexes;
        }
        private ((int, int), (int, int, int)) GetPositionOfFreePlaceNearHouse(List<(int, int, int)> positionsAndIndices)
        {
            var orderedPositions = positionsAndIndices.OrderBy(p => Math.Abs(p.Item1 - currentPosition.Item1)).ThenBy(p => Math.Abs(p.Item2 - currentPosition.Item2));

            ((int, int), (int, int, int)) result = default;

            List<House> listOfNearbyHouses = new();

            foreach (var pairOfCoorAndIndex in orderedPositions)
            {
                for (int x = pairOfCoorAndIndex.Item1 - 1; x < pairOfCoorAndIndex.Item1 + 1; x++)
                {
                    for (int y = pairOfCoorAndIndex.Item2 - 1; y < pairOfCoorAndIndex.Item2 + 1; y++)
                    {
                        if (x >= 0 && y >= 0 && x <= Form1.s_cols && y <= Form1.s_rows)
                        {
                            House HouseInThisPosition = MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<House>().FirstOrDefault();
                            if (result == default && HouseInThisPosition == null)
                            {
                                result = ((x, y), pairOfCoorAndIndex);
                            }
                            if (HouseInThisPosition != null)
                            {
                                listOfNearbyHouses.Add(HouseInThisPosition);
                            }
                        }
                    }
                }
            }

            //int indexOfChoosenBaseHouse = result.Item2.Item3;

            //foreach (House house in listOfNearbyHouses)
            //{
            //    int indexOfCurHouse = house.indexOfVillage;
            //    if (indexOfCurHouse != indexOfChoosenBaseHouse)
            //    {
            //        int indMin = Math.Min(indexOfCurHouse, indexOfChoosenBaseHouse);
            //        int indMax = Math.Max(indexOfCurHouse, indexOfChoosenBaseHouse);
            //        foreach (MapObject mapObj in MapObjectsControl.ListOfVillages[indMax])
            //        {
            //            if (mapObj is Human humanMO)
            //            {
            //                humanMO._indexOfVillage = indMin;
            //            }
            //            if (mapObj is House houseMO)
            //            {
            //                houseMO.indexOfVillage = indMin;
            //            }
            //            MapObjectsControl.ListOfVillages[indMin].Add(mapObj);
            //        }
            //        MapObjectsControl.ListOfVillages[indMax].Clear();
            //    }
            //}

            return result;
        }

        private bool CheckIfHasAHouse()
        {
            return _house != null;
        }

        private void BuildHouse((int, int) pos, (int, int, int) baseHouseData)
        {
            var newHouse = new House(pos, baseHouseData)
            {
                FemaleOwner = _partner,
                MaleOwner = this,
            };

            int ind = GetIndexOfHouse(baseHouseData);

            _indexOfVillage = ind;
            _partner._indexOfVillage = ind;
            newHouse.indexOfVillage = ind;
            _house = newHouse;
            _partner._house = newHouse;

            MapObjectsControl.ListOfVillages[ind].Add(newHouse);
            MapObjectsControl.ListOfVillages[ind].Add(this);
            MapObjectsControl.ListOfVillages[ind].Add(_partner);
            MapObjectsControl.ListOfHouses.Add(newHouse);

            _needToBuildHouse = false;
        }

        private int GetIndexOfHouse((int, int, int) baseHouseData)
        {
            if (baseHouseData == default)
            {
                MapObjectsControl.ListOfVillages.Add(new List<MapObject>());
                return MapObjectsControl.ListOfVillages.Count - 1;
            }
            return baseHouseData.Item3;
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
        private void GoToTheHouseToReproduce(List<Animal> listOfHumans)
        {
            (int, int) hp = _house.GetPosition();
            if (CheckPartnerReadiness(_partner))
            {
                MoveToTarget(hp);
                myGoal = PurposeOfMovement.goToHouseToReproduce;

                if (_partner.GetPosition() == hp && hp == currentPosition && gender == Gender.female)
                {
                    Reproduce(listOfHumans);
                    UpdateReproduceCharacters(_partner);
                }
            }
            else
                _noTarget = true;
        }
        private bool CheckIfHasAPartner()
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
                    human.CheckAgeForRelationship() && !human._isHungry && !human.CheckIfHasAPartner())
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
                    key = FindMaxValueAndItsKey().Key;
                    break;

                case Wolf:
                    key = FoodTypes.meat;
                    break;

                case Pig:
                    key = FindMaxValueAndItsKey().Key;

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
                //CallWolfForHelp(target);
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
            if (CheckIfHasAPartner())
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

            string result = string.Concat("Hey! I am a ", gender,
                "\r\n", "My age is ", _age,
                "\r\n", "My health level is ", _currentHealth,
                "\r\n", "My satiety level is ", _currentSatiety,
                "\r\n\r\n", "My position is ", currentPosition,
                "\r\n", "Now I am ", myGoal,
                "\r\n\r\n", _partner == null ? "No partner yet" : "My patner's coordinates: " + _partner.GetPosition(),
                "\r\n", _house == null ? "No house yet" : "Сoordinates of my house: " + _house.GetPosition(),
                "\r\n", _indexOfVillage == -1 ? "No village yet" : "My village index is " + _indexOfVillage,
                "\r\n\r\n", "Time sinse breeding: ", _timeSinceBreeding,
                "\r\n", "I am ", _isReadyToReproduce ? "" : "not ", "ready for reproducing",
                "\r\n\r\n", "My stocks:\r\n", string.Join(Environment.NewLine, linesS),
                "\r\n", "And my animals:\r\n", domAnimals);
            return result;
        }

    }
}
