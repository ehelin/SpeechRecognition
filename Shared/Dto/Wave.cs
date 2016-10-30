using System.Collections.Generic;

namespace Shared.Dto
{
    public class Wave : Base
    {
        public long LengthData { get; set; }
        public int ChannelsData { get; set; }
        public int BitsPerSampleData { get; set; }
        public long SamplesData { get; set; }
        public IList<Line> PlotLines { get; set; }
        public float MinValue { get; set; }
        public float MedianValue { get; set; }
        public float MaxValue { get; set; }
        public Dictionary<int, IList<float>> soundArrays { get; set; } 
        public IList<FloatValue> Samples { get; set; }

        public Wave()
        {
            PlotLines = new List<Line>();
            soundArrays = new Dictionary<int, IList<float>>();
            Samples = new List<FloatValue>();
        }

        public Wave(long pLengthData, int pChannelsData, int pBitsPerSampleData, long pSampleData, long pMinValue)
        {
            this.LengthData = pLengthData;
            this.ChannelsData = pChannelsData;
            this.BitsPerSampleData = pBitsPerSampleData;
            this.SamplesData = pSampleData;

            PlotLines = new List<Line>();
            soundArrays = new Dictionary<int, IList<float>>();
        }
    }
}
