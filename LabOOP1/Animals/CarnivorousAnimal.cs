using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public abstract class CarnivorousAnimal : Animal
    {
        private Movement movement = new(); 

        public CarnivorousAnimal((int, int) pos) : base(pos) {}


        //--------------------------------------------------<override methods>---------------------------------------------------------------

        protected override void MoveToRandomCell()
        {
            // верно
            var newPosition = movement.MoveToRandomCell1(currentPosition);
            SetPosition(newPosition);
        }
        protected override void MoveToFood(FoodForOmnivorous target)
        {
            //верно, но в Animal нужно указать Евклидово расстояние 
            var newPosAn = movement.MoveToTarget1(currentPosition, target.GetPosition());
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



    //--------------------------------------------------<inheritor classes>---------------------------------------------------------------



    public class Tiger : CarnivorousAnimal
    {
        public Tiger((int, int) pos) : base(pos) {}
        protected override int MaxHealth { get { return 120; } }
        protected override int MaxSatiety { get { return 120; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Tiger(currentPosition));
        }
    }

    public class Wolf : CarnivorousAnimal
    {
        public Wolf((int, int) pos) : base(pos) {}
        protected override int MaxHealth { get { return 130; } }
        protected override int MaxSatiety { get { return 130; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Wolf(currentPosition));
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
            listOfAnimals.Add(new Fox(currentPosition));

        }
    }
}
