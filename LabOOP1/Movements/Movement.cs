using System;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Movement
    {
        // случайное движение 
        public (int, int) MoveToRandomCell1((int, int) pos)
        {
            Random rnd = new();

            int x, y;
            do
            {
                x = pos.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols);

            do
            {
                y = pos.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows);

            return (x, y);
        }

        //случайное движение, не отходя больше чем на 5 клеток от места рождения/последней еды
        public (int, int) MoveToRandomCell2((int, int) pos, (int, int) lastTarget)
        {
            Random rnd = new();

            int x, y;
            do
            {
                x = pos.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols || Math.Abs(x - lastTarget.Item1) > 5);

            do
            {
                y = pos.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows || Math.Abs(y - lastTarget.Item2) > 5);

            return (x, y);
        }

        // движение к еде на 8 клеток
        public (int, int) MoveToTarget1((int, int) pos, (int, int) targetPos)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - targetPos.Item1;
            var disty = pos.Item2 - targetPos.Item2;

            if (distx < 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.right);
                    newPosAn = MoveToDirection(newPosAn, Direction.up);
                }
                else
                {
                    newPosAn = MoveToDirection(pos, Direction.right);
                }
            }
            else if (distx > 0)
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.left);
                    newPosAn = MoveToDirection(newPosAn, Direction.up); ;
                }
                else
                {
                    newPosAn = MoveToDirection(pos, Direction.left);
                }
            }
            else
            {
                if (disty > 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.down); ;

                }
                else if (disty < 0)
                {
                    newPosAn = MoveToDirection(pos, Direction.up); ;
                }
            }
            return newPosAn;
        }


        // движение к еде на 4 клетки с сохранением направления
        public (int, int) MoveToTarget2((int, int) pos, FoodForOmnivorous target)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - target.GetPosition().Item1;
            var disty = pos.Item2 - target.GetPosition().Item2;

            if (distx > 0)
                newPosAn = MoveToDirection(pos, Direction.left);
            else if (disty < 0)
                newPosAn = MoveToDirection(pos, Direction.right);
            else
            {
                if (disty > 0)
                    newPosAn = MoveToDirection(newPosAn, Direction.down);
                if (disty < 0)
                    newPosAn = MoveToDirection(newPosAn, Direction.up);
            }

            return newPosAn;
        }

        // случайное движение с вероятностью 60% к средней точке по всем существам вида
        public (int, int) MoveWithProbability(Animal thisAnimal, List<FoodForOmnivorous> listOfTargets)
        {
            int x = 0;
            int y = 0;
            int count = 0;
            foreach (FoodForOmnivorous f in listOfTargets)
            {
                if (f is Animal && f.GetType() == thisAnimal.GetType())
                {
                    x += f.GetPosition().Item1;
                    y += f.GetPosition().Item2;
                    count++;
                }
            }
            (int, int) averPoint = (x / count, y / count);
            Random random = new();
            int rand = random.Next(0, 100);
            (int, int) newPosition = (0, 0);
            if (rand < 60) 
            {
                newPosition = MoveToTarget1(thisAnimal.GetPosition(), averPoint);
            }
            else
            {
                newPosition = MoveToRandomCell1(thisAnimal.GetPosition());
            }


            return newPosition;
        }

        //вспомогательная функция
        private (int, int) MoveToDirection((int, int) pos, Direction direction)
        {
            int x = pos.Item1;
            int y = pos.Item2;

            switch (direction)
            {
                case Direction.right:
                    return (x + 1, y);
                case Direction.left:
                    return (x - 1, y);
                case Direction.up:
                    return (x, y + 1);
                case Direction.down:
                    return (x, y - 1);
                default:
                    break;
            }
            return (x, y);
        }
    }
}
