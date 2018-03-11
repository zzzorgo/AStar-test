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
                            State sameState = stack.Select(s => s).Where(s => s == state).SingleOrDefault();
                            if (sameState != null && state.Value < sameState.Value)
                            {
                                sameState.Parent = state.Parent;
                                sameState.Value = state.Value;
                            }
                            else if (sameState == null)
                            {
                                stack.Push(state);
                            }
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
                        targetList[i * yFieldSize + j] = null;
                    }
                    else if (!target[i, j] || (candidate[i, j] && target[i, j]))
                    {
                        targetList[i * yFieldSize + j] = null;
                    }
                }
            }

            for (int i = 0; i < wrongCellCoordinates.Count; i++)
            {
                double min = Double.MaxValue;
                int minIndex = 0;
                int flatIndex = wrongCellCoordinates[i].Y * xFieldSize + wrongCellCoordinates[i].X;
                for (int j = 0; j < targetList.Count; j++)
                {
                    if (targetList[j] != null && distances[flatIndex][j] < min)
                    {
                        min = distances[flatIndex][j];
                        minIndex = j;
                    }
                }

                targetList[minIndex] = null;
                result += distances[flatIndex][minIndex];
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
