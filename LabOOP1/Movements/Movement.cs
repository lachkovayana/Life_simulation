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
        public (int, int) MoveToRandomCell2((int, int) pos, (int, int) basisCellPos)
        {
            Random rnd = new();

            int x, y;
            do
            {
                x = pos.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols || Math.Abs(x - basisCellPos.Item1) > 5);

            do
            {
                y = pos.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows || Math.Abs(y - basisCellPos.Item2) > 5);

            return (x, y);
        }

        // случайное движение с вероятностью 50% к средней точке по всем существам вида
        public (int, int) MoveToRandomCell3(Animal thisAnimal)
        {
            int x = 0;
            int y = 0;
            int count = 1;
            foreach (Animal f in MapObjectsControl.listOfAnimalsCopy)
            {
                if (f.GetType() == thisAnimal.GetType())
                {
                    x += f.GetPosition().Item1;
                    y += f.GetPosition().Item2;
                    count++;
                }
            }
            // костыль
            if (count == 0) 
                return (thisAnimal.GetPosition());

            (int, int) averPoint = (x / count, y / count);
            Random random = new();
            int rand = random.Next(0, 10);
            (int, int) newPosition;
            if (rand < 5)
            {
                newPosition = MoveToTarget1(thisAnimal.GetPosition(), averPoint);
            }
            else
            {
                newPosition = MoveToRandomCell1(thisAnimal.GetPosition());
            }


            return newPosition;
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
        public (int, int) MoveToTarget2((int, int) pos, (int, int) targetPos)
        {
            (int, int) newPosAn = pos;
            var distx = pos.Item1 - targetPos.Item1;
            var disty = pos.Item2 - targetPos.Item2;

            if (distx > 0)
                newPosAn = MoveToDirection(pos, Direction.left);
            else if (distx < 0)
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



        //вспомогательные функции
        public (int, int) FindNewCell((int, int) position)
        {
            Random rnd = new Random();
            int x, y;
            do
            {
                x = position.Item1 + rnd.Next(-1, 2);
            }
            while (x < 0 || x >= Form1.s_cols);

            do
            {
                y = position.Item2 + rnd.Next(-1, 2);
            }
            while (y < 0 || y >= Form1.s_rows);
            return (x, y);
        }

        // Расстояние L1
        public int CountDistL1((int, int) posAnimal, (int, int) posFood)
        {
            var tmpx = Math.Abs(posAnimal.Item1 - posFood.Item1);
            var tmpy = Math.Abs(posAnimal.Item2 - posFood.Item2);
            return tmpx + tmpy;
        }

        // Евклидово расстояние
        public double CountDistEuclid((int, int) posAnimal, (int, int) posFood)
        {
            var tmpx = Math.Abs(posAnimal.Item1 - posFood.Item1);
            var tmpy = Math.Abs(posAnimal.Item2 - posFood.Item2);
            return Math.Sqrt(Math.Pow(tmpx, 2) + Math.Pow(tmpy, 2));
        }

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
