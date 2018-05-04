using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using contestanonymizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace contestanonymizerUnitTest
{
    [TestClass]
    public class BeatmapParserTest
    {        
        List<string> filePathList;

        [TestInitialize]
        public void TestInitialize()
        {
            BeatmapParser.BeatmapList = new BeatmapData();
            filePathList = new List<string>();

        }

        [TestMethod]
        [DeploymentItem("testfile.osu")]
        public void ReadFileDataTest() //makes sure the program reads files correctly
        {
            try
            {              
                filePathList.Add("testfile.osu");
                BeatmapParser.ReadFiles(filePathList);

                List<string> ExpectedFileData = new List<string>();

                ExpectedFileData = new List<string>(File.ReadAllLines("testfile.osu"));
                List<string> ActualFileData = BeatmapParser.BeatmapList[0].FileData;

                for (int i = 0; i < ExpectedFileData.Count; i++)
                {
                    Assert.AreEqual(ExpectedFileData[i], ActualFileData[i]);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        [DeploymentItem("testfile2.osu")]
        [ExpectedException(typeof(IOException), "Error! Could not locate Metadata section. Are you sure the file is really a .osu file?")]
        public void ParseBogusFileTest() //makes sure the program throws an exception if it reads a bogus file with no Metadata section
        {
            filePathList.Add("testfile2.osu");
            BeatmapParser.ReadFiles(filePathList);
            BeatmapParser.ParseFiles();
            Assert.Fail("Did not throw an exception.");
        }

        [TestMethod]
        [DeploymentItem("testfile.osu")]
        [DeploymentItem("testfile3.osu")]       
        public void DiscrepancyDetectionTest() //makes sure discrepancies are detected correctly
        {
            try
            {
                filePathList.Add("testfile.osu");
                filePathList.Add("testfile3.osu");

                BeatmapParser.ReadFiles(filePathList);
                BeatmapParser.ParseFiles();
                bool[] expectedBoolArray = BeatmapParser.FindDiscrepancies();
                Assert.IsFalse(expectedBoolArray[0]);
                Assert.IsFalse(expectedBoolArray[1]);
                Assert.IsFalse(expectedBoolArray[2]);
                Assert.IsTrue(expectedBoolArray[3]);
                Assert.IsFalse(expectedBoolArray[4]);
                Assert.IsFalse(expectedBoolArray[5]);
                Assert.IsTrue(expectedBoolArray[6]);
                Assert.IsFalse(expectedBoolArray[7]);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }
    }
}
