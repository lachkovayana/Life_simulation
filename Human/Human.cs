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
            { FoodTypes.meat, 2 },
            { FoodTypes.fruit, 2 },
            { FoodTypes.plant, 2 }
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
        private House _house;
        private Role _role;
        private bool _needToBuildHouse = false;
        private bool _noPlaceForBuilding = false;
        private bool _noFoodForTamingAnimal = false;
        private bool _partOfLargeVillage = false;
        private int _timeSinceFoundingVillage = 0;

        public Human((int, int) pos) : base(pos)
        { }
        public int indexOfVillage = -1;


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
                _noTarget = false;
                _noFoodForTamingAnimal = false;

                if (!_partOfLargeVillage && CheckIfHasAHouse())
                {
                    CheckVillage();
                }

                if (_isHungry && (CheckAbleToEat(listOfFoodForOmnivorous, CheckForEating) || CheckIfHasAHouse() && Stocks.CheckStocksContainFoodType(ref _house.foodStocks)))
                {
                    if (CheckIfHasAHouse() && Stocks.CheckStocksContainFoodType(ref _house.foodStocks))
                    {
                        GoToTheBuildingToEat(_house);
                    }

                    else if (CheckIfHasABarn())
                    {
                        foreach (MapObject m in MapObjectsControl.ListOfVillages[indexOfVillage])
                        {
                            if (m is Building b)
                            {
                                if (Stocks.CheckStocksContainFoodType(ref b.foodStocks))
                                {
                                    GoToTheBuildingToEat(b);
                                    break;
                                }
                            }
                        }
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
                    if (_partOfLargeVillage)
                    {
                        _timeSinceFoundingVillage++;
                        VoidsForDependentPerson(listOfFoodForOmnivorous, listOfAnimals, listOfPlants, listOfFruits);
                    }
                    else
                    {
                        VoidsForIndependentPerson(listOfFoodForOmnivorous, listOfPlants, listOfFruits);
                    }
                }
                if (_noTarget || _noPlaceForBuilding || _noFoodForTamingAnimal)
                {
                    MoveToRandomCell();
                    myGoal = PurposeOfMovement.goToRandomCell;
                }
            }
        }

        private void VoidsForDependentPerson(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits)
        {
            //собиратель несёт еду в амбар (если он есть) или дом при достижении лимита (2) хотя бы в одном из продуктов
            if (_role == Role.gatherer && (CheckIfHasAHouse() || CheckIfHasABarn()) && Stocks.CheckIfAtLastOneLimit(ref _foodStocks))
            {
                if (CheckIfHasABarn())
                    foreach (MapObject m in MapObjectsControl.ListOfVillages[indexOfVillage])
                    {
                        if (m is Building b)
                        {
                            if (GoToTheBuildingToPutFood(b))
                                break;
                        }
                    }
                else
                    GoToTheBuildingToPutFood(_house);
            }
            //действия женщины-собирателя
            else if (_role == Role.gatherer && Stocks.CheckStockNotReachedLimit(ref _foodStocks, FoodTypes.plant)
                && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is EdiblePlant f && f.IsHealthy && f.Stage != PlantStage.seed))
            {
                GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits,
                    (food) => food is EdiblePlant f && f.IsHealthy && f.Stage != PlantStage.seed);
            }
            else if (_role == Role.gatherer && Stocks.CheckStockNotReachedLimit(ref _foodStocks, FoodTypes.fruit)
                && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is Fruit f && f.IsHealthy))
            {
                GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Fruit fr && fr.IsHealthy);
            }
            else if (_role == Role.gatherer && Stocks.CheckStockNotReachedLimit(ref _foodStocks, FoodTypes.meat)
                && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is Animal an && an.IsDead))
            {
                GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Animal an && an.IsDead);
            }

            //пастух не имеет собственных ресурсов, поэтому должен иметь дом/амбар, из которого возьмет еду для приручения животного
            else if (_role == Role.shepherd && !CheckDomesticatedAnimalFullness() && (CheckIfHasAHouse() || CheckIfHasABarn()))
            {
                GoTameAnimals(listOfFoodForOmnivorous);
            }
            //строитель
            else if (_role == Role.builder && CheckTimeForBuildingBarn())
            {
                BarnBuildingProcess();
            }

            //охотник (если охота была удачной, несёт добычу или в амбар, если он уже построен, или в дом)
            else if (_role == Role.hunter && CheckAbleToHunt(listOfAnimals))
            {
                GoToHunt(listOfAnimals);
                if (_foodStocks[FoodTypes.meat] != 0)
                {
                    if (CheckIfHasABarn())
                    {
                        Barn barn = GetAnyBarn();
                        GoToTheBuildingToPutFood(barn);
                    }
                    else
                        GoToTheBuildingToPutFood(_house);

                }
            }
            else
            {
                MoveToRandomCell();
                myGoal = PurposeOfMovement.goToRandomCell;
            }
        }

        private Barn GetAnyBarn()
        {
            return MapObjectsControl.ListOfVillages[indexOfVillage].OfType<Barn>().FirstOrDefault();
        }

        private void VoidsForIndependentPerson(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Plant> listOfPlants, List<Fruit> listOfFruits)
        {
            if (Stocks.CheckStockNotReachedLimit(ref _foodStocks, FoodTypes.plant)
                           && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is EdiblePlant f && f.IsHealthy && f.Stage != PlantStage.seed))
            {
                GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Plant pl && pl.IsHealthy);
            }
            else if (Stocks.CheckStockNotReachedLimit(ref _foodStocks, FoodTypes.fruit)
                && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is Fruit f && f.IsHealthy))
            {
                GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Fruit fr && fr.IsHealthy);
            }
            else if (Stocks.CheckStockNotReachedLimit(ref _foodStocks, FoodTypes.meat)
                && CheckAbleToFindFood(listOfFoodForOmnivorous, (food) => food is Animal an && an.IsDead))
            {
                GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Animal an && an.IsDead);
            }


            else if (!CheckDomesticatedAnimalFullness() && Stocks.CheckStoksFullness(ref _foodStocks, 1))
            {
                GoTameAnimals(listOfFoodForOmnivorous);
            }
            else
            {
                MoveToRandomCell();
                myGoal = PurposeOfMovement.goToRandomCell;
            }
        }

        //------------------------------------------------------------< village >---------------------------------------------------------------

        private void CheckVillage()
        {
            //если объединено 3 и больше домов, то это деревня
            if (MapObjectsControl.ListOfVillages[indexOfVillage].Count >= 9)
            {
                _partOfLargeVillage = true;
                _house.partOfLargeVillage = true;
                GetRole();
            }
        }

        private void GetRole()
        {
            if (gender == Gender.female)
            {
                _role = Role.gatherer;
            }
            else
            {
                Random random = new();
                _role = (Role)random.Next(1, 4);

            }
        }



        //------------------------------------------------------------< barn >---------------------------------------------------------------
        private bool CheckIfHasABarn()
        {
            return MapObjectsControl.ListOfVillages[indexOfVillage].OfType<Barn>().FirstOrDefault() != null;
        }
        private void BarnBuildingProcess()
        {
            var listOfPosOfAllBuildings = GetPositionsOfBuildingsInVillage();
            var freePlacePosition = GetPosFreePlace(listOfPosOfAllBuildings);
            if (freePlacePosition != default)
            {
                BuildBarn(freePlacePosition);
            }
            else
                _noPlaceForBuilding = true;

        }

        private void BuildBarn((int, int) freePlacePosition)
        {
            var newBarn = new Barn(freePlacePosition)
            {
                indexOfVillage = indexOfVillage
            };
            MapObjectsControl.ListOfVillages[indexOfVillage].Add(newBarn);
            MapObjectsControl.ListOfBuildings.Add(newBarn);
        }

        private (int, int) GetPosFreePlace(List<(int, int)> listOfPosOfAllBuildings)
        {
            foreach (var pos in listOfPosOfAllBuildings)
            {
                for (int x = pos.Item1 - 1; x <= pos.Item1 + 1; x++)
                {
                    for (int y = pos.Item2 - 1; y <= pos.Item2 + 1; y++)
                    {
                        if (x >= 0 && y >= 0 && x < Form1.s_cols && y < Form1.s_rows)
                        {
                            if (MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<Building>().FirstOrDefault() == null)
                                return (x, y);
                        }
                    }
                }
            }
            return default;
        }

        private List<(int, int)> GetPositionsOfBuildingsInVillage()
        {
            List<(int, int)> allBuildingsPos = new();
            foreach (MapObject m in MapObjectsControl.ListOfVillages[indexOfVillage])
            {
                if (m is Building b)
                {
                    allBuildingsPos.Add(b.GetPosition());
                }
            }
            return allBuildingsPos;
        }

        private bool CheckTimeForBuildingBarn()
        {
            return _timeSinceFoundingVillage == 1 || _timeSinceFoundingVillage % 20 == 0;
        }

        //------------------------------------------------------------< house >---------------------------------------------------------------
        private void HouseBuildingProcess()
        {
            var positionsWithIndexes = GetPosAndIndOfClosestHouses();
            if (positionsWithIndexes.Count == 0)
            {
                BuildHouse(currentPosition, default);
            }
            else
            {
                var data = GetDataAboutNewHouse(positionsWithIndexes);
                if (data != default)
                {
                    MapObjectsControl.DefineIndices(data);
                    var positionOfPlaceForNewHouse = data.Item1;
                    var positionAndIndexOfBaseHouse = data.Item2;
                    BuildHouse(positionOfPlaceForNewHouse, positionAndIndexOfBaseHouse);
                }
                else
                    _noPlaceForBuilding = true;
            }
        }

        private bool GoToTheBuildingToPutFood(Building building)
        {
            (int, int) buildingPosition = building.GetPosition();
            MoveToTarget(buildingPosition);
            myGoal = PurposeOfMovement.goToHouseToPutFood;

            int limit = building is House ? Constants.MaxCountOfFoodStockForHouse : Constants.ImpVal;

            if (currentPosition == buildingPosition)
            {
                foreach (var pair in _foodStocks)
                {
                    if (Stocks.CheckStockNotReachedLimit(ref building.foodStocks, pair.Key, limit))
                    {
                        Stocks.PutFood(ref building.foodStocks, pair.Key, pair.Value);
                        _foodStocks[pair.Key] -= pair.Value;
                    }
                    else
                        return false;
                }
            }
            return true;
        }
        private void GoToTheBuildingToEat(Building building)
        {
            (int, int) buildingPosition = building.GetPosition();
            MoveToTarget(buildingPosition);
            myGoal = PurposeOfMovement.goToHouseToPutFood;

            if (currentPosition == buildingPosition)
            {
                Stocks.GetOneItem(ref building.foodStocks, FoodTypes.any);
                RiseSatiety();
            }
        }

        private List<(int, int, int)> GetPosAndIndOfClosestHouses()
        {
            List<(int, int, int)> positionsOfHousesWithIndexes = new();
            for (int x = currentPosition.Item1 - 3; x <= currentPosition.Item1 + 3; x++)
            {
                for (int y = currentPosition.Item2 - 3; y <= currentPosition.Item2 + 3; y++)
                {
                    if (x >= 0 && y >= 0 && x < Form1.s_cols && y < Form1.s_rows)
                    {
                        Building supposedHouse = MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<Building>().FirstOrDefault();
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
        private ((int, int), (int, int, int)) GetDataAboutNewHouse(List<(int, int, int)> positionsAndIndices)
        {
            //сортируем массив (коор1, коор2, индекс) так, чтобы в начале стояли ближайшие дома, в конце - самые дальние
            var orderedPositions = positionsAndIndices.OrderBy(p => Math.Abs(p.Item1 - currentPosition.Item1)).ThenBy(p => Math.Abs(p.Item2 - currentPosition.Item2));

            List<House> listOfNearbyHouses = new();
            //для каждого дома в радиусе 3 клеток (полученного из другого метода и уже отсортированного)
            foreach (var pairOfCoorAndIndex in orderedPositions)
            {
                //проверяем область вокруг него
                for (int x = pairOfCoorAndIndex.Item1 - 1; x <= pairOfCoorAndIndex.Item1 + 1; x++)
                {
                    for (int y = pairOfCoorAndIndex.Item2 - 1; y <= pairOfCoorAndIndex.Item2 + 1; y++)
                    {
                        if (x >= 0 && y >= 0 && x < Form1.s_cols && y < Form1.s_rows)
                        {
                            //как только находим свободное место, возвращаем
                            //1 элемент - пара координат подходящего для строительства ного дома места,
                            //2 элемент - данные о доме, относительно которого строим
                            if (MapObjectsControl.FieldOfAllMapObjects[x, y].OfType<Building>().FirstOrDefault() == null)
                            {
                                return ((x, y), pairOfCoorAndIndex);
                            }
                        }
                    }
                }
            }
            return default;
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

            indexOfVillage = ind;
            _partner.indexOfVillage = ind;
            newHouse.indexOfVillage = ind;
            _house = newHouse;
            _partner._house = newHouse;

            MapObjectsControl.ListOfVillages[ind].Add(newHouse);
            MapObjectsControl.ListOfVillages[ind].Add(this);
            MapObjectsControl.ListOfVillages[ind].Add(_partner);
            MapObjectsControl.ListOfBuildings.Add(newHouse);

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

        private void GetListOfDesiredAnimals()
        {
            desiredAnimalsList.Clear();

            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value == null)
                    desiredAnimalsList.Add(pair.Key);
            }
        }
        private (Building, FoodTypes) FeedAnimalIfVil(Animal target)
        {
            (Building, FoodTypes) data = default;

            switch (target)
            {
                case HerbivorousAnimal:
                    data = TryToGetNeededFood(FoodTypes.plant);
                    if (data == default)
                        data = TryToGetNeededFood(FoodTypes.fruit);
                    break;
                case CarnivorousAnimal:
                    data = TryToGetNeededFood(FoodTypes.meat);
                    break;
                case OmnivorousAnimal:
                    data = TryToGetNeededFood(FoodTypes.plant);
                    if (data == default)
                        data = TryToGetNeededFood(FoodTypes.fruit);
                    if (data == default)
                        data = TryToGetNeededFood(FoodTypes.meat);
                    break;
            }
            return data;
        }
        private void FeedAnimalIfIndep(Animal target)
        {
            switch (target)
            {
                case HerbivorousAnimal:
                    Stocks.GetOneItem(ref _foodStocks, FoodTypes.plant);
                    break;
                case CarnivorousAnimal:
                    Stocks.GetOneItem(ref _foodStocks, FoodTypes.meat);
                    break;
                case OmnivorousAnimal:
                    Stocks.GetOneItem(ref _foodStocks, FoodTypes.plant);
                    break;
            }
        }


        private void GoTameAnimals(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            GetListOfDesiredAnimals();
            foreach (Type type in desiredAnimalsList)
            {
                Animal target = (Animal)FindTarget(listOfFoodForOmnivorous, (obj) => obj.GetType() == type && obj is Animal an && !an.IsDomesticated);
                if (target != null)
                {
                    (Building, FoodTypes) data = default;
                    if (_partOfLargeVillage)
                    {
                        data = FeedAnimalIfVil(target);
                    }
                    if (!_partOfLargeVillage)
                    {
                        FeedAnimalIfIndep(target);
                    }

                    if (data != default || !_partOfLargeVillage)
                    {
                        MoveToTarget(target.GetPosition());
                        myGoal = PurposeOfMovement.goToTame;
                        if (currentPosition == target.GetPosition())
                        {
                            TameAnimal(target);
                        }
                        break;
                    }
                    else
                    {
                        _noFoodForTamingAnimal = true;
                    }
                }
                else
                    _noTarget = true;

            }
        }

        private (Building, FoodTypes) TryToGetNeededFood(FoodTypes ft)
        {
            if (Stocks.CheckStocksContainFoodType(ref _house.foodStocks, ft))
            {
                Stocks.GetOneItem(ref _house.foodStocks, ft);
                return (_house, ft);
            }
            else if (CheckIfHasABarn())
            {
                foreach (Building b in MapObjectsControl.ListOfBuildings)
                {
                    if (b is Barn barn)
                    {
                        if (Stocks.CheckStocksContainFoodType(ref barn.foodStocks, ft))
                        {
                            Stocks.GetOneItem(ref barn.foodStocks, ft);
                            return (barn, ft);
                        }
                    }
                }

            }
            return default;
        }

        private void TameAnimal(Animal target)
        {
            target.Feed();
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

        //--------------------------------------------------------< Voids for domesticated animals >-----------------------------------------------------------
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
        public bool CheckFoodForAnimals(FoodTypes food)
        {
            if (CheckIfHasABarn())
            {
                foreach (Building b in MapObjectsControl.ListOfBuildings)
                {
                    if (b is Barn barn)
                    {
                        if (Stocks.CheckStocksContainFoodType(ref barn.foodStocks, food))
                            return true;
                    }
                }
            }
            return false;
        }

        internal void FeedAnimal(Animal animal)
        {
            if (_partOfLargeVillage)
            {
                FeedAnimalIfVil(animal);
            }
            if (!_partOfLargeVillage)
            {
                FeedAnimalIfIndep(animal);
            }
            animal.Feed();
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

            foreach (var pair in _house.foodStocks)
            {
                if (pair.Value != Constants.MaxCountOfFoodStockForHouse)
                    desiredFoodTypesList.Add(pair.Key);
            }
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
        private bool CheckAbleToHunt(List<Animal> listOfAnimals)
        {
            Animal target = FindTarget(listOfAnimals, (food) => food is Animal a && !a.IsDomesticated);
            if (target != null)
            {
                return true;
            }

            return false;
        }
        private void GoToHunt(List<Animal> listOfAnimals)
        {
            Animal target = FindTarget(listOfAnimals, (food) => food is Animal a && !a.IsDomesticated);
            if (target != null)
            {
                MoveToTarget(target.GetPosition());
                myGoal = PurposeOfMovement.goToHunt;
                if (currentPosition == target.GetPosition())
                {
                    target.WasKilled = true;
                    Stocks.PutFood(ref _foodStocks, FoodTypes.meat, 10);
                }
            }
            else
            {
                _noTarget = true;
            }
        }


        //--------------------------------------------------------< stocks >-----------------------------------------------------------

        //private bool CheckStoksFullness(int count = Constants.MaxCountOfFoodStock)
        //{
        //    foreach (var pair in _foodStocks)
        //    {
        //        if (CheckStockNotReachedLimit(pair.Key, count))
        //            return false;
        //    }
        //    return true;
        //}
        //private bool CheckStockNotReachedLimit(FoodTypes ft, int count = Constants.MaxCountOfFoodStock)
        //{
        //    return _foodStocks[ft] < count;
        //}

        //public bool CheckStocks(FoodTypes food = FoodTypes.any)
        //{
        //    if (food == FoodTypes.any)
        //    {
        //        foreach (var pair in _foodStocks)
        //        {
        //            if (pair.Value != 0)
        //                return true;
        //        }
        //    }
        //    else
        //    {
        //        return _foodStocks[food] != 0;
        //    }

        //    return false;
        //}

        //private KeyValuePair<FoodTypes, int> FindMaxValueAndItsKey()
        //{
        //    return _foodStocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
        //}


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
                "\r\n", indexOfVillage == -1 ? "No village yet" : "My village index is " + indexOfVillage,
                _house == null ? "" : (_partOfLargeVillage ? "\r\nWe are part of a large village" : "\r\nOur settlement does not yet form a village"),
                 _partOfLargeVillage ? "\r\nMy role is " + _role : "",
                "\r\n\r\n", "Time sinse breeding: ", _timeSinceBreeding,
                "\r\n", "I am ", _isReadyToReproduce ? "" : "not ", "ready for reproducing",
                "\r\n\r\n", "My stocks", gender == Gender.female || gender == Gender.male && !_partOfLargeVillage ?
                ":\r\n" + string.Join(Environment.NewLine, linesS) : " are kept in house",
                "\r\n", "And my animals:\r\n", domAnimals);
            return result;
        }

    }
}
