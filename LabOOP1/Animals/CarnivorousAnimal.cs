using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class CarnivorousAnimal : Animal
    {
        public CarnivorousAnimal((int, int) pos) : base(pos) { }


        //--------------------------------------------------<override methods>---------------------------------------------------------------

        protected override bool CheckForEating(FoodForOmnivorous food)
        {
            return (food is Animal animal && !food.Equals(this) && animal.GetType() != GetType());
        }
        protected override void SetNutrition()
        {
            Nutrition = NutritionMethod.carnivorous;
        }

    }
    //--------------------------------------------------<inheritor classes>---------------------------------------------------------------

    public class Tiger : CarnivorousAnimal
    {
        public Tiger((int, int) pos) : base(pos) { }
        protected override int MaxHealth { get { return 120; } }
        protected override int MaxSatiety { get { return 120; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Tiger(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCOrdinary(currentPosition);
        }

        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            //евклидово расстояние добавить
            return movement.MoveToTarget3CellsForward(currentPosition, target.GetPosition());

        }
    }

    public class Wolf : CarnivorousAnimal
    {
        public Wolf((int, int) pos) : base(pos) { }
        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 130; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Wolf(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCNotGoingFar(currentPosition, BasisCellPosition);
        }
        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            //евклидово расстояние добавить
            return movement.MoveToTargetFor4Cells(currentPosition, target.GetPosition());

        }
    }

    public class Fox : CarnivorousAnimal
    {
        public Fox((int, int) pos) : base(pos) { }

        protected override int MaxHealth { get { return 110; } }
        protected override int MaxSatiety { get { return 110; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Fox(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCWithProbability(this);
        }
        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            //евклидово расстояние добавить
            return movement.MoveToTargetFor8Cells(currentPosition, target.GetPosition());
        }

    }
}
