using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LabOOP1
{   //ключ самого наибольшего по значению
    //(myNewCollection.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value).Key
    //(myNewCollection.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value).Value
    public class Human : OmnivorousAnimal
    {
        private Dictionary<FoodTypes, int> _stocks = new()
        {
            { FoodTypes.meat, 0 },
            { FoodTypes.fruit, 0 },
            { FoodTypes.plant, 0 }
        };
        private Dictionary<Type, Animal> _domesticatedAnimal = new()
        {
            { typeof(Wolf), null },
            { typeof(Horse), null },
            { typeof(Sheep), null }
        };

        List<FoodTypes> desiredFoodTypesList = new();

        private Human partner = null;

        private bool _noTarget = false;

        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 130; } }

        public Human((int, int) pos) : base(pos)
        {

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

        public void LiveHumanCicle(List<Animal> listOfHumans, List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits)
        {
            _noTarget = false;

            if (CheckTimeToDie())
                DieHuman(listOfHumans);

            if (!IsDead)
            {
                GeneralVoidsForLiveCicle(20);
                if (_isHungry)
                {
                    if (CheckStocks())
                    {
                        EatSmthFromStocks(this);

                        //----------
                        myGoal = PurposeOfMovement.standing;
                        
                    }
                    else
                        FindingAndEatingProcess(listOfFoodForOmnivorous, listOfPlants, listOfFruits);

                }
                else if (_isReadyToReproduce)
                {
                    if (CheckHavePartner())
                    {
                        if (CheckPartnerReadiness(partner))
                        {
                            MoveToTarget(partner);
                            myGoal = PurposeOfMovement.goingToPartner;
                        }
                    }
                    else
                    {
                        var target = FindPartner(listOfHumans);
                        if (target != null)
                        {
                            MoveToTarget(target);
                            myGoal = PurposeOfMovement.goingToPartner;

                            if (target.GetPosition() == currentPosition && gender == Gender.female)
                            {
                                partner = target;
                                target.partner = this;
                                Reproduce(listOfHumans);
                                UpdateReproduceCharacters(target);
                            }
                        }
                        else
                        {

                            //--------------------
                            _noTarget = true;
                        }
                    }
                }

                else
                {
                    if (!CheckDomesticatedAnimalFullness())
                    {
                        GoTameAnimals(listOfFoodForOmnivorous);
                    }
                    else if (!CheckStoksFullness())
                    {
                        if (CheckStockNotReachedLimit(FoodTypes.plant) && CheckAbleToFindPlant(listOfFoodForOmnivorous))
                        {
                            GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Plant pl && pl.IsHealthy());
                        }
                        else if (CheckStockNotReachedLimit(FoodTypes.fruit) && CheckAbleToFindFruit(listOfFoodForOmnivorous))
                        {
                            GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Fruit fr && fr.IsHealthy());
                        }
                        else if (CheckStockNotReachedLimit(FoodTypes.meat) && CheckAbleToFindMeat(listOfFoodForOmnivorous))
                        {
                            GoCollectPlantsMyself(listOfFoodForOmnivorous, listOfPlants, listOfFruits, (food) => food is Animal an && an.IsDead);
                        }
                        else
                        {
                            myGoal = PurposeOfMovement.goingToRandomCell;
                        }
                    }


                    if (myGoal != PurposeOfMovement.goingToCollectFood)
                    {
                        MoveToRandomCell();
                        myGoal = PurposeOfMovement.goingToRandomCell;
                    }
                }
                if (_noTarget == true)
                {
                    MoveToRandomCell();
                    myGoal = PurposeOfMovement.goingToRandomCell;
                }
            }
        }

        private bool CheckAbleToFindPlant(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is EdiblePlant f && f.IsHealthy() && f.Stage != PlantStage.seed)
                    return true;
            }
            return false;
        }
        private bool CheckAbleToFindFruit(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is Fruit f && f.IsHealthy())
                    return true;
            }
            return false;
        }

        private bool CheckAbleToFindMeat(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is Animal an && an.IsDead)
                    return true;
            }
            return false;
        }

        private void GoHuntWithWolf(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            var target = FindTarget(listOfFoodForOmnivorous, (food) => food is Animal a && !a.IsDomesticated);
            if (target != null)
            {
                CallWolfForHelp(target);
                MoveToTarget(target);
                myGoal = PurposeOfMovement.goingToCollectFood;
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
        private void GoCollectPlantsMyself(List<FoodForOmnivorous> listOfFoodForOmnivorous, List<Plant> listOfAllPlants, List<Fruit> listOfFruits, Func<FoodForOmnivorous, bool> myFunc)
        {

            var target = FindTarget(listOfFoodForOmnivorous, myFunc);

            if (target != null)
            {
                MoveToTarget(target);
                myGoal = PurposeOfMovement.goingToCollectFood;
                if (currentPosition == target.GetPosition())
                {
                    CollectFood(target, listOfAllPlants, listOfFruits);
                }
            }
            else
                _noTarget = true;
        }

        private void GoTameAnimals(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            var desiredAnimal = GetMissingAnimal();
            Animal target = (Animal)FindTarget(listOfFoodForOmnivorous, (obj) => obj.GetType() == desiredAnimal);
            if (target != null)
            {
                myGoal = PurposeOfMovement.goingToTame;
                MoveToTarget(target);
                if (currentPosition == target.GetPosition())
                {
                    TameAnimal(target);
                }
            }
            //------------
            else
                _noTarget = true;
        }

        private Type GetMissingAnimal()
        {
            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value == null)
                    return pair.Key;
            }
            return null;
        }

        private void TameAnimal(Animal target)
        {
            target.IsDomesticated = true;
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
        private bool CheckStoksFullness()
        {
            foreach (var pair in _stocks)
            {
                if (CheckStockNotReachedLimit(pair.Key))
                    return false;
            }
            return true;
        }
        private bool CheckPartnerReadiness(Human h)
        {
            return h._isReadyToReproduce;
        }

        private Human FindPartner(List<Animal> listOfHumans)
        {
            var minDist = Constants.ImpVal;
            Human target = null;

            foreach (Human h in listOfHumans)
            {
                if (CheckPartner(h))
                {
                    double dist = movement.CountDistL1(currentPosition, h.GetPosition());

                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = h;
                    }
                }
            }

            return target;
        }

        private bool CheckPartner(Human p)
        {
            return (p.partner == null && p.gender != gender && !p._isHungry && !p.Equals(this) && p._age > 20);
        }

        private bool CheckHavePartner()
        {
            return partner != null;
        }

        //съедание
        private void EatTarget(FoodForOmnivorous target, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is Fruit fruit)
                fruit.Die(listOfFruits);
            else if (target is EdiblePlant plant)
                plant.Die(listOfAllPlants);
            else if (target is Animal animal)
                animal.WasEaten = true;
            RiseSatiety();
        }

        //процесс: нахождение еды, движение к ней, съедание 
        private void FindingAndEatingProcess(List<FoodForOmnivorous> listOfFood, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            var target = FindTarget(listOfFood, CheckForHumanEating);
            if (target != null)
            {
                MoveToTarget(target);
                myGoal = PurposeOfMovement.goingToFood;

                if (target.GetPosition() == currentPosition)
                    EatTarget(target, listOfAllPlants, listOfFruits);
            }
            else
            {
                _noTarget = true;
            }
        }


        //проверка на то, подходит ли объект в качестве еды для человека
        private bool CheckForHumanEating(FoodForOmnivorous t)
        {
            return (t is FoodForHerbivorous f && f.IsHealthy()) || (t is Animal an && t.GetType() != GetType() && an.IsDead);
        }

        //проверка на то, можно ли добавить еду в конкретный запас
        private bool CheckStockNotReachedLimit(FoodTypes ft)
        {
            return _stocks[ft] < Constants.MaxCountOfStock;
        }

        //проверка на то, заполнены ли все запасы
        private void GetListOfDesiredFood()
        {
            desiredFoodTypesList = new();

            foreach (var pair in _stocks)
            {
                if (pair.Value != Constants.MaxCountOfStock)
                    desiredFoodTypesList.Add(pair.Key);
            }
        }


        private void CallWolfForHelp(FoodForOmnivorous target)
        {

        }
        private void CollectFood(FoodForOmnivorous target, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is Plant pl)
            {
                _stocks[FoodTypes.plant]++;
                pl.Die(listOfAllPlants);
            }
            else if (target is Fruit fr)
            {
                fr.Die(listOfFruits);
                _stocks[FoodTypes.fruit]++;
            }
            else if (target is Animal an)
            {
                an.WasEaten = true;
                _stocks[FoodTypes.meat]++;

                //switch (target)
                //{
                //    case Rabbit:
                //        _stocks[FoodTypes.meat] += 1;
                //        break;
                //    case Horse:
                //        _stocks[FoodTypes.meat] += 4;
                //        break;
                //    case Sheep:
                //        _stocks[FoodTypes.meat] += 2;
                //        break;
                //    case Tiger:
                //        _stocks[FoodTypes.meat] += 2;
                //        break;
                //    case Wolf:
                //        _stocks[FoodTypes.meat] += 2;
                //        break;
                //    case Fox:
                //        _stocks[FoodTypes.meat] += 2;
                //        break;
                //    case Bear:
                //        _stocks[FoodTypes.meat] += 5;
                //        break;
                //    case Pig:
                //        _stocks[FoodTypes.meat] += 3;
                //        break;
                //    case Rat:
                //        _stocks[FoodTypes.meat] += 1;
                //        break;
                //}
            }
        }

        //съесть либо запас, количество которого превышает остальные,
        //либо заданный запас (для человека или животного, которое он кормит)
        private void EatSmthFromStocks(Animal eater, FoodTypes food = FoodTypes.any)
        {
            FoodTypes key;
            int value;
            if (food == FoodTypes.any)
            {
                var tmp = _stocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
                key = tmp.Key;
                value = tmp.Value;
            }
            else
            {
                key = food;
            }
            _stocks[key]--;
            eater.RiseSatiety();
        }

        //проверка запасов
        private bool CheckStocks(FoodTypes food = FoodTypes.any)
        {
            if (food == FoodTypes.any)
            {
                //если хоть что-то есть в запасах
                foreach (var pair in _stocks)
                {
                    if (pair.Value != 0)
                        return true;
                }
            }
            else
            {
                //если есть определенный тип еды
                return (_stocks[food] != 0);
            }

            return false;
        }

        private KeyValuePair<FoodTypes, int> FindMaxValueAndItsKey()
        {
            return _stocks.OrderByDescending(z => z.Value).ToDictionary(a => a, s => s).First().Value;
        }

        private void DieHuman(List<Animal> listOfHumans)
        {
            SetPosition((0, 0));

            listOfHumans.Remove(this);
            IsDead = true;
        }

        protected override string GetInfo()
        {
            if (IsDead) { return "I'm dead... :("; }
            var linesS = _stocks.Select(kvp => "- " + kvp.Key + ": " + kvp.Value.ToString() + "/" + Constants.MaxCountOfStock);
            string animals = "";
            foreach (var pair in _domesticatedAnimal)
            {
                if (pair.Value != null)
                {
                    animals += pair.Key.Name + " (" + pair.Value.GetPosition() + "), ";
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
                "\r\n", "And my animals:\r\n", animals);
            //"\r\n", "Max count of food in stocks is ", Constants.MaxCountOfStock);

            return result;
        }
    }
}
