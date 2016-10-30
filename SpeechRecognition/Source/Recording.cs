using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SpeechRecognition.Source
{
    public class Recording : IRecording
    {
        private string audioFilename = string.Empty;
        private string filename = string.Empty;
        private MediaCapture capture = null;
        private InMemoryRandomAccessStream buffer = null;
        private static bool running;
        private StorageFolder storageFolder = null;

        public Recording()
        {
            audioFilename = "audio.wav";
            storageFolder = ApplicationData.Current.LocalFolder;
        }

        #region Event Handlers

        public async Task<bool> CleanOldFiles()
        {
            try
            {
                await CleanOutPreviousFiles();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;   //HACK!
        }

        public async Task<bool> Record()
        {
            try
            {
                await init();
                await capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto), buffer);

                if (running)
                    throw new InvalidOperationException("cannot excute two records at the same time");

                running = true;
            }
            catch (Exception e)
            {
                throw e;
            }

            return true; //HACK!
        }
        public async Task<bool> Stop(CoreDispatcher dispatcher)
        {
            try
            {
                await StopRecording(dispatcher).ConfigureAwait(false);
                byte[] audioData = GetBytes().Result;
                WriteBytesToFile(audioData);
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;   //HACK!
        }
        public async Task<StorageFile> DisplayRecording()
        {
            StorageFile audio = null;

            try
            {
                audio = await GetAudioFile();
            }
            catch (Exception e)
            {
                throw e;
            }

            return audio;
        }

        public async Task<IList<string>> GetCreatedAudioFiles()
        {
            IList<string> audiofiles = new List<string>();

            try
            {
                IReadOnlyList<StorageFile> filesInFolder = await this.storageFolder.GetFilesAsync();
                MediaElement playback = new MediaElement();

                foreach (StorageFile file in filesInFolder)
                {
                    if (file.Path.IndexOf("MyFile") != -1)
                    {
                        audiofiles.Add(file.Name);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return audiofiles;
        }
        
        public async Task<bool> Play(string fileName)
        {
            try
            {
                IReadOnlyList<StorageFile> filesInFolder = await this.storageFolder.GetFilesAsync();
                MediaElement playback = new MediaElement();
                
                foreach (StorageFile file in filesInFolder)
                {
                    if (file.Path.IndexOf(fileName) != -1)
                    {
                        IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                        playback.SetSource(stream, file.FileType);
                        playback.Play();

                        break;  //TODO - handle multiple 
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            //TODO - listen for the playback being done and release the file
            return true;//hack
        }

        #endregion

        #region Private Methods

        private async Task<bool> WriteBytesToFile(byte[] audio)
        {
            int ctr = 0;
            string fileName = "audioData.dat";
            Windows.Storage.StorageFile audioFile = null;

            while (true) //HACK
            {
                while (true) //HACK
                {
                    try
                    {
                        audioFile = await storageFolder.GetFileAsync(fileName);

                        if (audioFile != null)
                        {
                            await audioFile.DeleteAsync();
                        }

                        break;
                    }
                    catch (Exception e)
                    {
                        if (e.Message.IndexOf("The system cannot find the file specified") == -1)
                        {
                            int ignoreForNow = 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                try
                {
                    audioFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                    await FileIO.WriteBytesAsync(audioFile, audio);

                    break;
                }
                catch (Exception e)
                {
                    try  //TODO - Remove inner exception
                    {
                        audioFile = await storageFolder.GetFileAsync(fileName);
                        await audioFile.DeleteAsync();
                    }
                    catch (Exception ex)
                    {
                        int ignoreForNow = 1;
                    }

                    if (ctr > 5)
                    {
                        throw e;
                    }
                    else
                    {
                        ctr++;
                    }
                }
            }

            return true; //HACK
        }

        private async Task<StorageFile> GetAudioFile()
        {
            StorageFile audioFile = null;
            try
            {
                audioFile = await storageFolder.GetFileAsync("audioData.dat");
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return audioFile;
        }

        private async Task<byte[]> GetAudioFileByte()
        {
            byte[] audio = null;
            DataReader reader = null;
            StorageFile audioFile = null;
            IRandomAccessStreamWithContentType stream = null;

            try
            {
                audioFile = await storageFolder.GetFileAsync("audioData.dat");
                stream = await audioFile.OpenReadAsync();
                audio = new byte[stream.Size];
                reader = new DataReader(stream);

                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(audio);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                if (audioFile != null)
                {
                    audioFile = null;
                }

                if (stream != null)
                {
                    stream.Dispose();
                    stream = null;
                }
            }

            return audio;
        }

        private async Task<byte[]> GetBytes()
        {
            byte[] audioData = null;
            try
            {
                //wait until file is done saving
                while (running)
                    await Task.Delay(TimeSpan.FromSeconds(1));

                IReadOnlyList<StorageFile> filesInFolder = await this.storageFolder.GetFilesAsync();
                StorageFile bytesFile = filesInFolder[0];

                IRandomAccessStream stream = await bytesFile.OpenAsync(FileAccessMode.Read);
                
                var reader = new DataReader(stream.GetInputStreamAt(0));
                audioData = new byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(audioData);           
            }
            catch (Exception e)
            {
                throw e;
            }

            return audioData;
        }
        private async Task StopRecording(CoreDispatcher dispatcher)
        {
            try
            {
                StorageFile localStorageFile = null;
                string path = string.Empty;

                if (capture != null)
                {
                    await capture.StopRecordAsync();

                    IRandomAccessStream audio = buffer.CloneStream();
                    if (audio == null)
                        new ArgumentNullException("buffer");

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        localStorageFile = await storageFolder.CreateFileAsync(audioFilename, CreationCollisionOption.GenerateUniqueName);
                        filename = localStorageFile.Name;
                        path = localStorageFile.Path;

                        using (IRandomAccessStream fileStream = await localStorageFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await RandomAccessStream.CopyAndCloseAsync(audio.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                            await audio.FlushAsync();
                            audio.Dispose();
                        }

                        running = false;
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async Task<bool> CleanOutPreviousFiles()
        {
            while (true)  //HACK!
            {
                IReadOnlyList<StorageFile> filesInFolder = await this.storageFolder.GetFilesAsync();

                foreach (StorageFile file in filesInFolder)
                {
                    try
                    {
                        await file.DeleteAsync();
                    }
                    catch (Exception e)
                    {
                        int ignoreForNow = 1;
                    }
                }

                filesInFolder = await this.storageFolder.GetFilesAsync();

                if (filesInFolder.Count <= 0)
                {
                    break;
                }
            }

            return true;  //HACK! 
        }
        private async Task<bool> init()
        {
            try
            {
                await CleanOutPreviousFiles();

                if (buffer != null)
                {
                    buffer.Dispose();
                }
                buffer = new InMemoryRandomAccessStream();
                if (capture != null)
                {
                    capture.Dispose();
                }

                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                };
                capture = new MediaCapture();
                await capture.InitializeAsync(settings);

                capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
                {
                    running = false;
                    throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
                {
                    throw ex.InnerException;
                }
                throw;
            }
            return true;
        }

        #endregion
    }
}
