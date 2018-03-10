using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model.Game
{
    class DepthSearchGame : Game
    {
        public DepthSearchGame(int xFieldSize, int yFieldSize, GamePreset gamePreset) : base(xFieldSize, yFieldSize, gamePreset)
        {
        }

        public override Stack<State> Start()
        {
            stack.Push(initial);

            for (int i = 0; stack.Count != 0; i++)
            {
                current = stack.Pop();
                ThrowNextStateReady(i);
                if (current == target)
                {
                    break;
                }
                else
                {
                    path.Push(current);
                    foreach (State state in current.Children)
                    {
                        if (!path.Contains(state))
                        {
                            stack.Push(state);
                        }
                    }
                }
            }

            return path;
        }
    }
}
