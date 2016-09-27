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

            if (closestFlatSurfaceArea.Item1.X > marslanderX)
            {
                // we need to go right
                int dist = closestFlatSurfaceArea.Item1.X - marslanderX;
                int ticksToTarget = dist;
                if (hSpeed > 0)
                    ticksToTarget = dist / hSpeed;

                if (hSpeed > ticksToTarget * 4)
                    angle = 45;
                else
                    angle = -45;

                thrust = 4;
            }
            else if (closestFlatSurfaceArea.Item2.X < marslanderX)
            {
                // we need to go left
                int dist = marslanderX - closestFlatSurfaceArea.Item1.X;
                int ticksToTarget = dist;
                if (Math.Abs(hSpeed) > 0)
                    ticksToTarget = dist / Math.Abs(hSpeed);

                if (Math.Abs(hSpeed) > ticksToTarget * 4)
                    angle = -45;
                else
                    angle = 45;

                thrust = 4;
            }
            else
            {
                // we need to get down
                int dist = marslanderY - closestFlatSurfaceArea.Item1.Y;
                int ticksToLand = dist;
                if (vSpeed != 0)
                    ticksToLand = dist / Math.Abs(vSpeed);

                if (Math.Abs(vSpeed) > ticksToLand)
                    thrust = 4;
                else
                    thrust = 0;

                if (hSpeed > 0 &&
                    ticksToLand > 3)
                    angle = 10;
                else if (hSpeed < 0 &&
                         ticksToLand > 3)
                    angle = -10;
                else
                    angle = 0;
            }
            
            Console.WriteLine($"{angle} {thrust}");
        }
    }
}