using System;
using System.Collections.Generic;
using System.Drawing;

internal class Player
{
    private static void Main()
    {
        string[] inputs;

        int surfacePointCount = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.

        List<Point> surfacePoints = new List<Point>();
        while (surfacePointCount-- > 0)
        {
            inputs = Console.ReadLine().Split(' ');
            surfacePoints.Add(new Point(int.Parse(inputs[0]), int.Parse(inputs[1])));
        }

        Tuple<Point, Point> closestFlatSurfaceArea = null;

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int marslanderX = int.Parse(inputs[0]);
            int marslanderY = int.Parse(inputs[1]);
            int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

            if (closestFlatSurfaceArea == null)
            {
                // Get all flat surface areas
                List<Tuple<Point, Point>> flatSurfaceAreas = new List<Tuple<Point, Point>>();

                for (int i = 0; i < surfacePoints.Count - 1; i++)
                {
                    if (surfacePoints[i].Y == surfacePoints[i + 1].Y)
                        flatSurfaceAreas.Add(Tuple.Create(surfacePoints[i], surfacePoints[i + 1]));
                }

                // Get closest flat surface area
                int dist = Int32.MaxValue;
                foreach (Tuple<Point, Point> flatSurfaceArea in flatSurfaceAreas)
                {
                    // check if marslander is above a surface area
                    if (flatSurfaceArea.Item1.X < marslanderX &&
                        flatSurfaceArea.Item2.X > marslanderX)
                    {
                        closestFlatSurfaceArea = flatSurfaceArea;
                        break;
                    }

                    if (flatSurfaceArea.Item1.X > marslanderX)
                    {
                        int currentDist = flatSurfaceArea.Item1.X - marslanderX;
                        if (currentDist < dist)
                        {
                            dist = currentDist;
                            closestFlatSurfaceArea = flatSurfaceArea;
                        }
                    }

                    if (flatSurfaceArea.Item2.X < marslanderX)
                    {
                        int currentDist = marslanderX - flatSurfaceArea.Item2.X;
                        if (currentDist < dist)
                        {
                            dist = currentDist;
                            closestFlatSurfaceArea = flatSurfaceArea;
                        }
                    }
                }
            }

            int angle;
            int thrust;

            int leftBoundary = closestFlatSurfaceArea.Item1.X + 250;
            int rightBoundary = closestFlatSurfaceArea.Item2.X - 250;
            int surface = closestFlatSurfaceArea.Item1.Y;

            int distToLeftBoundary = Math.Abs(marslanderX - leftBoundary);
            int distToRightBoundary = Math.Abs(marslanderX - rightBoundary);
            int distToSurface = Math.Abs(marslanderY - surface);

            int ticksToLand = -1;
            if (vSpeed != 0)
                ticksToLand = distToSurface / Math.Abs(vSpeed);

            int ticksToRightBoundary = -1;
            int ticksToLeftBoundary = -1;
            if (Math.Abs(hSpeed) > 0)
            {
                ticksToRightBoundary = distToRightBoundary / Math.Abs(hSpeed);
                ticksToLeftBoundary = distToLeftBoundary / Math.Abs(hSpeed);
            }

            if (closestFlatSurfaceArea.Item1.X > marslanderX)
            {
                // we need to go right
                thrust = 4;

                // if we go down too fast
                if (ticksToLeftBoundary > ticksToLand &&
                    hSpeed > 20)
                    angle = 0;
                else if (ticksToRightBoundary != -1 &&
                         Math.Abs(hSpeed) > ticksToRightBoundary * 2)
                    angle = 45;
                else
                    angle = -45;
            }
            else if (closestFlatSurfaceArea.Item2.X < marslanderX)
            {
                // we need to go left
                thrust = 4;

                // if we go down to fast
                if (ticksToRightBoundary > ticksToLand &&
                    hSpeed < -20)
                    angle = 0;
                else if (ticksToLeftBoundary != -1 &&
                         Math.Abs(hSpeed) > ticksToLeftBoundary * 2)
                    angle = -45;
                else
                    angle = 45;
            }
            else
            {
                // we need to get down
                if (hSpeed > 0 &&
                    (hSpeed > ticksToRightBoundary || hSpeed > 20 || marslanderX > rightBoundary) &&
                    ticksToLand > 3)
                {
                    angle = 45;
                    thrust = 4;
                }
                else if (hSpeed < 0 &&
                         (Math.Abs(hSpeed) > ticksToLeftBoundary || Math.Abs(hSpeed) > 20 || marslanderX < leftBoundary) &&
                         ticksToLand > 3)
                {
                    angle = -45;
                    thrust = 4;
                }
                else
                {
                    angle = 0;
                    if (Math.Abs(vSpeed) > ticksToLand)
                        thrust = 4;
                    else
                        thrust = 3;
                }
            }

            Console.WriteLine($"{angle} {thrust}");
        }
    }
}