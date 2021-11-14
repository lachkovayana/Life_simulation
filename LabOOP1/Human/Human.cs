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
        private Dictionary<MapObject, int> _domesticatedAnimal = new()
        {
            { MapObject.wolf, 0 },
            { MapObject.horse, 0 },
            { MapObject.sheep, 0 }
        };
        private const int MaxCountOfStock = 3;
        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 80; } }


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

        public void LiveHumanCicle(List<Human> listOfHumans, List<FoodForOmnivorous> listOfFood, List<Animal> listOfAnimals, List<Plant> listOfPlants, List<Fruit> listOfFruits)
        {
            if (CheckTimeToDie())
                DieHuman(listOfHumans);

            if (!IsDead)
            {
                GeneralVoidsForLiveCicle(15);
                if (_isHungry)
                {
                    if (CheckStocks())
                    {
                        EatSmthFromStocks(this);
                        myGoal = GoalOfTheLastStep.goingToRandomCell;
                    }
                    else
                    {
                        myGoal = GoalOfTheLastStep.goingToFood;
                        FindingAndEatingProcess(listOfFood, listOfAnimals, listOfPlants, listOfFruits);
                    }
                }
                //else if (CheckTimeForRepr && ableToReproduce) Reproduce || FindTarget(partner)

                else
                {
                    //if !CheckStocksLimit()
                    myGoal = GoalOfTheLastStep.goingToRandomCell;
                    MoveToRandomCell();
                }
            }
            //HuntWithWolf();
        }

        //съедание
        private void EatTarget(FoodForOmnivorous target, List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            if (target is Fruit fruit)
                fruit.Die(listOfFruits);
            else if (target is EdiblePlant plant)
                plant.Die(listOfAllPlants);
            else if (target is Animal animal)
                animal.RemoveFromList(listOfAnimals);
            RiseSatiety();
            //RiseSatiety(target.);
        }

        //процесс: нахождение еды, движение к ней, съедание 
        private void FindingAndEatingProcess(List<FoodForOmnivorous> listOfFood, List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            var target = FindTarget(listOfFood, CheckForHumanEating);

            MoveToTarget(target);

            if (target.GetPosition() == currentPosition)
                EatTarget(target, listOfAnimals, listOfAllPlants, listOfFruits);
        }


        //проверка на то, подходит ли объект в качестве еды для человека
        private bool CheckForHumanEating(FoodForOmnivorous t)
        {
            return (t is EdiblePlant pl && pl.IsHealthy()) || (t is Fruit fr && fr.IsHealthy()) || t is Animal && t.GetType() != GetType();
        }

        //проверка на то, можно ли добавить еду в конкретный запас
        private bool CheckStocksLimit(FoodTypes ft)
        {
            return _stocks[ft] < 4;
        }

        //проверка на то, заполнены ли все запасы
        private bool CheckStocksFullness()
        {
            foreach (var pair in _stocks)
            {
                if (pair.Value != MaxCountOfStock) return false;
            }
            return true;
        }
        //private void HuntWithWolf()
        //{
        //    if (_domesticatedAnimal[MapObject.wolf] != 0)
        //    {

        //    }
        //}

        //private void CallWolfForHelp()
        //{

        //}
        //private void CollectFood(FoodForOmnivorous target)
        //{
        //    if (target is Plant)
        //    {
        //        _stocks[FoodTypes.plant] += 1;
        //    }
        //    else if (target is Fruit)
        //    {
        //        _stocks[FoodTypes.fruit] += 1;
        //    }
        //    else if (target is Animal)
        //    {
        //        switch (target)
        //        {
        //            case Rabbit:
        //                _stocks[FoodTypes.meat] += 1;
        //                break;
        //            case Horse:
        //                _stocks[FoodTypes.meat] += 4;
        //                break;
        //            case Sheep:
        //                _stocks[FoodTypes.meat] += 2;
        //                break;
        //            case Tiger:
        //                _stocks[FoodTypes.meat] += 2;
        //                break;
        //            case Wolf:
        //                _stocks[FoodTypes.meat] += 2;
        //                break;
        //            case Fox:
        //                _stocks[FoodTypes.meat] += 2;
        //                break;
        //            case Bear:
        //                _stocks[FoodTypes.meat] += 5;
        //                break;
        //            case Pig:
        //                _stocks[FoodTypes.meat] += 3;
        //                break;
        //            case Rat:
        //                _stocks[FoodTypes.meat] += 1;
        //                break;
        //        }
        //    }
        //}

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

        private void DieHuman(List<Human> listOfHumans)
        {
            SetPosition((0, 0));

            listOfHumans.Remove(this);
            IsDead = true;
        }

        protected override string GetInfo()
        {
            if (IsDead) { return "I'm dead... :("; }
            var lines = _stocks.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            //string name = GetType().ToString()[(GetType().ToString().IndexOf(".") + 1)..].ToLower();
            string result = string.Concat("Hey! I am a ", gender, " and I'm an ", Nutrition.ToString(),
                "\r\n", "My health level is ", _currentHealth,
                "\r\n", "My satiety level is ", _currentSatiety,
                "\r\n", "My position is ", currentPosition,
                "\r\n", "Now I am ", myGoal,
                "\r\n", "And my stocks is\r\n", string.Join(Environment.NewLine, lines),
                "\r\n", "Max count of food in stocks is ", MaxCountOfStock);

            return result;
        }
    }
}
