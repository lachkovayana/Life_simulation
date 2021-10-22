﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Rendering
    {

        public Rendering()
        {
        }


        public void DrawFirstGeneration(MapObject mapObject, int x, int y)
        {

            switch (mapObject)
            {
                case MapObject.animal:
                    Form1.s_graphics.FillRectangle(Brushes.Gold, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                    break;
                case MapObject.ediblePlantHealthy:
                    Form1.s_graphics.FillEllipse(Brushes.Lime, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                    break;

                case MapObject.ediblePlantPoisonous:
                    Form1.s_graphics.FillEllipse(Brushes.Beige, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                    break;

                case MapObject.inediblePlant:
                    Form1.s_graphics.FillEllipse(Brushes.Tomato, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                    break;

            }
            Form1.s_pictureBox.Refresh();


        }
        public void UpgradeField(List<Animal> listOfAnimals, List<Plant> listOfAllPlants, List<Fruit> listOfFruits)
        {
            Form1.s_graphics.Clear(Color.Black);

            foreach (Plant plant in listOfAllPlants)
            {
                int x = plant.GetPosition().Item1;
                int y = plant.GetPosition().Item2;
                switch (plant)
                {
                    case EdiblePlant plant1:
                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                if (plant1.IsHealthy())
                                    Form1.s_graphics.FillEllipse(Brushes.Lime, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                else
                                    Form1.s_graphics.FillEllipse(Brushes.Beige, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                break;
                            case PlantStage.sprout:
                                if (plant1.IsHealthy())
                                    Form1.s_graphics.FillRectangle(Brushes.Lime, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                else
                                    Form1.s_graphics.FillRectangle(Brushes.Beige, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);

                                break;
                            case PlantStage.grown:
                                if (plant1.IsHealthy())
                                {
                                    if (plant1.IsFruiting())
                                    {
                                        Form1.s_graphics.FillRectangle(Brushes.Green, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                    }
                                    else
                                    {
                                        Form1.s_graphics.FillRectangle(Brushes.MediumSpringGreen, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);

                                    }
                                }
                                else
                                     if (plant1.IsFruiting())
                                {
                                    Form1.s_graphics.FillRectangle(Brushes.White, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                }
                                else
                                {
                                    Form1.s_graphics.FillRectangle(Brushes.LightGoldenrodYellow, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);

                                }

                                break;
                            case PlantStage.dead:
                                if (plant1.IsHealthy())
                                    Form1.s_graphics.FillRectangle(Brushes.DarkOliveGreen, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                else
                                    Form1.s_graphics.FillRectangle(Brushes.Wheat, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);

                                break;
                        }
                        break;

                    case InediblePlant:

                        switch (plant.Stage)
                        {
                            case PlantStage.seed:
                                
                                    Form1.s_graphics.FillEllipse(Brushes.Tomato, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                               break;
                            case PlantStage.sprout:
                               
                                    Form1.s_graphics.FillRectangle(Brushes.Tomato, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                               break;
                            case PlantStage.grown:
                                if (plant.IsFruiting())
                                    {
                                        Form1.s_graphics.FillRectangle(Brushes.Red, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                                    }
                                    else
                                    {
                                        Form1.s_graphics.FillRectangle(Brushes.Crimson, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);

                                    }
                                break;
                            case PlantStage.dead:
                                
                                    Form1.s_graphics.FillRectangle(Brushes.Maroon, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
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
                    Form1.s_graphics.DrawEllipse(new Pen(Color.Chartreuse, 3), x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                }
                else
                {
                    Form1.s_graphics.DrawEllipse(new Pen(Color.BurlyWood, 3), x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
                }


            }
            foreach (Animal animal in listOfAnimals)
            {
                int x = animal.GetPosition().Item1;
                int y = animal.GetPosition().Item2;

                Form1.s_graphics.FillRectangle(Brushes.Gold, x * Form1.s_resolution, y * Form1.s_resolution, Form1.s_resolution, Form1.s_resolution);
            }
            Form1.s_pictureBox.Refresh();

        }
    }
}