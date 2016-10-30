using System.Collections.Generic;
using System;

namespace Shared.Dto
{
    public class FloatValue
    {
        public float Val { get; set; }
        public int Ctr { get; set; }
        public IList<float> Values { get; set; }
        public Wave WaveValue { get; set; }
        public IList<Line> Lines { get; set; }

        public FloatValue()
        {
            Values = new List<float>();
            Lines = new List<Line>();
        }

        public FloatValue(float pVal, int pCtr)
        {
            Init(pVal, pCtr);
        }

        private void Init(float pVal, int pCtr)
        {
            try
            {
                this.Val = pVal;
                this.Ctr = pCtr;

                Values = new List<float>();
                Lines = new List<Line>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            string val = string.Empty;

            val = Val.ToString() + "/" + Ctr.ToString();

            return val;
        }
    }
}
