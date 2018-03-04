using System;
using System.Collections.Generic;
using Lab1.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.model
{
    [TestClass]
    public class StateTest
    {

        bool[,] rawState1 = {
            { true, false, true },
            { false, true, false },
            { true, false, true }
        };

        bool[,] rawState2 = {
            { true, false, true },
            { false, true, false },
            { true, false, true }
        };

        [TestMethod]
        public void EqualsOperatorReturnsTrueForEqualStates()
        {
            State state1 = new State(rawState1);
            State state2 = new State(rawState2);

            Assert.IsTrue(state2 == state1);
        }

        [TestMethod]
        public void EqualsOperatorReturnsFalseForNotEqualStates()
        {
            bool[,] rawState2 = {
                { true, false, true },
                { false, true, false },
                { true, false, false }
            };

            State state1 = new State(rawState1);
            State state2 = new State(rawState2);

            Assert.IsFalse(state2 == state1);
        }

        [TestMethod]
        public void ContainsWithStates()
        {
            Stack<State> stack = new Stack<State>();

            bool[,] rawState3 = {
                { true, false, true },
                { false, false, false },
                { true, false, false }
            };

            State state1 = new State(rawState1);
            State state2 = new State(rawState2);

            stack.Push(state1);
            stack.Push(state2);

            Assert.IsTrue(stack.Contains(new State(rawState1)));
            Assert.IsFalse(stack.Contains(new State(rawState3)));
        }

        [TestMethod]
        public void GetChildren()
        {
            bool[,] rawState1 = {
                { true, false, false },
                { false, false, false },
                { false, false, true }
            };

            bool[,] rawState1_clockWise = {
                { false, true, false },
                { false, false, false },
                { false, false, true }
            };

            bool[,] rawState1_counterClockWise = {
                { false, false, false },
                { true, false, false },
                { false, false, true }
            };

            bool[,] rawState1_anotherCursorPosition = {
                { true, false, false },
                { false, false, false },
                { false, true, false }
            };

            State state = new State(rawState1);
            List<State> children = state.Children;

            Assert.IsTrue(children.Contains(new State(rawState1_clockWise)));
            Assert.IsTrue(children.Contains(new State(rawState1_counterClockWise)));
            Assert.IsTrue(children.Contains(new State(rawState1_anotherCursorPosition)));
        }
    }
}
