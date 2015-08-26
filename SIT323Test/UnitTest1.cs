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
            Crozzle crozzle = new Crozzle("Files/Names EASY crozzle.txt");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test2DArray()
        {
            string[,] str = new string[5,10];

            var upb = str.GetUpperBound(0);     //4
            var upbb = str.GetUpperBound(1);    //9
 
            var rank = str.Rank;             //2
            var length = str.Length;        //50


        }
    }
}
