using System.Reflection;
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

namespace GameGo.Views
{
    public partial class MainWindow : Window
    {
        private int n = 19;
        Canvas canvas;
        private double squareW;
        private double squareH;
        private Ellipse[,] stones; //Пробный массив камней
        private Brush color = Brushes.WhiteSmoke; //Пробный цвет

        public MainWindow()
        {
            InitializeComponent();
            DrawBoard();
        }

        public void DrawBoard()
        {
            canvas = new Canvas();
            canvas.Height = 400;
            canvas.Width = 400;
            canvas.Background = Brushes.Peru;
            canvas.Margin = new Thickness(40);

            ViewboxBoard.Child = canvas;

            double cWidth = canvas.Width;
            double cHeight = canvas.Height;
            squareW = cWidth / (n - 1);
            squareH = cHeight / (n - 1);
            stones = new Ellipse[n + 1, n + 1];

            //Здесь будет происходить смена текста в окне и цвета камней в зависимости от ходящего игрока
            /*
            if (turn == "black")
            {
                TurnStatus.Text = "Ходят черные";
                TurnStatus.Foreground = Brushes.Black;
                color = Brushes.Black;
            }
            else
            {
                TurnStatus.Text = "Ходят белые";
                TurnStatus.Foreground = Brushes.White;
                color = Brushes.WhiteSmoke
            }
            */

            canvas.MouseLeftButtonDown += AddStone;

            // Вертикальные линии
            for (int i = 0; i < n; i++)
            {
                var yStart = 0;
                var yEnd = cHeight;
                var x = i * squareW;
                Line line = new Line { X1 = x, X2 = x, Y1 = yStart, Y2 = yEnd, Stroke = Brushes.Black };
                canvas.Children.Add(line);
            }

            // Горизонтальные линии
            for (int i = 0; i < n; i++)
            {
                var xStart = 0;
                var xEnd = cWidth;
                var y = i * squareH;

                Line line = new Line { X1 = xStart, X2 = xEnd, Y1 = y, Y2 = y, Stroke = Brushes.Black };
                canvas.Children.Add(line);
            }

            // Разметка по столбцам
            for (int i = 0; i < n; i++)
            {
                char letter = (char)('A' + i);

                TextBlock label = new TextBlock { Text = letter.ToString(), FontSize = 12, FontWeight = FontWeights.Bold, Foreground = Brushes.Black };

                label.Measure(new Size(canvas.Height, canvas.Width));
                Canvas.SetLeft(label, i * squareW - label.DesiredSize.Width / 2);
                Canvas.SetTop(label, -squareH - 5);
                canvas.Children.Add(label);
            }

            // Разметка по строкам
            for (int i = 0; i < n; i++)
            {
                TextBlock label = new TextBlock { Text = (n - i).ToString(), FontSize = 12, FontWeight = FontWeights.Bold, Foreground = Brushes.Black };

                label.Measure(new Size(canvas.Height, canvas.Width));
                Canvas.SetLeft(label, -squareW - 5);
                Canvas.SetTop(label, i * squareH - label.DesiredSize.Height / 2);
                canvas.Children.Add(label);
            }
        }
        private void AddStone(object sender, MouseButtonEventArgs e)
        {
            Point coords = e.GetPosition(canvas);

            //Получение номера узла
            int xIndex = (int)Math.Round(coords.X / squareW);
            int yIndex = (int)Math.Round(coords.Y / squareH);

            //Получение данных на сколько отклонен клик от узла
            double xMod = coords.X % squareW;
            double yMod = coords.Y % squareH;

            //Проверка на выход за пределы доски, существует ли камень на этом узле и дополнительная проверка на дальность отклонения 
            if (xIndex >= 0 && xIndex <= n && yIndex >= 0 && yIndex <= n && stones[xIndex, yIndex] == null && (Math.Min(xMod, yMod) < squareW * 0.2 || Math.Max(xMod, yMod) > squareW * 0.8))
            {
                //Вычисление координат узла
                double stoneSize = squareW * 0.75;
                double xPos = xIndex * squareW - stoneSize / 2;
                double yPos = yIndex * squareH - stoneSize / 2;

                //Создание и вставка камня в канвас
                Ellipse stone = new Ellipse { Width = stoneSize, Height = stoneSize, Fill = color, Stroke = Brushes.Black, StrokeThickness = 1 };
                Canvas.SetLeft(stone, xPos);
                Canvas.SetTop(stone, yPos);
                canvas.Children.Add(stone);

                //Добавление камня в массив
                stones[xIndex, yIndex] = stone;
            }
        }
        private void EndGame(object sender, RoutedEventArgs e)
        {
            /*
            int blackScore = ;
            int whiteScore = ;
            string winner = "";
            string message = $"Игра окончена! \nЧерные: {blackScore} очков \nБелые: {whiteScore} очков \nПобедили: {winner}";
            */
            string message = $"Игра окончена! \nЧерные: 0 очков \nБелые: 0 очков \nПобедили: Черные";
            MessageBox.Show(message, "Конец партии", MessageBoxButton.OK);
            //Обнуление доски и ее отрисовка заново
            ViewboxBoard.Child = null;
            DrawBoard();
        }
    }
}