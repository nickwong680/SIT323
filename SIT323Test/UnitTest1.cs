using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIT323;
using SIT323.Models;

namespace SIT323Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Wordlist wordlist = new Wordlist("Files/Names EASY wordlist.csv");
            Assert.IsTrue(true);
        }
    }
}
