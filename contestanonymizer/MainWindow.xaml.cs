using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace contestanonymizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Assembly _assembly;

        public MainWindow()
        {
            InitializeComponent();           
            mapGrid.ItemsSource = BeatmapParser.BeatmapList;            
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                _assembly = Assembly.GetExecutingAssembly();

                //foreach (string s in _assembly.)
                //    nounBox.Items.Add(s);
                string[] lineOfContents = _assembly.GetManifestResourceNames();
                foreach (var line in lineOfContents) //retrieving combobox contents from embedded resources
                {
                    if (line.Substring(line.Length - 4, 4) == ".txt" && line.Substring(line.Length - 14, 14) != "adjectives.txt")
                        nounBox.Items.Add(line.Substring(28));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*private void DisplayBeatmaps()
        {

            mapGrid.ItemsSource = BeatmapParser.BeatmapList;
            //takes all metadata from beatmaplist's beatmaps, so that it excludes the filedata and filepath
            /*var metadataObjects = from beatmap in BeatmapParser.BeatmapList
                                  select new
                                  {
                                      beatmap.FileName,
                                      beatmap.Title,
                                      beatmap.TitleUnicode,
                                      beatmap.Artist,
                                      beatmap.ArtistUnicode,
                                      beatmap.Creator,
                                      beatmap.Version,
                                      beatmap.Source,
                                      beatmap.Tags
                                  };*/

        //displaying selected beatmaps' metadata

        //mapGrid.ItemsSource = metadataObjects;
        //}



        private void beatmapButton_Click(object sender, RoutedEventArgs e) //lets user select multiple beatmaps to anonymize
        { 
            try
            {
                if (e.Source == selectButton) //if select button, replaces the current list of beatmaps, if not, adds them to the list
                    BeatmapParser.ClearBeatmaps();

                List<string> filePathList = new List<string>();

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = ".osu files (*.osu)|*.osu|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                        filePathList.Add(filename);                    
                }

                BeatmapParser.ReadFiles(filePathList);
                BeatmapParser.ParseFiles();

                //DisplayBeatmaps();
            }
            catch (IOException ioex)
            {
                MessageBox.Show(ioex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            BeatmapParser.BeatmapList.Remove((Beatmap)mapGrid.SelectedItem);
        }

        private void mapGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void anonButton_Click(object sender, RoutedEventArgs e)
        {
            Anonymizer.GenerateKey(_assembly, nounBox.SelectionBoxItem.ToString());
        }       
    }
}
