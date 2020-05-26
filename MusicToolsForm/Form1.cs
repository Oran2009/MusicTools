using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using NAudio;
using WaveFormRendererLib;

namespace MusicToolsForm
{
    public partial class Form1 : Form
    {
        List<Panel> listPanel = new List<Panel>();
        List<string> videos = new List<string>();
        string single_audio, single_video, save_location;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listPanel.Add(panel1);
            listPanel[0].BringToFront();
            textBox2.Text = "Imported Audio: ";
            textBox3.Text = "Imported Video: ";
        }

        //SINGLE-IMPORT AUDIO
        private void button1_Click(object sender, EventArgs e)
        {
            //GET AUDIO FILE
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Audio Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "wav",
                Filter = "wav files (*.wav)|*.wav",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                single_audio = openFileDialog1.FileName;
                textBox1.Text += "Opened Audio File Successfully" + "\r\n";
                textBox2.Text = "Imported Audio: " + single_audio;
            }

            if (single_audio != null)
            {
                //DISPLAY WAVEFORM
                var maxPeakProvider = new MaxPeakProvider();
                var rmsPeakProvider = new RmsPeakProvider(200); // e.g. 200
                var samplingPeakProvider = new SamplingPeakProvider(200); // e.g. 200
                var averagePeakProvider = new AveragePeakProvider(4); // e.g. 4

                var myRendererSettings = new StandardWaveFormRendererSettings();
                myRendererSettings.Width = 651;
                myRendererSettings.TopHeight = 24;
                myRendererSettings.BottomHeight = 24;

                var renderer = new WaveFormRenderer();
                var audioFilePath = single_audio;
                var image = renderer.Render(audioFilePath, averagePeakProvider, myRendererSettings);

                pictureBox1.Image = image;
            }
            
        }

        //SINGLE-IMPORT VIDEO
        private void button2_Click(object sender, EventArgs e)
        {
            //GET VIDEO FILE
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Video Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "mp4",
                Filter = "mp4 files (*.mp4)|*.mp4",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                single_video = openFileDialog1.FileName;
                textBox1.Text += "Opened Video File Successfully" + "\r\n";
                textBox3.Text = "Imported Video: " + single_video;
            }

        }

        //SINGLE-SYNC
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //SINGLE-COMBINE
        private void button4_Click(object sender, EventArgs e)
        {
            //Lets user choose folder to save the combined video to
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.Description = "Select a folder to save the combined video to:";
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                save_location = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }

            //Checks if video and audio file is imported, then combines using the following FFMPEG batch script
            //ffmpeg -i VIDEOFILE.mp4 -i AUDIOFILE.wav -map 0:v -map 1:a -c:v copy -c:a aac -b:a 160k RESULT.mp4
            string errormsg = "";
            if (single_video == null)
            {
                errormsg += "Missing video file";
            }
            if (single_audio == null)
            {
                errormsg += "\r\n" + "Missing audio file";
            }
            if (save_location == null)
            {
                errormsg += "\r\n" + "Invalid save location";
            }
            if (errormsg != "")
            {
                MessageBox.Show(errormsg);
            }
            else
            {
                Process combine = new Process();
                combine.StartInfo.FileName = "ffmpeg";
                combine.StartInfo.UseShellExecute = false;
                combine.StartInfo.Arguments = "-i " + single_video + " -i " + single_audio + " -map 0:v -map 1:a -c:v copy -c:a aac -b:a 160k " + save_location + @"\combined.mp4";
                combine.StartInfo.RedirectStandardOutput = true;
                combine.Start();
                textBox1.Text += "Saved to " + save_location + " as combined.mp4";               
            }
            save_location = null;
        }

        //EXTRA BUTTONS

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
