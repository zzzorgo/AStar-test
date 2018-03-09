using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Model.Game
{
    public class CandidateStack : IEnumerable<State>
    {
        private List<State> items = new List<State>();

        public void Push(State item)
        {
            items.Add(item);
        }

        public State Pop()
        {
            if (items.Count > 0)
            {
                State temp = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return temp;
            }
            else
                return default(State);
        }

        public bool Remove(State item)
        {
            return items.Remove(item);
        }

        public IEnumerator<State> GetEnumerator()
        {
            return ((IEnumerable<State>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<State>)items).GetEnumerator();
        }

        public int Count { get { return items.Count; } }
    }
}
