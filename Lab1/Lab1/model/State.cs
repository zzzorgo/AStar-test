using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model
{
    public class Cursor
    {
        public Cursor (int x, int y)
        {
            X0 = x;
            Y0 = y;
            X1 = x + 1;
            Y1 = y + 1;
        }

        public int X0 { get; }
        public int Y0 { get; }
        public int X1 { get; }
        public int Y1 { get; }
    }

    public class State : IEnumerable<bool>, IComparable
    {
        bool[,] state;

        protected int xSize;
        protected int ySize;

        List<State> children;
        public List<State> Children {
            get
            {
                if (this.children != null) { return this.children; }

                List<State> children = new List<State>();

                // проходимся по всем возможным положениям курсора
                for (int i = 0; i < xSize - 1; i++)
                {
                    for (int j = 0; j < ySize - 1; j++)
                    {
                        Cursor cursor = new Cursor(i, j);
                        children.Add(RotateClockWise(cursor));
                        children.Add(RotateCounterClockWise(cursor));
                    }
                }

                this.children = children;
                return children;
            }
        }

        public int Value { get; set; }
        public State Parent { get; set; }

        public State(bool[,] state)
        {
            this.state = state;
            xSize = state.GetLength(0);
            ySize = state.GetLength(1);
        }

        public bool this[int i, int j]
        {
            get { return state[i, j]; }
        }

        public State RotateCounterClockWise(Cursor cursor)
        {
            bool[,] newRawState = (bool[,])state.Clone();

            int x0 = cursor.X0;
            int x1 = cursor.X1;
            int y0 = cursor.Y0;
            int y1 = cursor.Y1;

            newRawState[x0, y0] = state[x0, y1];
            newRawState[x1, y0] = state[x0, y0];
            newRawState[x1, y1] = state[x1, y0];
            newRawState[x0, y1] = state[x1, y1];

            return new State(newRawState);
        }

        public State RotateClockWise(Cursor cursor)
        {
            bool[,] newRawState = (bool[,])state.Clone();

            int x0 = cursor.X0;
            int x1 = cursor.X1;
            int y0 = cursor.Y0;
            int y1 = cursor.Y1;

            newRawState[x0, y0] = state[x1, y0];
            newRawState[x1, y0] = state[x1, y1];
            newRawState[x1, y1] = state[x0, y1];
            newRawState[x0, y1] = state[x0, y0];

            return new State(newRawState);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) { return false; }
            else
            {
                for (int i = 0; i < xSize; i++)
                {
                    for (int j = 0; j < ySize; j++)
                    {
                        bool cell1 = state[i, j];
                        bool cell2 = ((State)obj)[i, j];
                        if (cell1 != cell2) { return false; }
                    }
                }
                return true;
            }
        }

        public override string ToString()
        {
            return state.Cast<bool>()
                .Aggregate(new StringBuilder(), (acc, next) => { return acc.Append(next.ToString()); })
                .ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return state.Cast<bool>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return state.GetEnumerator();
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(((State)obj).Value);
        }

        public static bool operator == (State x, State y)
        {
            if (ReferenceEquals(x, y)) { return true; }
            return x.Equals(y);
        }
        public static bool operator != (State x, State y)
        {
            if (ReferenceEquals(x, y)) { return false; }
            return !x.Equals(y);
        }
    }
}
