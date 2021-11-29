﻿using System.Collections.Generic;
namespace LabOOP1
{
    public abstract class HerbivorousAnimal : Animal
    {
        public HerbivorousAnimal((int, int) pos) : base(pos) { }

        //--------------------------------------------------<override methods>---------------------------------------------------------------
       
        protected override bool CheckForEating(FoodForOmnivorous food)
        {
            return (food is FoodForHerbivorous);
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
        protected override int MaxHealth { get => 80;  }
        protected override int MaxSatiety { get { return 80; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rabbit(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCOrdinary(currentPosition);
        }
        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            return movement.MoveToTarget3CellsForward(currentPosition, target.GetPosition());
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
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCNotGoingFar(currentPosition, BasisCellPosition);
        }
        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            return movement.MoveToTargetFor8Cells(currentPosition, target.GetPosition());
        }
    }

    public class Sheep : HerbivorousAnimal
    {
        public Sheep((int, int) pos) : base(pos) { }
        protected override int MaxHealth { get { return 90; } }
        protected override int MaxSatiety { get { return 90; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Sheep(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCWithProbability(this);
        }
        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            return movement.MoveToTargetFor4Cells(currentPosition, target.GetPosition());
        }
    }


}
