using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public static class BeatmapParser
    {
        public static BeatmapData BeatmapList { get; set; }

        public static void ReadFiles(List<string> filePathList) //reads the actual beatmap file contents and creates new objects
        {
            foreach (string s in filePathList)
            {
                BeatmapList.Add(new Beatmap(File.ReadAllLines(s).ToList(), s)); //reading file lines, casting string array to string list
                //also creating new beatmap object, each file path corresponds to a specific beatmap... then appending them to a static observable collection
            }
        }

        public static void ParseFiles() //this part of the code parses all imported beatmaps to get their metadata information + filename
        {
            foreach (Beatmap b in BeatmapList)
            {
                if (b.FilePath != null) //parsing beatmap filename from path
                {
                    int splitIndex = b.FilePath.LastIndexOf('\\'); //getting the index of the last backslash in the filePath. What follows is the file name.
                    StringBuilder sb = new StringBuilder(b.FilePath);
                    sb.Remove(0, splitIndex + 1); //removing the file path up until the part where the file name starts
                    b.FileName = sb.ToString(); //what remains is the file name, converting back to string and depositing in a property
                }

                bool found = false; //temporary bool to track whether metadata section was found, throws an exception if not

                for (int i = 0; i < b.FileData.Count; i++)
                {
                    if (b.FileData[i] == "[Metadata]")
                    {
                        found = true; //found the metadata section in the file
                        b.Title = (b.FileData[i + 1]).Substring(6); //title is always the first line after the metadata section, same logic for the rest etc.
                        b.TitleUnicode = (b.FileData[i + 2]).Substring(13); //the substring removes the actual metadata identifiers (like 'TitleUnicode:') from the data
                        b.Artist = (b.FileData[i + 3]).Substring(7);
                        b.ArtistUnicode = (b.FileData[i + 4]).Substring(14);
                        b.Creator = (b.FileData[i + 5]).Substring(8);
                        b.Version = (b.FileData[i + 6]).Substring(8);
                        b.Source = (b.FileData[i + 7]).Substring(7);
                        b.Tags = (b.FileData[i + 8]).Substring(5);

                        break;
                    }
                }

                if (!found)
                    throw new IOException("Error! Could not locate Metadata section. Are you sure the file is really a .osu file?");
            }
        }

        public static bool[] FindDiscrepancies() //finds discrepancies between selected beatmap files' metadata
        {
            Beatmap comparison = BeatmapList[0];
            bool[] boolArray = new bool[8] { true, true, true, true, true, true, true, true}; //init all booleans as matching data

            foreach (Beatmap b in BeatmapList) //goes through all selected beatmaps and compares them to the first one
            {
                if (b.Title != comparison.Title)
                    boolArray[0] = false;
                if (b.TitleUnicode != comparison.TitleUnicode)
                    boolArray[1] = false;
                if (b.Artist != comparison.Artist)
                    boolArray[2] = false;
                if (b.ArtistUnicode != comparison.ArtistUnicode)
                    boolArray[3] = false;
                if (b.Creator != comparison.Creator)
                    boolArray[4] = false;
                if (b.Version != comparison.Version)
                    boolArray[5] = false;
                if (b.Source != comparison.Source)
                    boolArray[6] = false;
                if (b.Tags != comparison.Tags)
                    boolArray[7] = false;
            }

            return boolArray;
        }

        public static void WriteFiles() //writes the changed metadata back to the file
        {
            int keyindex = 0; //refers to beatmap index
            foreach (Beatmap b in BeatmapList)
            {
                bool found = false; //temporary bool to track whether metadata section was found, throws an exception if not

                for (int i = 0; i < b.FileData.Count; i++)
                {
                    if (b.FileData[i] == "[Metadata]")
                    {
                        found = true; //found the metadata section in the file
                        b.FileData[i + 1] = string.Format("Title:{0}", b.Title); //title is always the first line after the metadata section, same logic for the rest etc.
                        b.FileData[i + 2] = string.Format("TitleUnicode:{0}", b.TitleUnicode);
                        b.FileData[i + 3] = string.Format("Artist:{0}", b.Artist);
                        b.FileData[i + 4] = string.Format("ArtistUnicode:{0}", b.ArtistUnicode);
                        b.FileData[i + 5] = string.Format("Creator:{0}", b.Creator);
                        b.FileData[i + 6] = string.Format("Version:{0}", b.Version);
                        b.FileData[i + 7] = string.Format("Source:{0}", b.Source);
                        b.FileData[i + 8] = string.Format("Tags:{0}", b.Tags);

                        //writing the file again, attempting to anonymize file name diffname as well
                        int diffnameIndex = b.FileName.IndexOf('[');
                        if (diffnameIndex != -1)
                        {
                            b.FileName = b.FileName.Substring(0, diffnameIndex) + string.Format("[{0}].osu", b.Version);
                        }
                        else //if the .osu file name doesn't contain a diffname section, changing the filename to a general form
                        {
                            b.FileName = string.Format("{0:yyyyMMdd_HHmmss}_Anon_Entry_{1} [{2}].osu", DateTime.Now,keyindex,b.Version);
                        }

                        if(!Directory.Exists("Output"))
                            Directory.CreateDirectory("Output");
                        File.WriteAllLines(string.Format("Output\\{0}",b.FileName), b.FileData);
                        keyindex++;
                        break;
                    }
                }

                if (!found)
                    throw new IOException("Error! Could not locate Metadata section. Are you sure the file is really a .osu file?");
            }          
        }
     
        public static void ClearBeatmaps() //clears all beatmaps in the list
        {
            BeatmapList.Clear();
        }
       
    }
}
