using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
namespace SpeechRecognition.Source
{
    public class Utilities
    {
        public static Line SetLine(int X1, int X2, int Y1, int Y2, Color color)
        {
            Line line = new Line();

            line.X1 = X1;
            line.X2 = X2;
            line.Y1 = Y1;
            line.Y2 = Y2;
            line.Stroke = new SolidColorBrush(color);

            return line;
        }
    }
}
