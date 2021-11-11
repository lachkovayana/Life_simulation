﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class OmnivorousAnimal : Animal
    {
        public OmnivorousAnimal((int, int) pos) : base(pos) {}

        //--------------------------------------------------<override methods>---------------------------------------------------------------
        protected override bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {

            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is FoodForHerbivorous || (food is Animal && food.GetType() != this.GetType()))
                    return true;
            }
            return false;
        }
        protected override bool CheckForEating(FoodForOmnivorous food)
        {
            return (food is FoodForHerbivorous ||  (food is Animal animal && !food.Equals(this) && animal.GetType() != GetType()));
        }
        protected override void SetNutrition()
        {
            Nutrition = NutritionMethod.omnivorous;
        }
    }


    //--------------------------------------------------<inheritor classes>---------------------------------------------------------------

    public class Bear : OmnivorousAnimal
    {
        public Bear((int, int) pos) : base(pos) {}
        protected override int MaxHealth { get { return 120; } }
        protected override int MaxSatiety { get { return 120; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Bear(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCOrdinary(currentPosition);
        }

        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            return movement.MoveToTargetFor4Cells(currentPosition, target.GetPosition());
        }
    }

    public class Pig : OmnivorousAnimal
    {
        public Pig((int, int) pos) : base(pos) {}
        protected override int MaxHealth { get { return 140; } }
        protected override int MaxSatiety { get { return 140; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Pig(currentPosition));
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

    public class Rat : OmnivorousAnimal
    {
        public Rat((int, int) pos) : base(pos) {}
        protected override int MaxHealth { get { return 150; } }
        protected override int MaxSatiety { get { return 150; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rat(currentPosition));
        }
        protected override (int, int) MoveToRandomCellOver()
        {
            return movement.MoveToRCWithProbability(this);
        }

        protected override (int, int) MoveToTargetOver(FoodForOmnivorous target)
        {
            return movement.MoveToTarget3CellsForward(currentPosition, target.GetPosition());
        }
    }


}
