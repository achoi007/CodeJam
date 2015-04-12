using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfiniteHouseOfPancakes;

namespace InfiniteHouseOfPancakesTest
{
    public abstract class SolverTest
    {
        private ISolver mSolver;

        private List<int> CreateList(string str)
        {
            return str.Split(' ').Select(s => int.Parse(s)).ToList();
        }

        protected abstract ISolver CreateSolver();

        [TestInitialize]
        public void SetUp()
        {
            mSolver = CreateSolver();
        }

        [TestMethod]
        public void SimpleTest()
        {
            Assert.AreEqual(3, mSolver.Solve(CreateList("3")));
            Assert.AreEqual(2, mSolver.Solve(CreateList("1 2 1 2")));
            Assert.AreEqual(3, mSolver.Solve(CreateList("4")));
        }
    }

    [TestClass]
    public class BruteSolverTest : SolverTest
    {
        protected override ISolver CreateSolver()
        {
            return new BruteSolver();
        }
    }

}
