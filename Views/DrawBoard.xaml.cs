using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game_go
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int n = 18;
        Viewbox viewbox;
        Canvas canvas;
        double squareW;
        double squareH;

        public MainWindow()
        {
            InitializeComponent();
            DrawBoard();
        }

        private void DrawBoard()
        {
            canvas = new Canvas();
            canvas.Height = 400;
            canvas.Width = 400;
            ViewboxBoard.Child = canvas;

            double cWidth = canvas.Width;
            double cHeight = canvas.Height;
            squareW = cWidth / n;
            squareH = cHeight / n;

            // Вертикальные линии
            for (int i = 0; i <= n; i++)
            {
                var yStart = 0;
                var yEnd = cWidth;
                var x = i * squareH;
                Line line = new Line { X1 = x, X2 = x, Y1 = yStart, Y2 = yEnd, Stroke = Brushes.Black };
                canvas.Children.Add(line);
            }
            
            // Горизонтальные линии
            for (int i = 0; i <= n; i++)
            {
                var xStart = 0;
                var xEnd = cHeight;
                var y = i * squareW;

                Line line = new Line { X1 = xEnd, X2 = xStart, Y1 = y, Y2 = y, Stroke = Brushes.Black };
                canvas.Children.Add(line);
            }
            
            // Разметка по столбцам
            for (int i = 0; i <= n; i++)
            {
                char letter = (char)('A' + i);

                TextBlock label = new TextBlock
                {
                    Text = letter.ToString(),
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };
                label.Measure(new Size(canvas.Height, canvas.Width));
                Canvas.SetLeft(label, i * squareW - label.DesiredSize.Width / 2);
                Canvas.SetTop(label, -squareH * 1.2);
                canvas.Children.Add(label);
            }

            // Разметка по строкам
            for (int i = 0; i <= n ; i++)
            {
                TextBlock label = new TextBlock
                {
                    Text = (n + 1 - i).ToString(),
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black
                };
                label.Measure(new Size(canvas.Height, canvas.Width));
                Canvas.SetLeft(label, -squareW * 1.2);
                Canvas.SetTop(label, i * squareH - label.DesiredSize.Height / 2);
                canvas.Children.Add(label);
            }
        }
    }
}
