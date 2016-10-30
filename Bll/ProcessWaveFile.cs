using System;
using NAudio.Wave;
using Shared.Dto;
using System.Collections.Generic;
using Windows.Storage;
using System.Threading.Tasks;

namespace BLL
{
    public class ProcessWaveFile
    {
        private void CloseReader(AudioFileReader reader)
        {
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }
        }

        public async Task<IList<File>> GetReadTimes(IList<string> fileNames)
        {
            IList<File> filePlayTimes = new List<File>();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> filesInFolder = await storageFolder.GetFilesAsync();

            int millisecondfudge = 500;
            foreach (string fileName in fileNames)
            {
                foreach (StorageFile file in filesInFolder)
                {
                    if (file.Path.IndexOf(fileName) != -1)
                    {
                        AudioFileReader reader = new AudioFileReader(file.Path);
                        filePlayTimes.Add(new File
                        {
                            FileName = fileName,
                            PlayTime = (int)reader.TotalTime.TotalMilliseconds + millisecondfudge
                        });
                        CloseReader(reader);
                        break;
                    }
                }
            }

            return filePlayTimes;
        }

        private async Task<bool> CreateWaveFile(byte[] audioArray, string path, string fileName = "MyFile.wav")
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile audioFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            AudioFileReader reader = null;
            WaveFileWriter wfw = null;

            try
            {
                reader = new AudioFileReader(path);
                wfw = new WaveFileWriter(audioFile.Path, reader.WaveFormat);

                wfw.Write(audioArray, 0, audioArray.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (wfw != null)
                {
                    wfw.Dispose();
                    wfw = null;
                }

                CloseReader(reader);
            }

            return true;
        }

        public async Task<Wave> ProcessWaveOld(string path)
        {
            Wave wave = null;

            try
            {
                IList<FloatValue> values = ConvertWavFileToFloatList(path);
                wave = GetAudioSample(values);
            
                int fileCtr = 1;
                foreach (FloatValue fvSample in wave.Samples)
                {
                    float[] sample = new float[fvSample.Values.Count];
                    int ctr = 0;
                    foreach (float value in fvSample.Values)
                    {
                        sample[ctr] = value;
                        ctr++;
                    }

                    await CreateArrayAndFile(sample, path, "MyFile" + fileCtr.ToString() + ".wav");
                    fileCtr++;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return wave;
        }

        private byte[] GetBytes(string path)
        {
            byte[] byteValues = null;
            AudioFileReader reader = null;

            try
            {
                reader = new AudioFileReader(path);
                byteValues = new byte[reader.Length];
                reader.Read(byteValues, 0, byteValues.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CloseReader(reader);
            }

            return byteValues;
        }

        private IList<FloatValue> ConvertWavFileToFloatList(string path)
        {
            IList<FloatValue> values = new List<FloatValue>();

            try
            {
                byte[] byteValues = GetBytes(path);
                int ctr = 0;
                int fltBufCtr = 0;

                while (fltBufCtr < byteValues.Length)
                {
                    if (ctr >= byteValues.Length)
                        break;

                    byte[] floatBuffer = new byte[4];
                    int itvCtr = 0;
                    while (itvCtr < 4)
                    {
                        floatBuffer[itvCtr] = byteValues[ctr];
                        ctr++;
                        itvCtr++;
                    }

                    float curFloatVal = System.BitConverter.ToSingle(floatBuffer, 0);

                    if (curFloatVal != 0)
                        values.Add(new FloatValue(curFloatVal, ctr));

                    fltBufCtr++;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return values;
        }

        //yMax 92
        // positive -  val/x   92/150
        //yMid 150
        // negative -  val/x  150/208
        //yMin 220
        //Windows.UI.Xaml.Shapes.Line xamlLine = Utilities.SetLine(10, 100, 222, 92, Colors.Blue);
        //this.PageGrid.Children.Add(xamlLine);
        private int GetRatio(float val, int yMax, int yMid, int yMin)
        {
            int yValue = 0;

            if (val != 0)
            {
                float tempVal = val * 62;
                tempVal = tempVal / 1;
                tempVal = tempVal * 2;
                yValue = (int)tempVal;
            }
            else
            {
                yValue = 0;
            }
            
            return yValue;
        }

        //figure out how to get the float value mapped to a int to scale
        private Wave GetAudioSample(IList<FloatValue> values)
        {
            int ctr = 0;
            IList<int> thresholdCnt = new List<int>();
            FloatValue sample = new FloatValue();
            Wave wave = new Wave();
            int xStart = 10;
            int mid = 150;
            bool sampleFnd = false;
            int threshold = 145;
            int numNeeded = 10;
            
            foreach (FloatValue value in values)
            {
                if (ctr == 850)
                {
                    int yVal = GetRatio(value.Val, 92, 150, 208);
                    yVal = Math.Abs(yVal);

                    int yValDown = yVal + mid;
                    int yValUp = mid - yVal;

                    Line curLine = new Line(xStart, yValUp, xStart, yValDown, value.Val);
                    wave.PlotLines.Add(curLine);
                    xStart = xStart + 2;

                    if (yValUp < threshold && !sampleFnd)
                    {
                        sampleFnd = true;
                        thresholdCnt.Clear();
                    }
                    else if (sampleFnd && SampleDone(wave.PlotLines, numNeeded))
                    {
                        if (GoodSample(thresholdCnt, threshold, numNeeded))
                        {
                            sampleFnd = false;
                            wave.Samples.Add(sample);
                            sample = new FloatValue();
                            thresholdCnt.Clear();
                        }
                    }

                    thresholdCnt.Add(yValUp);

                    ctr = 0;
                }
                else
                {
                    ctr++;
                }

                sample.Values.Add(value.Val);
            }

            return wave;
        }

        private bool GoodSample(IList<int> thresholdCnt, int yValue, int numNeeded)
        {
            bool good = false;

            int ctr = 0;
            foreach (int threshold in thresholdCnt)
            {
                if (ctr >= numNeeded)
                {
                    good = true;
                    break;
                }
                else if (threshold < yValue)
                {
                    ctr++;
                }
            }
            
            return good;
        }

        private bool SampleDone(IList<Line> plotLines, int numNeeded)
        {
            bool done = false;
            int indexCtr = plotLines.Count - 1;
            int loopCtr = 0;

            while (indexCtr > 0)
            {
                Line curLine = plotLines[indexCtr];

                if (curLine.ToString() != "")
                {
                    break;
                }
                else if (loopCtr > numNeeded)
                {
                    done = true;
                    break;
                }

                loopCtr++;
                indexCtr--;
            }

            return done;
        }
        
        private async Task<bool> CreateArrayAndFile(float[] sample, string path, string fileName)
        {
            var byteArray = new byte[sample.Length * 4];
            System.Buffer.BlockCopy(sample, 0, byteArray, 0, byteArray.Length);
            await CreateWaveFile(byteArray, path, fileName);

            return true; //HACK!!
        }
    }
}
