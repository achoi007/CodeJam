using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StandingOvation;

namespace StandingOvationTest
{
    [TestClass]
    public class SolverTest
    {
        Solver mSolver;

        [TestInitialize]
        public void SetUp()
        {
            mSolver = new Solver();
        }

        [TestMethod]
        public void SimpleTest()
        {
            Assert.AreEqual(0, mSolver.Solve(4, "11111"));
            Assert.AreEqual(1, mSolver.Solve(1, "09"));
            Assert.AreEqual(2, mSolver.Solve(5, "110011"));
            Assert.AreEqual(0, mSolver.Solve(0, "1"));
        }
    }
}
