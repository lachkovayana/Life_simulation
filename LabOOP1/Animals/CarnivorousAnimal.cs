using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class CarnivorousAnimal : Animal
    {
        private Movement MoveWay = new();
        public CarnivorousAnimal((int, int) pos) : base(pos)
        {
        }
        protected override void MoveToRandomCell()
        {
            var newPosition = MoveWay.MoveToRandomCell1(position);
            SetPosition(newPosition);
        }
        protected override void MoveToFood(FoodForOmnivorous target)
        {
            var newPosAn = MoveWay.MoveToTarget1(position, target);
            SetPosition(newPosAn);
        }
        protected override bool CheckAbleToEat(List<FoodForOmnivorous> listOfFoodForOmnivorous)
        {
            foreach (FoodForOmnivorous food in listOfFoodForOmnivorous)
            {
                if (food is Animal && food.GetType() != GetType())
                    return true;
            }
            return false;
        }
        protected override void SetNutrition()
        {
            Nutrition = NutritionMethod.carnivorous;
        }

    }
    public class Leopard : CarnivorousAnimal
    {
        public Leopard((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 120; } }
        protected override int MaxSatiety { get { return 120; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Leopard(position));
        }
    }

    public class Wolf : CarnivorousAnimal
    {
        public Wolf((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 130; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Wolf(position));
        }
    }

    public class Fox : CarnivorousAnimal
    {
        public Fox((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 110; } }
        protected override int MaxSatiety { get { return 110; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Fox(position));

        }
    }
}
