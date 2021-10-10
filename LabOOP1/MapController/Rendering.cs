using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

public class Rendering
{
    private Graphics _graphics;
    private int _resolution;
    private int _rows;
    private int _cols;
    private PictureBox _pictureBox;

    public Rendering(int cols, int rows, int resolution, PictureBox pictureBox, Graphics graphics)
    {
        _rows = rows;
        _cols = cols;
        _pictureBox = pictureBox;
        _resolution = resolution;
        _graphics = graphics;
    }

  
    public void Draw(MapObject mapObject, int x, int y)
    {
       
        switch (mapObject)
        {
            case MapObject.animal:
            _graphics.FillRectangle(Brushes.Gray, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
            case MapObject.ediblePlantHealthy:
                _graphics.FillEllipse(Brushes.Lime, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
            case MapObject.ediblePlantPoisonous:
                _graphics.FillEllipse(Brushes.Beige, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
            case MapObject.inediblePlant:
                _graphics.FillEllipse(Brushes.Red, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
        }
        _pictureBox.Refresh();


    }
    public void UpgradeField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
    {
        _graphics.Clear(Color.Black);

        foreach (Plant plant in listOfAllPlants)
        {
            int x = plant.GetPosition().Item1;
            int y = plant.GetPosition().Item2;
            switch (plant)
            {
                case EdiblePlant:
                    EdiblePlant pl = (EdiblePlant)plant;
                    switch (plant.Stage)
                    {
                        case PlantStage.seed:
                            if (pl.IsHealthy() == true)
                                _graphics.FillEllipse(Brushes.Lime, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillEllipse(Brushes.Beige, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.sprout:
                            if (pl.IsHealthy() == true)
                                _graphics.FillRectangle(Brushes.Lime, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillRectangle(Brushes.Beige, x * _resolution, y * _resolution, _resolution, _resolution);

                            break;
                        case PlantStage.grown:
                            if (pl.IsHealthy() == true)
                                _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillRectangle(Brushes.White, x * _resolution, y * _resolution, _resolution, _resolution);

                            break;
                        case PlantStage.dead:
                            if (pl.IsHealthy() == true)
                                _graphics.FillRectangle(Brushes.DarkOliveGreen, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillRectangle(Brushes.Wheat, x * _resolution, y * _resolution, _resolution, _resolution);

                            break;
                    }
                    break;

                case InediblePlant:
                    switch (plant.Stage)
                    {
                        case PlantStage.seed:
                            _graphics.FillEllipse(Brushes.Red, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.sprout:
                            _graphics.FillRectangle(Brushes.Red, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.grown:
                            _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.dead:
                            _graphics.FillRectangle(Brushes.Tomato, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                    }
                    break;
            }

        }
        foreach (Fruit fruit in listOfFruits)
        {
            int x = fruit.GetPosition().Item1;
            int y = fruit.GetPosition().Item2;
            if (fruit.IsHealthy())
            {
                _graphics.DrawEllipse(new Pen(Color.Gold, 3), x * _resolution, y * _resolution, _resolution, _resolution);
            }
            else
            {
                _graphics.DrawEllipse(new Pen(Color.BurlyWood, 3), x * _resolution, y * _resolution, _resolution, _resolution);
            }

            
        }
        foreach (Animal animal in listOfAnimals)
        {
            int x = animal.GetPosition().Item1;
            int y = animal.GetPosition().Item2;

            _graphics.FillRectangle(Brushes.Gray, x * _resolution, y * _resolution, _resolution, _resolution);
        }
        _pictureBox.Refresh();

    }
}


