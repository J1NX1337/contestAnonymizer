using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public static class Anonymizer
    {
        static StreamReader _textStreamReader;

        public static List<string> GenerateKey(Assembly _assembly, string resourceFile)
        {
            _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream(string.Format("contestanonymizer.{0}", resourceFile)));
            List<string> nounList = new List<string>();

            if (_textStreamReader.Peek() != -1)
            {
                //textBox1.Text = _textStreamReader.ReadLine();
            }

            return nounList;
        }
    }
}
