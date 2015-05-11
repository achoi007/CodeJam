using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace P2.Test
{
    [TestClass]
    public class SmartSolverTest
    {
        private SmartSolver mSolver;

        [TestInitialize]
        public void SetUp()
        {
            mSolver = new SmartSolver();
        }

        [TestMethod]
        public void CalcLongestPrefixIdxTest()
        {
            Assert.AreEqual(-1, mSolver.CalcLongestPrefixIdx("abcdef"));
            Assert.AreEqual(1, mSolver.CalcLongestPrefixIdx("aa"));
            Assert.AreEqual(1, mSolver.CalcLongestPrefixIdx("aaa"));
            Assert.AreEqual(4, mSolver.CalcLongestPrefixIdx("aabcaa"));
            Assert.AreEqual(3, mSolver.CalcLongestPrefixIdx("abcabcabc"));
        }

        [TestMethod]
        public void CalcWordProbabilityTest()
        {
            string s = "GOOGLINGER";    // 3 g's, 2 o's, 
            var charDict = s.GroupBy(k => k).Select(g => new { Key = g.Key, Count = g.Count() })
                .ToDictionary(nvp => nvp.Key, nvp => nvp.Count);

            Assert.AreEqual(3, charDict['G']);
            Assert.AreEqual(2, charDict['O']);
            Assert.AreEqual(1, charDict['L']);
            Assert.IsFalse(charDict.ContainsKey('Z'));

            int numKeys = s.Length;
            double delta = 0.001;
            Assert.AreEqual(0, mSolver.CalcWordProbability(charDict, numKeys, "GLAD"), delta);
            Assert.AreEqual(.3, mSolver.CalcWordProbability(charDict, numKeys, "G"), delta);
            Assert.AreEqual(.3 * .2, mSolver.CalcWordProbability(charDict, numKeys, "GO"), delta);
            Assert.AreEqual(.3 * .2 * .1, mSolver.CalcWordProbability(charDict, numKeys, "EGO"), delta);
        }

        [TestMethod]
        public void MaxBananasTest()
        {
            // No repeats
            Assert.AreEqual(0, mSolver.CalcMaxBananas("regal", 3));
            Assert.AreEqual(1, mSolver.CalcMaxBananas("regal", 7));
            Assert.AreEqual(2, mSolver.CalcMaxBananas("regal", 11));
            Assert.AreEqual(2, mSolver.CalcMaxBananas("regal", 14));
            Assert.AreEqual(5, mSolver.CalcMaxBananas("regal", 25));

            // Repeats
            Assert.AreEqual(0, mSolver.CalcMaxBananas("abcab", 3));
            Assert.AreEqual(1, mSolver.CalcMaxBananas("abcab", 7));
            Assert.AreEqual(2, mSolver.CalcMaxBananas("abcab", 8));
            Assert.AreEqual(2, mSolver.CalcMaxBananas("abcab", 9));
            Assert.AreEqual(2, mSolver.CalcMaxBananas("abcab", 10));
            Assert.AreEqual(3, mSolver.CalcMaxBananas("abcab", 11));
            Assert.AreEqual(4, mSolver.CalcMaxBananas("abcab", 14));
        }
    }
}
