using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkWithCSV
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CSVfile> csvfile = new List<CSVfile>();
        List<names> name = new List<names>();

        string PathFile = @"";
        string PathLoad = @"";

        Task prog;
        Task prog2;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV File (*.csv) |*.csv";
            if(fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textLoadPath.Text = fileDialog.FileName;
                PathLoad = fileDialog.FileName;
            }
        }

        private void savePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if(folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textSavePath.Text = folderDialog.SelectedPath;
                PathFile = folderDialog.SelectedPath;
            }
        }

        private void startJob_Click(object sender, RoutedEventArgs e)
        {
            loadingText.Visibility = Visibility.Visible;
            if (textLoadPath.Text == "" || textSavePath.Text == "")
            {
                System.Windows.MessageBox.Show("Choose a path!");
            }else
            {
                prog = new Task(processingFile);
                prog2 = new Task(loading);
                loadNames();
                prog.Start();
                prog2.Start();
            }
        }

        private void loading()
        {
            while(true)
            {
                if(prog.IsCompleted == false)
                {
                    Dispatcher.Invoke(() => loadingText.Text = "Traitement.");
                    Thread.Sleep(1000);
                    Dispatcher.Invoke(() => loadingText.Text = "Traitement..");
                    Thread.Sleep(1000);
                    Dispatcher.Invoke(() => loadingText.Text = "Traitement...");
                    Thread.Sleep(1000);
                }else
                {
                    Dispatcher.Invoke(() => loadingText.Text = "Terminé");
                    break;
                }
            }
            
        }

        private void loadNames()
        {
            string line;
            using (StreamReader sr = new StreamReader("./files/names.csv"))
            {
                while((line = sr.ReadLine()) != null)
                {
                    string[] words= line.Split(';');
                    names name = new names(words[0], words[1]);
                    this.name.Add(name);
                }
            }
        }

        private void processingFile()
        {
            try
            {
                string line;
                using (StreamReader sr = new StreamReader(PathLoad))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        string[] words = line.Split(';');
                        if(words[0] != "VarName")
                        {
                            double timeStart = Convert.ToDouble(words[4]) % 1000000;
                            timeStart = timeStart / 1000000;
                            int hour = (int)(timeStart * 24);

                            timeStart = timeStart * 24;
                            timeStart = Math.Round(timeStart % 1, 6);

                            int min = (int)(timeStart * 60);

                            timeStart = timeStart * 60;
                            timeStart = Math.Round(timeStart % 1, 6);

                            int sec = (int)(timeStart * 60);

                            for(int i=0;i<name.Count();i++)
                            {
                                if(name[i].oldName == words[0])
                                {
                                    words[0] = name[i].newName;
                                    break;
                                }
                            }
                            CSVfile file = new CSVfile(words[0], words[1], words[2], words[3],
                                words[4], hour.ToString() + ":" + min.ToString() + ":" + sec.ToString());
                            csvfile.Add(file);
                        }
                    }
                }
                string allText = "VarName;TimeString;VarValue;Validity;Time_MS;Time\n";
                for(int i=0;i< csvfile.Count;i++)
                {
                    allText += csvfile[i].VarName + ";" + csvfile[i].TimeString + ";" + csvfile[i].VarValue + ";" +
                        csvfile[i].Validity + ";" + csvfile[i].Time_MS + ";" + csvfile[i].Time + "\n";
                }
                using (StreamWriter sw = new StreamWriter(PathFile+"\\"+DateTime.Now.ToString("yyyy-MM-dd HH-mm") +".csv"))
                {
                    sw.Write(allText);
                }
            }catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Error " + ex);
            }
        }

        private void cancelJob_Click(object sender, RoutedEventArgs e)
        {
            textLoadPath.Text = "";
            textSavePath.Text = "";
        }
    }
}
