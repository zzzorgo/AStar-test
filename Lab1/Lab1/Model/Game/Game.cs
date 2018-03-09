using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model.Game
{
    public class NextStateEventArgs : EventArgs
    {
        public NextStateEventArgs(State state)
        {
            State = state;
        }

        public State State { get; set; }
    }

    public class Game
    {
        public event EventHandler<NextStateEventArgs> NextStateReady;

        int xFieldSize;
        int yFieldSize;

        State initial;
        State target;
        State current;

        public State Current { get { return current; } }
        public State Target { get { return target; } }

        public Game(int xFieldSize, int yFieldSize)
        {
            this.xFieldSize = xFieldSize;
            this.yFieldSize = yFieldSize;

            var rawStates = Input.CreateGameStates(xFieldSize, yFieldSize);
            bool[,] rawInitial = rawStates.Item1;
            bool[,] rawTarget = rawStates.Item2;

            initial = new State(rawInitial);
            target = new State(rawTarget);
            current = new State(rawInitial);
        }

        public Stack<State> Start()
        {
            State result = new State(new bool[xFieldSize, yFieldSize]);
            Stack<State> stack = new Stack<State>();
            stack.Push(initial);
            
            StackPath path = new StackPath();

            for (int i = 0; stack.Count != 0; i++)
            {
                State nextState = stack.Pop();
                NextStateReady(this, new NextStateEventArgs(nextState));
                if (nextState == target)
                {
                    break;
                }   
                else
                {
                    path.Push(nextState);
                    foreach (State state in nextState.Children)
                    {
                        if (!path.Contains(state))
                        {
                            stack.Push(state);
                        }
                    }
                }
            }

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
    }
}
