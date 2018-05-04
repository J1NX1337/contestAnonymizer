using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using contestanonymizer;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace contestanonymizerUnitTest
{
    [TestClass]
    public class AnonymizerTest
    {
        string keyD;
        [TestInitialize]
        public void TestInitialize()
        {
            BeatmapParser.BeatmapList = new BeatmapData();
            BeatmapParser.BeatmapList.Add(new Beatmap(new List<string>(), ""));
            BeatmapParser.BeatmapList.Add(new Beatmap(new List<string>(), ""));
            BeatmapParser.BeatmapList.Add(new Beatmap(new List<string>(), ""));
            BeatmapParser.BeatmapList.Add(new Beatmap(new List<string>(), ""));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Amount of selected beatmaps is larger than the amount of nouns in the .txt file!")]
        [DeploymentItem("Adjectives\\testfileAnon.txt", "Adjectives\\")]
        [DeploymentItem("Nouns\\testfileAnon2.txt", "Nouns\\")]
        public void GenerateKeyWithInsufficientNounsTest()
        {
            Anonymizer.GenerateKey("testfileAnon.txt", "testfileAnon2.txt", 5, out keyD);
            Assert.Fail("Did not throw exception.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DeploymentItem("Adjectives\\testfileAnon.txt", "Adjectives\\")]
        [DeploymentItem("Nouns\\testfileAnon2.txt", "Nouns\\")]
        public void GenerateKeyWithEmptyWordTest()
        {
            Anonymizer.GenerateKey("testfileAnon.txt", "testfileAnon2.txt", 4, out keyD);
            Assert.Fail("Did not throw exception.");
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        [DeploymentItem("Adjectives\\testfileAnon3.txt", "Adjectives\\")]
        [DeploymentItem("Nouns\\testfileAnon4.txt", "Nouns\\")]
        public void GenerateKeyWithDuplicatesTest()
        {
            List<string> anonlist = Anonymizer.GenerateKey("testfileAnon3.txt", "testfileAnon4.txt", 4, out keyD);
            Assert.Fail("Did not throw exception.");
        }
    }
}
