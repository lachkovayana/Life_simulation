using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

class MapObjectsControl
{
    private List<Animal> listOfAnimals = new List<Animal>();
    private List<Animal> listOfDies = new List<Animal>();

    private List<Plant> listOfPlants = new List<Plant>();
    private List<Plant> listOfRemoves = new List<Plant>();

    private int rows;
    private int cols;
    private int densityAnimals;
    private int densityPlants;
    private int resolution;

    private int[,] field;

    private Rendering rendering;
    private PictureBox pictureBox;

    public MapObjectsControl(int rows, int cols, int density1, int density2, int resolution, PictureBox pictureBox, Graphics graphics)
    {
        this.pictureBox = pictureBox;
        this.resolution = resolution;
        this.rows = rows;
        this.cols = cols;
        densityAnimals = density1;
        densityPlants = density2;

        field = new int[cols, rows];
        rendering = new Rendering(this.cols, this.rows, this.resolution, this.pictureBox, graphics);
    }

    private void GetFirstField()
    {
        int[,] tmpField = new int[cols, rows];
        Random random = new Random();
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (random.Next(densityAnimals) == 0)
                {
                    this.field[x, y] = 1;
                    rendering.DrawAnimal(x, y);
                    listOfAnimals.Add(new Animal((x, y)));
                }
                else if (random.Next(densityPlants) == 0)
                {
                    this.field[x, y] = 2;
                    rendering.DrawPlant(x, y);
                    listOfPlants.Add(new Plant((x, y)));
                }
                else
                {
                    this.field[x, y] = 0;
                }
            }
        }
    }

    public void DrawFirstGeneration()
    {
        GetFirstField();
    }

    public void DrawAllChanges()
    {
        foreach (Animal animal in listOfAnimals)
        {
            animal.DecreaseSatiety();
            if (animal.GetHP().Item2 < 30)
            {
                MoveToTarget(animal);
            }
            else
            {
                MoveToRandomCell(animal);
            }
        }
    }

    private void MoveToRandomCell(Animal animal)
    {
        Random rnd = new Random();

        int x = animal.GetPosition().Item1 + rnd.Next(-1, 1); ;
        while (x < 0 || x > cols)
        {
            x = animal.GetPosition().Item1 + rnd.Next(-1, 1);
        }
        int y = animal.GetPosition().Item2 + rnd.Next(-1, 1);
        while (y < 0 || y > rows)
        {
            y = animal.GetPosition().Item2 + rnd.Next(-1, 1);
        }

        rendering.removeFromField(animal.GetPosition());
        animal.SetPosition((x,y));
        rendering.Draw((x, y));

    }

    private void MoveToTarget(Animal an)
    {
        var posAn = an.GetPosition();
        var min = 100000;
        Plant target = new Plant((0, 0));
        (int, int) newPosAn = posAn;

        foreach (Plant plant in listOfPlants)
        {
            var posPl = plant.GetPosition();
            var tmpx = Math.Abs(posAn.Item1 - posPl.Item1);
            var tmpy = Math.Abs(posAn.Item2 - posPl.Item2);

            if ((int)Math.Sqrt(tmpx * tmpx + tmpy * tmpy) < min)
            {
                min = (int)Math.Sqrt(tmpx * tmpx + tmpy * tmpy);
                target = plant;
            }
        }

        var distx = posAn.Item1 - target.GetPosition().Item1;
        var disty = posAn.Item2 - target.GetPosition().Item2;

        if (distx > 0)
        {
            if (disty > 0)
            {
                newPosAn = MoveToRight(posAn);
                newPosAn = MoveToDown(newPosAn);
            }
            else if (disty < 0)
            {
                newPosAn = MoveToRight(posAn);
                newPosAn = MoveToUp(newPosAn);
            }
            else
            {
                newPosAn = MoveToRight(posAn);

            }
        }
        else if (distx < 0)
        {
            if (disty > 0)
            {
                newPosAn = MoveToLeft(posAn);
                newPosAn = MoveToDown(newPosAn);


            }
            else if (disty < 0)
            {
                newPosAn = MoveToLeft(posAn);
                newPosAn = MoveToUp(newPosAn); ;
            }
            else
            {
                newPosAn = MoveToLeft(posAn);
            }
        }
        else
        {
            if (disty > 0)
            {
                newPosAn = MoveToDown(posAn); ;

            }
            else if (disty < 0)
            {
                newPosAn = MoveToUp(posAn); ;

            }
            else
            {
                //an.riseSatiety();
                //Plant newPlant = new Plant(FindFreeCell());
                //listOfPlants[listOfPlants.FindIndex(ind => ind.Equals(target))] = newPlant;
                //rendering.DrawPlant(newPlant.GetPosition().Item1, newPlant.GetPosition().Item2);
            }
        }

        rendering.removeFromField(posAn);
        an.SetPosition(newPosAn);
        rendering.Draw(newPosAn);

    }

    void setNewPosition(Animal an, (int, int) newPosAn)
    {

    }


    private bool CheckHealth(Animal an)
    {
        return true;
    }

    private (int, int) MoveToRight((int, int) pos)
    {
        (int, int) newPos = pos;

        return (newPos.Item1 + 1, newPos.Item2);
    }
    private (int, int) MoveToLeft((int, int) pos)
    {
        (int, int) newPos = pos;
        return (newPos.Item1 - 1, newPos.Item2);
    }

    private (int, int) MoveToUp((int, int) pos)
    {
        (int, int) newPos = pos;
        return (newPos.Item1, newPos.Item2 + 1);
    }
    private (int, int) MoveToDown((int, int) pos)
    {
        (int, int) newPos = pos;
        return (newPos.Item1, newPos.Item2 - 1);
    }

    public (int, int) FindFreeCell()
    {
        Random rnd = new Random();
        int x = rnd.Next(cols - 1);
        int y = rnd.Next(rows - 1);
        if (this.field[x, y] == 0)
        {
            return (x, y);
        }

        return FindFreeCell();
    }

}
