using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public class Beatmap : MetadataObject
    {
        private List<string> fileData;
        private string filePath;     

        public Beatmap(List<string> data, string path)
        {
            FileData = data;
            FilePath = path;
        }

        public List<string> FileData
        {
            get { return fileData; }
            set { fileData = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }       
    }
}
