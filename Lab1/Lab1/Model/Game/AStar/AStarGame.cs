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

        public AStarGame(int xFieldSize, int yFieldSize) : base(xFieldSize, yFieldSize)
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
                ThrowNextStateReady();

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


        int HeuristicEstimation(State candidate)
        {
            int wrongCellCount = 0;
            for (int i = 0; i < xFieldSize; i++)
            {
                for (int j = 0; j < yFieldSize; j++)
                {
                    wrongCellCount = candidate[i, j] == target[i, j] ? wrongCellCount : wrongCellCount + 1;
                }
            }

            return wrongCellCount;
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
