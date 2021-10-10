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


    public void DrawFirstGeneration(MapObject mapObject, int x, int y)
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
            
            case MapObject.inediblePlantHealthy:
                _graphics.FillEllipse(Brushes.Tomato, x * _resolution, y * _resolution, _resolution, _resolution);
                break;
            case MapObject.inediblePlantPoisonous:
                _graphics.FillEllipse(Brushes.Tan, x * _resolution, y * _resolution, _resolution, _resolution);
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
                    switch (plant.Stage)
                    {
                        case PlantStage.seed:
                            if (plant.IsHealthy())
                                _graphics.FillEllipse(Brushes.Lime, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillEllipse(Brushes.Beige, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.sprout:
                            if (plant.IsHealthy())
                                _graphics.FillRectangle(Brushes.Lime, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillRectangle(Brushes.Beige, x * _resolution, y * _resolution, _resolution, _resolution);

                            break;
                        case PlantStage.grown:
                            if (plant.IsHealthy())
                            {
                                if (plant.IsFruiting())
                                {
                                    _graphics.FillRectangle(Brushes.Green, x * _resolution, y * _resolution, _resolution, _resolution);
                                }
                                else
                                {
                                    _graphics.FillRectangle(Brushes.MediumSpringGreen, x * _resolution, y * _resolution, _resolution, _resolution);

                                }
                            }
                            else
                                 if (plant.IsFruiting())
                            {
                                _graphics.FillRectangle(Brushes.White, x * _resolution, y * _resolution, _resolution, _resolution);
                            }
                            else
                            {
                                _graphics.FillRectangle(Brushes.LightGoldenrodYellow, x * _resolution, y * _resolution, _resolution, _resolution);

                            }

                            break;
                        case PlantStage.dead:
                            if (plant.IsHealthy())
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
                            if (plant.IsHealthy())
                                _graphics.FillEllipse(Brushes.Tomato, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillEllipse(Brushes.Tan, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.sprout:
                            if (plant.IsHealthy())
                                _graphics.FillRectangle(Brushes.Tomato, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillRectangle(Brushes.Tan, x * _resolution, y * _resolution, _resolution, _resolution);
                            break;
                        case PlantStage.grown:
                            if (plant.IsHealthy())
                            {
                                if (plant.IsFruiting())
                                {
                                    _graphics.FillRectangle(Brushes.Red, x * _resolution, y * _resolution, _resolution, _resolution);
                                }
                                else
                                {
                                    _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);

                                }
                            }
                            else
                            {
                                if (plant.IsFruiting())
                                {
                                    _graphics.FillRectangle(Brushes.LightCoral, x * _resolution, y * _resolution, _resolution, _resolution);
                                }
                                else
                                {
                                    _graphics.FillRectangle(Brushes.Sienna, x * _resolution, y * _resolution, _resolution, _resolution);

                                }
                            }

                            break;
                        case PlantStage.dead:
                            if (plant.IsHealthy())
                                _graphics.FillRectangle(Brushes.Maroon, x * _resolution, y * _resolution, _resolution, _resolution);
                            else
                                _graphics.FillRectangle(Brushes.SaddleBrown, x * _resolution, y * _resolution, _resolution, _resolution);

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


