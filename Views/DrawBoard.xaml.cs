using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GoGame.Models;
using GoGame.ViewModels;

namespace GoGame.Views;

public partial class MainWindow : Window
{
    private int _size = 19; //размер сетки
    private double _squareW;
    private double _squareH;
    private Canvas _canvas;
    private Ellipse[,] _stones; //Пробный массив камней
    private Board _board;
    public Brush _color => (_board.MoveCounter % 2 == 0) ? Brushes.Black : Brushes.White;
    public Brush Color { get { return _color; } }

    public MainWindow()
    {
        InitializeComponent();
        _board = new(19);
        DrawBoard();
    }
    private void DrawBoard()
    {
        DataContext = _board;
        _canvas = new Canvas
        {
            Height = 400,
            Width = 400,
            Background = Brushes.Peru,
            Margin = new Thickness(400 / (_size - 1) * 1.5)
        };

        ViewboxBoard.Child = _canvas;

        double cWidth = _canvas.Width;
        double cHeight = _canvas.Height;
        _squareW = cWidth / (_size - 1);
        _squareH = cHeight / (_size - 1);
        _stones = new Ellipse[_size + 1, _size + 1];


        _canvas.MouseLeftButtonDown += AddStone;

        // Вертикальные линии
        for (int i = 0; i < _size; i++)
        {
            var yStart = 0;
            var yEnd = cHeight;
            var x = i * _squareW;
            Line line = new() { X1 = x, X2 = x, Y1 = yStart, Y2 = yEnd, Stroke = Brushes.Black };
            _canvas.Children.Add(line);
        }

        // Горизонтальные линии
        for (int i = 0; i < _size; i++)
        {
            var xStart = 0;
            var xEnd = cWidth;
            var y = i * _squareH;

            Line line = new() { X1 = xStart, X2 = xEnd, Y1 = y, Y2 = y, Stroke = Brushes.Black };
            _canvas.Children.Add(line);
        }

        // Разметка по столбцам
        for (int i = 0; i < _size; i++)
        {
            char letter = (char)('A' + i);

            TextBlock label = new()
            {
                Text = letter.ToString(),
                FontSize = 12 + _squareH * 0.1,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };

            label.Measure(new Size(_canvas.Height, _canvas.Width));
            Canvas.SetLeft(label, i * _squareW - label.DesiredSize.Width / 2);
            Canvas.SetTop(label, -_squareH * 1.4);
            _canvas.Children.Add(label);
        }

        // Разметка по строкам
        for (int i = 0; i < _size; i++)
        {
            TextBlock label = new()
            {
                Text = (_size - i).ToString(),
                FontSize = 12 + _squareW * 0.1,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black
            };

            label.Measure(new Size(_canvas.Height, _canvas.Width));
            Canvas.SetLeft(label, -_squareW * 1.4);
            Canvas.SetTop(label, i * _squareH - label.DesiredSize.Height / 2);
        }
    }
    private void AddStone(object sender, MouseButtonEventArgs e)
    {
        Point coords = e.GetPosition(_canvas);

        //Получение номера узла
        int xIndex = (int)Math.Round(coords.X / _squareW);
        int yIndex = (int)Math.Round(coords.Y / _squareH);

        //Получение данных на сколько отклонен клик от узла
        double xMod = coords.X % _squareW;
        double yMod = coords.Y % _squareH;

        //Проверка на выход за пределы доски, существует ли камень на этом узле и дополнительная проверка на
        //дальность отклонения
        if (xIndex >= 0 && xIndex < _size && yIndex >= 0 && yIndex < _size && _stones[xIndex, yIndex] == null &&
            (Math.Min(xMod, yMod) < _squareW * 0.2 || Math.Max(xMod, yMod) > _squareW * 0.8))
        {
            double stoneSize = _squareW * 0.75;
            int x = (int)(xIndex * _squareW - stoneSize / 2);
            int y = (int)(yIndex * _squareH - stoneSize / 2);
            Stone stone = new(_color == Brushes.Black ? CellStatus.Black : CellStatus.White, x, y);

            //Создание и вставка камня в канвас
            Ellipse circle = new()
            {
                Width = stoneSize,
                Height = stoneSize,
                Fill = _color,
                StrokeThickness = 1
            };
            Canvas.SetLeft(circle, x);
            Canvas.SetTop(circle, y);
            _canvas.Children.Add(circle);

            //Добавление камня в массив
            _stones[xIndex, yIndex] = circle;
            //_board[x, y] = stone;
            _board.MoveCounter += 1;
        }
    }
    private void EndGame(object sender, RoutedEventArgs e)
    {
        string message = $"Игра окончена! \nЧерные: 0 очков \nБелые: 0 очков \nПобедили: Черные";
        MessageBox.Show(message, "Конец партии", MessageBoxButton.OK);
        //Обнуление доски и ее отрисовка заново
        ViewboxBoard.Child = null;
        _board = new(19);
    }
}
