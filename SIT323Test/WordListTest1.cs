﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIT323;
using SIT323.Models;

namespace SIT323Test
{

    [TestClass]
    public class TestValidator
    {
        readonly int MinWordCount = 10;
        readonly int MaxWordCount = 1000;
        readonly int MinCrozzleWeightCount = 4;
        readonly int MaxCrozzleHeightCount = 400;

        private IntValidator Validator;

        private List<LogMessage> logger;

        [TestInitialize]
        public void InitLogger()
        {
           logger = new List<LogMessage>();
        }

        [TestMethod]
        public void TestStringValidtor()
        {
            var stringvalidtor = new StringValidtor(":-", "word");
            var stringvalidtor2 = new StringValidtor("", "word");
            Assert.IsTrue(stringvalidtor.LogList.Count == 1);
            Assert.IsTrue(stringvalidtor2.LogList.Count == 1);

        }

        [TestMethod]
        public void TestIntValidtor()
        {
            var intvalidtor = new IntValidator("", "field 0");
            var intvalidtor2 = new IntValidator("5", "word").IsInRange(10,1000);
            Assert.IsTrue(intvalidtor.LogList.Count == 1);
            Assert.IsTrue(intvalidtor2.LogList.Count == 1);
        }
    }

    [TestClass]
    public class WordListTest1
    {
        Wordlist Wordlist;
        //Validator Validator = new Validator(new List<LogMessage>());

        readonly int MinWordCount = 10;
        readonly int MaxWordCount = 1000;
        readonly int MinCrozzleWeightCount = 4;
         readonly int MaxCrozzleHeightCount = 400;

        [TestInitialize]
        public void InitWordList()
        {
            Wordlist = new Wordlist("Files/Test 6 - wordlist.csv");
        }

        [TestMethod]
        public void TestCheckWordsCount()
        {
            bool test = (Wordlist.WordsCount > MinWordCount && Wordlist.WordsCount < MaxWordCount);
            Assert.IsTrue(test);
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
