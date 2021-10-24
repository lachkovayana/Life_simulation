﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class HerbivorousAnimal : Animal
    {
        private Movement MoveWay = new();
        FoodForOmnivorous tar = null;
        public HerbivorousAnimal((int, int) pos) : base(pos)
        {
        }
        protected override void MoveToRandomCell()
        {
            var newPosition = MoveWay.MoveToRandomCell1(position);
            //var newPosition = MoveWay.MoveToRandomCell2(position, BasisCellPosition);
            SetPosition(newPosition);
        }
        protected override void MoveToFood(FoodForOmnivorous target)
        {
            var newPosAn = MoveWay.MoveToTarget1(position, target.GetPosition());
            SetPosition(newPosAn);
        }
        protected override bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is Plant) 
                    return true;
            }
            return false;
        }
        protected override void SetNutrition()
        {
            Nutrition = NutritionMethod.herbivorous;
        }
        
    }

    public class Rabbit : HerbivorousAnimal
    {

        public Rabbit((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 80; } }
        protected override int MaxSatiety { get { return 80; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rabbit(position));
        }
    }
    
    public class Horse : HerbivorousAnimal
    {

        public Horse((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 100; } }
        protected override int MaxSatiety { get { return 100; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Horse(position));

        }
    }
    
    public class Giraffe : HerbivorousAnimal
    {

        public Giraffe((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 90; } }
        protected override int MaxSatiety { get { return 90; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Giraffe(position));

        }
    }


}