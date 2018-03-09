using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    //class Point
    //{
    //    public Point(int x, int y)
    //    {
    //        X = x;
    //        Y = y;
    //    }

    //    public int X { get; set; }
    //    public int Y { get; set; }
    //}

    public class Input
    {
        static Random random = new Random();

        static public Tuple<bool[,], bool[,]> CreateGameStates(int xSize, int ySize)
        {
            int trueCells = 0;
            bool[,] newInitialState = new bool[xSize, ySize];

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    newInitialState[i, j] = random.Next(2) == 0;
                    if (newInitialState[i, j]) { trueCells++; }
                }
            }

            bool[,] newTargetState = (bool[,]) newInitialState.Clone();
            for (int i = 0; i < xSize * ySize; i++)
            {
                int randomX1 = random.Next(0, xSize);
                int randomY1 = random.Next(0, ySize);

                int randomX2 = random.Next(0, xSize);
                int randomY2 = random.Next(0, ySize);

                bool buffer = newTargetState[randomX1, randomY1];
                newTargetState[randomX1, randomY1] = newTargetState[randomX2, randomY2];
                newTargetState[randomX2, randomY2] = buffer;
            }

            return new Tuple<bool[,], bool[,]>(newInitialState, newTargetState);
        }
    }
}
