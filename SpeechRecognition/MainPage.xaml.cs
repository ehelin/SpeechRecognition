using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Storage;
using SpeechRecognition.Source;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI;
using BLL;
using Shared.Dto;
using Shared;

namespace SpeechRecognition
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer dispatcherTimer = null;
        private IRecording recording = null;

        public MainPage()
        {
            try
            {
                this.InitializeComponent();

                recording = new Recording();
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();

                ApplicationView.PreferredLaunchViewSize = new Size(1000, 300);
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region private  methods

        private async void DispatcherTimer_Tick(object sender, object e)
        {
            try
            {
                dispatcherTimer.Stop();

                bool isCommand = await IsListenCommand();

                if (isCommand)
                {
                    await RunIteration();
                }

                dispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<bool> IsListenCommand()
        {
            bool isCommand = false;

            try
            {
                this.PageGrid.Children.Clear();

                //TODO - Put back when perceptron is trained!

                //this.tbAudioFileCount.Text = "Recording command...";
                //await ActionPerformed(Constants.ACTION_PERFORMED_RECORD);
                //await Task.Delay(5000);

                //this.tbAudioFileCount.Text = "Stopping";
                //await ActionPerformed(Constants.ACTION_PERFORMED_STOP);
                //await Task.Delay(5000);

                //StorageFile audio = await recording.DisplayRecording();

                isCommand = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return isCommand;  //HACK!
        }
        private async Task<bool> RunIteration()
        {
            try
            {
                this.PageGrid.Children.Clear();

                this.tbAudioFileCount.Text = "Recording";
                await ActionPerformed(Constants.ACTION_PERFORMED_RECORD);
                await Task.Delay(5000);

                this.tbAudioFileCount.Text = "Stopping";
                await ActionPerformed(Constants.ACTION_PERFORMED_STOP);
                await Task.Delay(5000);

                this.tbAudioFileCount.Text = "Creating";
                await ActionPerformed(Constants.ACTION_PERFORMED_DISPLAY_CREATE_FILES);
                await Task.Delay(1000);

                this.tbAudioFileCount.Text = "Playing";
                await ActionPerformed(Constants.ACTION_PERFORMED_PLAY_CREATED_FILES);
                await Task.Delay(1000);
            }
            catch (Exception e)
            {
                throw e;
            }

            return true; //HACK!
        }
        private async Task<bool> ActionPerformed(int actionPerformed)
        {
            try
            {
                if (actionPerformed == Constants.ACTION_PERFORMED_RECORD)
                {
                    this.recording = new Recording();
                }

                switch (actionPerformed)
                {
                    case Constants.ACTION_PERFORMED_RECORD:
                        await recording.Record();
                        break;
                    case Constants.ACTION_PERFORMED_STOP:
                        await recording.Stop(Dispatcher);
                        break;
                    case Constants.ACTION_PERFORMED_DISPLAY_CREATE_FILES:
                        StorageFile audio = await recording.DisplayRecording();
                        BuildAudio(audio);
                        break;
                    case Constants.ACTION_PERFORMED_PLAY_CREATED_FILES:
                        await PlayDirectory();
                        break;
                    default:
                        throw new Exception("Unknown Action Performed - actionPerformed: " + actionPerformed.ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true; //HACK!
        }
        private async Task<bool> PlayDirectory()
        {
            try
            {
                IRecording recording = new Recording();
                IList<string> audioFiles = await recording.GetCreatedAudioFiles();
                ProcessWaveFile pwf = new ProcessWaveFile();
                var files = await pwf.GetReadTimes(audioFiles);

                if (audioFiles != null && audioFiles.Count > 0)
                {
                    foreach (File file in files)
                    {
                        await recording.Play(file.FileName);
                        await Task.Delay(file.PlayTime);
                    }
                }

                await recording.CleanOldFiles();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true; //HACK!!
        }
        private async void BuildAudio(StorageFile audio)
        {
            try
            {
                ProcessWaveFile pwf = new ProcessWaveFile();
                Wave waveFile = await pwf.ProcessWaveOld(audio.Path);
                BuildChart(waveFile);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void BuildChart(Shared.Dto.Wave waveFile)
        {
            this.PageGrid.Children.Clear();

            Windows.UI.Xaml.Shapes.Line line = Utilities.SetLine(10, 10, 92, 208, Colors.Black);
            this.PageGrid.Children.Add(line);

            line = Utilities.SetLine(10, waveFile.PlotLines.Count, 150, 150, Colors.Black);
            this.PageGrid.Children.Add(line);

            this.tbAudioFileCount.Text = waveFile.Samples.Count.ToString() + " audio files";

            int ctr = 0;
            foreach (Shared.Dto.Line lne in waveFile.PlotLines)
            {
                Windows.UI.Xaml.Shapes.Line xamlLine = Utilities.SetLine(lne.lineX1, lne.lineX2, lne.lineY1, lne.lineY2, Colors.Blue);
                this.PageGrid.Children.Add(xamlLine);

                ctr++;
            }
        }

        #endregion
    }
}
