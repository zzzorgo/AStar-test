using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model.Game
{
    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    class AStarGame : Game
    {
        int FROM_PARENT_TO_CHILD_VALUE = 1;
        List<State> pathList = new List<State>();
        List<List<double>> distances;

        public AStarGame(int xFieldSize, int yFieldSize, GamePreset gamePreset) : base(xFieldSize, yFieldSize, gamePreset)
        {
            distances = Input.CalculateFieldDistances(xFieldSize, yFieldSize);
        }

        public override Stack<State> Start()
        {
            initial.Value = HeuristicEstimation(initial);
            stack.Push(initial);

            for (int i = 0; stack.Count != 0; i++)
            {
                current = stack.Min<State>();
                stack.Remove(current);
                ThrowNextStateReady(i);

                if (current == target)
                {
                    break;
                }
                else
                {
                    pathList.Add(current);
                    foreach (State state in current.Children)
                    {
                        state.Value = FROM_PARENT_TO_CHILD_VALUE + pathList.Count * FROM_PARENT_TO_CHILD_VALUE + HeuristicEstimation(state);
                        state.Parent = current;

                        bool alreadyContains = false;

                        for (int j = 0; j < pathList.Count; j++)
                        {
                            if (state == pathList[j])
                            {
                                pathList[j] = state.Value < pathList[j].Value ? state : pathList[j];
                                alreadyContains = true;
                                break;
                            }
                        }

                        if (!alreadyContains)
                        {
                            stack.Push(state);
                        }
                    }
                }
            }

            ReconstructPath();
            Console.WriteLine(path.Count);

            return path;
        }


        double HeuristicEstimation(State candidate)
        {
            double result = 0;
            List<Point> wrongCellCoordinates = new List<Point>();
            List<bool?> targetList = target.Cast<bool?>().ToList();

            for (int i = 0; i < xFieldSize; i++)
            {
                for (int j = 0; j < yFieldSize; j++)
                {
                    if (candidate[i, j] && !target[i, j])
                    {
                        wrongCellCoordinates.Add(new Point(i, j));
                        targetList[j * xFieldSize + i] = null;
                    }
                    else if (!target[i, j] || (candidate[i, j] && target[i, j]))
                    {
                        targetList[j * xFieldSize + i] = null;
                    }
                }
            }

            for (int i = 0, j = 0; i < wrongCellCoordinates.Count; i++)
            {
                while (targetList[j] == null)
                {
                    j++;
                }

                int y1 = wrongCellCoordinates[i].X;
                int x1 = wrongCellCoordinates[i].Y;
                int y2 = j / xFieldSize;
                int x2 = j - y2 * xFieldSize;

                targetList[j] = null;
                result += Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            }

            return result;
        }

        void ReconstructPath()
        {
            PathStack path = new PathStack();
            path.Push(current);

            State parent = current.Parent;
            while(parent != null)
            {
                path.Push(parent);
                parent = parent.Parent;
            }

            this.path = path;
        }
    }
}
