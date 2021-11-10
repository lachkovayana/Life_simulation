using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class HerbivorousAnimal : Animal
    {
        private Movement MoveWay = new();
        public HerbivorousAnimal((int, int) pos) : base(pos) { }

        //--------------------------------------------------<override methods>---------------------------------------------------------------


        protected override void MoveToRandomCell()
        {
            //Должно быть
            var newPosition = MoveWay.MoveToRandomCell2(currentPosition, BasisCellPosition);
            //var newPosition = MoveWay.MoveToRandomCell1(currentPosition);
            SetPosition(newPosition);
        }
        protected override void MoveToFood(FoodForOmnivorous target)
        {
            //Должно быть
            var newPosAn = MoveWay.MoveToTarget2(currentPosition, target.GetPosition());
            //var newPosAn = MoveWay.MoveToTarget2(position, target.GetPosition());
            SetPosition(newPosAn);
        }
        protected override bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is FoodForHerbivorous)
                    return true;
            }
            return false;
        }
        protected override void SetNutrition()
        {
            Nutrition = NutritionMethod.herbivorous;
        }

    }

    //--------------------------------------------------<inheritor classes>---------------------------------------------------------------


    public class Rabbit : HerbivorousAnimal
    {

        public Rabbit((int, int) pos) : base(pos) { }
        protected override int MaxHealth { get { return 80; } }
        protected override int MaxSatiety { get { return 80; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rabbit(currentPosition));
        }
    }

    public class Horse : HerbivorousAnimal
    {

        public Horse((int, int) pos) : base(pos) { }
        protected override int MaxHealth { get { return 100; } }
        protected override int MaxSatiety { get { return 100; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Horse(currentPosition));
        }
    }

    public class Giraffe : HerbivorousAnimal
    {
        public Giraffe((int, int) pos) : base(pos) { }
        protected override int MaxHealth { get { return 90; } }
        protected override int MaxSatiety { get { return 90; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Giraffe(currentPosition));
        }
    }


}
