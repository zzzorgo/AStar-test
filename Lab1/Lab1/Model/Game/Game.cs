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

    public abstract class Game
    {
        public event EventHandler<NextStateEventArgs> NextStateReady;

        protected int xFieldSize;
        protected int yFieldSize;

        protected State initial;
        protected State target;
        protected State current;

        public State Current { get { return current; } }
        public State Target { get { return target; } }

        protected PathStack path = new PathStack();
        protected CandidateStack stack = new CandidateStack();

        protected void ThrowNextStateReady()
        {
            NextStateReady(this, new NextStateEventArgs(current));
        }

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

        public abstract Stack<State> Start();
    }
}
