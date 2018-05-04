using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public class Beatmap : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> fileData;
        private string filePath;
        private string fileName;
        private string title;
        private string titleUnicode;
        private string artist;
        private string artistUnicode;
        private string creator;
        private string version;
        private string source;
        private string tags;

        public Beatmap(List<string> data, string path)
        {
            FileData = data;
            FilePath = path;
        }

        protected void OnPropertyChanged(string s) //lets the view know if any of the beatmap properties were changed
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(s));
            }
        }       

        public List<string> FileData { get => fileData; set => fileData = value; }
        public string FilePath { get => filePath; set => filePath = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string Title { get => title; set { title = value; OnPropertyChanged("Title"); } }
        public string TitleUnicode { get => titleUnicode; set { titleUnicode = value; OnPropertyChanged("TitleUnicode"); } }
        public string Artist { get => artist; set { artist = value; OnPropertyChanged("Artist"); } }
        public string ArtistUnicode { get => artistUnicode; set { artistUnicode = value; OnPropertyChanged("ArtistUnicode"); } }
        public string Creator { get => creator; set { creator = value; OnPropertyChanged("Creator"); } }
        public string Version { get => version; set { version = value; OnPropertyChanged("Version"); } }
        public string Source { get => source; set { source = value; OnPropertyChanged("Source"); } }
        public string Tags { get => tags; set { tags = value; OnPropertyChanged("Tags"); } }       
    }
}
