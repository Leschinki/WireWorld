using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

class WPFDraw
{
    private Canvas canvas;
    public WPFDraw(Canvas canvas_)
    {
        canvas = canvas_;
    }

    public void drawRectangle(double Top, double Left, double XSize, double YSize, Brush color)
    {
        Rectangle rect = getRectangle(XSize, YSize, color);

        rect.SetValue(Canvas.TopProperty, Top);
        rect.SetValue(Canvas.LeftProperty, Left);

        canvas.Children.Add(rect);
    }
    public void DrawLine(int x1, int y1, int x2, int y2, Brush color)
    {
        Line line = getLine(x1, y1, x2, y2,color);

        //line.SetValue(Canvas.TopProperty, y1);
        //line.SetValue(Canvas.LeftProperty, x1);

        canvas.Children.Add(line);
    }

    public void Clear()
    {
        canvas.Children.Clear();
    }

    private Rectangle getRectangle(double XSize, double YSize,Brush color)
    {
        Rectangle returnRectangle = new Rectangle();

        returnRectangle.Fill = color;
        returnRectangle.Height = YSize;
        returnRectangle.Width = XSize;

        return returnRectangle;
    }
    private Line getLine(int x1, int y1, int x2, int y2,Brush color)
    {
        Line returnLine = new Line();

        returnLine.X1 = x1;
        returnLine.X2 = x2;
        returnLine.Y1 = y1;
        returnLine.Y2 = y2;
        returnLine.Stroke = color;
        returnLine.StrokeThickness = 2;

        return returnLine;
    }
}
