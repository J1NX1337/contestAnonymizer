using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public class MetadataObject
    {
        private string fileName;
        private string title;
        private string titleUnicode;
        private string artist;
        private string artistUnicode;
        private string creator;
        private string version;
        private string source;
        private string tags;

        public MetadataObject()
        {
            
        }

        public MetadataObject(string fn, string title, string tu, string artist, string au, string creator, string version, string source, string tags)
        {
            fileName = fn;
            this.title = title;
            titleUnicode = tu;
            this.artist = artist;
            artistUnicode = au;
            this.creator = creator;
            this.version = version;
            this.source = source;
            this.tags = tags;
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
            }
        }

        public string TitleUnicode
        {
            get { return titleUnicode; }
            set
            {
                titleUnicode = value;
            }
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
            }
        }

        public string ArtistUnicode
        {
            get { return artistUnicode; }
            set
            {
                artistUnicode = value;
            }
        }

        public string Creator
        {
            get { return creator; }
            set
            {
                creator = value;
            }
        }

        public string Version
        {
            get { return version; }
            set
            {
                version = value;
            }
        }

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
            }
        }

        public string Tags
        {
            get { return tags; }
            set
            {
                tags = value;
            }
        }
    }
}
