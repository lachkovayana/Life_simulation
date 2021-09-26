using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

class MapController
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

    public MapController(int rows, int cols, int density1, int density2, int resolution, PictureBox pictureBox, Graphics graphics)

    {
        this.pictureBox = pictureBox;
        this.resolution = resolution;
        this.rows = rows;
        this.cols = cols;
        densityAnimals = density1;
        densityPlants = density2;

        field = GetField();
        rendering = new Rendering(this.cols, this.rows, this.resolution, field, this.pictureBox, graphics);
    }

    private int[,] GetField()
    {
        int[,] tmpField = new int[cols, rows];
        Random random = new Random();
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (random.Next(densityAnimals) == 0)
                {
                    tmpField[x, y] = 1;

                    listOfAnimals.Add(new Animal((x, y)));

                }
                else if (random.Next(densityPlants) == 0)
                {
                    tmpField[x, y] = 2;
                    listOfPlants.Add(new Plant((x, y)));
                }
            }
        }
        return tmpField;
    }

    public void DrawFirstGeneration()
    {
        rendering.DrawGeneration();

    }

    public void DrawAllChanges()
    {
        moveToRight();
    }

    private void moveToRight()
    {
        foreach (Animal an in listOfAnimals)
        {
            if (an.GetPosition().Item1 <= pictureBox.Width/resolution - 1)
            {
                rendering.removeFromField(an.GetPosition());
                an.SetPosition((an.GetPosition().Item1 + 1, an.GetPosition().Item2));
                rendering.Draw(an.GetPosition());
            }
            else
            {
                rendering.removeFromField(an.GetPosition());
                listOfDies.Add(an);
            }
        }
        foreach (Animal an in listOfDies)
        {
            listOfAnimals.Remove(an);
        }
    }

    private void CheckAll()
    {

    }

}
