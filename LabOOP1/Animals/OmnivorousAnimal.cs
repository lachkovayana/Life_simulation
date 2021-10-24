using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class OmnivorousAnimal : Animal
    {
        private Movement MoveWay = new();

        public OmnivorousAnimal((int, int) pos) : base(pos)
        {
        }
        protected override void MoveToRandomCell()
        {
            var newPosition = MoveWay.MoveToRandomCell1(position);
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
                if (food is Plant || (food is Animal && food.GetType() != GetType()))
                    return true;
            }

            return false;
        }
        protected override void SetNutrition()
        {
            Nutrition = NutritionMethod.omnivorous;
        }
    }

    public class Bear : OmnivorousAnimal
    {
        public Bear((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 120; } }
        protected override int MaxSatiety { get { return 120; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Bear(position));
        }
    }

    public class Pig : OmnivorousAnimal
    {
        public Pig((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 140; } }
        protected override int MaxSatiety { get { return 140; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Pig(position));

        }
    }

    public class Rat : OmnivorousAnimal
    {
        public Rat((int, int) pos) : base(pos)
        {
        }
        protected override int MaxHealth { get { return 150; } }
        protected override int MaxSatiety { get { return 150; } }
        protected override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rat(position));

        }
    }


}
