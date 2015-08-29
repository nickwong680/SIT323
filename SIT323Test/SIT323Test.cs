using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIT323;
using SIT323.Models;

namespace SIT323Test
{
    public class SIT323Test
    {
        
    }

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
    public class TestWordListValidation
    {
        [TestInitialize]
        public void InitWordList()
        {
        }
        [TestMethod]
        public void TestWordList1()
        {
            var LogList = new Wordlist("Files/Test 1 - wordlist.csv").LogList;
            Assert.IsTrue(LogList.Count == 0);
        }
        [TestMethod]
        public void TestWordList5()
        {
            var LogList = new Wordlist("Files/Test 5 - wordlist.csv").LogList;
            Assert.IsTrue(LogList.Count == 4);
        }
        [TestMethod]
        public void TestWordList6()
        {
            var LogList = new Wordlist("Files/Test 6 - wordlist.csv").LogList;
            Assert.IsTrue(LogList.Count == 6);
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

    [TestClass]
    public class TestCrozzleValidation
    {
        [TestMethod]
        public void TestCrozzle7()
        {
            var Wordlist = new Wordlist("Files/Test 7 - wordlist.csv");
            Assert.IsTrue(Wordlist.LogList.Count == 0);

            var Crozzle = new Crozzle("Files/Test 7 - crozzle.txt", Wordlist);
            Assert.IsTrue(Crozzle.LogList.Count == 2);
        }
        [TestMethod]
        public void TestCrozzle8()
        {
            var Wordlist = new Wordlist("Files/Test 8 - wordlist.csv");
            Assert.IsTrue(Wordlist.LogList.Count == 0);

            var Crozzle = new Crozzle("Files/Test 8 - crozzle.txt", Wordlist);
            Assert.IsTrue(Crozzle.LogList.Count == 7);
        }
    }

    [TestClass]
    public class TestCrozzleConstrainst 
    {
        [TestMethod]
        public void TestConstrainstEasy()
        {

            var wordlist1 = new Wordlist("Files/Test 1 - wordlist.csv");
            Assert.IsTrue(wordlist1.LogList.Count == 0);
            var crozzle1 = new Crozzle("Files/Test 1 - crozzle.txt", wordlist1);
            Assert.IsTrue(crozzle1.LogList.Count == 0);
            var constraints1 = new EasyConstraints(crozzle1, wordlist1);
            Assert.IsTrue(constraints1.LogList.Count == 0);
            var score4 = Score.PointsFactory(constraints1.WordsFromCrozzle, PointScheme.OneEach).TotalScore;


            var wordlist5 = new Wordlist("Files/Test 5 - wordlist.csv");
            Assert.IsTrue(wordlist5.LogList.Count == 4);

            var wordlist9 = new Wordlist("Files/Test 9 - wordlist.csv");
            Assert.IsTrue(wordlist9.LogList.Count == 0);
            var crozzle9 = new Crozzle("Files/Test 9 - crozzle.txt", wordlist9);
            Assert.IsTrue(crozzle9.LogList.Count == 0);
            var constraints9 = new EasyConstraints(crozzle9, wordlist9);
            Assert.IsTrue(constraints9.LogList.Count == 8);

        }
        [TestMethod]
        public void TestConstrainstMedium()
        {
            var wordlist6 = new Wordlist("Files/Test 6 - wordlist.csv");
            Assert.IsTrue(wordlist6.LogList.Count == 7);

            var wordlist10 = new Wordlist("Files/Test 10 - wordlist.csv");
            Assert.IsTrue(wordlist10.LogList.Count == 0);
            var crozzle10 = new Crozzle("Files/Test 10 - crozzle.txt", wordlist10);
            Assert.IsTrue(crozzle10.LogList.Count == 0);
            var constraints10 = new EasyConstraints(crozzle10, wordlist10);
            Assert.IsTrue(constraints10.LogList.Count == 4);

            var wordlist2 = new Wordlist("Files/Test 2 - wordlist.csv");
            Assert.IsTrue(wordlist2.LogList.Count == 0);
            var crozzle2 = new Crozzle("Files/Test 2 - crozzle.txt", wordlist2);
            Assert.IsTrue(crozzle2.LogList.Count == 0);
            var constraints2 = new MediumConstraints(crozzle2, wordlist2);
            Assert.IsTrue(constraints2.LogList.Count == 0);
        }
        [TestMethod]
        public void TestConstrainsHard()
        {
            var wordlist7 = new Wordlist("Files/Test 7 - wordlist.csv");
            Assert.IsTrue(wordlist7.LogList.Count == 0);
            var crozzle7 = new Crozzle("Files/Test 7 - crozzle.txt", wordlist7);
            Assert.IsTrue(crozzle7.LogList.Count == 2);

            var wordlist11 = new Wordlist("Files/Test 11 - wordlist.csv");
            Assert.IsTrue(wordlist11.LogList.Count == 0);
            var crozzle11 = new Crozzle("Files/Test 11 - crozzle.txt", wordlist11);
            Assert.IsTrue(crozzle11.LogList.Count == 0);
            var constraints11 = new HardConstraints(crozzle11, wordlist11);
            Assert.IsTrue(constraints11.LogList.Count == 4);

            var wordlist3 = new Wordlist("Files/Test 3 - wordlist.csv");
            Assert.IsTrue(wordlist3.LogList.Count == 0);
            var crozzle3 = new Crozzle("Files/Test 3 - crozzle.txt", wordlist3);
            Assert.IsTrue(crozzle3.LogList.Count == 0);
            var constraints3 = new HardConstraints(crozzle3, wordlist3);
            Assert.IsTrue(constraints3.LogList.Count == 0);
        }
        [TestMethod]
        public void TestConstrainsExtreme()
        {

            var wordlist12 = new Wordlist("Files/Test 12 - wordlist.csv");
            Assert.IsTrue(wordlist12.LogList.Count == 0);
            var crozzle12 = new Crozzle("Files/Test 12 - crozzle.txt", wordlist12);
            Assert.IsTrue(crozzle12.LogList.Count == 0);
            var constraints12 = new ExtremeConstraints(crozzle12, wordlist12);
            //Assert.IsTrue(constraints12.LogList.Count == 4);


            var wordlist8 = new Wordlist("Files/Test 8 - wordlist.csv");
            Assert.IsTrue(wordlist8.LogList.Count == 0);
            var crozzle8 = new Crozzle("Files/Test 8 - crozzle.txt", wordlist8);
            Assert.IsTrue(crozzle8.LogList.Count == 12);

            var wordlist4 = new Wordlist("Files/Test 4 - wordlist.csv");
            Assert.IsTrue(wordlist4.LogList.Count == 0);
            var crozzle4 = new Crozzle("Files/Test 4 - crozzle.txt", wordlist4);
            Assert.IsTrue(crozzle4.LogList.Count == 0);
            var constraints4 = new ExtremeConstraints(crozzle4, wordlist4);
            Assert.IsTrue(constraints4.LogList.Count == 0);
            var score4 = Score.PointsFactory(constraints4.WordsFromCrozzle, PointScheme.Custom).TotalScore;


        }
        [TestMethod]
        public void Array()
        {
            int width = 9;
            int height = 9;
            for (int i = 0; i < width*height; ++i)
            {
                int x = i%width;
                int y = i/width; //Integer division
            }
        }
    }
}
