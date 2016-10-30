namespace Shared.Dto
{
    public class Line
    {
        public int lineX1 { get; set; }
        public int lineX2 { get; set; }
        public int lineY1 { get; set; }
        public int lineY2 { get; set; }
        public float value { get; set; }

        public Line() { }

        public Line(int lineX1, int lineY1, int lineX2, int lineY2, float value)
        {
            this.lineX1 = lineX1;
            this.lineY1 = lineY1;
            this.lineX2 = lineX2;
            this.lineY2 = lineY2;
            this.value = value;
        }

        public override string ToString()
        {
            string val = string.Empty;
            
            if (150 - lineY1 > 5)
                val = lineY1.ToString() + " - 150 - " + lineY2.ToString();

            return val;
        }

    }
}
