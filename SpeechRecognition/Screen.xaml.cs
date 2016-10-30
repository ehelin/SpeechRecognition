using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SpeechRecognition.Source.Dto;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpeechRecognition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Screen : Page
    {
        public Screen()
        {
            this.InitializeComponent();
        }

        //----------------------------
        //private byte[] audio = null;

        //public Screen(byte[] pAudio)
        //{
        //    InitializeComponent();
        //    audio = pAudio;
            
        //    //buildGui(percept);
        //}
        
        //public Screen(List<Perceptron> percept)
        //{
        //    InitializeComponent();
        //    buildGui(percept);
        //}

        ////x distance left to 0 - (330/2) = -115
        ////x distance right from 0 - (330/2) = +115
        ////y distance top from 0 - (330/2) = +115
        ////y distance bottom to 0 - (330/2) = -115

        ////x axis is .1
        ////y axis is .1

        ////510-330 = 180/2 = 90
        //private int xCenter = 255;
        //private int yCenter = 160;
        //private int wholeNumberPixelDistance = 100;

        //private void addTestShape()
        //{
        //    PathFigure pthFigure = new PathFigure();
        //    pthFigure.StartPoint = new Point(10, 100);

        //    System.Windows.Point Point1 = new System.Windows.Point(10, 100);
        //    System.Windows.Point Point2 = new System.Windows.Point(100, 200);
        //    System.Windows.Point Point3 = new System.Windows.Point(200, 30);
        //    System.Windows.Point Point4 = new System.Windows.Point(250, 200);
        //    System.Windows.Point Point5 = new System.Windows.Point(200, 150);

        //    PolyLineSegment plineSeg = new PolyLineSegment();
        //    plineSeg.Points.Add(Point1);
        //    plineSeg.Points.Add(Point2);
        //    plineSeg.Points.Add(Point3);
        //    plineSeg.Points.Add(Point4);
        //    plineSeg.Points.Add(Point5);

        //    PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
        //    myPathSegmentCollection.Add(plineSeg);

        //    pthFigure.Segments = myPathSegmentCollection;

        //    PathFigureCollection pthFigureCollection = new PathFigureCollection();

        //    pthFigureCollection.Add(pthFigure);

        //    PathGeometry pthGeometry = new PathGeometry();

        //    pthGeometry.Figures = pthFigureCollection;

        //    Path arcPath = new Path();

        //    arcPath.Stroke = new SolidColorBrush(Colors.Black);

        //    arcPath.StrokeThickness = 1;

        //    arcPath.Data = pthGeometry;

        //    arcPath.Fill = new SolidColorBrush(Colors.Yellow);

        //    this.Content.Children.Clear();
        //    this.Content.Children.Add(arcPath);
        //}
        //private void addGraphiLines()
        //{
        //    this.ContentCanvas.Children.Add(DrawLine(95, 165, 425, 165, Brushes.Black)); //x line
        //    this.ContentCanvas.Children.Add(DrawLine(260, 0, 260, 330, Brushes.Black)); //y line
        //}
        //private Line DrawLine(int startX, int startY, int endX, int endY, Brush brsh)
        //{
        //    Line line = new Line();
        //    line.Visibility = System.Windows.Visibility.Visible;
        //    line.Stroke = brsh;
        //    line.X1 = startX;
        //    line.X2 = endX;
        //    line.Y1 = startY;
        //    line.Y2 = endY;

        //    return line;
        //}
        //private void drawXAxisTics()
        //{
        //    this.ContentCanvas.Children.Add(DrawLine(420, 160, 420, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(410, 160, 410, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(400, 160, 400, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(390, 160, 390, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(380, 160, 380, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(370, 160, 370, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(360, 160, 360, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(350, 160, 350, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(340, 160, 340, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(330, 160, 330, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(320, 160, 320, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(310, 160, 310, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(300, 160, 300, 170, Brushes.Black));  // 1
        //    this.ContentCanvas.Children.Add(DrawLine(290, 160, 290, 170, Brushes.Black));  //.75
        //    this.ContentCanvas.Children.Add(DrawLine(280, 160, 280, 170, Brushes.Black));  //.5
        //    this.ContentCanvas.Children.Add(DrawLine(270, 160, 270, 170, Brushes.Black));  //.25

        //    //this.ContentCanvas.Children.Add(DrawLine(260, 160, 260, 170, Brushes.Black));
        //    //this.ContentCanvas.Children.Add(DrawLine(260, 160, 260, 170, Brushes.Black));

        //    this.ContentCanvas.Children.Add(DrawLine(250, 160, 250, 170, Brushes.Black));  //- .25
        //    this.ContentCanvas.Children.Add(DrawLine(240, 160, 240, 170, Brushes.Black));  //- .5
        //    this.ContentCanvas.Children.Add(DrawLine(230, 160, 230, 170, Brushes.Black));  //- .75
        //    this.ContentCanvas.Children.Add(DrawLine(220, 160, 220, 170, Brushes.Black));  //-  1
        //    this.ContentCanvas.Children.Add(DrawLine(210, 160, 210, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(200, 160, 200, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(190, 160, 190, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(180, 160, 180, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(170, 160, 170, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(160, 160, 160, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(150, 160, 150, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(140, 160, 140, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(130, 160, 130, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(120, 160, 120, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(110, 160, 110, 170, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(100, 160, 100, 170, Brushes.Black));
        //}
        //private void drawYAxisTics()
        //{
        //    this.ContentCanvas.Children.Add(DrawLine(255, 325, 265, 325, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 315, 265, 315, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 305, 265, 305, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 295, 265, 295, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 285, 265, 285, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 275, 265, 275, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 265, 265, 265, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 255, 265, 255, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 245, 265, 245, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 235, 265, 235, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 225, 265, 225, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 215, 265, 215, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 205, 265, 205, Brushes.Black));  //1
        //    this.ContentCanvas.Children.Add(DrawLine(255, 195, 265, 195, Brushes.Black));  //.75
        //    this.ContentCanvas.Children.Add(DrawLine(255, 185, 265, 185, Brushes.Black));  //.5
        //    this.ContentCanvas.Children.Add(DrawLine(255, 175, 265, 175, Brushes.Black));  //.25

        //    //this.ContentCanvas.Children.Add(DrawLine(260, 160, 260, 170, Brushes.Black));
        //    //this.ContentCanvas.Children.Add(DrawLine(260, 160, 260, 170, Brushes.Black));
        //    //this.ContentCanvas.Children.Add(DrawLine(255, 160, 265, 160, Brushes.Black));
        //    //this.ContentCanvas.Children.Add(DrawLine(255, 160, 265, 160, Brushes.Black));

        //    this.ContentCanvas.Children.Add(DrawLine(255, 155, 265, 155, Brushes.Black));  //- .25
        //    this.ContentCanvas.Children.Add(DrawLine(255, 145, 265, 145, Brushes.Black));  //- .50
        //    this.ContentCanvas.Children.Add(DrawLine(255, 135, 265, 135, Brushes.Black));  //- .75
        //    this.ContentCanvas.Children.Add(DrawLine(255, 125, 265, 125, Brushes.Black));  //-  1
        //    this.ContentCanvas.Children.Add(DrawLine(255, 115, 265, 115, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 105, 265, 105, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 95, 265, 95, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 85, 265, 85, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 75, 265, 75, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 65, 265, 65, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 55, 265, 55, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 45, 265, 45, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 35, 265, 35, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 25, 265, 25, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 15, 265, 15, Brushes.Black));
        //    this.ContentCanvas.Children.Add(DrawLine(255, 5, 265, 5, Brushes.Black));
        //}
        //private void drawPoint(int x, int y, bool correct)
        //{
        //    Ellipse e = new Ellipse();
        //    Thickness t = new Thickness(x, y, 0, 0);

        //    e.Stroke = Brushes.Black;
        //    e.StrokeThickness = 2;
        //    e.Width = 10;
        //    e.Height = 10;
        //    e.Margin = t;

        //    if (correct)
        //        e.Fill = Brushes.Green;
        //    else
        //        e.Fill = Brushes.Red;

        //    this.ContentCanvas.Children.Add(e);
        //}
        //public void drawDataPoints(List<Perceptron> percept)
        //{
        //    int curX = 0;
        //    int curY = 0;

        //    foreach (Perceptron p in percept)
        //    {
        //        foreach (TrainingSet ts in p.trainingInputs)
        //        {
        //            curX = xCenter + (ts.Inputs[0] * wholeNumberPixelDistance);
        //            curY = yCenter - (ts.Inputs[1] * wholeNumberPixelDistance);

        //            drawPoint(curX, curY, ts.CorrectOutput);
        //        }
        //    }
        //}
        //private void buildGui(List<Perceptron> percept)
        //{
        //    this.ContentCanvas.Children.Clear();

        //    //addTestShape();
        //    addGraphiLines();
        //    drawXAxisTics();
        //    drawYAxisTics();
        //    drawDataPoints(percept);  //TODO - get data from parent form and feed to this method
        //}
        //----------------------------
    }
}
