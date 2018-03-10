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
        public NextStateEventArgs(State state, int iterations)
        {
            State = state;
            Iterations = iterations;
        }

        public State State { get; set; }
        public int Iterations { get; set; }
    }

    public class GamePreset
    {
        public GamePreset(int xFieldSize, int yFieldSize)
        { 
            var rawStates = Input.CreateGameStates(xFieldSize, yFieldSize);
            bool[,] rawInitial = rawStates.Item1;
            bool[,] rawTarget = rawStates.Item2;

            Initial = new State(rawInitial);
            Target = new State(rawTarget);
            Current = Initial;
        }

        public GamePreset(State initial, State current, State target)
        {
            Initial = initial;
            Current = current;
            Target = target;
        }

        public State Initial { get; }
        public State Current { get; }
        public State Target { get; }
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
        public State Target { get { return target; } set { target = value; } }

        protected PathStack path = new PathStack();
        protected CandidateStack stack = new CandidateStack();

        protected void ThrowNextStateReady(int iterations)
        {
            NextStateReady(this, new NextStateEventArgs(current, iterations));
        }

        public Game(int xFieldSize, int yFieldSize, GamePreset gamePreset)
        {
            this.xFieldSize = xFieldSize;
            this.yFieldSize = yFieldSize;

            initial = gamePreset.Initial;
            target = gamePreset.Target;
            current = initial;
        }

        public abstract Stack<State> Start();
    }
}
