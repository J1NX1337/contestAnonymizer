using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.ComponentModel;
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
        private string defaultBMDirectory;
        private Brush empty;
        private Brush match;
        private Brush mismatch;
        private TextBox[] textBoxArray;

        public MainWindow()
        {
            InitializeComponent();
            BeatmapParser.BeatmapList = new BeatmapData();
            mapGrid.ItemsSource = BeatmapParser.BeatmapList;
            defaultBMDirectory = Directory.GetCurrentDirectory() + "\\Test Files";
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                //retrieving adjectives and nouns from their respective folders, and adding them to comboboxes
                (Directory.GetFiles("Adjectives", "*.txt").Select(System.IO.Path.GetFileName).ToList()).ForEach(x => adjBox.Items.Add(x));
                (Directory.GetFiles("Nouns", "*.txt").Select(System.IO.Path.GetFileName).ToList()).ForEach(x => nounBox.Items.Add(x));

                match = (Brush)(new BrushConverter().ConvertFrom("#8ee222")); //initializing discrepancy match and mismatch colors
                mismatch = (Brush)(new BrushConverter().ConvertFrom("#e2bc22"));
                empty = (Brush)(new BrushConverter().ConvertFrom("#FFFFFF"));
                textBoxArray = new TextBox[8] { //initializing array for metadata textboxes
                    titleBox,
                    titleUnicodeBox,
                    artistBox,
                    artistUnicodeBox,
                    creatorBox,
                    versionBox,
                    sourceBox,
                    tagsBox };          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void beatmapButton_Click(object sender, RoutedEventArgs e) //lets user select multiple beatmaps to anonymize
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                if (Directory.Exists(defaultBMDirectory))
                    openFileDialog.InitialDirectory = defaultBMDirectory;
                openFileDialog.Filter = ".osu files (*.osu)|*.osu|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    List<string> filePathList = new List<string>();

                    foreach (string filename in openFileDialog.FileNames)
                        filePathList.Add(filename);

                    if (e.Source == selectButton) //if select button, replaces the current list of beatmaps, if not, adds them to the list
                        BeatmapParser.ClearBeatmaps();

                    BeatmapParser.ReadFiles(filePathList);
                    BeatmapParser.ParseFiles();

                    foreach (Beatmap b in BeatmapParser.BeatmapList)
                    {
                        b.PropertyChanged += Beatmap_OnPropertyChanged;
                    }

                    HighlightDiscrepancies();

                    foreach (DataGridColumn dgc in mapGrid.Columns) //resizes columns so that they won't take more space than they need to accommodate the longest datastring
                    {
                        dgc.Width = 0;
                        dgc.Width = DataGridLength.Auto;
                    }
                }
                mapGrid.Focus(); //directing focus out of the button so it stops blinking
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

        public void HighlightDiscrepancies() //changes textbox colors based on found discrepancies
        {
            if (BeatmapParser.BeatmapList.Count > 0)
            {
                bool[] boolArray = BeatmapParser.FindDiscrepancies();

                for (int i = 0; i < 8; i++) //values inited in window initializer
                {
                    if (boolArray[i])
                        textBoxArray[i].Background = match;
                    else
                        textBoxArray[i].Background = mismatch;
                }
            }
            else //if there are no beatmaps left selected (after pressing remove button), makes the textboxes white again
            {
                foreach (TextBox tb in textBoxArray)
                {
                    tb.Background = empty;
                }
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e) //removing selected beatmap from beatmaplist and grid
        {
            try
            {
                BeatmapParser.BeatmapList.Remove((Beatmap)mapGrid.SelectedItem);
                HighlightDiscrepancies();
                mapGrid.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }      

        private void anonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BeatmapParser.BeatmapList.Count == 0) //if no .osu files are selected, doesn't commence anonymization
                    MessageBox.Show("Select at least one or more .osu files to anonymize.");
                else if (adjBox.SelectionBoxItem.ToString() == "")               
                    MessageBox.Show("Select list of adjectives for anonymization.");                
                else if (nounBox.SelectionBoxItem.ToString() == "")               
                    MessageBox.Show("Select list of nouns for anonymization.");
                else
                {
                    string keyDirectory;
                    int anonCount = BeatmapParser.BeatmapList.Count; //generating anonymized version metadata based on amount of selected maps
                    List<string> keyList = Anonymizer.GenerateKey(adjBox.SelectionBoxItem.ToString(), nounBox.SelectionBoxItem.ToString(), anonCount, out keyDirectory);
                    for (int i = 0; i < BeatmapParser.BeatmapList.Count; i++)
                    {
                        BeatmapParser.BeatmapList[i].Version = keyList[i];
                    }
                    
                    BeatmapParser.WriteFiles(); //writing anonymized copies of .osu files into Output
                    mapGrid.Items.Refresh(); //refreshing selected files' metadata in the view after anonymization
                    HighlightDiscrepancies(); //checking for discrepancies again
                    MessageBox.Show(string.Format("{0} entries anonymized successfully. Key can be found in {1}.\nAnonymized entries can be found in the Output folder inside the program folder.", anonCount, keyDirectory));
                }
            }
            catch (IndexOutOfRangeException iex)
            {
                MessageBox.Show(iex.Message);
            }
            catch (ArgumentNullException anex)
            {
                MessageBox.Show(anex.Message);
            }
            catch (AmbiguousMatchException amex)
            {
                MessageBox.Show(amex.Message);
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

        private void extraWordsButton_Click(object sender, RoutedEventArgs e) //event handler for choosing custom nouns and adjectives
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string wordPath = ((Button)sender).Tag.ToString(); //retrieving a tag given to both noun and adjective buttons so determine which one has been pressed
                openFileDialog.Filter = ".txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\" + wordPath + @"\";
                if (openFileDialog.ShowDialog() == true)
                {
                    //will add a file to the noun/adjective box by copying the selected file to the word folder
                    string filepath = openFileDialog.FileName;
                    string filename = System.IO.Path.GetFileName(filepath);
                    if (!File.Exists(wordPath + @"\" + filename) || MessageBox.Show("This will overwrite " + filename + " in the " + wordPath + " folder. Continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {   //if the word folder already has the file, it will ask for overwrite confirmation  

                        File.Copy(filepath, wordPath + @"\" + filename, true);
                        if (e.Source == extraNounsButton)
                        {
                            if (!nounBox.Items.Contains(filename))
                                nounBox.Items.Add(filename); //adding word to combobox if it doesn't already contain it
                            nounBox.SelectedItem = filename; //selecting the .txt file that was copied over in the combobox
                        }
                        else if (e.Source == extraAdjButton)
                        {
                            if (!adjBox.Items.Contains(filename))
                                adjBox.Items.Add(filename);
                            adjBox.SelectedItem = filename;
                        }
                    }

                }
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

        private void mapGrid_Loaded(object sender, RoutedEventArgs e) //collapsing the first two columns that aren't necessary for the user to see
        {
            mapGrid.Columns[0].Visibility = Visibility.Collapsed;
            mapGrid.Columns[1].Visibility = Visibility.Collapsed;
        }

        private void Beatmap_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HighlightDiscrepancies(); //rechecks for discrepancies if a Beatmap property value change is detected
        }
    }
}
