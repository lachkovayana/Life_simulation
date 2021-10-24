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

    public class Bear : OmnivorousAnimal
    {
        public Bear((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Bear(_position));
        }
    }

    public class Pig : OmnivorousAnimal
    {
        public Pig((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Pig(_position));

        }
    }

    public class Rat : OmnivorousAnimal
    {
        public Rat((int, int) pos) : base(pos)
        {
        }
        internal override void Reproduce(List<Animal> listOfAnimals)
        {
            listOfAnimals.Add(new Rat(_position));

        }
    }


}
