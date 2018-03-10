using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model.Game
{
    class AStarGame : Game
    {
        int FROM_PARENT_TO_CHILD_VALUE = 1;
        List<State> pathList = new List<State>();

        public AStarGame(int xFieldSize, int yFieldSize, GamePreset gamePreset) : base(xFieldSize, yFieldSize, gamePreset)
        {
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
            int i = 0, j = 0;
            foreach (bool cell1 in candidate)
            {
                if (cell1)
                {
                    foreach (bool cell2 in target)
                    {
                        if (cell2)
                        {
                            break;
                        }
                        j++;
                    }

                    int x1 = i / xFieldSize;
                    int y1 = i - x1 * xFieldSize;

                    int x2 = j / xFieldSize;
                    int y2 = j - x2 * xFieldSize;

                    result += Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
                }
                i++;
            }

            return result;

            //for (int x1 = 0, x2 = 0; x1 < xFieldSize; x1++)
            //{
            //    for (int y1 = 0, y2 = 0; y1 < yFieldSize; y1++)
            //    {
            //        if (candidate[x1, y1])
            //        {
            //            for (; x2 < xFieldSize; x2++)
            //            {
            //                for (; y2 < yFieldSize; y2++)
            //                {
            //                    if (target[x2, y2])
            //                    {
            //                        y2++;
            //                        break;
            //                    }
            //                }
            //                if (y2 >= yFieldSize || target[x2, y2])
            //                {
            //                    x2++;
            //                    break;
            //                }
            //            }
            //            result += Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            //        }
            //    }
            //}
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
