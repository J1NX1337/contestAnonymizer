using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public static class BeatmapParser
    {
        private static BeatmapData beatmapList = new BeatmapData();

        public static void ReadFiles(List<string> filePathList) //reads the actual beatmap file contents and creates new objects
        {
            List<string> fileLineList = new List<string>();
            foreach (string s in filePathList)
            {
                beatmapList.Add(new Beatmap(File.ReadAllLines(s).ToList(), s)); //reading file lines, casting string array to string list
                //also creating new beatmap object, each file path corresponds to a specific beatmap... then appending them to a static list
            }
        }

        public static void ParseFiles() //this part of the code parses all imported beatmaps to get their metadata information + filename
        {
            foreach (Beatmap b in beatmapList)
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

        public static BeatmapData BeatmapList
        {
            get
            {
                return beatmapList;
            }
            set
            {
                beatmapList = value;
            }
        }

        public static void ClearBeatmaps() //clears all beatmaps in the list
        {
            beatmapList.Clear();
        }

        public static void ClearBeatmaps(IList<Beatmap> il) //clears beatmaps based on an item list from the datagrid
        {
            foreach (Beatmap b in il)
            {
                beatmapList.Remove(b);
            }
        }
    }
}
