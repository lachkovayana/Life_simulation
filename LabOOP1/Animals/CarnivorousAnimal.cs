using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOOP1
{
    public abstract class CarnivorousAnimal : Animal
    {
        private Movement MoveWay = new();
        public CarnivorousAnimal((int, int) pos) : base(pos)
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
    public class Leopard : CarnivorousAnimal
    {
        public Leopard((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Leopard(_position));
        }
    }

    public class Wolf : CarnivorousAnimal
    {
        public Wolf((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
        }
    }

    public class Fox : CarnivorousAnimal
    {
        public Fox((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Fox(_position));

        }
    }
}
