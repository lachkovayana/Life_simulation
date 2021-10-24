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

        public HerbivorousAnimal((int, int) pos) : base(pos)
        {
        }
        internal override void MoveToRandomCell()
        {
            var newPosition = MoveWay.MoveToRandomCell1(_position);
            SetPosition(newPosition);
        }
        internal override void MoveToFood(FoodForOmnivorous target)
        {
            var newPosAn = MoveWay.MoveToTarget1(_position, target);
            SetPosition(newPosAn);
        }

    }

    public class Rabbit : HerbivorousAnimal
    {

        public Rabbit((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rabbit(_position));
        }
    }
    
    public class Horse : HerbivorousAnimal
    {

        public Horse((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Horse(_position));

        }
    }
    
    public class Giraffe : HerbivorousAnimal
    {

        public Giraffe((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Giraffe(_position));

        }
    }


}
