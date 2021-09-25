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
    private List<Plant> listOfPlants = new List<Plant>();
    int newRows;
    int newCols;
    int densityAnimals;
    int densityPlants;

    private int[,] newField;

    public MapController(int rows, int cols, int density1, int density2)
    {
        newRows = rows;
        newCols = cols;
        densityAnimals = density1;
        densityPlants = density2;

        newField = new int[newRows, newCols];

    }

    public int[,] GetField()
    {
        Random random = new Random();
        for (int x = 0; x < newCols; x++)
        {
            for (int y = 0; y < newRows; y++)
            {
                if (random.Next(densityAnimals) == 0)
                {
                    newField[x, y] = 1;
                    listOfAnimals.Add(new Animal((x,y)));
                }
                else if (random.Next(densityPlants) == 0)
                {
                    newField[x, y] = 2;
                    listOfPlants.Add(new Plant((x, y)));
                }
            }
        }
        return newField;
    }

}
